using DevSpot.Constants;
using DevSpot.Models;
using DevSpot.Repositiries;
using DevSpot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevSpot.Controllers
{
    [Authorize]
    public class JobPostingsController : Controller
    {
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager; //make interface for UserManager

        public JobPostingsController(
            IRepository<JobPosting> repository,
            UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
           

            if(User.IsInRole(Roles.EMPLOYER))
            {
                var allJobPostings = await _repository.GetAllAsync(); // IEnumerable <JobPosting>
                var userId = _userManager.GetUserId(User);
                var filteredJobPostings = allJobPostings.Where(jp => jp.UserId == userId); // Filter by current user's job postings
                return View(filteredJobPostings);
            }

            var jobPostings = await _repository.GetAllAsync();

            return View(jobPostings);
        }

        [Authorize(Roles = "Admin,Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingViewModel)
        {
            //ModelState.Remove("UserId"); // Remove UserId from ModelState to avoid validation error
            //for "JobPosting jobPosting" // bad praktice
            //ModelState.Remove("User"); // Remove User from ModelState to avoid validation error
            //for "JobPosting jobPosting" // bad praktice

            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting
                {
                    Title = jobPostingViewModel.Title,
                    Description = jobPostingViewModel.Description,
                    Company = jobPostingViewModel.Company,
                    Location = jobPostingViewModel.Location,
                    //IsApproved = false, // Default set to false in model
                    UserId = _userManager.GetUserId(User) // Set the UserId to the current user's Id
                };

                await _repository.AddAsync(jobPosting);
                return RedirectToAction(nameof(Index));
            }

            return View(jobPostingViewModel);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (jobPosting.UserId != userId && !User.IsInRole(Roles.ADMIN))
            {
                return Forbid(); // Only allow the owner or admin to delete
            }

            await _repository.DeleteAsync(id);

            return Ok();
        }

        //[Authorize(Roles = "Admin,Employer")]
        //public async Task<IActionResult> DeleteEasy(int id)
        //{
        //    var jobPosting = await _repository.GetByIdAsync(id);
        //    if (jobPosting == null)
        //    {
        //        return NotFound();
        //    }

        //    var userId = _userManager.GetUserId(User);
        //    if (jobPosting.UserId != userId && !User.IsInRole(Roles.ADMIN))
        //    {
        //        return Forbid(); // Only allow the owner or admin to delete
        //    }

        //    await _repository.DeleteAsync(id);

        //    return RedirectToAction(nameof(Index));
        //}


    }
}
