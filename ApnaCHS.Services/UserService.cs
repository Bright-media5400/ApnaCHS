using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using ApnaCHS.Entities;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.AppCommon;

namespace ApnaCHS.Services
{
    public class UserService : UserManager<ApplicationUser, long>, IUserService
    {
        private IUserRepository _userRepository;
        private IApplicationRoleService _roleService;

        public UserService()
            : base(new UserRepository())
        {
            _userRepository = new UserRepository();
            _roleService = new ApplicationRoleService();
        }

        public ApplicationUser FindByUserName(string userName)
        {
            return (Store as UserRepository).FindByUserName(userName);
        }

        public ApplicationUser FindByPhoneNumber(string phoneNumber)
        {
            return (Store as UserRepository).FindByPhoneNumber(phoneNumber);
        }

        public Task<ApplicationUser> Read(long id)
        {
            return (Store as UserRepository).Read(id);
        }

        public Task<List<ApplicationUser>> UserListForAdmin()
        {
            return _userRepository.UserListForAdmin();
        }

        public Task<List<ApplicationUser>> UserListForSociety(long societyId)
        {
            return _userRepository.UserListForSociety(societyId);
        }

        public Task<List<Society>> SocietyListForUser(long userId)
        {
            return _userRepository.SocietyListForUser(userId);
        }

        public Task<List<ApplicationUser>> UserListForComplex(long complexId)
        {
            return _userRepository.UserListForComplex(complexId);
        }

        public Task<List<Complex>> ComplexListForUser(long userId)
        {
            return _userRepository.ComplexListForUser(userId);
        }

        public Task MakeAdmin(long userid, long societyId)
        {
            return _userRepository.MakeAdmin(userid, societyId);
        }

        public Task RemoveAdmin(long userid, long societyId)
        {
            return _userRepository.RemoveAdmin(userid, societyId);
        }

        public Task<List<ApplicationUser>> AdminListForSociety(long societyId)
        {
            return _userRepository.AdminListForSociety(societyId);
        }

        public Task<List<ApplicationUser>> MemberListForSociety(long societyId)
        {
            return _userRepository.MemberListForSociety(societyId);
        }

        public Task<List<ApplicationUser>> TenantListForSociety(long societyId)
        {
            return _userRepository.TenantListForSociety(societyId);
        }

        public async Task<IdentityResult> CreateLogin(string userName, string email, string password, ApplicationRole[] roles, int? appSettingsId, string name,
            string phoneNumber, bool bBlocked, bool bChangePass, bool isback, long? societyId, long? complexId, ApplicationUser currentUser)
        {
            if (!currentUser.IsBack) //front user
            {

                if (isback) //back user not allowed to add by current front user
                {
                    throw new Exception("Cannot create user");
                }
                else
                {
                    if (!societyId.HasValue && !complexId.HasValue) { throw new Exception("Society or complex reference required"); }

                    if (societyId.HasValue)
                    {
                        var socs = await SocietyListForUser(currentUser.Id);

                        if (!socs.Any(s => s.Id == societyId.Value))
                        {
                            throw new Exception("Cannot create user");
                        }
                    }

                    if (complexId.HasValue)
                    {
                        var complexes = await ComplexListForUser(currentUser.Id);

                        if (!complexes.Any(s => s.Id == complexId.Value))
                        {
                            throw new Exception("Cannot create user");
                        }
                    }
                }
            }
            else //back user
            {
                if (!isback) //front user
                {
                    //front user
                    if (!societyId.HasValue && !complexId.HasValue) { throw new Exception("Society or complex reference required"); }
                }
                else
                {
                    //back user
                }
            }

            var usr = _userRepository.FindByPhoneNumber(phoneNumber);
            if (usr != null)
            {
                //Member Role
                if (roles.Any(r => r.Id == ProgramCommon.MemberRoleId))
                {
                    await AddToRolesAsync(usr.Id, "Member");
                    await _userRepository.MapUserToSociety(usr.Id, societyId.Value, ProgramCommon.MemberRoleId);
                }
                else
                {
                    if (societyId.HasValue && usr.Societies.Any(s => s.UserId == usr.Id && s.SocietyId == societyId.Value))
                    {
                        throw new Exception("User already registered with the society");
                    }

                    if (complexId.HasValue && usr.Complexes.Any(s => s.UserId == usr.Id && s.ComplexId == complexId.Value))
                    {
                        throw new Exception("User already registered with the complex");
                    }
                }

                return null;
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                Name = name,
                MaxAttempts = ProgramCommon.MaxAttempts,
                bBlocked = bBlocked,
                bChangePass = bChangePass,
                Deleted = false,
                CreatedBy = currentUser.UserName,
                ModifiedBy = currentUser.UserName,
                PhoneNumber = phoneNumber,
                IsBack = isback,
                IsDefault = false
            };

            IdentityResult result = await CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (isback)
                {
                    await AddToRolesAsync(user.Id, "Backend");
                }
                else
                {
                    await AddToRolesAsync(user.Id, "Frontend");
                }

                await AddToRolesAsync(user.Id, roles.Select(r => r.Name).ToArray());

                if (societyId.HasValue)
                {
                    foreach (var role in roles)
                    {
                        var r = await _roleService.GetApplicationRole(role.Name);
                        await _userRepository.MapUserToSociety(user.Id, societyId.Value, r.Id);
                    }
                }


                if (complexId.HasValue)
                {
                    foreach (var role in roles)
                    {
                        var r = await _roleService.GetApplicationRole(role.Name);
                        await _userRepository.MapUserToComplex(user.Id, complexId.Value, r.Id);
                    }
                }
            }

            return result;
        }

        public Task RegisterFlatOwner(FlatOwner flatowner, long? societyId, long? complexId, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                //Role Member = 5
                var memberRole = new ApplicationRole() { Id = ProgramCommon.MemberRoleId, Name = "Member" };
                var roles = new ApplicationRole[] { memberRole };

                IdentityResult result = await CreateLogin(ProgramCommon.GetFormattedUsername(flatowner.Name), flatowner.EmailId, flatowner.MobileNo, roles
                    , null, flatowner.Name, flatowner.MobileNo, bBlocked: false, bChangePass: true, isback: false, societyId: societyId, complexId: complexId, currentUser: currentUser);
            });
        }

        public Task RegisterFlatOwnerFamily(FlatOwnerFamily family, long? societyId, long? complexId, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                //Role Member Family = 8
                var familyRole = new ApplicationRole() { Id = ProgramCommon.MemberFamily, Name = "Owner Family" };
                var roles = new ApplicationRole[] { familyRole };

                IdentityResult result = await CreateLogin(ProgramCommon.GetFormattedUsername(family.Name), null, family.MobileNo, roles
                    , null, family.Name, family.MobileNo, bBlocked: false, bChangePass: true, isback: false, societyId: societyId, 
                    complexId: complexId, currentUser: currentUser);
            });
        }

        public Task RegisterTenant(FlatOwner flatowner, long? societyId, long? complexId, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                //Role Tenant = 7
                var memberRole = new ApplicationRole() { Id = ProgramCommon.TenantRoleId, Name = "Tenant" };
                var roles = new ApplicationRole[] { memberRole };

                IdentityResult result = await CreateLogin(ProgramCommon.GetFormattedUsername(flatowner.Name), flatowner.EmailId, flatowner.MobileNo, roles
                    , null, flatowner.Name, flatowner.MobileNo, bBlocked: false, bChangePass: true, isback: false, societyId: societyId, complexId: complexId, currentUser: currentUser);
            });
        }

        public Task RegisterTenantFamily(FlatOwnerFamily family, long? societyId, long? complexId, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                //Role Tenant Family = 9
                var familyRole = new ApplicationRole() { Id = ProgramCommon.TenantFamily, Name = "Tenant Family" };
                var roles = new ApplicationRole[] { familyRole };

                IdentityResult result = await CreateLogin(ProgramCommon.GetFormattedUsername(family.Name), null, family.MobileNo, roles
                    , null, family.Name, family.MobileNo, bBlocked: false, bChangePass: true, isback: false, societyId: societyId, 
                    complexId: complexId, currentUser: currentUser);
            });
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await base.CreateAsync(user, password);
        }

        public override Task<IdentityResult> AddToRolesAsync(long userId, params string[] roles)
        {
            return base.AddToRolesAsync(userId, roles);
        }

        public async Task Update(long userId, string email, ApplicationRole[] roles, int? appSettingsId, string name, int maxAttempts, string whatsappNumber, string phoneNumber,
            int bExpired, int expireMinutes, bool bBlocked, bool bChangePass, ApplicationUser currentUser)
        {
            var user = await FindByIdAsync(userId);
            if (user == null) { throw new Exception("User not found"); }

            await base.SetEmailAsync(userId, email);

            var rolesForUser = await _roleService.GetRolesForUsers(user.Id);
            await base.RemoveFromRolesAsync(userId, rolesForUser.Select(r => r.Name).ToArray());
            await base.AddToRolesAsync(userId, roles.Select(r => r.Name).ToArray());

            await _userRepository.Update(user.Id, appSettingsId, name, maxAttempts, whatsappNumber, phoneNumber,
                bExpired, expireMinutes, bBlocked, bChangePass);
        }

        public async Task Delete(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null) { throw new Exception("User not found"); }

            await base.RemovePasswordAsync(user.Id);
            await _userRepository.Delete(user);
        }

        public async Task Active(long userId, string password)
        {
            var user = await FindByIdAsync(userId);
            if (user == null) { throw new Exception("User not found"); }

            await base.AddPasswordAsync(userId, password);
            await _userRepository.Active(user);
        }

        public Task UpdateFailedLoginAttempt(string username, string password)
        {
            return _userRepository.UpdateFailedLoginAttempt(username, password);
        }

        public Task ResetLoginAttempt(long id)
        {
            return _userRepository.ResetLoginAttempt(id);
        }

        public Task UpdateSuccessLoginAttempt(ApplicationUser user, string username, string password)
        {
            return _userRepository.UpdateSuccessLoginAttempt(user, username, password);
        }

        public Task<IdentityResult> NewSocietyUser(ApplicationUser user, ApplicationRole[] roles, long? societyId, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                IdentityResult result = await CreateLogin(ProgramCommon.GetFormattedUsername(user.Name), user.Email, user.PhoneNumber, roles
                    , null, user.Name, user.PhoneNumber, bBlocked: false, bChangePass: true, isback: false, societyId: societyId, complexId: null, currentUser: currentUser);

                return result;
            });
        }

        public Task BlockSocietyUser(long userid, long societyId)
        {
            return _userRepository.BlockSocietyUser(userid, societyId);
        }
    }

    public interface IUserService : IDisposable
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

        Task<IdentityResult> CreateLogin(string userName, string email, string password, ApplicationRole[] roles, int? appSettingsId, string name,
            string phoneNumber, bool bBlocked, bool bChangePass, bool isback, long? societyId, long? complexId, ApplicationUser currentUser);

        Task RegisterFlatOwner(FlatOwner flatowner, long? societyId, long? complexId, ApplicationUser currentUser);

        Task RegisterFlatOwnerFamily(FlatOwnerFamily family, long? societyId, long? complexId, ApplicationUser currentUser);

        Task RegisterTenant(FlatOwner flatowner, long? societyId, long? complexId, ApplicationUser currentUser);

        Task RegisterTenantFamily(FlatOwnerFamily family, long? societyId, long? complexId, ApplicationUser currentUser);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task Update(long userId, string email, ApplicationRole[] roles, int? appSettingsId, string name, int maxAttempts, string whatsappNumber, string phoneNumber,
            int bExpired, int expireMinutes, bool bBlocked, bool bChangePass, ApplicationUser currentUser);

        Task Delete(long userId);

        Task Active(long userId, string password);

        Task UpdateFailedLoginAttempt(string username, string password);

        Task ResetLoginAttempt(long id);

        Task UpdateSuccessLoginAttempt(ApplicationUser user, string username, string password);

        Task<IdentityResult> NewSocietyUser(ApplicationUser user, ApplicationRole[] roles, long? societyId, ApplicationUser currentUser);

        Task BlockSocietyUser(long userid, long societyId);
    }
}
