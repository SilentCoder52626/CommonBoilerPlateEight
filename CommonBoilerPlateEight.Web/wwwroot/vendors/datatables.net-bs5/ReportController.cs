using Application.CourseLessons.DTOs;
using Application.Courses.DTOs;
using Application.Report.Dto;
using Application.Report.Interface;
using Application.StudentCourse.DTOs;
using Application.StudentCourse.Interfaces;
using Application.Subjects.Interfaces;
using Application.TransactionLog.Interfaces;
using Application.Universities.DTOs;
using Application.Universities.Interfaces;
using Application.UserManagement.Interfaces;
using Domain.DbContext;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Exceptions;
using WebAPP.ActionFilters;
using WebAPP.Extension;
using WebAPP.Extensions;
using WebAPP.Models;

namespace WebAPP.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IUniversityService _universityService;
        private readonly ISubjectService _subjectService;
        private readonly IStudentCourseService _studentCourseService;
        private readonly InstituteDBContext _context;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService reportService,
            IUniversityService universityService,
            ISubjectService subjectService,
            ILogger<ReportController> logger,
            IStudentCourseService studentCourseService,
            InstituteDBContext context,
            IUserService userService)
        {
            _reportService = reportService;
            _universityService = universityService;
            _subjectService = subjectService;
            _logger = logger;
            _context = context;
            _studentCourseService = studentCourseService;
            _userService = userService;
        }

        [Authorize(Policy = "Report-PurchaseReport")]
        [TypeFilter(typeof(ActivityLogFilterAttribute))]
        public async Task<IActionResult> Index()
        {
            ViewBag.IsTutor = await _userService.IsTutor(this.GetCurrentUserId());
            ViewBag.Universities = await _universityService.GetUniversitiesForDropdown();
            return View();
        }

        [Authorize(Policy = "Report-UniversityWiseReport")]
        [TypeFilter(typeof(ActivityLogFilterAttribute))]
        public async Task<IActionResult> UniversityWiseReport()
        {
            ViewBag.Universities = await _universityService.GetUniversitiesForDropdown();
            return View(new UniversityWiseReportFilterModel() { FromDate = DateTime.Now.AddDays(-15),ToDate = DateTime.Now});
        }

        [HttpPost]
        public async Task<IActionResult> GetUniversityWiseReport(UniversityWiseReportFilterModel filter)
        {
            try
            {
                var filterDto = new UniversityWiseReportFilterDto
                {
                    Skip = filter.start,
                    Take = filter.length,
                    FromDate = filter.FromDate,
                    ToDate = filter.ToDate,

                    Status = filter.Status,
                    SubjectId = filter.Subject,
                    UniversityId = filter.UniversityId
                };
                var response = await _reportService.GetUniversityWiseReport(filterDto);
                var jsonData = new { draw = filter.draw, recordsFiltered = response.TotalCount, recordsTotal = response.TotalCount, data = response.Data };             
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetUniversityWiseReport");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }
        }

        [Authorize(Policy = "Report-InstructorWiseReport")]
        [TypeFilter(typeof(ActivityLogFilterAttribute))]
        public async Task<IActionResult> InstructorWiseReport()
        {
            ViewBag.Tutors = await _userService.GetTutorUsers();
            return View(new InstructorWiseReportFilterViewModel() { FromDate = DateTime.Now.AddDays(-15), ToDate = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> GetGetInstructorWiseReport(InstructorWiseReportFilterViewModel filter)
        {
            try
            {
                var filterDto = new InstructorWiseReportFilterDto
                {
                    Skip = filter.start,
                    Take = filter.length,
                    FromDate = filter.FromDate,
                    ToDate = filter.ToDate,
                    Status = filter.Status,
                    InstructorId = filter.InstructorId,
                    CourseId = filter.CourseId
                };
                var response = await _reportService.GetInstructorWiseReport(filterDto);
                var jsonData = new { draw = filter.draw, recordsFiltered = response.TotalCount, recordsTotal = response.TotalCount, data = response.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetUniversityWiseReport");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(string orderId, Guid transactionId)
        {
            try
            {
                var transaction = await _context.Transactions.Include(a => a.Student).Where(a => a.Id == transactionId).FirstOrDefaultAsync() ?? throw new CustomException("Transaction Not Found");
                var studentCourse = await _context.StudentCourse.Where(a => !a.IsDeleted && a.ActiveStatus && a.OrderId == orderId).FirstOrDefaultAsync().ConfigureAwait(false);
                var studentCourseLesson = await _context.StudentCourseLesson.Where(a => !a.IsDeleted && a.ActiveStatus && a.OrderId == orderId).FirstOrDefaultAsync().ConfigureAwait(false);
                if (studentCourse == null && studentCourseLesson == null) throw new CustomException("Transaction record not found");
                var transactionDetail = new TransactionDetail
                {
                    PaymentAction = transaction.Type,
                    Comment = transaction.Description,
                    CreatedOn = transaction.ActionOn.ToString("yyyy-mm-dd hh:mm tt")
                };
                if (studentCourse != null)
                {
                    var studentCourseDetail = await _studentCourseService.GetStudentCourseDetail(studentCourse.Id, true);
                    if(transaction.Type == CommonConstant.BalanceSheetTypeRefund)
                    {
                        transactionDetail.ActionBy = studentCourseDetail.RefundedBy;
                    }
                    studentCourseDetail.TransactionDetail = transactionDetail;
                    var orderDetailsView = this.RenderViewAsync("~/Views/TransactionLog/_CourseOrderDetailView.cshtml", studentCourseDetail, true).GetAwaiter().GetResult();
                    return Ok(new RtnCommon
                    {
                        Status = true,
                        Data = orderDetailsView
                    });
                }
                else
                {
                    var studentCourseLessonDetail = await _studentCourseService.GetStudentCourseLessonDetail(studentCourseLesson.Id, true);
                    if (transaction.Type == CommonConstant.BalanceSheetTypeRefund)
                    {
                        transactionDetail.ActionBy = studentCourseLessonDetail.RefundedBy;
                    }

                    studentCourseLessonDetail.TransactionDetail = transactionDetail;
                    var orderDetailsView = this.RenderViewAsync("~/Views/TransactionLog/_CourseLessonOrderDetailView.cshtml", studentCourseLessonDetail, true).GetAwaiter().GetResult();
                    return Ok(new RtnCommon
                    {
                        Status = true,
                        Data = orderDetailsView
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetOrderDetails : Report");
                return BadRequest(new RtnCommon
                {
                    Message = ex.Message,
                    Status = false
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetCourseWiseStudentCount([FromForm] CourseWisePurchaseFilterViewModel model)
        {
            try
            {
                var filterDto = new CourseWisePurchaseFilterDto
                {
                    Skip = model.start,
                    Take = model.length,
                    Search = model.Search,
                    UniversityId = model.UniversityId
                };
                var response = await _reportService.GetCourseWiseStudentCount(filterDto,this.GetCurrentUserId());
                var jsonData = new { draw = model.draw, recordsFiltered = response.TotalCount, recordsTotal = response.TotalCount, data = response.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetCourseWiseStudentCount");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }

        }
        [Authorize(Policy = "Report-PurchaseReport")]
        public async Task<IActionResult> GetCourseLessonWiseStudentCount(Guid courseId)
        {
            try
            {
                var response = await _reportService.GetCourseLessonWiseStudentCount(courseId);
                var jsonData = new { recordsFiltered = response.TotalCount, recordsTotal = response.TotalCount, data = response.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetCourseLessonWiseStudentCount");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }
        }

        [Authorize(Policy = "Report-PurchaseReport")]
        public async Task<IActionResult> GetAllStudentNamesPurchasingTheCourse(Guid courseId)
        {
            try
            {
                var response = await _reportService.GetAllStudentPurchasingCourse(courseId);
                var allStudentListView = this.RenderViewAsync("~/Views/Report/_StudentPurchasingCourseListView.cshtml", response, true).GetAwaiter().GetResult();
                return Ok(new RtnCommon()
                {
                    Data = allStudentListView,
                    Status = true,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetAllStudentNamesPurchasingTheCourse");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }
        }
        [Authorize(Policy = "Report-PurchaseReport")]
        public async Task<IActionResult> GetAllStudentNamesPurchasingTheCourseLesson(Guid courseLessonId)
        {
            try
            {
                var response = await _reportService.GetAllStudentPurchasingCourseLesson(courseLessonId);
                var allStudentListView = this.RenderViewAsync("~/Views/Report/_StudentPurchasingCourseLessonListView.cshtml", response, true).GetAwaiter().GetResult();
                return Ok(new RtnCommon()
                {
                    Data = allStudentListView,
                    Status = true,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in method GetAllStudentNamesPurchasingTheCourseLesson");
                return BadRequest(new RtnCommon { Status = false, Message = ex.Message });
            }
        }
    }
}
