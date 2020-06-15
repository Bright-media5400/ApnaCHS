using ApnaCHS.AppCommon;
using ApnaCHS.Common;
using ApnaCHS.DataAccess.Repositories;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.Services
{
    public interface ISocietyService
    {
        Task<Society> Create(Society society, ApplicationUser user);

        Task<Society> Read(int key);

        Task Update(Society society);

        Task UpdateSetting(Society society);

        Task Delete(int id);

        Task<List<Society>> List(SocietySearchParams searchParams);

        Task<List<SocietyImportResult>> Import(List<Society> societies);

        Task<int> FlatCount(long societyId);

        Task UpdateLoginDetails(Society society);
    }
    public class SocietyService : ISocietyService
    {
        ISocietyRepository _societyRepository = null;
        IApplicationRoleRepository _applicationRoleRepository = null;
        IUserService _userService = null;
        IUserRepository _userRepository = null;

        public SocietyService()
        {
            _societyRepository = new SocietyRepository();
            _applicationRoleRepository = new ApplicationRoleRepository();
            _userService = new UserService();
            _userRepository = new UserRepository();
        }

        public Task<Society> Create(Society society, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(async () =>
            {
                var soc = await _societyRepository.Create(society);

                ApplicationUser user = new ApplicationUser
                {
                    UserName = society.PhoneNo,
                    Email = society.Email,
                    Name = society.ContactPerson,
                    MaxAttempts = ProgramCommon.MaxAttempts,
                    bBlocked = false,
                    bChangePass = true,
                    Deleted = false,
                    CreatedBy = currentUser.UserName,
                    ModifiedBy = currentUser.UserName,
                    PhoneNumber = society.PhoneNo,
                    IsBack = false,
                    IsDefault = true
                };

                await _userRepository.CreateSocietyUser(user, soc.Id, currentUser);
                return soc;
            });

            return taskResult;
        }

        public Task Delete(int id)
        {
            return _societyRepository.Delete(id);
        }

        public Task<List<Society>> List(SocietySearchParams searchParams)
        {
            return _societyRepository.List(searchParams);
        }

        public Task<Society> Read(int key)
        {
            return _societyRepository.Read(key);
        }

        public Task Update(Society society)
        {
            return _societyRepository.Update(society);
        }

        public Task UpdateSetting(Society society)
        {
            return _societyRepository.UpdateSetting(society);
        }

        public Task<List<SocietyImportResult>> Import(List<Society> societies)
        {
            return _societyRepository.Import(societies);
        }

        public Task<int> FlatCount(long societyId)
        {
            return _societyRepository.FlatCount(societyId);
        }

        public Task UpdateLoginDetails(Society society)
        {
            return _societyRepository.UpdateLoginDetails(society);
        }
    }
}
