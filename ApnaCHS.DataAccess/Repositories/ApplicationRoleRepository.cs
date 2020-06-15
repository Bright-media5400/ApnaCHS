using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.Entities;
using System.Security.Claims;
using ApnaCHS.AppCommon;
using ApnaCHS.Common;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IApplicationRoleRepository
    {
        Task<List<ApplicationRole>> AdminRoles();

        Task<List<ApplicationRole>> Groups(ApplicationRoleSearchParams searchParams);

        Task<ApplicationRole> GetApplicationRole(long id);

        Task<ApplicationRole> GetApplicationRole(string name);

        Task<ApplicationRole> NewApplicationRole(ApplicationRole role);

        Task<ApplicationRole> UpdateApplicationRole(ApplicationRole role);

        Task<List<ApplicationRole>> GetRolesForUsers(long userId);

        Task Delete(long id);
    }

    public class ApplicationRoleRepository : IApplicationRoleRepository
    {
        IUserRepository userRepository = null;

        public ApplicationRoleRepository()
        {
            userRepository = new UserRepository();
        }

        public Task<ApplicationRole> GetApplicationRole(long id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (id == 1 || id == 2)
                    {
                        throw new Exception("Role not found");
                    }

                    var role = context
                        .Roles
                        .Include(s => s.Users)
                        .FirstOrDefault(p => p.Id == id);

                    if (role != null)
                    {
                        role.Name = role.DisplayName;
                    }

                    return role;
                }
            });

            return taskResult;
        }

        public Task<ApplicationRole> NewApplicationRole(ApplicationRole role)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    //PATCH CODE
                    //-------------------------------------------------------------/
                    role.SocietyId = null;
                    //-------------------------------------------------------------/
                    role.IsDefault = false;
                    role.CanChange = true;

                    role.DisplayName = role.Name;

                    if (role.Society != null)
                    {
                        role.SocietyId = role.Society.Id;
                        role.Society = null;

                        //role.Name = string.Format(ProgramCommon.SocietyRoleName, role.Name, role.SocietyId);
                    }

                    if (role.Complex != null)
                    {
                        role.ComplexId = role.Complex.Id;
                        role.Complex = null;

                        //role.Name = string.Format(ProgramCommon.ComplexRoleName, role.Name, role.ComplexId);
                    }

                    context.Roles.Add(role);
                    context.SaveChanges();
                    return role;
                }
            });
            return taskResult;
        }

        public Task<List<ApplicationRole>> AdminRoles()
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .Roles
                        //.Include(s => s.Users)
                        .Where(r => r.Id != 1 && r.Id != 2)
                        .ToList();
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationRole>> Groups(ApplicationRoleSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .Roles
                        .Where(r => !r.IsBack.HasValue);

                    return ctx.ToList();
                }
            });

            return taskResult;
        }

        public Task<ApplicationRole> UpdateApplicationRole(ApplicationRole role)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var currentUser = await BTAuthorization.HasEditOrOwn(new BTAuthorizationAction(EnActionList.EditOwn, EnModuleList.Roles),
                        new BTAuthorizationAction(EnActionList.EditAll, EnModuleList.Roles));

                    var entity = context.Roles.FirstOrDefault(s => s.Id == role.Id);
                    if (entity == null)
                    {
                        throw new Exception("Role not found");
                    }

                    if (!entity.CanChange)
                    {
                        throw new Exception("Cannot update role");
                    }                    

                    entity.DisplayName = role.Name;
                    

                    if (entity.Society != null)
                    {
                        //entity.Name = string.Format(ProgramCommon.SocietyRoleName, role.Name, entity.SocietyId);
                    }

                    if (entity.Complex != null)
                    {
                        //entity.Name = string.Format(ProgramCommon.ComplexRoleName, role.Name, entity.ComplexId);
                    }

                    entity.Name = role.Name;
                    entity.ModifiedBy = currentUser.UserName;
                    entity.ModifiedDate = DateTime.Now;

                    context.SaveChanges();

                    return role;
                }
            });
            return taskResult;
        }

        public Task<ApplicationRole> GetApplicationRole(string name)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .Roles
                        .Include(s => s.Users)
                        .Where(r => r.Id != 1 && r.Id != 2)
                        .FirstOrDefault(p => p.Name == name);
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationRole>> GetRolesForUsers(long userId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    List<ApplicationRole> roles = new List<ApplicationRole>();

                    var r = context
                        .Users
                        .FirstOrDefault(s => s.Id == userId)
                        .Roles;

                    if (r.Any())
                    {
                        foreach (var rl in r)
                        {
                            var rn = context
                                .Roles
                                .Where(role => role.Id != 1 && role.Id != 2)
                                .FirstOrDefault(x => x.Id == rl.RoleId);
                            if (rn != null)
                            {
                                roles.Add(rn);
                            }
                        }
                    }
                    return roles;
                }
            });

            return taskResult;
        }

        public Task Delete(long id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (id == 1 || id == 2)
                    {
                        throw new Exception("Role not found");
                    }

                    if (context.Users.SelectMany(u => u.Roles).Any(r => r.RoleId == id))
                    {
                        throw new Exception("Role assigned to user(s). Cannot delete.");
                    }

                    var role = context.Roles.FirstOrDefault(r => r.Id == id);
                    if (role == null)
                    {
                        throw new Exception("Role not found.");
                    }

                    if (!role.CanChange)
                    {
                        throw new Exception("Cannot delete role");
                    }                    

                    context.Roles.Remove(role);
                    context.SaveChanges();
                }
            });

            return taskResult;
        }

    }
}
