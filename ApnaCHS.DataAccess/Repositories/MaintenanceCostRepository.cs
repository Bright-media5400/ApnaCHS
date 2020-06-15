using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Entity.Core.Objects;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IMaintenanceCostRepository
    {
        Task<MaintenanceCost> Create(MaintenanceCost maintenanceCost);

        Task<MaintenanceCost> Read(int key);

        Task Update(MaintenanceCost maintenanceCost);

        Task UpdateEndDate(MaintenanceCost maintenanceCost);

        Task Delete(int id);

        Task<List<MaintenanceCost>> List(MaintenanceCostSearchParams searchParams);

        Task<MaintenanceCost> Approve(int id, string note, ApplicationUser currentUser);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);

        Task AssignedFlats(int mcid, Flat[] assignedFlats, Flat[] unassignedFlats);

        Task<List<MaintenanceCost>> GetCostLineForBillGeneration(long flatid, long societyId, DateTime generationDate);

        Task Restore(int id);
    }
    public class MaintenanceCostRepository : IMaintenanceCostRepository
    {
        IDataApprovalRepository _dataApprovalRepository = null;
        ICommentRepository _commentRepository = null;

        public MaintenanceCostRepository()
        {
            _dataApprovalRepository = new DataApprovalRepository();
            _commentRepository = new CommentRepository();
        }

        public Task<MaintenanceCost> Create(MaintenanceCost maintenanceCost)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    //Data Validation
                    //---------------------------------------------------------------------------------//
                    //duplication check
                    if (context.MaintCostList.Any(m => m.DefinitionId == maintenanceCost.Definition.Id
                        && m.SocietyId == maintenanceCost.Society.Id
                        && (DbFunctions.TruncateTime(maintenanceCost.FromDate.Value) < DbFunctions.TruncateTime(m.FromDate.Value)
                        || !m.ToDate.HasValue
                        || DbFunctions.TruncateTime(maintenanceCost.FromDate.Value) < DbFunctions.TruncateTime(m.ToDate.Value))))
                    {
                        throw new Exception("Duplicate maintenance cost found");
                    }

                    var society = context.Societies.FirstOrDefault(p => p.Id == maintenanceCost.Society.Id);
                    if (maintenanceCost.FromDate < society.DateOfIncorporation)
                    {
                        throw new Exception("Start date cannot be before date of incorporation of the society");
                    }

                    if (maintenanceCost.ToDate < maintenanceCost.FromDate)
                    {
                        throw new Exception("End date cannot be before start date");
                    }
                    //---------------------------------------------------------------------------------//
                    //Data Process
                    //---------------------------------------------------------------------------------//
                    if (maintenanceCost.Definition != null)
                    {
                        maintenanceCost.DefinitionId = maintenanceCost.Definition.Id;
                        maintenanceCost.Definition = null;
                    }

                    if (maintenanceCost.Society != null)
                    {
                        maintenanceCost.SocietyId = maintenanceCost.Society.Id;
                        maintenanceCost.Society = null;
                    }

                    maintenanceCost.Date = DateTime.Now;
                    //---------------------------------------------------------------------------------//
                    //Data Insertion
                    //---------------------------------------------------------------------------------//
                    context.MaintCostList.Add(maintenanceCost);
                    context.SaveChanges();
                    //---------------------------------------------------------------------------------//

                    return maintenanceCost;
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
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        existingRecord.Deleted = true;
                        context.SaveChanges();
                    }
                    else
                    {
                        context.MaintCostList.Remove(existingRecord);
                        context.SaveChanges();
                    }
                }
            });
            return taskresult;
        }

        public Task<MaintenanceCost> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .MaintCostList
                        .Include(m => m.Society)
                        .Include(m => m.Definition)
                        .Include(m => m.Flats)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(MaintenanceCost maintenanceCost)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == maintenanceCost.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost detail not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Maintenance Cost approved. Cannot update");
                    }

                    var society = context.Societies.FirstOrDefault(p => p.Id == maintenanceCost.Society.Id);
                    if (maintenanceCost.FromDate < society.DateOfIncorporation)
                    {
                        throw new Exception("Start date cannot be before date of incorporation of the society");
                    }

                    if (maintenanceCost.ToDate < maintenanceCost.FromDate)
                    {
                        throw new Exception("End date cannot be before start date");
                    }

                    existingRecord.Amount = maintenanceCost.Amount;
                    existingRecord.Date = maintenanceCost.Date;
                    existingRecord.FromDate = maintenanceCost.FromDate;
                    existingRecord.ToDate = maintenanceCost.ToDate;
                    existingRecord.AllFlats = maintenanceCost.AllFlats;
                    existingRecord.PerSqrArea = maintenanceCost.PerSqrArea;
                    existingRecord.IsRejected = false;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateEndDate(MaintenanceCost maintenanceCost)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == maintenanceCost.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost detail not found");
                    }
                    if (existingRecord.ToDate.HasValue == true)
                    {
                        throw new Exception("End Date already set, cannot update date");
                    }
                    existingRecord.ToDate = maintenanceCost.ToDate;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<MaintenanceCost>> List(MaintenanceCostSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .MaintCostList
                        .Where(c => c.SocietyId == searchParams.SocietyId)
                        .Include(m => m.Society)
                        .Include(m => m.Definition)
                        .Include(m => m.Flats)
                        .Include(m => m.Approvals)
                        .Include(m => m.Comments);

                    if (searchParams.IsApproved.HasValue)
                    {
                        ctx = ctx.Where(c => c.IsApproved == searchParams.IsApproved.Value);
                    }

                    if (searchParams.IsDeleted.HasValue)
                    {
                        ctx = ctx.Where(c => c.Deleted == searchParams.IsDeleted.Value);
                    }

                    if (searchParams.IsActive.HasValue)
                    {
                        ctx = ctx.Where(c => !c.ToDate.HasValue || c.ToDate.Value > DateTime.Now);
                    }

                    if (searchParams.IsRejected.HasValue)
                    {
                        ctx = ctx.Where(c => c.IsRejected == searchParams.IsRejected.Value);
                    }

                    return ctx.ToList();
                }
            });
            return taskResult;
        }

        public Task<MaintenanceCost> Approve(int id, string note, ApplicationUser currentUser)
        {
            var taskresult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Already approved.");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Maintenance Cost is rejected. Please update to approve.");
                    }

                    await _dataApprovalRepository.ApproveMaintenanceCost(id, currentUser, note);
                    existingRecord.IsApproved = await _dataApprovalRepository.IsMaintenanceCostApproved(id, existingRecord.SocietyId);
                    context.SaveChanges();

                    await _commentRepository.New(new CommentMC() { Text = note, MaintenanceCostId = id }, currentUser);
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
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Already Rejected.");
                    }

                    existingRecord.IsRejected = true;
                    _commentRepository.New(new CommentMC() { Text = note, MaintenanceCostId = id }, currentUser);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task AssignedFlats(int mcid, Flat[] assignedFlats, Flat[] unassignedFlats)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == mcid);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }

                    if (existingRecord.IsApproved)
                    {
                        throw new Exception("Maintenance Cost approved. Cannot update");
                    }

                    //assigned list
                    foreach (var item in assignedFlats)
                    {
                        var flat = new Flat() { Id = item.Id };
                        context.Flats.Attach(flat);
                        existingRecord.Flats.Add(flat);
                    }

                    //unassigned list
                    foreach (var item in unassignedFlats)
                    {
                        var exist = existingRecord.Flats.FirstOrDefault(f => f.Id == item.Id);
                        if (exist != null)
                        {
                            existingRecord.Flats.Remove(exist);
                        }
                    }

                    context.SaveChanges();
                }

            });
            return taskresult;
        }

        public Task<List<MaintenanceCost>> GetCostLineForBillGeneration(long flatid, long societyId, DateTime generationDate)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var flatIdParameter = new SqlParameter("@FlatId", DBNull.Value);
                    flatIdParameter.Value = flatid;

                    var societyIdParameter = new SqlParameter("@SocietyId", DBNull.Value);
                    societyIdParameter.Value = societyId;

                    var generationDateParameter = new SqlParameter("@GenerationDate", DBNull.Value);
                    generationDateParameter.Value = generationDate;

                    var ml = context
                        .Database
                        .SqlQuery<long>("GetMCLines @FlatId, @SocietyId, @GenerationDate", flatIdParameter, societyIdParameter, generationDateParameter)
                        .ToList();

                    return context
                        .MaintCostList
                        .Include(m => m.Definition)
                        .Where(c => ml.Contains(c.Id))
                        .ToList();
                }

            });
            return taskresult;
        }

        public Task Restore(int id)
        {
            var taskresult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .MaintCostList
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Maintenance Cost not found");
                    }

                    existingRecord.Deleted = false;
                    context.SaveChanges();
                }
            });
            return taskresult;
        }
    }
}
