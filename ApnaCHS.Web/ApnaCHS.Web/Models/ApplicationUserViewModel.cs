using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApnaCHS.Web.Models
{
    public class ApplicationUserViewModel
    {

        public int id { get; set; }

        public string name { get; set; }

        public int maxAttempts { get; set; }

        public string whatsappNumber { get; set; }

        public int bExpired { get; set; }

        public int expireMinutes { get; set; }

        public bool bBlocked { get; set; }

        public bool bChangePass { get; set; }

        public string email { get; set; }

        public string userName { get; set; }

        public ApplicationRoleViewModel[] userroles { get; set; }

        public string phoneNumber { get; set; }

        public bool bDeleted { get; set; }
    }

    public class ApplicationUserTrimViewModel
    {

        public int id { get; set; }

        public int? appSettingsId { get; set; }

        public string name { get; set; }

        public int maxAttempts { get; set; }

        public string whatsappNumber { get; set; }

        public int bExpired { get; set; }

        public int expireMinutes { get; set; }

        public bool bBlocked { get; set; }

        public bool bChangePass { get; set; }

        public string email { get; set; }

        public string userName { get; set; }

        public bool bDeleted { get; set; }
    }


    public class RegisterUserViewModel
    {
        public long id { get; set; }

        [Required]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        [Display(Name = "Roles")]
        [Required]
        [MinLength(1)]
        public ApplicationRoleTrimViewModel[] userroles { get; set; }

        public int? appSettingsId { get; set; }

        public string name { get; set; }

        public int maxAttempts { get; set; }

        public string whatsappNumber { get; set; }

        public string phoneNumber { get; set; }

        public int bExpired { get; set; }

        public int expireMinutes { get; set; }

        public bool bBlocked { get; set; }

        public bool bChangePass { get; set; }


        [Required]
        public string email { get; set; }

        public bool isback { get; set; }

        public long? societyId { get; set; }

        public long? complexId { get; set; }
    }

    public class RegisterUpdateUserViewModel
    {
        public long id { get; set; }

        [Required]
        public string userName { get; set; }

        [Display(Name = "Roles")]
        [Required]
        [MinLength(1)]
        public ApplicationRoleTrimViewModel[] userroles { get; set; }

        public int? appSettingsId { get; set; }

        public string name { get; set; }

        public int maxAttempts { get; set; }

        public string whatsappNumber { get; set; }

        public string phoneNumber { get; set; }

        public int bExpired { get; set; }

        public int expireMinutes { get; set; }

        public bool bBlocked { get; set; }

        public bool bChangePass { get; set; }


        [Required]
        public string email { get; set; }

    }

}