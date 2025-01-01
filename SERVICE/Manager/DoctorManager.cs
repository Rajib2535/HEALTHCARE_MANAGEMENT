using DATA.Interface;
using DATA.Models;
using SERVICE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Manager
{
    public class DoctorManager : IDocterManager
    {
        private readonly IDoctorRepository _doctorRepository;
        
        public DoctorManager(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        public async Task<int> CreateDoctor(Doctor doctor)
        {
            return await _doctorRepository.CreateDoctor(doctor);
        }

        public async Task<bool> DeleteDoctor(int id)
        {
            return await _doctorRepository.DeleteDoctor(id);
        }

        public async Task<List<Doctor>> GetAllDoctors()
        {
          return await _doctorRepository.GetAllDoctors();
        }

        public async Task<Doctor> GetDoctorById(int id)
        {
            return await _doctorRepository.GetDoctorById(id);
        }

        public async  Task<bool> UpdateDoctor(Doctor doctor)
        {
          return await _doctorRepository.UpdateDoctor(doctor); 
        }
    }
}
