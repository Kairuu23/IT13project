using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Services
{
    public class MemberService
    {
        private readonly AppDbContext _db;

        public MemberService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Member>> GetMembersAsync()
        {
            return await _db.Members.ToListAsync();
        }

        public async Task<Member?> GetMemberByIdAsync(int id)
        {
            return await _db.Members.FirstOrDefaultAsync(m => m.MemberID == id);
        }

        public async Task AddMemberAsync(Member member)
        {
            await _db.Members.AddAsync(member);
            await _db.SaveChangesAsync();
        }


        public async Task UpdateMemberAsync(Member member)
        {
            var existing = await _db.Members.FirstOrDefaultAsync(m => m.MemberID == member.MemberID);

            if (existing != null)
            {
                existing.FirstName = member.FirstName;
                existing.MiddleInitial = member.MiddleInitial;
                existing.LastName = member.LastName;
                existing.ContactNumber = member.ContactNumber;
                existing.Email = member.Email;
                existing.Address = member.Address;
                existing.JoinDate = member.JoinDate;
                existing.Status = member.Status;
                existing.MembershipTypeID = member.MembershipTypeID;
            }

            await _db.SaveChangesAsync();
        }


        public async Task DeleteMemberAsync(int id)
        {
            var member = await _db.Members.FirstOrDefaultAsync(m => m.MemberID == id);
            if (member != null)
            {
                _db.Members.Remove(member);
                await _db.SaveChangesAsync();
            }
        }

    }
}
