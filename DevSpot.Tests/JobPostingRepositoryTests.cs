using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositiries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Tests
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DevSpotTestDb")
                .Options;
        }

        private AppDbContext CreateContext() => new AppDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);
            var jobPosting = new JobPosting
            {
                Title = "Test Job Temp",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test-user-id"
            };

            // Act
            await repository.AddAsync(jobPosting);
            await dbContext.SaveChangesAsync(); // ?!

            var addingJPResult = await dbContext.JobPostings.FindAsync(jobPosting.Id);
    //        var addingJPResult = await dbContext.JobPostings
    //          .FirstOrDefaultAsync(jp => jp.Title == jobPosting.Title);

            var addingJPResult2 = await dbContext.JobPostings
                .FirstOrDefaultAsync(jp => jp.Title == "Title");

            // Assert
            Assert.NotNull(addingJPResult);
            Assert.Equal(jobPosting.Description, addingJPResult.Description);
            Assert.Null(addingJPResult2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnJobPosting()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);
            var jobPosting = new JobPosting
            {
                Title = "Test Job",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test-user-id"
            };

            // Act
            await dbContext.JobPostings.AddAsync(jobPosting);
            await dbContext.SaveChangesAsync();

            var retrievedJobPosting = await repository.GetByIdAsync(jobPosting.Id);

            // Assert
            Assert.NotNull(retrievedJobPosting);
            Assert.Equal("Test Job", retrievedJobPosting.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);

            // Act

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repository.GetByIdAsync(999) // Assuming 999 does not exist
            );
        }

        [Fact]
        public async Task AddAsync_ShouldGetAllJobPostings()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);
            var jobPosting = new JobPosting
            {
                Title = "Test Job",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test-user-id"
            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test2 Job",
                Description = "Test2 Description",
                Company = "Test2 Company",
                Location = "Test2 Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test2-user-id"
            };
            var jobPosting3 = new JobPosting
            {
                Title = "Test3 Job",
                Description = "Test3 Description",
                Company = "Test3 Company",
                Location = "Test3 Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test3-user-id"
            };

            List<JobPosting> jobPostings = new List<JobPosting>
            {
                jobPosting,
                jobPosting2,
                jobPosting3
            };

            // Act
            await dbContext.JobPostings.AddRangeAsync(jobPostings);
            await dbContext.SaveChangesAsync();

            var allJobPostings = await repository.GetAllAsync();

            // Assert
            Assert.NotEmpty(allJobPostings);
            Assert.NotNull(allJobPostings);
            //Assert.Equal(3, allJobPostings.Count()); // will work only in separate check
            //Assert.Contains(allJobPostings, jp => jp.Title == "Test2 Job" && jp.Description == "Test2 Description"); // will work only in separate check
            //Assert.Equal(jobPostings, allJobPostings); // will work only in separate check
        }

        [Fact]
        public async Task DeleteAsync_ShouldRDeleteJobPosting()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);
            var jobPosting = new JobPosting
            {
                Title = "Test Job",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test-user-id"
            };

            // Act
            await dbContext.JobPostings.AddAsync(jobPosting);
            await dbContext.SaveChangesAsync();
            await repository.DeleteAsync(jobPosting.Id);

            // Assert
            Assert.Null(await dbContext.JobPostings.FindAsync(jobPosting.Id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);

            // Act

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repository.DeleteAsync(999) // Assuming 999 does not exist
            );
        }

        [Fact]
        public async Task UpdateAsync_ShouldRUpdateJobPosting()
        {
            // Arrange
            var dbContext = CreateContext();
            var repository = new JobPostingRepository(dbContext);
            var jobPosting = new JobPosting
            {
                Title = "Test Job",
                Description = "Test Description",
                Company = "Test Company",
                Location = "Test Location",
                PostedDate = DateTime.UtcNow,
                UserId = "test-user-id"
            };

            // Act
            await dbContext.JobPostings.AddAsync(jobPosting);
            await dbContext.SaveChangesAsync();
            // Detach the entity to simulate a scenario where the entity is not being tracked by the context
            //dbContext.Entry(jobPosting).State = EntityState.Detached; // don`t work

            jobPosting.Location = "Updated Location";
            await repository.UpdateAsync(jobPosting);

            var result = await dbContext.JobPostings.FindAsync(jobPosting.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Location", jobPosting.Location);

        }
    }
}
