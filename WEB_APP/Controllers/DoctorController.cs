using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels;
using DATA.Models;
using DATA.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SERVICE.Interface;
using SERVICE.Manager;
using System.Net;

namespace WEB_APP.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDocterManager _service;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IUserRoleManager _userRoleManager;
        public DoctorController(IDocterManager service, IUserManager userManager,IRoleManager roleManager,IUserRoleManager userRoleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoleManager = userRoleManager;
        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _service.GetAllDoctors();
            return View(doctors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _service.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task< IActionResult> Create(Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //       await _service.CreateDoctor(doctor);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(doctor);
        //}
       
        public async Task<ActionResult> Create(DoctorRegisterViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    UserInfoViewModel userInfoViewModel = new UserInfoViewModel()
                    {
                        Name = model.UserName,
                        Email = model.Email,
                        Mobile = model.PhoneNumber,
                        Password = model.Password,
                        Status = true,
                        StatusInput =true,
                        IsPasswordReset = false,
                        CreateTime = DateTime.Now,
                    };
                    var result = await _userManager.AddUser(userInfoViewModel);
                    if (result > 0)
                    {
                        Doctor doctor = new Doctor()
                        {
                            Name = model.UserName,
                            Email = model.Email,
                            Address = model.Address,
                            ClinicName = model.ClinicName,
                            PhoneNumber = model.PhoneNumber,
                            Specialty = model.Specialty,
                            StartTime=model.StartTime,
                            EndTime=model.EndTime,
                            Qualification = model.Qualification,
                            YearsOfExperience = model.YearsOfExperience,
                            UserId=result,
                            CreatedAt = DateTime.Now,
                        };
                        if (await _service.CreateDoctor(doctor) > 0)
                        {
                            var role = await _roleManager.GetRoleByType(2);
                            UserRole userRole = new UserRole()
                            {
                                RoleId = role.Id,
                                UserId = result,
                            };
                            await _userRoleManager.AddUserRole(userRole);
                            TempData["success"] = "Doctor Create successfully";
                            return RedirectToAction("Index");
                        }

                    }
                    return View(model);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _service.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task< IActionResult> Edit(int id, Doctor doctor)
        //{
        //    if (id != doctor.Id || !ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    await _service.UpdateDoctor(doctor);
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                TempData["failed"] = "Invalid doctor ID.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            if (!ModelState.IsValid)
            {
                TempData["failed"] = "Validation failed. Please correct the errors.";
                return View(doctor); // Return the view with the current model to show validation errors
            }

            try
            {
                var result = await _service.UpdateDoctor(doctor);

                if (result) // Assume the service returns a boolean indicating success
                {
                    TempData["success"] = "Doctor details updated successfully.";
                }
                else
                {
                    TempData["failed"] = "Failed to update doctor details.";
                }
            }
            catch (Exception ex)
            {
                TempData["failed"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _service.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteDoctor(id);
            return RedirectToAction(nameof(Index));
        }
        //protected override void Dispose(bool disposing)
        //{
        //    //db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
