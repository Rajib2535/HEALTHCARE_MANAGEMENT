using DATA.Models;
using DATA.Models.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WEB_APP.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToList();
            return View(appointments);
        }
        //public async Task<IActionResult> PatientAppointment()
        //{
        //    var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
        //    var patientInfo = await _context.Patients.Where(x => x.UserId == Convert.ToInt32(userId)).FirstOrDefaultAsync();
        //    var appointment = await _context.Appointments.Where(x =>x.PatientId == patientInfo.Id).ToListAsync();
        //    return View(appointment);
        //}
        public async Task<IActionResult> PatientAppointment()
        {
            try
            {
                List<Appointment> appointments=new List<Appointment>();
                var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");

                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "User");

                var patientInfo = await _context.Patients
                    .Where(x => x.UserId == Convert.ToInt32(userId))
                    .FirstOrDefaultAsync();

                if (patientInfo == null)
                {
                    return View(appointments);
                }
                   

                appointments = await _context.Appointments
                    .Where(x => x.PatientId == patientInfo.Id).Include(x=>x.Doctor).Include(x=>x.Patient)
                    .ToListAsync();

                return View(appointments);
            }
            catch (Exception ex)
            {
                // Log the error (e.g., using Serilog or any logging library)
                TempData["error"] = "An error occurred while loading appointments.";
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> DoctorAppointment()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "User");

            var doctorInfo = await _context.Doctors
                    .Where(x => x.UserId == Convert.ToInt32(userId))
                    .FirstOrDefaultAsync();

            if (doctorInfo == null)
                return NotFound("Doctor information not found.");

            var appointments = await _context.Appointments
                   .Where(x => x.DoctorId == doctorInfo.Id).Include(x => x.Doctor).Include(x => x.Patient)
                   .ToListAsync();
            return View(appointments);
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name");
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name");
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {

            if (ModelState.IsValid)
            {
                appointment.CreatedAt = DateTime.Now;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["success"] = "Appointment created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
            return View(appointment);
        }
        public IActionResult CreateByPatient()
        {
           
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name");
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByPatient(Appointment appointment)
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");
            var patientInfo = await _context.Patients
                .Where(x => x.UserId == Convert.ToInt32(userId))
                .FirstOrDefaultAsync();

            appointment.PatientId = patientInfo.Id;

            if (ModelState.IsValid)
            {
                appointment.CreatedAt = DateTime.Now;
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["success"] = "Appointment created successfully.";
                return RedirectToAction(nameof(PatientAppointment));
            }

            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
            return View(appointment);
        }


        //GET: Appointment/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();
            string role_name = _httpContextAccessor.HttpContext.Session.GetString("RoleName");
            if (role_name.ToLower().ToString() == "doctor")
            {
                ViewBag.IsDoctor = true;
            }


            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
            return View(appointment);
        }
        

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment, IFormFile AppointmentImage)
        {
            if (id != appointment.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (AppointmentImage != null && AppointmentImage.Length > 0)
                    {
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Prescription");

                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var fileName = Path.GetFileName(AppointmentImage.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await AppointmentImage.CopyToAsync(stream);
                        }

                        appointment.FilePath = "/Prescription/" + fileName;
                    }

                    appointment.UpdatedAt = DateTime.Now;
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Appointment updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appointments.Any(e => e.Id == appointment.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(DoctorAppointment));
            }

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);

            return View(appointment);
        }
        [HttpGet]
        public async Task<IActionResult> MedicalHistory(int id)
        {
            
                var prescriptions = await _context.Appointments
                    .Where(a => a.PatientId == id)
                    .Select(a => new
                    {
                        a.Id,
                        a.FilePath
                    })
                    .ToListAsync();

                var model = prescriptions.Select(img => new MedicalHistoryViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.FilePath
                }).ToList();

                return View(model);
            
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            TempData["success"] = "Appointment deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }

}
