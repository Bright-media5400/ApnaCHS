using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.Entities;
using ApnaCHS.Common;
using System.Data.Entity;
using ApnaCHS.AppCommon;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IFlatRepository
    {
        Task<Flat> Create(Flat flat, int? count);

        Task<Flat> Read(int key);

        Task Update(Flat flat);

        Task Delete(int id);

        Task<List<Flat>> List(FlatSearchParams searchParams);

        Task<List<FlatReportResult>> Report(FlatSearchParams searchParams);

        Task<List<UploadFlat>> ExportFloors(long societyId);

        Task<List<UploadFlat>> UploadFlats(List<UploadFlat> flats);

        Task<Flat> Approve(int id, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);
    }

    public class FlatRepository : IFlatRepository
    {
        IFacilityRepository facilityRepository = null;
        IDataApprovalRepository _dataApprovalRepository = null;
        ICommentRepository _commentRepository = null;

        public FlatRepository()
        {
            facilityRepository = new FacilityRepository();
            _dataApprovalRepository = new DataApprovalRepository();
            _commentRepository = new CommentRepository();
        }

        public Task<Flat> Create(Flat flat, int? count)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var cnt = (!count.HasValue || count.Value == 0 || count.Value == 1) ? 1 : count.Value;
                    var floor = context.Floors.First(f => f.Id == flat.Floor.Id);
                    var flats = context.Flats.Count(f => f.FloorId == flat.Floor.Id);
                    var facility = context.Facilities.First(f => f.Id == floor.FacilityId);

                    //get total flats in the facility
                    var flatcount = await facilityRepository.FlatCount(facility.Id);
                    if (flatcount + cnt > facility.NoOfFlats)
                    {
                        throw new Exception("Max " + facility.NoOfFlats.ToString() + " allowed");
                    }

                    if (cnt > 1)
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            var newF = new Flat();

                            newF.FloorId = flat.Floor.Id;
                            newF.FlatTypeId = flat.FlatType.Id;
                            newF.TotalArea = flat.TotalArea;
                            newF.CarpetArea = flat.CarpetArea;
                            newF.BuildUpArea = flat.BuildUpArea;
                            newF.IsCommercialSpace = flat.IsCommercialSpace;
                            newF.HaveParking = flat.HaveParking;
                            //if (cnt != 1)
                            //{
                            newF.Name = ((floor.FloorNumber * 100) + flats + (i + 1)).ToString();
                            //}

                            context.Flats.Add(newF);
                            flat.Id = newF.Id;
                        }

                        context.SaveChanges();
                        return flat;
                    }
                    else
                    {
                        //check duplicate flat by name
                        var exists = (from f in context.Facilities
                                      join flr in context.Floors on f.Id equals flr.FacilityId
                                      join fls in context.Flats on flr.Id equals fls.FloorId
                                      where f.Id == facility.Id && fls.Name == flat.Name
                                      select fls)
                                      .FirstOrDefault();

                        if (exists != null)
                        {
                            throw new Exception("Flat with same name exists in the building");
                        }

                        if (flat.Floor != null)
                        {
                            flat.FloorId = flat.Floor.Id;
                            flat.Floor = null;
                        }

                        if (flat.FlatType != null)
                        {
                            flat.FlatTypeId = flat.FlatType.Id;
                            flat.FlatType = null;
                        }

                        context.Flats.Add(flat);
                        context.SaveChanges();
                        return flat;
                    }
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
                    var existingRecord = context
                        .Flats
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Cannot delete. Flat approved");
                    }

                    if (existingRecord.FlatOwners.Any())
                    {
                        throw new Exception("Cannot delete. Flat owner details found.");
                    }
                    if (existingRecord.FlatParkings.Any())
                    {
                        throw new Exception("Cannot delete. Flat has assign parking.");
                    }
                    if (existingRecord.MaintenanceCosts.Any())
                    {
                        throw new Exception("Cannot delete. Flat has assign Maintenance Costs.");
                    }

                    context.Flats.Remove(existingRecord);
                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<Flat> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Flats
                        .Include(f => f.FlatType)
                        .Include(f => f.Floor)
                        .Include(f => f.FlatParkings)
                        .Include(f => f.FlatOwners)
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.Gender))
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.Flats))
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.Vehicles))
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.FlatOwnerFamilies))
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.FlatOwnerFamilies.Select(o => o.Gender)))
                        .Include(f => f.FlatOwners.Select(g => g.FlatOwner.FlatOwnerFamilies.Select(o => o.Relationship)))
                        .Include(f=>f.Approvals)
                        .Include(f=>f.Comments)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Flat not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Flat flat)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Flats.FirstOrDefault(p => p.Id == flat.Id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Flat detail not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Cannot update. Flat approved");
                    }

                    var floor = context.Floors.First(f => f.Id == flat.Floor.Id);
                    var facility = context.Facilities.First(f => f.Id == floor.FacilityId);

                    //check duplicate flat by name
                    var exists = (from f in context.Facilities
                                  join flr in context.Floors on f.Id equals flr.FacilityId
                                  join fls in context.Flats on flr.Id equals fls.FloorId
                                  where f.Id == facility.Id && fls.Name == flat.Name && fls.Id != flat.Id
                                  select fls)
                                  .FirstOrDefault();

                    if (exists != null)
                    {
                        throw new Exception("Flat with same name exists in the building");
                    }

                    if (flat.FlatType != null)
                    {
                        existingRecord.FlatTypeId = flat.FlatType.Id;
                        existingRecord.FlatType = null;
                    }

                    existingRecord.Name = flat.Name;
                    existingRecord.TotalArea = flat.TotalArea;
                    existingRecord.CarpetArea = flat.CarpetArea;
                    existingRecord.BuildUpArea = flat.BuildUpArea;
                    existingRecord.HaveParking = flat.HaveParking;
                    existingRecord.IsRented = flat.IsRented;
                    existingRecord.IsCommercialSpace = flat.IsCommercialSpace;
                    existingRecord.IsRejected = false;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Flat>> List(FlatSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    var qry = (from msf in context.MapsSocietiesToFacilities
                               join f in context.Facilities on msf.FacilityId equals f.Id
                               join flr in context.Floors on f.Id equals flr.FacilityId
                               join fls in context.Flats on flr.Id equals fls.FloorId
                               select new
                               {
                                   MapSocietyToFacility = msf,
                                   Facility = f,
                                   Floor = flr,
                                   Flat = fls
                               });

                    if (searchParams.FloorId.HasValue)
                    {
                        qry = qry
                            .Where(f => f.Floor.Id == searchParams.FloorId.Value);
                    }

                    if (searchParams.SocietyId.HasValue)
                    {
                        qry = qry
                            .Where(f => f.MapSocietyToFacility.SocietyId == searchParams.SocietyId.Value);
                    }

                    if (searchParams.IsApproved.HasValue)
                    {
                        qry = qry
                            .Where(f => f.Flat.IsApproved == searchParams.IsApproved.Value);
                    }

                    if (searchParams.IsRejected.HasValue)
                    {
                        qry = qry
                            .Where(f => f.Flat.IsApproved == searchParams.IsRejected.Value);
                    }

                    return qry
                        .Select(f => f.Flat)
                        .Include(f => f.Floor)
                        .Include(f => f.FlatType)
                        .Include(f => f.FlatParkings)
                        .Include(f => f.FlatOwners)
                        .Include(f => f.FlatOwners.Select(s => s.Flat))
                        .Include(f => f.FlatOwners.Select(s => s.FlatOwner))
                        .Include(f => f.FlatOwners.Select(s => s.FlatOwner.Gender))
                        .Include(m => m.Approvals)
                        .Include(m => m.Comments)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<FlatReportResult>> Report(FlatSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var societyIdParameter = new SqlParameter("@SocietyId", searchParams.SocietyId);

                    var floorIdParameter = new SqlParameter("@FloorId", DBNull.Value);
                    if (searchParams.FloorId.HasValue)
                        floorIdParameter.Value = searchParams.FloorId.Value;

                    var facilityIdParameter = new SqlParameter("@FacilityId", DBNull.Value);
                    if (searchParams.FacilityId.HasValue)
                        facilityIdParameter.Value = searchParams.FacilityId.Value;

                    var flatNameParameter = new SqlParameter("@FlatName", DBNull.Value);
                    if (!string.IsNullOrEmpty(searchParams.FlatName))
                        flatNameParameter.Value = searchParams.FlatName;

                    var ownerParameter = new SqlParameter("@Owner", DBNull.Value);
                    if (!string.IsNullOrEmpty(searchParams.Owner))
                        ownerParameter.Value = searchParams.Owner;

                    var tenantParameter = new SqlParameter("@Tenant", DBNull.Value);
                    if (!string.IsNullOrEmpty(searchParams.Tenant))
                        tenantParameter.Value = searchParams.Tenant;

                    var usernameParameter = new SqlParameter("@Username", DBNull.Value);
                    if (!string.IsNullOrEmpty(searchParams.Username))
                        usernameParameter.Value = searchParams.Username;

                    var result = context.Database
                        .SqlQuery<FlatReportResult>("GetFlatwiseOwnerTenant @SocietyId,@FloorId,@FacilityId,@FlatName,@Owner,@Tenant,@Username",
                        societyIdParameter, floorIdParameter, facilityIdParameter, flatNameParameter, ownerParameter, tenantParameter, usernameParameter)
                        .ToList();

                    foreach (var item in result)
                    {
                        if (!string.IsNullOrEmpty(item.Tenants))
                        {
                            var ap = XDocument.Parse(item.Tenants);
                            item.TenantList = new List<TenantResult>();

                            foreach (var element in ap.Descendants("TRow"))
                            {
                                var tenant = new TenantResult();
                                tenant.Name = element.Element("Name").Value;
                                tenant.Id = Convert.ToInt64(element.Element("Id").Value);

                                item.TenantList.Add(tenant);
                            }
                        }
                    }
                    return result;
                }
            });
            return taskResult;
        }

        public Task<List<UploadFlat>> ExportFloors(long societyId)
        {

            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var flats = (from msf in context.MapsSocietiesToFacilities
                                 join f in context.Facilities on msf.FacilityId equals f.Id
                                 join flr in context.Floors on f.Id equals flr.FacilityId

                                 where msf.Society.Id == societyId

                                 select new UploadFlat()
                                 {
                                     RegistrationNo = msf.Society.RegistrationNo,
                                     Society = msf.Society.Name,
                                     Building = f.Name,
                                     Wing = f.Wing,
                                     Floor = flr.Name,
                                     FloorType = flr.Type == (byte)EnFloorType.CommercialSpace ? "Commercial Space" : flr.Type == (byte)EnFloorType.Parkings ? "Parkings" : "Floor"
                                 })
                               .ToList();

                    return flats;
                }
            });
            return taskResult;
        }

        public Task<List<UploadFlat>> UploadFlats(List<UploadFlat> flats)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //validate 
                    foreach (var item in flats)
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

                        if (string.IsNullOrEmpty(item.FlatType))
                        {
                            sb.Append(",Flat Type");
                        }

                        if (string.IsNullOrEmpty(item.Name))
                        {
                            sb.Append(",Name");
                        }

                        if (!item.TotalArea.HasValue)
                        {
                            sb.Append(",Total Area");
                        }

                        if (!item.CarpetArea.HasValue)
                        {
                            sb.Append(",Carpet Area");
                        }

                        if (sb.Length > 0)
                        {
                            sb.Append(" is/are missing.");
                            sb.Append("<br />");
                        }

                        var flattype = context
                            .MasterValues
                            .FirstOrDefault(m => m.Type == (byte)EnMasterValueType.FlatTypes && m.Text.Equals(item.FlatType, StringComparison.InvariantCultureIgnoreCase));
                        if (flattype == null)
                        {
                            sb.Append("Flat type text not found in master.");
                            sb.Append("<br />");
                        }

                        if (sb.Length > 0)
                        {
                            item.IsSuccess = false;
                            item.Message = sb.ToString().Trim(',');
                            continue;
                        }

                        var floor = (from msf in context.MapsSocietiesToFacilities
                                     join f in context.Facilities on msf.FacilityId equals f.Id
                                     join flr in context.Floors on f.Id equals flr.FacilityId

                                     where msf.Society.Name.Equals(item.Society, StringComparison.InvariantCultureIgnoreCase)
                                             && msf.Society.RegistrationNo.Equals(item.RegistrationNo, StringComparison.InvariantCultureIgnoreCase)
                                             && f.Name.Equals(item.Building, StringComparison.InvariantCultureIgnoreCase)
                                             && f.Wing.Equals(item.Wing, StringComparison.InvariantCultureIgnoreCase)
                                             && flr.Name.Equals(item.Floor, StringComparison.InvariantCultureIgnoreCase)
                                     select new
                                     {
                                         floor = flr
                                     })
                                    .Select(f => f.floor)
                                    .FirstOrDefault();

                        if (floor == null)
                        {
                            item.IsSuccess = false;
                            item.Message = string.Format("No floor found for {0}-{1}-{2}-{3}.", item.Society, item.Building, item.Wing, item.Floor);
                            continue;
                        }

                        if (floor.Type == (byte)EnFloorType.Parkings)
                        {
                            item.IsSuccess = false;
                            item.Message = string.Format("Floor type is parking. Cannot add flats - {0}", item.Floor);
                            continue;
                        }

                        var flat = new Flat()
                        {
                            Name = item.Name,
                            TotalArea = item.TotalArea.Value,
                            CarpetArea = item.CarpetArea.Value,
                            BuildUpArea = item.BuildUpArea,
                            HaveParking = item.HaveParking,
                            IsRented = false,
                            IsCommercialSpace = floor.Type == (byte)EnFloorType.CommercialSpace ? true : item.IsCommercialSpace,
                            Floor = new Floor() { Id = floor.Id },
                            FlatType = new MasterValue() { Id = flattype.Id }
                        };

                        try
                        {
                            await Create(flat, null);
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

                    return flats;
                }
            });
            return taskResult;
        }

        public Task<Flat> Approve(int id, string note, ApplicationUser currentUser)
        {
            var taskresult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .Flats
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Already approved.");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Flat is rejected. Please update to approve.");
                    }

                    var society = (from msf in context.MapsSocietiesToFacilities
                                   join f in context.Facilities on msf.FacilityId equals f.Id
                                   join flr in context.Floors on f.Id equals flr.FacilityId
                                   join fls in context.Flats on flr.Id equals fls.FloorId
                                   where fls.Id == id
                                   select new
                                   {
                                       society = msf.Society
                                   })
                               .FirstOrDefault();


                    await _dataApprovalRepository.ApproveFlat(id, currentUser, note);
                    existingRecord.IsApproved = await _dataApprovalRepository.IsFlatApproved(id, society.society.Id);
                    context.SaveChanges();

                    await _commentRepository.New(new CommentFlat() { Text = note, FlatId = id }, currentUser);
                    return existingRecord;
                }
            });
            return taskresult;
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser)
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
                            await Approve(id, note, currentUser);
                            result.Add(new ApprovalReply() { Id = id, Message = "Approved", IsSucces = true });
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
                        .Flats
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat not found");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Already Rejected.");
                    }

                    existingRecord.IsRejected = true;
                    _commentRepository.New(new CommentFlat() { Text = note, FlatId = id }, currentUser);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }
    }
}
