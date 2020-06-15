using ApnaCHS.Common;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface ISocietyStaffService
    {
        Task<SocietyStaff> Create(SocietyStaff societyStaff);

        Task<SocietyStaff> Read(int key);

        Task Update(SocietyStaff societyStaff);

        Task UpdateLastWorkingDay(SocietyStaff societyStaff);

        Task Delete(int id);

        Task<List<SocietyStaff>> List(SocietyStaffSearchParams searchParams);

    }
    public class SocietyStaffService : ISocietyStaffService
    {
        ISocietyStaffRepository _societyStaffRepository = null;

        public SocietyStaffService()
        {
            _societyStaffRepository = new SocietyStaffRepository();
        }
        public Task<SocietyStaff> Create(SocietyStaff societyStaff)
        {
            return _societyStaffRepository.Create(societyStaff);
        }

        public Task Delete(int id)
        {
            return _societyStaffRepository.Delete(id);
        }

        public Task<List<SocietyStaff>> List(SocietyStaffSearchParams searchParams)
        {
            return _societyStaffRepository.List(searchParams);
        }

        public Task<SocietyStaff> Read(int key)
        {
            return _societyStaffRepository.Read(key);
        }

        public Task Update(SocietyStaff societyStaff)
        {
            return _societyStaffRepository.Update(societyStaff);
        }

        public Task UpdateLastWorkingDay(SocietyStaff societyStaff)
        {
            return _societyStaffRepository.UpdateLastWorkingDay(societyStaff);
        }
    }
}
