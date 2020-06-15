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
    public interface IFlatOwnerService
    {
        Task<FlatOwner> Create(FlatOwner flatOwner);

        Task<FlatOwner> Read(int key);

        Task Update(FlatOwner flatOwner);

        Task UpdateTillDate(MapFlatToFlatOwner flatOwner);

        Task UpdateSinceDate(MapFlatToFlatOwner flatOwner);

        Task Delete(int id);

        Task<List<FlatOwner>> List(FlatOwnerSearchParams searchParams);

        Task Approve(long flatOwner, long societyId, long? complexId, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);

        Task<FlatOwner> CurrentOwner(long flatId);

        Task<FlatOwner> CurrentOccupant(long flatId);

        Task<List<UploadFlatOwner>> UploadFlatOwners(List<UploadFlatOwner> owners, byte flatOwnerType);

        Task<List<ReportFlatOwnersTenantsDetail>> ExportFlatOwners(ReportFlatOwnersTenantsDetailSearchParams searchParams);

        Task<List<ReportFlatOwnersTenantsDetail>> UploadFlatOwnersFamily(List<ReportFlatOwnersTenantsDetail> families);

        Task<List<ReportFlatOwnersVehicleDetail>> ExportFlatOwnersVehicles(ReportFlatOwnersTenantsDetailSearchParams searchParams);

        Task<List<ReportFlatOwnersVehicleDetail>> UploadFlatOwnersVehicle(List<ReportFlatOwnersVehicleDetail> vehicles);
    }

    public class FlatOwnerService : IFlatOwnerService
    {
        IFlatOwnerRepository _flatOwnerServiceRepository = null;
        IDataApprovalRepository _dataApprovalRepository = null;
        IUserService _userService = null;

        public FlatOwnerService()
        {
            _flatOwnerServiceRepository = new FlatOwnerRepository();
            _userService = new UserService();
            _dataApprovalRepository = new DataApprovalRepository();
        }

        public Task<FlatOwner> Create(FlatOwner flatOwner)
        {
            return _flatOwnerServiceRepository.Create(flatOwner);
        }

        public Task Delete(int id)
        {
            return _flatOwnerServiceRepository.Delete(id);
        }

        public Task<List<FlatOwner>> List(FlatOwnerSearchParams searchParams)
        {
            return _flatOwnerServiceRepository.List(searchParams);
        }

        public Task<FlatOwner> Read(int key)
        {
            return _flatOwnerServiceRepository.Read(key);
        }

        public Task Update(FlatOwner flatOwner)
        {
            return _flatOwnerServiceRepository.Update(flatOwner);
        }

        public Task UpdateTillDate(MapFlatToFlatOwner flatOwner)
        {
            return _flatOwnerServiceRepository.UpdateTillDate(flatOwner);
        }

        public Task UpdateSinceDate(MapFlatToFlatOwner flatOwner)
        {
            return _flatOwnerServiceRepository.UpdateSinceDate(flatOwner);
        }

        public Task Approve(long flatOwner, long societyId, long? complexId, string note, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                var owner = await _flatOwnerServiceRepository.Approve(flatOwner, societyId, note, currentUser);
                if (owner.IsApproved)
                {
                    var flat = owner.Flats.First();
                    if (!flat.MemberTillDate.HasValue)
                    {
                        if (flat.FlatOwnerType == (byte)EnOwnerType.Owner)
                        {
                            await _userService.RegisterFlatOwner(owner, societyId, complexId, currentUser);
                        }
                        else
                        {
                            await _userService.RegisterTenant(owner, societyId, complexId, currentUser);
                        }

                        var user = _userService.FindByPhoneNumber(owner.MobileNo);
                        await _flatOwnerServiceRepository.UpdateUserId(owner, user.Id);
                    }
                }
            });
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser)
        {
            return Task.Run(async () =>
            {
                var result = await _flatOwnerServiceRepository.BulkApprove(ids, societyId, note, currentUser);
                foreach (var item in result)
                {
                    if (item.IsSucces)
                    {
                        var owner = (FlatOwner)item.Obj;
                        if (owner.IsApproved)
                        {
                            var flat = owner.Flats.First();
                            if (!flat.MemberTillDate.HasValue)
                            {

                                if (flat.FlatOwnerType == (byte)EnOwnerType.Owner)
                                {
                                    await _userService.RegisterFlatOwner(owner, societyId, null, currentUser);
                                }
                                else
                                {
                                    await _userService.RegisterTenant(owner, societyId, null, currentUser);
                                }

                                var user = _userService.FindByPhoneNumber(owner.MobileNo);
                                await _flatOwnerServiceRepository.UpdateUserId(owner, user.Id);
                            }
                        }
                    }
                }
                return result;
            });
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            return _flatOwnerServiceRepository.Reject(id, note, currentUser);
        }

        public Task<FlatOwner> CurrentOwner(long flatId)
        {
            return _flatOwnerServiceRepository.CurrentOwner(flatId);
        }

        public Task<FlatOwner> CurrentOccupant(long flatId)
        {
            return _flatOwnerServiceRepository.CurrentOccupant(flatId);
        }

        public Task<List<UploadFlatOwner>> UploadFlatOwners(List<UploadFlatOwner> owners, byte flatOwnerType)
        {
            return _flatOwnerServiceRepository.UploadFlatOwners(owners, flatOwnerType);
        }

        public Task<List<ReportFlatOwnersTenantsDetail>> ExportFlatOwners(ReportFlatOwnersTenantsDetailSearchParams searchParams)
        {
            return _flatOwnerServiceRepository.ExportFlatOwners(searchParams);
        }

        public Task<List<ReportFlatOwnersTenantsDetail>> UploadFlatOwnersFamily(List<ReportFlatOwnersTenantsDetail> families)
        {
            return _flatOwnerServiceRepository.UploadFlatOwnersFamily(families);
        }

        public Task<List<ReportFlatOwnersVehicleDetail>> ExportFlatOwnersVehicles(ReportFlatOwnersTenantsDetailSearchParams searchParams)
        {
            return _flatOwnerServiceRepository.ExportFlatOwnersVehicles(searchParams);
        }

        public Task<List<ReportFlatOwnersVehicleDetail>> UploadFlatOwnersVehicle(List<ReportFlatOwnersVehicleDetail> vehicles)
        {
            return _flatOwnerServiceRepository.UploadFlatOwnersVehicle(vehicles);
        }
    }
}
