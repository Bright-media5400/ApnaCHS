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
    public interface ISecurityStaffService
    {
        Task<SecurityStaff> Create(SecurityStaff securityStaff);

        Task<SecurityStaff> Read(int key);

        Task Update(SecurityStaff securityStaff);

        Task UpdateLastWorkingDay(SecurityStaff securityStaff);

        Task Delete(int id);

        Task<List<SecurityStaff>> List(SecurityStaffSearchParams searchParams);

    }
    public class SecurityStaffService : ISecurityStaffService
    {
        ISecurityStaffRepository _securityStaffRepository = null;

        public SecurityStaffService()
        {
            _securityStaffRepository = new SecurityStaffRepository();
        }
        public Task<SecurityStaff> Create(SecurityStaff securityStaff)
        {
            return _securityStaffRepository.Create(securityStaff);
        }
        public Task Delete(int id)
        {
            return _securityStaffRepository.Delete(id);
        }

        public Task<List<SecurityStaff>> List(SecurityStaffSearchParams searchParams)
        {
            return _securityStaffRepository.List(searchParams);
        }

        public Task<SecurityStaff> Read(int key)
        {
            return _securityStaffRepository.Read(key);
        }

        public Task Update(SecurityStaff securityStaff)
        {
            return _securityStaffRepository.Update(securityStaff);
        }

        public Task UpdateLastWorkingDay(SecurityStaff securityStaff)
        {
            return _securityStaffRepository.UpdateLastWorkingDay(securityStaff);
        }
    }
}
