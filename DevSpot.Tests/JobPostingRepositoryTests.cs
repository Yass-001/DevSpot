using DevSpot.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Tests
{
    internal class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DevSpotTestDb")
                .Options;
                    }

        private AppDbContext CreateContext() => new AppDbContext(_options);     

    }
}
