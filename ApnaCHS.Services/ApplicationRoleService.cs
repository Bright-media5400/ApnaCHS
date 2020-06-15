using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using ApnaCHS.Common;

namespace ApnaCHS.Services
{
    public class ApplicationRoleService : IApplicationRoleService
    {

        private IApplicationRoleRepository _applicationRoleRepository;

        public ApplicationRoleService()
        {
            _applicationRoleRepository = new ApplicationRoleRepository();
        }

        public Task<ApplicationRole> GetApplicationRole(long id)
        {
            return _applicationRoleRepository.GetApplicationRole(id);
        }

        public Task<ApplicationRole> NewApplicationRole(ApplicationRole role)
        {
            return _applicationRoleRepository.NewApplicationRole(role);
        }

        public Task<ApplicationRole> UpdateApplicationRole(ApplicationRole role)
        {
            return _applicationRoleRepository.UpdateApplicationRole(role);
        }

        public Task<List<ApplicationRole>> AdminRoles()
        {
            return _applicationRoleRepository.AdminRoles();
        }


        public Task<List<ApplicationRole>> Groups(ApplicationRoleSearchParams searchParams)
        {
            return _applicationRoleRepository.Groups(searchParams);
        }

        public Task<List<UserRole>> UserRoleMappingListForDisplay()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationRole> GetApplicationRole(string name)
        {
            return _applicationRoleRepository.GetApplicationRole(name);
        }

        public Task<List<ApplicationRole>> GetRolesForUsers(long userId)
        {
            return _applicationRoleRepository.GetRolesForUsers(userId);
        }

        public Task Delete(long id)
        {
            return _applicationRoleRepository.Delete(id);
        }
    }

    public interface IApplicationRoleService : IDisposable
    {
        Task<List<ApplicationRole>> AdminRoles();

        Task<List<ApplicationRole>> Groups(ApplicationRoleSearchParams searchParams);

        Task<ApplicationRole> GetApplicationRole(long id);

        Task<ApplicationRole> NewApplicationRole(ApplicationRole role);

        Task<ApplicationRole> UpdateApplicationRole(ApplicationRole role);

        Task<List<UserRole>> UserRoleMappingListForDisplay();

        Task<ApplicationRole> GetApplicationRole(string name);

        Task<List<ApplicationRole>> GetRolesForUsers(long userId);

        Task Delete(long id);
    }
}
