using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using AutoMapper;
using ApnaCHS.Web.Models;
using ApnaCHS.Services;
using ApnaCHS.Entities;
using ApnaCHS.Web.Results;

namespace ApnaCHS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";
        protected readonly IApplicationRoleService _roleService = null;

        public AccountController()
            : base(new UserService())
        {
            _roleService = new ApplicationRoleService();
        }

        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity oAuthIdentity = await UserManager.CreateIdentityAsync(user,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await UserManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                throw new NotImplementedException();
                //AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                //Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityUser user = new IdentityUser
            {
                UserName = model.UserName
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            IdentityUser user = new IdentityUser
            {
                UserName = model.UserName
            };
            user.Logins.Add(new IdentityUserLogin
            {
                LoginProvider = externalLogin.LoginProvider,
                ProviderKey = externalLogin.ProviderKey
            });
            IdentityResult result = await UserManager.CreateAsync(user);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpPost]
        [Route("UserListForAdmin")]
        public async Task<IHttpActionResult> UserListForAdmin()
        {
            var users = await _userService.UserListForAdmin();
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UserListForSociety")]
        public async Task<IHttpActionResult> UserListForSociety([FromBody] long id)
        {
            var users = await _userService.UserListForSociety(id);
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("UserListForComplex")]
        public async Task<IHttpActionResult> UserListForComplex([FromBody] long id)
        {
            var users = await _userService.UserListForComplex(id);
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("MakeAdmin")]
        public async Task<IHttpActionResult> MakeAdmin([FromBody]dynamic inparams)
        {
            long userid = inparams.userId;
            long societyId = inparams.societyId;

            await _userService.MakeAdmin(userid, societyId);
            return Ok();
        }

        [HttpPost]
        [Route("RemoveAdmin")]
        public async Task<IHttpActionResult> RemoveAdmin([FromBody]dynamic inparams)
        {
            long userid = inparams.userId;
            long societyId = inparams.societyId;

            await _userService.RemoveAdmin(userid, societyId);
            return Ok();
        }

        [HttpPost]
        [Route("AdminListForSociety")]
        public async Task<IHttpActionResult> AdminListForSociety([FromBody] long id)
        {
            var users = await _userService.AdminListForSociety(id);
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                viewModel.bBlocked = user.Societies.First(u => u.SocietyId == id).IsBlocked;

                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("MemberListForSociety")]
        public async Task<IHttpActionResult> MemberListForSociety([FromBody] long id)
        {
            var users = await _userService.MemberListForSociety(id);
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                viewModel.bBlocked = user.Societies.First(u => u.SocietyId == id).IsBlocked;

                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("TenantListForSociety")]
        public async Task<IHttpActionResult> TenantListForSociety([FromBody] long id)
        {
            var users = await _userService.TenantListForSociety(id);
            var viewmodels = new List<ApplicationUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _roleService.GetRolesForUsers(user.Id);
                var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
                viewModel.bBlocked = user.Societies.First(u => u.SocietyId == id).IsBlocked;

                var roleids = user.Roles.Select(r => r.RoleId).ToArray();
                var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
                var rolesViewModel = new List<ApplicationRoleViewModel>();

                foreach (var role in userroles)
                {
                    rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
                }

                viewModel.userroles = rolesViewModel.ToArray();
                viewmodels.Add(viewModel);
            }

            return Ok(viewmodels);
        }

        [HttpPost]
        [Route("Get")]
        public async Task<IHttpActionResult> Get([FromBody] long id)
        {
            var user = await _userService.Read(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _roleService.GetRolesForUsers(user.Id);
            var viewModel = Mapper.Map<ApplicationUserViewModel>(user);
            var roleids = user.Roles.Select(r => r.RoleId).ToArray();
            var userroles = roles.Where(r => roleids.Contains(r.Id)).ToArray();
            var rolesViewModel = new List<ApplicationRoleViewModel>();

            foreach (var role in userroles)
            {
                rolesViewModel.Add(Mapper.Map<ApplicationRoleViewModel>(role));
            }

            viewModel.userroles = rolesViewModel.ToArray();

            return Ok(viewModel);
        }


        [HttpPost]
        [Route("New")]
        [Authorize(Roles = "Backend")]
        public async Task<IHttpActionResult> New([FromBody]RegisterUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors[0].ErrorMessage);
            }

            var roles = new List<ApplicationRole>();
            foreach (var item in viewModel.userroles)
            {
                roles.Add(Mapper.Map<ApplicationRole>(item));
            }

            IdentityResult result = await _userService.CreateLogin(viewModel.userName, viewModel.email, viewModel.password, roles.ToArray()
                , viewModel.appSettingsId, viewModel.name, viewModel.phoneNumber, viewModel.bBlocked, viewModel.bChangePass, viewModel.isback,
                viewModel.societyId, viewModel.complexId, CurrentUser);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            var user = _userService.FindByUserName(viewModel.userName);
            return Ok(Mapper.Map<ApplicationUserViewModel>(user));
        }

        [HttpPost]
        [Route("Update")]
        [Authorize(Roles = "Backend")]
        public async Task<IHttpActionResult> Update([FromBody]RegisterUpdateUserViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.First().Value.Errors[0].ErrorMessage);
            }

            var roles = new List<ApplicationRole>();
            foreach (var item in viewModel.userroles)
            {
                roles.Add(Mapper.Map<ApplicationRole>(item));
            }

            await _userService.Update(viewModel.id, viewModel.email, roles.ToArray(), viewModel.appSettingsId, viewModel.name, viewModel.maxAttempts,
                viewModel.whatsappNumber, viewModel.phoneNumber, viewModel.bExpired, viewModel.expireMinutes, viewModel.bBlocked, viewModel.bChangePass, CurrentUser);

            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete([FromBody]long id)
        {
            await _userService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("Active")]
        public async Task<IHttpActionResult> Active([FromBody]dynamic inParams)
        {
            int id = inParams.id;
            string password = inParams.password;

            await _userService.Active(id, password);
            return Ok();
        }

        [HttpPost]
        [Route("FindByUserName")]
        public IHttpActionResult FindByUserName([FromBody] string userName)
        {
            var user = _userService.FindByUserName(userName);

            return Ok(Mapper.Map<ApplicationUserViewModel>(user));

        }

        [Route("NewSocietyUser")]
        [Authorize(Roles = "Backend,Frontend,Super Admin,Admin")]
        public async Task<IHttpActionResult> NewSocietyUser([FromBody]RegisterUserViewModel viewmodel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.First().Value.Errors[0].ErrorMessage);
            //}

            var roles = new List<ApplicationRole>();
            foreach (var item in viewmodel.userroles)
            {
                roles.Add(Mapper.Map<ApplicationRole>(item));
            }
            viewmodel.userroles = null;

            var user = new ApplicationUser()
            {
                PhoneNumber = viewmodel.phoneNumber,
                Email = viewmodel.email,
                Name = viewmodel.name
            };

            IdentityResult result = await _userService.NewSocietyUser(user, roles.ToArray(), viewmodel.societyId, CurrentUser);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            var response = _userService.FindByPhoneNumber(viewmodel.phoneNumber);
            return Ok(Mapper.Map<ApplicationUserViewModel>(response));
        }

        [Route("BlockSocietyUser")]
        public async Task<IHttpActionResult> BlockSocietyUser([FromBody]dynamic viewmodel)
        {

            long userid = viewmodel.userid;
            long societyId = viewmodel.societyId;

            await _userService.BlockSocietyUser(userid, societyId);
            return Ok();
        }
            
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
