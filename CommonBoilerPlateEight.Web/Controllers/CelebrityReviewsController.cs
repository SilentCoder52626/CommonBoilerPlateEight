using Microsoft.AspNetCore.Mvc;
using CommonBoilerPlateEight.Domain.Interfaces;
using X.PagedList;

namespace CommonBoilerPlateEight.Web.Controllers
{
    public class CelebrityReviewsController : Controller
    {
        private readonly IReviewService _celebrityReviewService;

        public CelebrityReviewsController(IReviewService celebrityReviewService)
        {
            _celebrityReviewService = celebrityReviewService;
        }

        //Ignore celebrity review for now

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reviews = await _celebrityReviewService.GetAllReviewsAsync();
            // Assuming you want pagination, you'll need to convert the list to an IPagedList
            var pagedReviews = reviews.ToPagedList(1, 10); // You can replace 1 and 10 with dynamic page and page size

            return View(pagedReviews);
        }


        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _celebrityReviewService.ApproveReviewByIdAsync(id);

            if (!result)
            {
                return Json(new { Status = "Error", Errors = new[] { "Review not found or could not be approved." } });
            }

            return Json(new { Status = "Success", Message = "Review approved successfully." });
        }
    }

}
