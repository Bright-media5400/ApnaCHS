using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess
{
    public class BTAuthorization
    {
        public static Task<ApplicationUser> HasPermissionAysnc(params BTAuthorizationAction[] actions)
        {
            var taskResult = Task.Run(async () =>
            {
                IUserRepository userRepository = new UserRepository();
                var currentUser = userRepository.FindByUserName(Thread.CurrentPrincipal.Identity.Name);
                if (!currentUser.IsBack)
                {
                    var society = await GetLoggedSociety(userRepository, currentUser);
                    //currentUser.Societies = new Society[] { society };
                    throw new Exception("TODO");
                }
                
                //var currentPermissions = userRepository.FindPermissionByRole(currentUser.Roles.Select(s => s.RoleId).ToList());

                //var found = false;
                //foreach (var item in actions)
                //{
                //    if (currentPermissions.Any(s => s.Key == (int)item.module && s.Value == item.action))
                //    {
                //        found = true;
                //    }
                //}


                //if (!found)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append("Access Denied.");

                //    foreach (var item in actions)
                //    {
                //        sb.AppendLine(string.Format("Action - {0}, Module - {1}", item.action, item.module.ToString()));
                //    }

                //    throw new Exception(sb.ToString());
                //}
                return currentUser;
            });

            return taskResult;
        }

        public static Task<ApplicationUser> HasEditOrOwn(params BTAuthorizationAction[] actions)
        {
            var taskResult = Task.Run(async () =>
            {
                IUserRepository userRepository = new UserRepository();
                var currentUser = userRepository.FindByUserName(Thread.CurrentPrincipal.Identity.Name);
                if (!currentUser.IsBack)
                {
                    var society = await GetLoggedSociety(userRepository, currentUser);
                    //currentUser.Societies = new Society[] { society };
                    throw new Exception("TODO");
                }
                //var currentPermissions = userRepository.FindPermissionByRole(currentUser.Roles.Select(s => s.RoleId).ToList());

                //if (!currentPermissions.Any(s => s.Key == (int)module &&
                //    (s.Value == CHIConstants.EditAll || (s.Value == CHIConstants.EditOwn && createdBy.Equals(currentUsername, StringComparison.InvariantCultureIgnoreCase)))))
                //{
                //    throw new Exception(string.Format("Access Denied. Action - {0} or {1}, Module - {2}", CHIConstants.EditAll, CHIConstants.EditOwn, module.ToString()));
                //}

                //var found = false;
                //foreach (var item in actions)
                //{
                //    if (currentPermissions.Any(s => s.Key == (int)item.module &&
                //        (s.Value == CHIConstants.EditAll || (s.Value == CHIConstants.EditOwn && item.action.Equals(currentUsername, StringComparison.InvariantCultureIgnoreCase)))))
                //    {
                //        found = true;
                //    }
                //}


                //if (!found)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append("Access Denied.");

                //    foreach (var item in actions)
                //    {
                //        sb.AppendLine(string.Format("Action - {0}-{1}, Module - {2}", CHIConstants.EditOwn, CHIConstants.EditAll, item.module.ToString()));
                //    }

                //    throw new Exception(sb.ToString());
                //}
                return currentUser;
            });

            return taskResult;
        }

        public static ApplicationUser HasDeleteOrOwn(params BTAuthorizationAction[] actions)
        {
            var currentUsername = !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name)
                                    ? Thread.CurrentPrincipal.Identity.Name
                                    : "admin";

            IUserRepository userRepository = new UserRepository();
            var currentUser = userRepository.FindByUserName(Thread.CurrentPrincipal.Identity.Name);
            //var currentPermissions = userRepository.FindPermissionByRole(currentUser.Roles.Select(s => s.RoleId).ToList());

            //if (!currentPermissions.Any(s => s.Key == (int)module &&
            //    (s.Value == CHIConstants.DeleteAll || (s.Value == CHIConstants.DeleteOwn && createdBy.Equals(currentUsername, StringComparison.InvariantCultureIgnoreCase)))))
            //{
            //    throw new Exception(string.Format("Access Denied. Action - {0} or {1}, Module - {2}", CHIConstants.DeleteAll, CHIConstants.DeleteOwn, module.ToString()));
            //}

            //var found = false;
            //foreach (var item in actions)
            //{
            //    if (currentPermissions.Any(s => s.Key == (int)item.module &&
            //        (s.Value == CHIConstants.DeleteAll || (s.Value == CHIConstants.DeleteOwn && item.action.Equals(currentUsername, StringComparison.InvariantCultureIgnoreCase)))))
            //    {
            //        found = true;
            //    }
            //}


            //if (!found)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("Access Denied.");

            //    foreach (var item in actions)
            //    {
            //        sb.AppendLine(string.Format("Action - {0}-{1}, Module - {2}", CHIConstants.DeleteOwn, CHIConstants.DeleteAll, item.module.ToString()));
            //    }

            //    throw new Exception(sb.ToString());
            //}

            return currentUser;
        }

        static Task<Society> GetLoggedSociety(IUserRepository userRepository, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var claims = await userRepository.GetClaimsForUser(currentUser);

                    if (claims.Count > 0)
                    {
                        var claim = claims.FirstOrDefault(c => c.Type == "MySociety");
                        if (claim != null)
                        {
                            return context
                                .Societies
                                .FirstOrDefault();
                        }
                    }

                    return null;
                }
            });

            return taskResult;
        }
    }
}
