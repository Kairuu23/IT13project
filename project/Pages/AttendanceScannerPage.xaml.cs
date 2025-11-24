using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using project.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using project.Models;

namespace project.Pages;

public partial class AttendanceScannerPage : ContentPage
{
    private readonly IAttendanceService? _attendanceService;
    private readonly IMemberService? _memberService;
    private readonly INativeNavigationService? _nativeNavigationService;

    private bool _isProcessing = false;
    private DateTime _lastScanTime = DateTime.MinValue;
    private const int MIN_SCAN_INTERVAL_MS = 2000;

    public AttendanceScannerPage(
        IAttendanceService? attendanceService = null,
        IMemberService? memberService = null,
        INativeNavigationService? nativeNavigationService = null)
    {
        InitializeComponent();

        _attendanceService = attendanceService ??
            Application.Current?.Handler?.MauiContext?.Services?.GetService<IAttendanceService>();

        _memberService = memberService ??
            Application.Current?.Handler?.MauiContext?.Services?.GetService<IMemberService>();

        _nativeNavigationService = nativeNavigationService ??
            Application.Current?.Handler?.MauiContext?.Services?.GetService<INativeNavigationService>();
    }

    private async void OnBarcodeDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var timeDiff = (DateTime.Now - _lastScanTime).TotalMilliseconds;
        if (timeDiff < MIN_SCAN_INTERVAL_MS) return;

        if (_isProcessing || e.Results == null || e.Results.Length == 0)
            return;

        var qrCode = e.Results[0].Value;
        if (string.IsNullOrWhiteSpace(qrCode)) return;

        _isProcessing = true;
        _lastScanTime = DateTime.Now;

        try
        {
            BarcodeReader.IsDetecting = false;
        }
        catch { }

        try
        {
            if (!qrCode.StartsWith("GYM:MEMBER:"))
            {
                await ShowAlertAsync("Invalid QR Code", "This is not a valid gym member QR code.");
                await Task.Delay(500);
                RestartCamera();
                return;
            }

            var parts = qrCode.Split(':');
            if (parts.Length < 3 || !int.TryParse(parts[2], out var memberId))
            {
                await ShowAlertAsync("Invalid Code", "Invalid gym QR format.");
                RestartCamera();
                return;
            }

            Member? member = null;

            if (_memberService != null)
            {
                member = await _memberService.GetMemberByIdAsync(memberId);
                if (member == null)
                {
                    await ShowAlertAsync("Member Not Found", $"Member ID {memberId} not found.");
                    RestartCamera();
                    return;
                }

                if (!IsMemberEligibleForAttendance(member, out var reason))
                {
                    await ShowAlertAsync("Attendance Blocked", reason);
                    RestartCamera();
                    return;
                }
            }

            if (_attendanceService != null)
            {
                var success = await _attendanceService.ProcessQrAttendanceAsync(qrCode);
                if (success)
                {
                    string name = member != null ? $"{member.FirstName} {member.LastName}" : $"Member {memberId}";

                    await ShowAlertAsync("Success", $"{name} attendance processed!");
                    await Task.Delay(300);
                    await CloseScannerSafely();
                }
                else
                {
                    await ShowAlertAsync("Error", "Failed to process attendance.");
                    RestartCamera();
                }
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", ex.Message);
            RestartCamera();
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private async Task CloseScannerSafely()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try
            {
                BarcodeReader.IsDetecting = false;
                await Task.Delay(200);
                if (Navigation.NavigationStack.Count > 1)
                    await Navigation.PopAsync();

                _nativeNavigationService?.NotifyScannerClosed();
            }
            catch { }
        });
    }

    private void RestartCamera()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                if (!_isProcessing)
                    BarcodeReader.IsDetecting = true;
            }
            catch { }
        });
    }

    private async void OnCloseClicked(object? sender, EventArgs e)
    {
        await CloseScannerSafely();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try { BarcodeReader.IsDetecting = true; } catch { }
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            try { BarcodeReader.IsDetecting = false; } catch { }
        });
    }

    private Task ShowAlertAsync(string title, string message)
    {
        return MainThread.InvokeOnMainThreadAsync(async () =>
        {
            try { await DisplayAlert(title, message, "OK"); } catch { }
        });
    }

    private static bool IsMemberEligibleForAttendance(Member member, out string reason)
    {
        if (member.IsArchived)
        {
            reason = $"{member.FullName} is archived.";
            return false;
        }

        if (!string.Equals(member.Status, "Active", StringComparison.OrdinalIgnoreCase))
        {
            reason = $"{member.FullName} is {member.Status?.ToLower() ?? "inactive"}.";
            return false;
        }

        if (member.ExpirationDate.HasValue &&
            member.ExpirationDate.Value.Date < DateTime.Today)
        {
            reason = $"{member.FullName}'s membership expired on {member.ExpirationDate.Value:MM/dd/yyyy}.";
            return false;
        }

        reason = string.Empty;
        return true;
    }
}
