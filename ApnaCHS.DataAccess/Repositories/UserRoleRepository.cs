using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.DataAccess;
using ApnaCHS.Entities;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IUserRoleRepository
    {
        Task<List<ApplicationUserRole>> ApplicationRoleListForDisplay(long userId);

        Task<List<ApplicationUserRole>> AddUserRoles(List<ApplicationUserRole> userRoles);        
    }

    public class UserRoleRepository : IUserRoleRepository
    {       
        public Task<List<ApplicationUserRole>> AddUserRoles(List<ApplicationUserRole> userRoles)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (userRoles.Any())
                    {
                        var userId = userRoles.FirstOrDefault().UserId;
                        var previousRoles = context.Users.FirstOrDefault(s => s.Id == userId).Roles.ToList();
                        foreach (var role in previousRoles)
                        {
                            if (userRoles.FirstOrDefault(s => s.RoleId == role.RoleId) == null)
                            {
                                context.Users.FirstOrDefault(s => s.Id == userId).Roles.Remove(role);
                            }
                        }
                        foreach (var role in userRoles)
                        {
                            if (previousRoles.FirstOrDefault(s => s.RoleId == role.RoleId) == null)
                            {
                                context.Users.FirstOrDefault(s => s.Id == userId).Roles.Add(role);
                            }
                        }
                        context.SaveChanges();
                    }
                    return userRoles;
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUserRole>> ApplicationRoleListForDisplay(long userId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    return context
                        .Users
                        .FirstOrDefault(s=> s.Id == userId)
                        .Roles
                        .ToList();
                }
            });

            return taskResult;
        }
    }
}
