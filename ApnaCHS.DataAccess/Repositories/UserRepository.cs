using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using ApnaCHS.Entities;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using ApnaCHS.AppCommon;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IUserRepository : IUserStore<ApplicationUser, long>
    {
        ApplicationUser FindByUserName(string userName);

        ApplicationUser FindByPhoneNumber(string phoneNumber);

        Task<ApplicationUser> Read(long id);

        Task<List<ApplicationUser>> UserListForAdmin();

        Task<List<ApplicationUser>> UserListForSociety(long societyId);

        Task<List<Society>> SocietyListForUser(long userId);

        Task<List<ApplicationUser>> UserListForComplex(long complexId);

        Task<List<Complex>> ComplexListForUser(long userId);

        Task MakeAdmin(long userid, long societyId);

        Task RemoveAdmin(long userid, long societyId);

        Task<List<ApplicationUser>> AdminListForSociety(long societyId);

        Task<List<ApplicationUser>> MemberListForSociety(long societyId);

        Task<List<ApplicationUser>> TenantListForSociety(long societyId);

        Task Delete(ApplicationUser user);

        Task Update(long id, int? appSettingsId, string name, int maxAttempts, string whatsappNumber, string phoneNumber,
            int bExpired, int expireMinutes, bool bBlocked, bool bChangePass);

        Task Active(ApplicationUser user);

        Task UpdateFailedLoginAttempt(string username, string password);

        Task ResetLoginAttempt(long id);

        Task UpdateSuccessLoginAttempt(ApplicationUser user, string username, string password);

        Task<System.Collections.Generic.IList<Claim>> GetClaimsForUser(ApplicationUser user);

        Task MapUserToSociety(long userId, long societyId, long roleId);

        Task MapUserToComplex(long userId, long complexId, long roleId);

        Task<ApplicationUser> CreateComplexUser(ApplicationUser user, long complexId, ApplicationUser currentUser);

        Task<ApplicationUser> CreateSocietyUser(ApplicationUser user, long societyId, ApplicationUser currentUser);

        Task BlockSocietyUser(long userid, long societyId);
    }


    public class UserRepository : UserStore<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUserRepository
    {
        public UserRepository()
            : base(new DbContext())
        {
        }

        public ApplicationUser FindByUserName(string userName)
        {
            return (Context as DbContext)
                .Users
                .Include(u => u.Societies)
                .Include(u => u.Complexes)
                .SingleOrDefault(u => userName.Equals(u.UserName, StringComparison.InvariantCultureIgnoreCase));
        }

        public ApplicationUser FindByPhoneNumber(string phoneNumber)
        {
            return (Context as DbContext)
                .Users
                .Include(u => u.Societies)
                .Include(u => u.Complexes)
                .SingleOrDefault(u => phoneNumber.Equals(u.PhoneNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public Task<List<ApplicationUser>> UserListForAdmin()
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .Users
                        .Where(u => u.Id != 1 && !u.Deleted && u.IsBack)
                        .Include(r => r.Roles)
                        .OrderByDescending(r => !r.Deleted)
                        .ThenByDescending(r => r.ModifiedDate)
                        .ToList();
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> UserListForSociety(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join s in context.MapUserToSocieties on u.Id equals s.UserId
                                 where !u.Deleted && !u.IsBack && !u.IsDefault && s.SocietyId == societyId
                                 select u)
                                .Include(r => r.Roles)
                                .OrderByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> UserListForSocietyMembers(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join s in context.MapUserToSocieties on u.Id equals s.UserId
                                 where !u.Deleted && !u.IsBack && !u.IsDefault && s.SocietyId == societyId
                                 select u)
                                .Include(r => r.Roles)
                                .OrderByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<Society>> SocietyListForUser(long userId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from s in context.Societies
                                 join u in context.MapUserToSocieties on s.Id equals u.SocietyId
                                 where u.UserId == userId
                                 select s)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> UserListForComplex(long complexId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join c in context.MapUserToComplexes on u.Id equals c.UserId
                                 where !u.Deleted && !u.IsBack && !u.IsDefault && c.ComplexId == complexId
                                 select u)
                                .Include(r => r.Roles)
                                .OrderByDescending(r => !r.Deleted)
                                .ThenByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<Complex>> ComplexListForUser(long userId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from s in context.ComplexList
                                 join u in context.MapUserToComplexes on s.Id equals u.ComplexId
                                 where u.UserId == userId
                                 select s)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task MakeAdmin(long userId, long societyId)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var user = context
                        .Users
                        .Include(r => r.Roles)
                        .FirstOrDefault(u => u.Id == userId);

                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }

                    if (user.IsDefault || user.IsBack || user.Deleted)
                    {
                        throw new Exception("Cannot make admin.");
                    }

                    if (user.Roles.Any(r => r.RoleId == ProgramCommon.Admin || r.RoleId == ProgramCommon.SuperAdmin))
                    {
                        throw new Exception("User already an admin");
                    }

                    user.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.Admin });
                    context.SaveChanges();
                    await MapUserToSociety(userId, societyId, ProgramCommon.Admin);
                }
            });

            return taskResult;
        }

        public Task RemoveAdmin(long userid, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var user = context
                        .Users
                        .Include(r => r.Roles)
                        .FirstOrDefault(u => u.Id == userid);

                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }

                    if (user.IsDefault || user.IsBack || user.Deleted)
                    {
                        throw new Exception("Cannot revoke from admin group.");
                    }

                    if (!user.Roles.Any(r => r.RoleId == ProgramCommon.Admin))
                    {
                        throw new Exception("User not an admin");
                    }

                    var role = user.Roles.First(r => r.RoleId == ProgramCommon.Admin);
                    user.Roles.Remove(role);

                    var map = context.MapUserToSocieties.First(m => m.UserId == user.Id && m.RoleId == ProgramCommon.Admin && m.SocietyId == societyId);
                    context.MapUserToSocieties.Remove(map);

                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> AdminListForSociety(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join s in context.MapUserToSocieties on u.Id equals s.UserId
                                 where !u.Deleted
                                 && !u.IsBack
                                 && s.SocietyId == societyId
                                 && u.Roles.Any(r => r.RoleId == s.RoleId && (r.RoleId == ProgramCommon.SuperAdmin || r.RoleId == ProgramCommon.Admin))
                                 select u)
                                .Include(r => r.Roles)
                                .Include(r => r.Societies)
                                .Distinct()
                                .OrderByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> MemberListForSociety(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join s in context.MapUserToSocieties on u.Id equals s.UserId
                                 where !u.Deleted
                                 && !u.IsBack
                                 && s.SocietyId == societyId
                                 && u.Roles.Any(r => r.RoleId == s.RoleId && r.RoleId == ProgramCommon.MemberRoleId)
                                 && !u.Roles.Any(r => (r.RoleId == ProgramCommon.SuperAdmin || r.RoleId == ProgramCommon.Admin))
                                 select u)
                                .Include(r => r.Roles)
                                .Include(r => r.Societies)
                                .Distinct()
                                .OrderByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<List<ApplicationUser>> TenantListForSociety(long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var query = (from u in context.Users
                                 join s in context.MapUserToSocieties on u.Id equals s.UserId
                                 where !u.Deleted
                                 && !u.IsBack
                                 && s.SocietyId == societyId
                                 && u.Roles.Any(r => r.RoleId == s.RoleId && r.RoleId == ProgramCommon.TenantRoleId)
                                 select u)
                                .Include(r => r.Roles)
                                .Include(r => r.Societies)
                                .Distinct()
                                .OrderByDescending(r => r.ModifiedDate)
                                .ToList();

                    return query;
                }
            });

            return taskResult;
        }

        public Task<ApplicationUser> Read(long id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .Users
                        .Include(r => r.Roles)
                        .FirstOrDefault(u => u.Id == id);

                }
            });

            return taskResult;
        }

        public Task Delete(ApplicationUser user)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context.Users.FirstOrDefault(u => u.Id == user.Id);
                    if (existing == null) { throw new Exception("User not found"); }

                    existing.Deleted = true;
                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task Update(long id, int? appSettingsId, string name, int maxAttempts, string whatsappNumber, string phoneNumber,
            int bExpired, int expireMinutes, bool bBlocked, bool bChangePass)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context.Users.FirstOrDefault(u => u.Id == id);

                    existing.Name = name;
                    existing.MaxAttempts = maxAttempts;
                    existing.PhoneNumber = phoneNumber;
                    existing.bBlocked = bBlocked;
                    existing.bChangePass = bChangePass;

                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task Active(ApplicationUser user)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context.Users.FirstOrDefault(u => u.Id == user.Id);
                    if (existing == null) { throw new Exception("User not found"); }

                    existing.Deleted = false;
                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task UpdateFailedLoginAttempt(string username, string password)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Users
                        .FirstOrDefault(u => u.UserName == username);

                    if (existing != null && !existing.Deleted && !existing.bBlocked)
                    {
                        existing.MaxAttempts = existing.MaxAttempts - 1;

                        if (existing.MaxAttempts < 1)
                        {
                            existing.bBlocked = true;
                        }
                    }

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task ResetLoginAttempt(long id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Users
                        .FirstOrDefault(u => u.Id == id);

                    //TODO
                    //MaxAttempts = 5, Put this in some kind of config
                    var sysPro = 5;
                    existing.MaxAttempts = sysPro;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateSuccessLoginAttempt(ApplicationUser user, string username, string password)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Users
                        .FirstOrDefault(u => u.Id == user.Id);

                    //TODO
                    //MaxAttempts = 5, Put this in some kind of config
                    var sysPro = 5;
                    existing.MaxAttempts = sysPro;
                    context.SaveChanges();

                }
            });
            return taskResult;
        }

        public Task<System.Collections.Generic.IList<Claim>> GetClaimsForUser(ApplicationUser user)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    return await this.GetClaimsAsync(user);
                }
            });
            return taskResult;
        }

        public Task MapUserToSociety(long userId, long societyId, long roleId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.MapUserToSocieties.Any(m => m.UserId == userId && m.SocietyId == societyId && m.RoleId == roleId))
                    {
                        return;
                    }

                    var map = new MapUserToSociety() { UserId = userId, SocietyId = societyId, RoleId = roleId };
                    context.MapUserToSocieties.Add(map);
                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task MapUserToComplex(long userId, long complexId, long roleId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var map = new MapUserToComplex() { UserId = userId, ComplexId = complexId, RoleId = roleId };
                    context.MapUserToComplexes.Add(map);
                    context.SaveChanges();
                }
            });

            return taskResult;
        }

        public Task<ApplicationUser> CreateComplexUser(ApplicationUser user, long complexId, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //check if user exist with username before
                    var existing = context
                        .Users
                        .FirstOrDefault(u => u.PhoneNumber == user.PhoneNumber);

                    if (existing != null)
                    {
                        //Map user to super admin role for the complex. Super Admin = 3
                        var roleexist = existing.Roles.FirstOrDefault(r => r.RoleId == ProgramCommon.SuperAdmin);
                        if (roleexist == null)
                        {
                            existing.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.SuperAdmin });
                        }

                        var map = new MapUserToComplex() { UserId = existing.Id, ComplexId = complexId, RoleId = ProgramCommon.SuperAdmin };
                        context.MapUserToComplexes.Add(map);
                        context.SaveChanges();

                        return existing;
                    }
                    else
                    {
                        user.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.Frontend });
                        //Map user to super admin role for the complex. Super Admin = 3
                        user.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.SuperAdmin });

                        var passwordHash = new PasswordHasher();
                        user.PasswordHash = passwordHash.HashPassword(user.PhoneNumber);
                        user.SecurityStamp = Guid.NewGuid().ToString();

                        await base.CreateAsync(user);
                        //await base.AddToRoleAsync(user, "Frontend");

                        var map = new MapUserToComplex() { UserId = user.Id, ComplexId = complexId, RoleId = ProgramCommon.SuperAdmin };
                        context.MapUserToComplexes.Add(map);
                        context.SaveChanges();

                        return user;
                    }
                }
            });
            return taskResult;
        }

        public Task<ApplicationUser> CreateSocietyUser(ApplicationUser user, long societyId, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //check if user exist with username before
                    var existing = context
                        .Users
                        .FirstOrDefault(u => u.UserName == user.UserName);

                    if (existing != null)
                    {
                        //Map user to super admin role for the complex. Super Admin = 3
                        var roleexist = existing.Roles.FirstOrDefault(r => r.RoleId == ProgramCommon.SuperAdmin);
                        if (roleexist == null)
                        {
                            existing.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.SuperAdmin });
                        }

                        var map = new MapUserToSociety() { UserId = existing.Id, SocietyId = societyId, RoleId = ProgramCommon.SuperAdmin };
                        context.MapUserToSocieties.Add(map);
                        context.SaveChanges();

                        return existing;
                    }
                    else
                    {
                        user.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.Frontend });
                        //Map user to super admin role for the complex. Super Admin = 3
                        user.Roles.Add(new ApplicationUserRole() { RoleId = ProgramCommon.SuperAdmin });

                        var passwordHash = new PasswordHasher();
                        user.PasswordHash = passwordHash.HashPassword(user.PhoneNumber);
                        user.SecurityStamp = Guid.NewGuid().ToString();

                        await base.CreateAsync(user);

                        var map = new MapUserToSociety() { UserId = user.Id, SocietyId = societyId, RoleId = ProgramCommon.SuperAdmin };
                        context.MapUserToSocieties.Add(map);
                        context.SaveChanges();

                        return user;
                    }
                }
            });
            return taskResult;
        }

        public Task BlockSocietyUser(long userid, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .MapUserToSocieties
                        .FirstOrDefault(m => m.UserId == userid && m.SocietyId == societyId);

                    if (existing == null)
                    {
                        throw new Exception("User not found");
                    }

                    existing.IsBlocked = true;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }
    }
}
