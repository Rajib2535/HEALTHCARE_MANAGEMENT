using DATA.Models;
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
                var userId = _httpContextAccessor.HttpContext.Session.GetString("user_id");

                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "User");

                var patientInfo = await _context.Patients
                    .Where(x => x.UserId == Convert.ToInt32(userId))
                    .FirstOrDefaultAsync();

                if (patientInfo == null)
                    return NotFound("Patient information not found.");

                var appointments = await _context.Appointments
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

            var patients = await _context.Appointments.Where(x => x.DoctorId == Convert.ToInt32(userId)).ToListAsync();
            return View(patients);
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


        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Patients = new SelectList(_context.Patients, "Id", "Name", appointment.PatientId);
            ViewBag.Doctors = new SelectList(_context.Doctors, "Id", "Name", appointment.DoctorId);
            return View(appointment);
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
