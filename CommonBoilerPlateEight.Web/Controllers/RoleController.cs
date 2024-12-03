using CommonBoilerPlateEight.Domain.Constants;
using CommonBoilerPlateEight.Domain.Entity;
using CommonBoilerPlateEight.Domain.Exceptions;
using CommonBoilerPlateEight.Domain.Interfaces;
using CommonBoilerPlateEight.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Account.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        public RoleController(RoleManager<IdentityRole> roleManager,
          IRoleService roleService,
          UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _roleService = roleService;
            _userManager = userManager;
        }
        //[Authorize(Policy ="Role-View")]
        public async Task<IActionResult> Index()
        {
            var roles =await  _roleManager.Roles.Where(a=>a.Name != RoleConstant.RoleAdmin).ToListAsync();
            var roleIndexViewModels = new List<RoleIndexViewModel>();
            var i = 1;
            foreach (var role in roles)
            {
                roleIndexViewModels.Add(new RoleIndexViewModel
                {
                    Sno = i,
                    Id = role.Id,
                    Name = role.Name,

                });
                i++;
              
            }
            return View(roleIndexViewModels);
        }

        //[Authorize(Policy = "Role-Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            try
            {
                var dto = new CreateRoleViewModel { Name = model.Name };
                await _roleService.Create(dto);
                this.NotifySuccess("Role Created Successfully");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.NotifyError(ex.Message);
            }
            return View(model);
        }
        //[Authorize(Policy = "Role-Update")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(true) ?? throw new CustomException("Role not found.");
            var roleEditViewModel = new UpdateRoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(roleEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            try
            {
                await _roleService.Update(new UpdateRoleViewModel { Id=model.Id,Name=model.Name});
                this.NotifySuccess("Role Updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this.NotifyError(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

      
    }
}
