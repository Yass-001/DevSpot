using DevSpot.Models;
using DevSpot.Repositiries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevSpot.Controllers
{
    public class JobPostingsController : Controller
    {
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(
            IRepository<JobPosting> repository,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllAsync(); // IEnumerable <JobPosting>
            return View(jobPostings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobPosting jobPosting)
        {
            //ModelState.Remove("UserId"); // Remove UserId from ModelState to avoid validation error // bad praktice
            //ModelState.Remove("User"); // Remove User from ModelState to avoid validation error // bad praktice

            if (ModelState.IsValid)
            {
                // Set the UserId to the current user's Id
                jobPosting.UserId = _userManager.GetUserId(User);
                await _repository.AddAsync(jobPosting);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
