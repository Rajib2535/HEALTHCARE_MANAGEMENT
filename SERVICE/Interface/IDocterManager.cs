using DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE.Interface
{
    public interface IDocterManager
    {
        Task<List<Doctor>> GetAllDoctors();
        Task<Doctor> GetDoctorById(int id);
        Task<int> CreateDoctor(Doctor doctor);
        Task<bool> UpdateDoctor(Doctor doctor);
        Task<bool> DeleteDoctor(int id);
    }
}
