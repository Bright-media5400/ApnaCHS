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
    public interface IVehicleRepository
    {
        Task<Vehicle> Create(Vehicle vehicle);

        Task<Vehicle> Read(int key);

        Task Update(Vehicle vehicle);

        Task Delete(int id);

        Task<List<Vehicle>> List(VehicleSearchParams searchParams);

        Task<Vehicle> Approve(long id, long societyId, string note, ApplicationUser current);

        Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser);

        Task Reject(int id, string note, ApplicationUser currentUser);

    }
    public class VehicleRepository : IVehicleRepository
    {
        IDataApprovalRepository _dataApprovalRepository = null;
        ICommentRepository _commentRepository = null;

        public VehicleRepository()
        {
            _dataApprovalRepository = new DataApprovalRepository();
            _commentRepository = new CommentRepository();
        }

        public Task<Vehicle> Create(Vehicle vehicle)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    //Check duplicate vehicle
                    if (context.Vehicles.Any(r => r.Number == vehicle.Number))
                    {
                        throw new Exception("Duplicate numbers found. All vehicles should have different number");
                    }

                    if (vehicle.FlatOwner != null)
                    {
                        vehicle.FlatOwnerId = vehicle.FlatOwner.Id;
                        vehicle.FlatOwner = null;
                    }
                    if (vehicle.Flat != null)
                    {
                        vehicle.FlatId = vehicle.Flat.Id;
                        vehicle.Flat = null;
                    }
                    context.Vehicles.Add(vehicle);
                    context.SaveChanges();

                    return vehicle;
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
                        .Vehicles
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Vehicle not found");
                    }

                    context.Vehicles.Remove(existingRecord);
                    context.SaveChanges();
                }
            });

            return taskresult;
        }

        public Task<Vehicle> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Vehicles
                        .Include(s => s.FlatOwner)
                        .Include(s => s.Flat)
                        .Include(s => s.Approvals)
                        .Include(s => s.Comments)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Vehicle not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Vehicle vehicle)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Vehicles.FirstOrDefault(p => p.Id == vehicle.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Flat detail not found");
                    }
                    if (vehicle.FlatOwner != null)
                    {
                        vehicle.FlatOwnerId = vehicle.FlatOwner.Id;
                        vehicle.FlatOwner = null;
                    }
                    if (vehicle.Flat != null)
                    {
                        vehicle.FlatId = vehicle.Flat.Id;
                        vehicle.Flat = null;
                    }
                    existingRecord.Name = vehicle.Name;
                    existingRecord.Make = vehicle.Make;
                    existingRecord.Number = vehicle.Number;
                    existingRecord.Type = vehicle.Type;
                    existingRecord.IsRejected = false;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Vehicle>> List(VehicleSearchParams searchParams)
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
                               join v in context.Vehicles on new { k1 = fo.Id, k2 = fls.Id } equals new { k1 = v.FlatOwnerId, k2 = v.FlatId } into result
                               from r in result.DefaultIfEmpty()

                               where msf.SocietyId == searchParams.SocietyId && r != null
                               select r);

                    if (searchParams.FlatId.HasValue)
                    {
                        ctx = ctx.Where(b => b.FlatId == searchParams.FlatId.Value);
                    }

                    if (searchParams.FlatOwnerId.HasValue)
                    {
                        ctx = ctx.Where(b => b.FlatOwnerId == searchParams.FlatOwnerId.Value);
                    }

                    return ctx
                        .Include(s => s.FlatOwner)
                        .Include(f => f.FlatOwner.Gender)
                        .Include(s => s.Flat)
                        .Include(s => s.Approvals)
                        .Include(s => s.Comments)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<Vehicle> Approve(long id, long societyId, string note, ApplicationUser current)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Vehicles.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Vehicles not found");
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

                                   where fls.Id == existingRecord.FlatId
                                            && fo.Id == existingRecord.FlatOwnerId

                                   select msf.Society)
                                   .FirstOrDefault();

                    if (society.Id != societyId)
                    {
                        throw new Exception("Society not found");
                    }

                    await _dataApprovalRepository.ApproveVehicle(id, current, note);
                    existingRecord.IsApproved = await _dataApprovalRepository.IsVehicleApproved(id, society.Id);
                    context.SaveChanges();

                    await _commentRepository.New(new CommentVehicle() { Text = note, VehicleId = id }, current);
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
                            await Approve(id, societyId, note, currentUser);
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
                        .Vehicles
                        .FirstOrDefault(p => p.Id == id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Vehicle not found");
                    }

                    if (existingRecord.IsRejected)
                    {
                        throw new Exception("Already Rejected.");
                    }

                    existingRecord.IsRejected = true;
                    _commentRepository.New(new CommentVehicle() { Text = note, VehicleId = id }, currentUser);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

    }
}
