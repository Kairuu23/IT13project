using project.Data;
using project.Models;
using Microsoft.EntityFrameworkCore;



namespace project.Services
{
    public class TrainerService
    {
        private readonly AppDbContext _db;

        public TrainerService(AppDbContext db)
        {
            _db = db;
        }

        // GET ALL TRAINERS
        public async Task<List<Trainer>> GetTrainersAsync()
        {
            return await _db.Trainers
                           .OrderBy(t => t.TrainerID)
                           .ToListAsync();
        }

        // ADD TRAINER
        public async Task AddTrainerAsync(Trainer trainer)
        {
            _db.Trainers.Add(trainer);
            await _db.SaveChangesAsync();
        }

        // UPDATE TRAINER
        public async Task UpdateTrainerAsync(Trainer trainer)
        {
            _db.Trainers.Update(trainer);
            await _db.SaveChangesAsync();
        }

        // DELETE TRAINER
        public async Task DeleteTrainerAsync(Trainer trainer)
        {
            _db.Trainers.Remove(trainer);
            await _db.SaveChangesAsync();
        }
    }
}
