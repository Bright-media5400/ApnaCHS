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
    public interface IFlatOwnerFamilyService
    {
        Task<FlatOwnerFamily> Create(FlatOwnerFamily flat);

        Task<FlatOwnerFamily> Read(int key);

        Task Update(FlatOwnerFamily flat);

        Task Delete(int id);

        Task<List<FlatOwnerFamily>> List(FlatOwnerFamilySearchParams searchParams);

        Task<List<ReportFamilyResult>> Report(ReportFamilySearchParams searchParams);

        Task Approve(long id, long societyId, long? complexId, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);
    }
    public class FlatOwnerFamilyService : IFlatOwnerFamilyService
    {
        IFlatOwnerFamilyRepository _flatOwnerFamilyRepository = null;
        IUserService _userService = null; 

        public FlatOwnerFamilyService()
        {
            _flatOwnerFamilyRepository = new FlatOwnerFamilyRepository();
            _userService = new UserService(); 
        }

        public Task<FlatOwnerFamily> Create(FlatOwnerFamily flatOwnerFamily)
        {
            return _flatOwnerFamilyRepository.Create(flatOwnerFamily);
        }

        public Task Delete(int id)
        {
            return _flatOwnerFamilyRepository.Delete(id);
        }

        public Task<List<FlatOwnerFamily>> List(FlatOwnerFamilySearchParams searchParams)
        {
            return _flatOwnerFamilyRepository.List(searchParams);
        }

        public Task<List<ReportFamilyResult>> Report(ReportFamilySearchParams searchParams)
        {
            return _flatOwnerFamilyRepository.Report(searchParams);
        }

        public Task<FlatOwnerFamily> Read(int key)
        {
            return _flatOwnerFamilyRepository.Read(key);
        }

        public Task Update(FlatOwnerFamily flatOwnerFamily)
        {
            return _flatOwnerFamilyRepository.Update(flatOwnerFamily);
        }

        public Task Approve(long id, long societyId, long? complexId, string note, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                var family = await _flatOwnerFamilyRepository.Approve(id, societyId, note, currentUser);
                if (family.IsApproved)
                {
                    var flat = family.FlatOwner.Flats.First();
                    if (!flat.MemberTillDate.HasValue)
                    {
                        var user = _userService.FindByPhoneNumber(family.MobileNo);
                        if (user != null)
                        {
                            if (flat.FlatOwnerType == (byte)EnOwnerType.Owner)
                            {
                                await _userService.RegisterFlatOwnerFamily(family, societyId, complexId, currentUser);
                            }
                            else
                            {
                                await _userService.RegisterTenantFamily(family, societyId, complexId, currentUser);
                            }

                            user = _userService.FindByPhoneNumber(family.MobileNo);
                            await _flatOwnerFamilyRepository.UpdateUserId(family, user.Id);
                        }
                    }
                }
            });
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                var result = await _flatOwnerFamilyRepository.BulkApprove(ids, societyId, note, currentUser);
                foreach (var item in result)
                {
                    if (item.IsSucces)
                    {
                        var family = (FlatOwnerFamily)item.Obj;
                        if (family.IsApproved)
                        {
                            var flat = family.FlatOwner.Flats.First();
                            if (!flat.MemberTillDate.HasValue)
                            {
                                var user = _userService.FindByPhoneNumber(family.MobileNo);
                                if (user != null)
                                {
                                    if (flat.FlatOwnerType == (byte)EnOwnerType.Owner)
                                    {
                                        await _userService.RegisterFlatOwnerFamily(family, societyId, null, currentUser);
                                    }
                                    else
                                    {
                                        await _userService.RegisterTenantFamily(family, societyId, null, currentUser);
                                    }

                                    user = _userService.FindByPhoneNumber(family.MobileNo);
                                    await _flatOwnerFamilyRepository.UpdateUserId(family, user.Id);
                                }
                            }
                        }
                    }
                }
                return result;
            });
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            return _flatOwnerFamilyRepository.Reject(id, note, currentUser);
        }

    }
}
