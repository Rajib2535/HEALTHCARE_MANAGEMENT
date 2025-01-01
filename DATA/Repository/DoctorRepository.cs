using DATA.Interface;
using DATA.Models;
using DATA;
using Microsoft.EntityFrameworkCore;

public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DoctorRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CreateDoctor(Doctor doctor)
    {

        try
        {

            doctor.CreatedAt = DateTime.UtcNow;
            doctor.UpdatedAt = DateTime.UtcNow;

            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            return doctor.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            throw;
        }
       
    }


    public async Task<bool> DeleteDoctor(int id)
    {
        var doctor = await _dbContext.Doctors.FindAsync(id);
        if (doctor != null)
        {
            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Doctor> GetDoctorById(int id)
    {
        try
        {
            var doctor = await _dbContext.Doctors.FindAsync(id);
            if (doctor == null)
            {
                Console.WriteLine($"Doctor with ID {id} not found.");
            }
            return doctor;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving doctor by ID: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateDoctor(Doctor doctor)
    {
        var existingDoctor = await _dbContext.Doctors.FindAsync(doctor.Id);
        if (existingDoctor != null)
        {
            existingDoctor.Name = doctor.Name;
            existingDoctor.Email = doctor.Email;
            existingDoctor.PhoneNumber = doctor.PhoneNumber;
            existingDoctor.Specialty = doctor.Specialty;
            existingDoctor.Qualification = doctor.Qualification;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ClinicName = doctor.ClinicName;
            existingDoctor.Address = doctor.Address;
            existingDoctor.StartTime = doctor.StartTime;
            existingDoctor.EndTime = doctor.EndTime;
            existingDoctor.UpdatedAt = DateTime.UtcNow;

            _dbContext.Doctors.Update(existingDoctor);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<Doctor>> GetAllDoctors()
    {
        return await _dbContext.Doctors.ToListAsync();
    }
}
