using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IFlatOwnerFamilyRepository
    {
        Task<FlatOwnerFamily> Create(FlatOwnerFamily flatOwnerFamily);

        Task<FlatOwnerFamily> Read(int key);

        Task Update(FlatOwnerFamily flatOwnerFamily);

        Task UpdateUserId(FlatOwnerFamily flatOwnerFamily, long userid);

        Task Delete(int id);

        Task<List<FlatOwnerFamily>> List(FlatOwnerFamilySearchParams searchParams);

        Task<List<ReportFamilyResult>> Report(ReportFamilySearchParams searchParams);

        Task<FlatOwnerFamily> Approve(long id, long societyId, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);

    }
    public class FlatOwnerFamilyRepository : IFlatOwnerFamilyRepository
    {
        IDataApprovalRepository _dataApprovalRepository = null;
        ICommentRepository _commentRepository = null;

        public FlatOwnerFamilyRepository()
        {
            _dataApprovalRepository = new DataApprovalRepository();
            _commentRepository = new CommentRepository();
        }

        public Task<FlatOwnerFamily> Create(FlatOwnerFamily flatOwnerFamily)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (flatOwnerFamily.FlatOwner != null)
                    {
                        flatOwnerFamily.FlatOwnerId = flatOwnerFamily.FlatOwner.Id;
                        flatOwnerFamily.FlatOwner = null;
                    }
                    if (flatOwnerFamily.Gender != null)
                    {
                        flatOwnerFamily.GenderId = flatOwnerFamily.Gender.Id;
                        flatOwnerFamily.Gender = null;
                    }

                    if (flatOwnerFamily.Relationship != null)
                    {
                        flatOwnerFamily.RelationshipId = flatOwnerFamily.Relationship.Id;
                        flatOwnerFamily.Relationship = null;
                    }

                    context.FlatOwnerFamilies.Add(flatOwnerFamily);
                    context.SaveChanges();
                    return flatOwnerFamily;
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
                    var existingRecord = context.FlatOwnerFamilies.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Flatowner family not found");
                    }

                    context.FlatOwnerFamilies.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<FlatOwnerFamily> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .FlatOwnerFamilies
                        .Include(g => g.Gender)
                        .Include(r => r.Relationship)
                        .Include(f => f.FlatOwner)
                        .Include(f => f.FlatOwner.Gender)
                        .Include(s => s.Approvals)
                        .Include(s => s.Comments)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Flatowner family not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(FlatOwnerFamily flatOwnerFamily)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.FlatOwnerFamilies.FirstOrDefault(p => p.Id == flatOwnerFamily.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flatowner family detail not found");
                    }
                    if (flatOwnerFamily.FlatOwner != null)
                    {
                        flatOwnerFamily.FlatOwnerId = flatOwnerFamily.FlatOwner.Id;
                        flatOwnerFamily.FlatOwner = null;
                    }
                    if (flatOwnerFamily.Gender != null)
                    {
                        flatOwnerFamily.GenderId = flatOwnerFamily.Gender.Id;
                        flatOwnerFamily.Gender = null;
                    }

                    if (flatOwnerFamily.Relationship != null)
                    {
                        flatOwnerFamily.RelationshipId = flatOwnerFamily.Relationship.Id;
                        flatOwnerFamily.Relationship = null;
                    }
                    existingRecord.Name = flatOwnerFamily.Name;
                    existingRecord.AdminMember = flatOwnerFamily.AdminMember;
                    existingRecord.ApproverMember = flatOwnerFamily.ApproverMember;
                    existingRecord.MobileNo = flatOwnerFamily.MobileNo;
                    existingRecord.AadhaarCardNo = flatOwnerFamily.AadhaarCardNo;
                    existingRecord.DateOfBirth = flatOwnerFamily.DateOfBirth;
                    existingRecord.IsRejected = false;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateUserId(FlatOwnerFamily flatOwnerFamily, long userid)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .FlatOwnerFamilies
                        .FirstOrDefault(p => p.Id == flatOwnerFamily.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Family detail not found");
                    }

                    existingRecord.UserId = userid;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<FlatOwnerFamily>> List(FlatOwnerFamilySearchParams searchParams)
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
                               join fof in context.FlatOwnerFamilies on fo.Id equals fof.FlatOwnerId
                               where msf.SocietyId == searchParams.SocietyId
                               select fof);


                    if (searchParams.FlatOwnerId.HasValue)
                    {
                        ctx = ctx.Where(c => c.FlatOwnerId == searchParams.FlatOwnerId.Value);
                    }

                    return ctx
                        .Include(g => g.Gender)
                        .Include(r => r.Relationship)
                        .Include(f => f.FlatOwner)
                        .Include(f => f.FlatOwner.Gender)
                        .Include(s => s.Approvals)
                        .Include(s => s.Comments)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<ReportFamilyResult>> Report(ReportFamilySearchParams searchParams)
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
                               join fof in context.FlatOwnerFamilies
                              .Include(s => s.Approvals)
                              .Include(s => s.Comments)
                               on fo.Id equals fof.FlatOwnerId

                               where msf.SocietyId == searchParams.SocietyId
                               select new ReportFamilyResult()
                               {
                                   FlatId = fls.Id,
                                   FlatName = fls.Name,
                                   Id = fof.Id,
                                   Name = fof.Name,
                                   MobileNo = fof.MobileNo,
                                   AadhaarCardNo = fof.AadhaarCardNo,
                                   GenderText = fof.Gender.Text,
                                   RelationshipText = fof.Relationship.Text,
                                   FlatOwnerName = fof.FlatOwner.Name,
                                   Deleted = fof.Deleted,
                                   IsRejected = fof.IsRejected,
                                   IsApproved = fof.IsApproved,
                                   Approvals = fof.Approvals,
                                   Comments = fof.Comments,

                               })
                                   .ToList();
                    return ctx;
                }
            });
            return taskResult;
        }

        public Task<FlatOwnerFamily> Approve(long id, long societyId, string note, ApplicationUser currentUser)
        {
            var taskresult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .FlatOwnerFamilies
                        .Include(f => f.FlatOwner.Flats)                        
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Family member not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Already approved.");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Family member is rejected. Please update to approve.");
                    }

                    var society = (from msf in context.MapsSocietiesToFacilities
                                   join f in context.Facilities on msf.FacilityId equals f.Id
                                   join flr in context.Floors on f.Id equals flr.FacilityId
                                   join fls in context.Flats on flr.Id equals fls.FloorId
                                   join mffo in context.MapsFlatToFlatOwner on fls.Id equals mffo.FlatId
                                   join fo in context.FlatOwners on mffo.FlatOwnerId equals fo.Id
                                   join fof in context.FlatOwnerFamilies on fo.Id equals fof.FlatOwnerId

                                   where fof.Id == id
                                   select msf.Society)
                                   .FirstOrDefault();

                    if (society.Id != societyId)
                    {
                        throw new Exception("Society not found");
                    }

                    await _dataApprovalRepository.ApproveFlatOwnerFamily(id, currentUser, note);
                    existingRecord.IsApproved = await _dataApprovalRepository.IsFlatOwnerFamilyApproved(id, society.Id);
                    context.SaveChanges();

                    await _commentRepository.New(new CommentFlatOwnerFamily() { Text = note, FlatOwnerFamilyId = id }, currentUser);
                    return existingRecord;
                }
            });
            return taskresult;
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
                            var family = await Approve(id, societyId, note, currentUser);
                            result.Add(new ApprovalReply() { Id = id, Message = "Approved", IsSucces = true, Obj = family });
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
                        .FlatOwnerFamilies
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Family member not found");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Already Rejected.");
                    }

                    existingRecord.IsRejected = true;
                    _commentRepository.New(new CommentFlatOwnerFamily() { Text = note, FlatOwnerFamilyId = id }, currentUser);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

    }
}
