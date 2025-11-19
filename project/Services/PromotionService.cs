using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Services
{
    public class PromotionService
    {
        private readonly AppDbContext _db;

        public PromotionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Promotion>> GetPromotionsAsync()
        {
            return await _db.Promotions
                .AsNoTracking()
                .OrderBy(p => p.PromoID)
                .ToListAsync();
        }

        public async Task<Promotion?> GetPromotionByIdAsync(int id)
        {
            return await _db.Promotions.FirstOrDefaultAsync(p => p.PromoID == id);
        }

        public async Task AddPromotionAsync(Promotion promo)
        {
            _db.Promotions.Add(promo);
            await _db.SaveChangesAsync();
        }

        public async Task UpdatePromotionAsync(Promotion promo)
        {
            _db.Promotions.Update(promo);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePromotionAsync(int id)
        {
            var promo = await _db.Promotions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PromoID == id);

            if (promo != null)
            {
                _db.Promotions.Remove(promo);
                await _db.SaveChangesAsync();
            }
        }

    }
}
