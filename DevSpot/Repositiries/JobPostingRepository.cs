using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositiries
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly AppDbContext _context;

        public JobPostingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(JobPosting entity)
        {
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            //await _context.JobPostings
            //    .Where(jp => jp.Id == id)
            //    .ExecuteDeleteAsync();
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting != null)
            {
                _context.JobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"JobPosting with ID {id} not found.");
            }
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            // Use AsNoTracking for better performance if entities are not being updated
            return await _context.JobPostings.ToListAsync();

            //List<JobPosting> list = await _context.JobPostings
            //    .AsNoTracking()
            //    .Include(jp => jp.User)
            //    .ToListAsync();
            //return list;
        }

        public async Task<JobPosting> GetByIdAsync(int id)
        {
            var jopPosting = await _context.JobPostings.FindAsync(id);
            if (jopPosting == null)
            {
                throw new KeyNotFoundException($"JobPosting with ID {id} not found.");
            }

            //JobPosting jp = await _context.JobPostings
            //    .Include(jp => jp.User)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(jp => jp.Id == id);
            return jopPosting;
        }

        public async Task UpdateAsync(JobPosting entity)
        {
            _context.JobPostings.Update(entity);
            //await _context.JobPostings
            //    .Where(jp => jp.Id == entity.Id)
            //    .ExecuteUpdateAsync(jp => jp
            //        .SetProperty(jp => jp.Title, entity.Title)
            //        .SetProperty(jp => jp.Description, entity.Description)
            //        .SetProperty(jp => jp.Company, entity.Company)
            //        .SetProperty(jp => jp.Location, entity.Location)
            //        .SetProperty(jp => jp.IsApproved, entity.IsApproved));
            await _context.SaveChangesAsync();
        }
    }
}
