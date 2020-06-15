using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;

namespace ApnaCHS.Services
{
    public class UserRoleService :IUserRoleService
    {
        private IUserRoleRepository _userRoleRepository;

        public UserRoleService()
        {
            _userRoleRepository = new UserRoleRepository();
        }

        public Task<List<ApplicationUserRole>> AddUserRoles(List<ApplicationUserRole> userRoles)
        {
            return _userRoleRepository.AddUserRoles(userRoles);
        }

        public Task<List<ApplicationUserRole>> ApplicationRoleListForDisplay(long userId)
        {
            return _userRoleRepository.ApplicationRoleListForDisplay(userId);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRoleService : IDisposable
    {
        Task<List<ApplicationUserRole>> ApplicationRoleListForDisplay(long userId);

        Task<List<ApplicationUserRole>> AddUserRoles(List<ApplicationUserRole> userRoles);
        
    }
}
