using DevSpot.Data;
using DevSpot.Models;

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

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<JobPosting> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(JobPosting entity)
        {
            throw new NotImplementedException();
        }
    }
}
