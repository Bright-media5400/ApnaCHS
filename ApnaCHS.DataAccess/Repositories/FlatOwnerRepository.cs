using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ApnaCHS.AppCommon;
using System.Data.SqlClient;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IFlatOwnerRepository
    {
        Task<FlatOwner> Create(FlatOwner flatOwner);

        Task<FlatOwner> Read(int key);

        Task Update(FlatOwner flatOwner);

        Task UpdateTillDate(MapFlatToFlatOwner flatOwner);

        Task UpdateSinceDate(MapFlatToFlatOwner flatOwner);

        Task Delete(int id);

        Task<List<FlatOwner>> List(FlatOwnerSearchParams searchParams);

        Task UpdateUserId(FlatOwner owner, long userId);

        Task<FlatOwner> Approve(long flatOwner, long societyId, string note, ApplicationUser current);

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

    public class FlatOwnerRepository : IFlatOwnerRepository
    {
        IFlatOwnerFamilyRepository _flatOwnerFamilyRepository = null;
        IVehicleRepository _vehicleRepository = null;
        IDataApprovalRepository _dataApprovalRepository = null;
        ICommentRepository _commentRepository = null;

        public FlatOwnerRepository()
        {
            _flatOwnerFamilyRepository = new FlatOwnerFamilyRepository();
            _vehicleRepository = new VehicleRepository();
            _dataApprovalRepository = new DataApprovalRepository();
            _commentRepository = new CommentRepository();
        }

        public Task<FlatOwner> Create(FlatOwner flatOwner)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var flats = flatOwner.Flats;
                    foreach (var item in flats) //iteration on flat
                    {
                        var flatId = item.Flat.Id;

                        var owners = (from f in context.Flats
                                      join m in context.MapsFlatToFlatOwner.Include(o => o.FlatOwner) on f.Id equals m.FlatId
                                      where f.Id == flatId
                                      select m)
                                      .ToList();

                        if (item.FlatOwnerType == (byte)EnOwnerType.Owner && owners.Any(o => !o.MemberTillDate.HasValue && o.FlatOwnerType == (byte)EnOwnerType.Owner))
                        {
                            throw new Exception("Cannot add new owner. Update member till date of previous owner");
                        }

                        if (!context.FlatOwners.Any(f => f.Flats.Any(fl => fl.FlatId == flatId))
                            && flatOwner.Flats.Any(f => f.FlatOwnerType != (byte)EnOwnerType.Owner))
                        {
                            throw new Exception("Please add flat owner before adding tenant or paying guest");
                        }

                        if (owners.Any(o => item.MemberSinceDate < o.MemberSinceDate))
                        {
                            throw new Exception("Member since date of owner, cannot be before member since any previous owners.");
                        }

                        if (owners.Any(o => o.FlatOwner.MobileNo.Equals(flatOwner.MobileNo, StringComparison.InvariantCultureIgnoreCase)
                                        || o.FlatOwner.EmailId.Equals(flatOwner.EmailId, StringComparison.InvariantCultureIgnoreCase)
                                        || o.FlatOwner.AadhaarCardNo == flatOwner.AadhaarCardNo
                                        || o.MemberSinceDate == item.MemberSinceDate))
                        {
                            throw new Exception("Duplicate member found. Flat owner with same mobile no, aadhaar card and member since date found");
                        }

                        var society = (from msf in context.MapsSocietiesToFacilities
                                       join f in context.Facilities on msf.FacilityId equals f.Id
                                       join flr in context.Floors on f.Id equals flr.FacilityId
                                       join fls in context.Flats on flr.Id equals fls.FloorId
                                       where fls.Id == flatId
                                       select new
                                       {
                                           Society = msf.Society
                                       })
                                       .Select(s => s.Society)
                                   .FirstOrDefault();

                        if (society == null)
                        {
                            throw new Exception("Society not found");
                        }

                        if (item.MemberSinceDate < society.DateOfIncorporation)
                        {
                            throw new Exception("Member since date cannot be before date of incorporation of the society");
                        }

                        if (flatOwner.DateOfBirth.HasValue && item.MemberSinceDate < flatOwner.DateOfBirth.Value)
                        {
                            throw new Exception("Member since date cannot be before Date of Birth");
                        }
                    }

                    if (flatOwner.Gender != null)
                    {
                        flatOwner.GenderId = flatOwner.Gender.Id;
                        flatOwner.Gender = null;
                    }

                    flatOwner.Flats = null;
                    context.FlatOwners.Add(flatOwner);

                    context.SaveChanges();

                    foreach (var item in flats)
                    {
                        context.MapsFlatToFlatOwner.Add(
                            new MapFlatToFlatOwner()
                            {
                                FlatId = item.Flat.Id,
                                FlatOwnerId = flatOwner.Id,
                                MemberSinceDate = item.MemberSinceDate,
                                FlatOwnerType = item.FlatOwnerType,
                                MemberTillDate = item.MemberTillDate.HasValue ? item.MemberTillDate.Value : new Nullable<DateTime>()
                            });
                    }
                    context.SaveChanges();
                    return flatOwner;
                }
            });
            return taskResult;
        }

        public Task Delete(int id)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.FlatOwners.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Flatowner not found");
                    }
                    if (existingRecord.FlatOwnerFamilies.Any())
                    {
                        throw new Exception("Cannot delete. Family member details found.");
                    }
                    if (existingRecord.Vehicles.Any())
                    {
                        throw new Exception("Cannot delete. Vehicle details found.");
                    }
                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Cannot delete. Flat owner approved.");
                    }

                    context.FlatOwners.Remove(existingRecord);
                    context.SaveChanges();

                }
            });
            return taskresult;
        }

        public Task<FlatOwner> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .FlatOwners
                        .Include(f => f.Gender)
                        .Include(f => f.Flats)
                        .Include(f => f.Flats.Select(o => o.Flat))
                        .Include(f => f.Flats.Select(o => o.FlatOwner))
                        .Include(f => f.Vehicles)
                        .Include(f => f.FlatOwnerFamilies)
                        .Include(f => f.FlatOwnerFamilies.Select(o => o.Gender))
                        .Include(f => f.FlatOwnerFamilies.Select(o => o.Relationship))
                        .Include(g => g.Comments)
                        .Include(g => g.Approvals)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Flatowner not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(FlatOwner flatOwner)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .FlatOwners
                        .FirstOrDefault(p => p.Id == flatOwner.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flatowner detail not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Cannot update. Flat owner details approved.");
                    }

                    var flats = existingRecord.Flats;
                    foreach (var item in flats) //iteration on flat
                    {
                        var flatId = item.Flat.Id;

                        var owners = (from f in context.Flats
                                      join m in context.MapsFlatToFlatOwner on f.Id equals m.FlatId
                                      where f.Id == flatId
                                      select m)
                                      .ToList();

                        if (owners.Any(o => o.FlatOwnerId != item.FlatOwnerId && item.MemberSinceDate < o.MemberSinceDate))
                        {
                            throw new Exception("Member since date of owner, cannot be before member since any previous owners.");
                        }

                        if (flatOwner.DateOfBirth.HasValue && item.MemberSinceDate < flatOwner.DateOfBirth.Value)
                        {
                            throw new Exception("Member since date cannot be before Date of Birth");
                        }

                        if (owners.Any(o => o.FlatOwnerId != flatOwner.Id
                                        && (o.FlatOwner.MobileNo.Equals(item.FlatOwner.MobileNo, StringComparison.InvariantCultureIgnoreCase)
                                        || o.FlatOwner.EmailId.Equals(item.FlatOwner.EmailId, StringComparison.InvariantCultureIgnoreCase)
                                        || o.FlatOwner.AadhaarCardNo == item.FlatOwner.AadhaarCardNo
                                        || o.MemberSinceDate == item.MemberSinceDate)))
                        {
                            throw new Exception("Duplicate member found. Flat owner with same mobile no, aadhaar card and member since date found");
                        }

                    }

                    if (flatOwner.Gender != null)
                    {
                        flatOwner.GenderId = flatOwner.Gender.Id;
                        flatOwner.Gender = null;
                    }

                    existingRecord.Name = flatOwner.Name;
                    existingRecord.MobileNo = flatOwner.MobileNo;
                    existingRecord.EmailId = flatOwner.EmailId;
                    existingRecord.DateOfBirth = flatOwner.DateOfBirth;
                    existingRecord.AadhaarCardNo = flatOwner.AadhaarCardNo;
                    existingRecord.IsRejected = false;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateTillDate(MapFlatToFlatOwner flatOwner)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MapsFlatToFlatOwner
                        .FirstOrDefault(p => p.FlatOwnerId == flatOwner.FlatOwner.Id && p.FlatId == flatOwner.Flat.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat owner details not found");
                    }

                    if (existingRecord.MemberTillDate.HasValue)
                    {
                        throw new Exception("Cannot update. Member till date already found.");
                    }

                    if (existingRecord.MemberSinceDate > flatOwner.MemberTillDate)
                    {
                        throw new Exception("Member Till date cannot be before Member Since Date");
                    }

                    existingRecord.MemberTillDate = flatOwner.MemberTillDate;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateSinceDate(MapFlatToFlatOwner flatOwner)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MapsFlatToFlatOwner
                        .FirstOrDefault(p => p.FlatOwnerId == flatOwner.FlatOwner.Id && p.FlatId == flatOwner.Flat.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat owner details not found");
                    }

                    var society = (from msf in context.MapsSocietiesToFacilities
                                   join f in context.Facilities on msf.FacilityId equals f.Id
                                   join flr in context.Floors on f.Id equals flr.FacilityId
                                   join fls in context.Flats on flr.Id equals fls.FloorId
                                   where fls.Id == flatOwner.FlatId
                                   select new
                                   {
                                       Society = msf.Society
                                   })
                                   .Select(s => s.Society)
                                   .FirstOrDefault();

                    if (flatOwner.MemberSinceDate < society.DateOfIncorporation)
                    {
                        throw new Exception("Member since date cannot be before date of incorporation of the society");
                    }

                    if (existingRecord.FlatOwner.IsApproved)
                    {
                        throw new Exception("Cannot update. Flat owner details approved.");
                    }

                    existingRecord.MemberSinceDate = flatOwner.MemberSinceDate;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<FlatOwner>> List(FlatOwnerSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = (from msf in context.MapsSocietiesToFacilities
                               join f in context.Facilities on msf.FacilityId equals f.Id
                               join flr in context.Floors on f.Id equals flr.FacilityId
                               join fls in context.Flats on flr.Id equals fls.FloorId
                               join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id

                               where msf.SocietyId == searchParams.SocietyId
                               select new
                               {
                                   Flat = mffo,
                                   Flatowner = fo
                               });

                    if (searchParams.IsApproved.HasValue)
                    {
                        ctx = ctx
                            .Where(f => f.Flatowner.IsApproved == searchParams.IsApproved.Value);
                    }

                    if (searchParams.IsRejected.HasValue)
                    {
                        ctx = ctx
                            .Where(f => f.Flatowner.IsRejected == searchParams.IsRejected.Value);
                    }

                    if (searchParams.FlatOwnerType.HasValue)
                    {
                        ctx = ctx
                            .Where(f => f.Flat.FlatOwnerType == searchParams.FlatOwnerType.Value);
                    }

                    return ctx
                        .Select(f => f.Flatowner)
                        .Include(f => f.Flats)
                        .Include(f => f.Flats.Select(o => o.Flat))
                        .Include(f => f.Flats.Select(o => o.FlatOwner))
                        .Include(v => v.Vehicles)
                        .Include(f => f.FlatOwnerFamilies)
                        .Include(f => f.FlatOwnerFamilies.Select(fow => fow.Gender))
                        .Include(f => f.FlatOwnerFamilies.Select(fow => fow.Relationship))
                        .Include(g => g.Gender)
                        .Include(g => g.Comments)
                        .Include(g => g.Approvals)
                        .ToList();
                }
            });

            return taskResult;
        }

        public Task UpdateUserId(FlatOwner owner, long userId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.FlatOwners.FirstOrDefault(p => p.Id == owner.Id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Flat owner details not found");
                    }

                    existingRecord.UserId = userId;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<FlatOwner> Approve(long flatOwner, long societyId, string note, ApplicationUser current)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.FlatOwners.FirstOrDefault(p => p.Id == flatOwner);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat owner details not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Already approved.");
                    }

                    var society = (from msf in context.MapsSocietiesToFacilities
                                   join f in context.Facilities on msf.FacilityId equals f.Id
                                   join flr in context.Floors on f.Id equals flr.FacilityId
                                   join fls in context.Flats on flr.Id equals fls.FloorId
                                   join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                                   join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id

                                   where fo.Id == flatOwner

                                   select new
                                   {
                                       society = msf.Society,
                                       flat = mffo
                                   })
                                   .FirstOrDefault();


                    if (society.society.Id != societyId)
                    {
                        throw new Exception("Society not found");
                    }

                    await _dataApprovalRepository.ApproveFlatOwner(flatOwner, current, note);
                    existingRecord.IsApproved = await _dataApprovalRepository.IsFlatOwnerApproved(flatOwner, society.society.Id);
                    context.SaveChanges();

                    await _commentRepository.New(new CommentFlatOwner() { Text = note, FlatOwnerId = flatOwner },current);

                    existingRecord.Flats = new List<MapFlatToFlatOwner>();
                    existingRecord.Flats.Add(society.flat);

                    return existingRecord;
                }
            });
            return taskResult;
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser)
        {
            var taskresult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var result = new List<ApprovalReply>();

                    foreach (var id in ids)
                    {
                        try
                        {
                            var owner = await Approve(id, societyId, note, currentUser);
                            result.Add(new ApprovalReply() { Id = id, Message = "Approved", IsSucces = true, Obj = owner });
                        }
                        catch (Exception ex)
                        {
                            result.Add(new ApprovalReply() { Id = id, Message = ex.Message, IsSucces = false });
                        }
                    }
                    return result;
                }
            });
            return taskresult;
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .FlatOwners
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat owner not found");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Already Rejected.");
                    }

                    existingRecord.IsRejected = true;
                    _commentRepository.New(new CommentFlatOwner() { Text = note, FlatOwnerId = id }, currentUser);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<FlatOwner> CurrentOwner(long flatId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var res = (from mffo in context.MapsFlatToFlatOwner
                               join f in context.Flats on mffo.FlatId equals f.Id
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                               where mffo.FlatId == flatId && !mffo.MemberTillDate.HasValue && mffo.FlatOwnerType == (byte)EnOwnerType.Owner
                               select new
                               {
                                   mffo = mffo,
                                   f = f,
                                   fo = fo
                               })
                               .OrderByDescending(o => o.mffo.MemberSinceDate)
                               .FirstOrDefault();

                    return res != null ? res.fo : null;
                }
            });
            return taskResult;
        }

        public Task<FlatOwner> CurrentOccupant(long flatId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var res = (from mffo in context.MapsFlatToFlatOwner
                               join f in context.Flats on mffo.FlatId equals f.Id
                               join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                               where mffo.FlatId == flatId && !mffo.MemberTillDate.HasValue
                               select new
                               {
                                   mffo = mffo,
                                   f = f,
                                   fo = fo
                               })
                               .OrderByDescending(o => o.mffo.MemberSinceDate)
                               .FirstOrDefault();

                    return res != null ? res.fo : null;
                }
            });
            return taskResult;
        }

        public Task<List<UploadFlatOwner>> UploadFlatOwners(List<UploadFlatOwner> owners, byte flatOwnerType)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //validate 
                    foreach (var item in owners)
                    {
                        StringBuilder sb = new StringBuilder();

                        if (string.IsNullOrEmpty(item.RegistrationNo))
                        {
                            sb.Append(",Registration No");
                        }

                        if (string.IsNullOrEmpty(item.Society))
                        {
                            sb.Append(",Society");
                        }

                        if (string.IsNullOrEmpty(item.Building))
                        {
                            sb.Append(",Building");
                        }

                        if (string.IsNullOrEmpty(item.Wing))
                        {
                            sb.Append(",Wing");
                        }

                        if (string.IsNullOrEmpty(item.Floor))
                        {
                            sb.Append(",Floor");
                        }

                        if (string.IsNullOrEmpty(item.Flat))
                        {
                            sb.Append(",Flat");
                        }

                        if (string.IsNullOrEmpty(item.Name))
                        {
                            sb.Append(",Name");
                        }
                        if (string.IsNullOrEmpty(item.MobileNo))
                        {
                            sb.Append(",Mobile No.");
                        }
                        if (string.IsNullOrEmpty(item.EmailId))
                        {
                            sb.Append(",Email");
                        }
                        if (string.IsNullOrEmpty(item.Gender))
                        {
                            sb.Append(",Gender");
                        }
                        if (!item.MemberSinceDate.HasValue)
                        {
                            sb.Append(",Member Since Date");
                        }

                        if (sb.Length > 0)
                        {
                            sb.Append(" is/are missing.");
                            sb.Append("<br />");
                        }

                        var gender = context.MasterValues.FirstOrDefault(m => m.Type == (byte)EnMasterValueType.Gender && m.Text.Equals(item.Gender, StringComparison.InvariantCultureIgnoreCase));
                        if (gender == null)
                        {
                            sb.Append("Gender text not found in master.");
                            sb.Append("<br />");
                        }

                        if (sb.Length > 0)
                        {
                            item.IsSuccess = false;
                            item.Message = sb.ToString().Trim(',');
                            continue;
                        }

                        var flat = (from msf in context.MapsSocietiesToFacilities
                                    join f in context.Facilities on msf.FacilityId equals f.Id
                                    join flr in context.Floors on f.Id equals flr.FacilityId
                                    join fls in context.Flats on flr.Id equals fls.FloorId
                                    where msf.Society.Name.Equals(item.Society, StringComparison.InvariantCultureIgnoreCase)
                                             && msf.Society.RegistrationNo.Equals(item.RegistrationNo, StringComparison.InvariantCultureIgnoreCase)
                                             && f.Name.Equals(item.Building, StringComparison.InvariantCultureIgnoreCase)
                                             && f.Wing.Equals(item.Wing, StringComparison.InvariantCultureIgnoreCase)
                                             && flr.Name.Equals(item.Floor, StringComparison.InvariantCultureIgnoreCase)
                                             && fls.Name.Equals(item.Flat, StringComparison.InvariantCultureIgnoreCase)
                                    select new
                                    {
                                        flat = fls
                                    })
                                    .Select(f => f.flat)
                                   .FirstOrDefault();

                        if (flat == null)
                        {
                            item.IsSuccess = false;
                            item.Message = string.Format("No flat found for {0}-{1}-{2}-{3}-{4}.", item.Society, item.Building, item.Wing, item.Floor, item.Flat);
                            continue;
                        }

                        var flatOwner = new FlatOwner()
                        {
                            Name = item.Name,
                            MobileNo = item.MobileNo,
                            EmailId = item.EmailId,
                            DateOfBirth = item.DateOfBirth,
                            AadhaarCardNo = item.AadhaarCardNo,
                            GenderId = gender.Id,
                            Flats = new List<MapFlatToFlatOwner>() { 
                                new MapFlatToFlatOwner() { 
                                    Flat = new Flat() { Id = flat.Id }, 
                                    MemberSinceDate = item.MemberSinceDate.Value, 
                                    FlatOwnerType = flatOwnerType,
                                    MemberTillDate = item.MemberTillDate.HasValue ? item.MemberTillDate.Value : new Nullable<DateTime>()
                                } }
                        };

                        try
                        {
                            await Create(flatOwner);
                        }
                        catch (Exception ex)
                        {
                            item.IsSuccess = false;
                            item.Message = ex.Message;
                            continue;
                        }

                        item.IsSuccess = true;
                        item.Message = "Done";
                    }

                    return owners;
                }
            });
            return taskResult;
        }

        public Task<List<ReportFlatOwnersTenantsDetail>> ExportFlatOwners(ReportFlatOwnersTenantsDetailSearchParams searchParams)
        {

            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var societyIdParameter = new SqlParameter("@SocietyId", searchParams.SocietyId);

                    var result = context.Database
                        .SqlQuery<ReportFlatOwnersTenantsDetail>("GetFlatOwnersTenantsDetails @SocietyId", societyIdParameter)
                        .ToList();

                    return result;
                }
            });
            return taskResult;
        }

        public Task<List<ReportFlatOwnersTenantsDetail>> UploadFlatOwnersFamily(List<ReportFlatOwnersTenantsDetail> families)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //validate 
                    foreach (var item in families)
                    {
                        StringBuilder sb = new StringBuilder();

                        if (string.IsNullOrEmpty(item.Society))
                        {
                            sb.Append(",Society");
                        }

                        if (string.IsNullOrEmpty(item.Building))
                        {
                            sb.Append(",Building");
                        }

                        if (string.IsNullOrEmpty(item.Wing))
                        {
                            sb.Append(",Wing");
                        }

                        if (string.IsNullOrEmpty(item.Floor))
                        {
                            sb.Append(",Floor");
                        }

                        if (string.IsNullOrEmpty(item.Flat))
                        {
                            sb.Append(",Flat");
                        }

                        if (string.IsNullOrEmpty(item.Owner))
                        {
                            sb.Append(",Flat Owner");
                        }

                        if (string.IsNullOrEmpty(item.OwnerType))
                        {
                            sb.Append(",Flat Owner Type");
                        }

                        if (string.IsNullOrEmpty(item.Name))
                        {
                            sb.Append(",Name");
                        }
                        if (string.IsNullOrEmpty(item.Gender))
                        {
                            sb.Append(",Gender.");
                        }
                        if (string.IsNullOrEmpty(item.Relationship))
                        {
                            sb.Append(",Relationship");
                        }

                        if (sb.Length > 0)
                        {
                            sb.Append(" is/are missing.");
                            sb.Append("<br />");
                        }

                        var gender = context
                            .MasterValues
                            .FirstOrDefault(m => m.Type == (byte)EnMasterValueType.Gender && m.Text.Equals(item.Gender, StringComparison.InvariantCultureIgnoreCase));
                        if (gender == null)
                        {
                            sb.Append("Gender text not found in master.");
                            sb.Append("<br />");
                        }

                        var relationship = context
                            .MasterValues
                            .FirstOrDefault(m => m.Type == (byte)EnMasterValueType.Relationship && m.Text.Equals(item.Relationship, StringComparison.InvariantCultureIgnoreCase));
                        if (relationship == null)
                        {
                            sb.Append("Relationship text not found in master.");
                            sb.Append("<br />");
                        }

                        if (sb.Length > 0)
                        {
                            item.IsSuccess = false;
                            item.Message = sb.ToString().Trim(',');
                            continue;
                        }

                        var flatowner = (from msf in context.MapsSocietiesToFacilities
                                         join f in context.Facilities on msf.FacilityId equals f.Id
                                         join flr in context.Floors on f.Id equals flr.FacilityId
                                         join fls in context.Flats on flr.Id equals fls.FloorId
                                         join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                                         join fos in context.FlatOwners on mffo.FlatOwnerId equals fos.Id

                                         where msf.Society.Name.Equals(item.Society, StringComparison.InvariantCultureIgnoreCase)
                                                  && f.Name.Equals(item.Building, StringComparison.InvariantCultureIgnoreCase)
                                                  && f.Wing.Equals(item.Wing, StringComparison.InvariantCultureIgnoreCase)
                                                  && flr.Name.Equals(item.Floor, StringComparison.InvariantCultureIgnoreCase)
                                                  && fls.Name.Equals(item.Flat, StringComparison.InvariantCultureIgnoreCase)
                                                  && fos.Name.Equals(item.Owner, StringComparison.InvariantCultureIgnoreCase)
                                                  && mffo.FlatOwnerType == (item.OwnerType.Equals("Owner", StringComparison.InvariantCultureIgnoreCase) ? 1 : 2)

                                         select new
                                         {
                                             fo = fos
                                         })
                                    .Select(f => f.fo)
                                   .FirstOrDefault();

                        if (flatowner == null)
                        {
                            item.IsSuccess = false;
                            item.Message = string.Format("No flat found for {0}-{1}-{2}-{3}-{4}.", item.Society, item.Building, item.Wing, item.Floor, item.Flat);
                            continue;
                        }

                        var family = new FlatOwnerFamily()
                        {
                            Name = item.Name,
                            MobileNo = item.MobileNo,
                            AadhaarCardNo = item.AadhaarCardNo,
                            DateOfBirth = item.DateOfBirth,
                            GenderId = gender.Id,
                            RelationshipId = relationship.Id,
                            FlatOwnerId = flatowner.Id
                        };

                        try
                        {
                            await _flatOwnerFamilyRepository.Create(family);
                        }
                        catch (Exception ex)
                        {
                            item.IsSuccess = false;
                            item.Message = ex.Message;
                            continue;
                        }

                        item.IsSuccess = true;
                        item.Message = "Done";
                    }

                    return families;
                }
            });
            return taskResult;
        }

        public Task<List<ReportFlatOwnersVehicleDetail>> ExportFlatOwnersVehicles(ReportFlatOwnersTenantsDetailSearchParams searchParams)
        {

            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var societyIdParameter = new SqlParameter("@SocietyId", searchParams.SocietyId);

                    var result = context.Database
                        .SqlQuery<ReportFlatOwnersVehicleDetail>("GetFlatOwnersTenantsDetails @SocietyId", societyIdParameter)
                        .ToList();

                    return result;
                }
            });
            return taskResult;
        }

        public Task<List<ReportFlatOwnersVehicleDetail>> UploadFlatOwnersVehicle(List<ReportFlatOwnersVehicleDetail> vehicles)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //validate 
                    foreach (var item in vehicles)
                    {
                        StringBuilder sb = new StringBuilder();

                        if (string.IsNullOrEmpty(item.Society))
                        {
                            sb.Append(",Society");
                        }

                        if (string.IsNullOrEmpty(item.Building))
                        {
                            sb.Append(",Building");
                        }

                        if (string.IsNullOrEmpty(item.Wing))
                        {
                            sb.Append(",Wing");
                        }

                        if (string.IsNullOrEmpty(item.Floor))
                        {
                            sb.Append(",Floor");
                        }

                        if (string.IsNullOrEmpty(item.Flat))
                        {
                            sb.Append(",Flat");
                        }

                        if (string.IsNullOrEmpty(item.FlatOwner))
                        {
                            sb.Append(",Flat Owner");
                        }

                        if (string.IsNullOrEmpty(item.FlatOwnerType))
                        {
                            sb.Append(",Flat Owner Type");
                        }

                        if (string.IsNullOrEmpty(item.Name))
                        {
                            sb.Append(",Name");
                        }
                        if (string.IsNullOrEmpty(item.Number))
                        {
                            sb.Append(",Number.");
                        }
                        if (!item.Type.HasValue)
                        {
                            sb.Append(",Type");
                        }

                        if (sb.Length > 0)
                        {
                            sb.Append(" is/are missing.");
                            sb.Append("<br />");
                        }

                        if (sb.Length > 0)
                        {
                            item.IsSuccess = false;
                            item.Message = sb.ToString().Trim(',');
                            continue;
                        }

                        var flatowner = (from msf in context.MapsSocietiesToFacilities
                                         join f in context.Facilities on msf.FacilityId equals f.Id
                                         join flr in context.Floors on f.Id equals flr.FacilityId
                                         join fls in context.Flats on flr.Id equals fls.FloorId
                                         join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                                         join fos in context.FlatOwners on mffo.FlatOwnerId equals fos.Id

                                         where msf.Society.Name.Equals(item.Society, StringComparison.InvariantCultureIgnoreCase)
                                                  && f.Name.Equals(item.Building, StringComparison.InvariantCultureIgnoreCase)
                                                  && f.Wing.Equals(item.Wing, StringComparison.InvariantCultureIgnoreCase)
                                                  && flr.Name.Equals(item.Floor, StringComparison.InvariantCultureIgnoreCase)
                                                  && fls.Name.Equals(item.Flat, StringComparison.InvariantCultureIgnoreCase)
                                                  && fos.Name.Equals(item.FlatOwner, StringComparison.InvariantCultureIgnoreCase)
                                                  && mffo.FlatOwnerType == (item.FlatOwnerType.Equals("Owner", StringComparison.InvariantCultureIgnoreCase) ? 1 : 2)

                                         select new
                                         {
                                             fo = fos,
                                             flat = fls
                                         })
                                   .FirstOrDefault();

                        if (flatowner == null)
                        {
                            item.IsSuccess = false;
                            item.Message = string.Format("No flat found for {0}-{1}-{2}-{3}-{4}.", item.Society, item.Building, item.Wing, item.Floor, item.Flat);
                            continue;
                        }

                        var vehicle = new Vehicle()
                        {
                            Name = item.Name,
                            Number = item.Number,
                            Make = item.Make,
                            Type = item.Type,
                            FlatId = flatowner.flat.Id,
                            FlatOwnerId = flatowner.fo.Id
                        };

                        try
                        {
                            await _vehicleRepository.Create(vehicle);
                        }
                        catch (Exception ex)
                        {
                            item.IsSuccess = false;
                            item.Message = ex.Message;
                            continue;
                        }

                        item.IsSuccess = true;
                        item.Message = "Done";
                    }

                    return vehicles;
                }
            });
            return taskResult;
        }
    }
}
