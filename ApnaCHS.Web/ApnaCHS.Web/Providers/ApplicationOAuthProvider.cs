using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ApnaCHS.Entities;
using ApnaCHS.Services;
using ApnaCHS.Web.Models;

namespace ApnaCHS.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<UserManager<IdentityUser>> _userManagerFactory;

        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<IdentityUser>> userManagerFactory)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            if (userManagerFactory == null)
            {
                throw new ArgumentNullException("userManagerFactory");
            }

            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //using (UserManager<IdentityUser> userManager = _userManagerFactory())
            using (UserService userManager = context.OwinContext.GetUserManager<UserService>())
            {
                //var form = await context.Request.ReadFormAsync();
                //var instanceId = Convert.ToInt32(form["instance"]);

                var user = userManager.FindByPhoneNumber(context.UserName);
                var username = user != null ? user.UserName : context.UserName;
                var existing = await userManager.FindAsync(username, context.Password);


                if (existing == null || existing.Deleted || existing.bBlocked)/// || user.InstanceId != instanceId)
                {
                    //await userManager.UpdateFailedLoginAttempt(context.UserName, context.Password);

                    if (existing != null && existing.bBlocked)
                        context.SetError("invalid_grant", "User login blocked.");
                    else
                        context.SetError("invalid_grant", "Invalid login details.");
                    return;
                }
                else
                {
                    await userManager.ResetLoginAttempt(existing.Id);
                    //await userManager.UpdateSuccessLoginAttempt(user, context.UserName, context.Password);
                }

                //var permission = await userManager.FindPermissionIdsByRole(user.Roles.Select(s => s.RoleId).ToList());

                //try
                //{
                    ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(existing,
                        context.Options.AuthenticationType);
                    ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(existing,
                        CookieAuthenticationDefaults.AuthenticationType);
                    AuthenticationProperties properties = CreateProperties(existing);
                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                //}
                //catch (Exception ex)
                //{
                //    if (ex != null)
                //    {

                //    }
                //}
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user)
        {
            var societies = new List<MapUserToSocietyViewModel>();
            foreach (var item in user.Societies)
            {
                societies.Add(Mapper.Map<MapUserToSocietyViewModel>(item));
            }

            var complexes = new List<MapUserToComplexViewModel>();
            foreach (var item in user.Complexes)
            {
                complexes.Add(Mapper.Map<MapUserToComplexViewModel>(item));
            }

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
                //{"instance", JsonConvert.SerializeObject( instance)},
                {"type", user.IsBack ? "1":"2" },
                {"userFullName", user.Name},
                {"societies",JsonConvert.SerializeObject(societies)},
                {"complexes",JsonConvert.SerializeObject(complexes)},
                {"roles",JsonConvert.SerializeObject(user.Roles.Where(r => r.RoleId != 1 || r.RoleId != 2).Select(r => r.RoleId).ToArray())}
            };
            return new AuthenticationProperties(data);
        }
    }
}