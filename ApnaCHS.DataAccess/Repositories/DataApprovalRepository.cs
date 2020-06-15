using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IDataApprovalRepository
    {
        Task<DataApproval> ApproveFlatOwner(long id, ApplicationUser user, string note);

        Task<bool> IsFlatOwnerApproved(long id, long societyId);

        Task<DataApproval> ApproveMaintenanceCost(long id, ApplicationUser user, string note);

        Task<bool> IsMaintenanceCostApproved(long id, long societyId);

        Task<DataApproval> ApproveFlat(long id, ApplicationUser user, string note);

        Task<bool> IsFlatApproved(long id, long societyId);

        Task<DataApproval> ApproveFlatOwnerFamily(long id, ApplicationUser user, string note);

        Task<bool> IsFlatOwnerFamilyApproved(long id, long societyId);

        Task<DataApproval> ApproveVehicle(long id, ApplicationUser user, string note);

        Task<bool> IsVehicleApproved(long id, long societyId);
    }

    public class DataApprovalRepository : IDataApprovalRepository
    {
        public Task<DataApproval> ApproveFlatOwner(long id, ApplicationUser user, string note)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.DataApprovals.Where(a => a.FlatOwnerId == id).Any(a => a.ApprovedBy == user.UserName))
                    {
                        throw new Exception("Flat owner already approved");
                    }

                    var dataApproval = new DataApproval()
                    {
                        FlatOwnerId = id,
                        ApprovedDate = DateTime.Now,
                        ApprovedBy = user.UserName,
                        ApprovedName = user.Name,
                        Note = note
                    };

                    context.DataApprovals.Add(dataApproval);
                    context.SaveChanges();

                    return dataApproval;
                }
            });
            return taskResult;
        }

        public Task<bool> IsFlatOwnerApproved(long id, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context.Societies.FirstOrDefault(s => s.Id == societyId);
                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    var flatowner = context.FlatOwners.FirstOrDefault(s => s.Id == id);
                    if (flatowner == null)
                    {
                        throw new Exception("Flat owner not found");
                    }

                    return context.DataApprovals.Count(d => d.FlatOwnerId == id) >= society.ApprovalsCount;
                }
            });
            return taskResult;
        }

        public Task<DataApproval> ApproveMaintenanceCost(long id, ApplicationUser user, string note)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.DataApprovals.Where(a => a.MaintenanceCostId == id).Any(a => a.ApprovedBy == user.UserName))
                    {
                        throw new Exception("Maintenance cost already approved");
                    }

                    var dataApproval = new DataApproval()
                    {
                        MaintenanceCostId = id,
                        ApprovedDate = DateTime.Now,
                        ApprovedBy = user.UserName,
                        ApprovedName = user.Name,
                        Note = note
                    };

                    context.DataApprovals.Add(dataApproval);
                    context.SaveChanges();

                    return dataApproval;
                }
            });
            return taskResult;
        }

        public Task<bool> IsMaintenanceCostApproved(long id, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context.Societies.FirstOrDefault(s => s.Id == societyId);
                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    var mc = context.MaintCostList.FirstOrDefault(s => s.Id == id);
                    if (mc == null)
                    {
                        throw new Exception("Maintenance cost not found");
                    }

                    return context.DataApprovals.Count(d => d.MaintenanceCostId == id) >= society.ApprovalsCount;
                }
            });
            return taskResult;
        }

        public Task<DataApproval> ApproveFlat(long id, ApplicationUser user, string note)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.DataApprovals.Where(a => a.FlatId == id).Any(a => a.ApprovedBy == user.UserName))
                    {
                        throw new Exception("Flat already approved");
                    }

                    var dataApproval = new DataApproval()
                    {
                        FlatId = id,
                        ApprovedDate = DateTime.Now,
                        ApprovedBy = user.UserName,
                        ApprovedName = user.Name,
                        Note = note
                    };

                    context.DataApprovals.Add(dataApproval);
                    context.SaveChanges();

                    return dataApproval;
                }
            });
            return taskResult;
        }

        public Task<bool> IsFlatApproved(long id, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context.Societies.FirstOrDefault(s => s.Id == societyId);
                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    var flat = context.Flats.FirstOrDefault(s => s.Id == id);
                    if (flat == null)
                    {
                        throw new Exception("Flat not found");
                    }

                    return context.DataApprovals.Count(d => d.FlatId== id) >= society.ApprovalsCount;
                }
            });
            return taskResult;
        }

        public Task<DataApproval> ApproveFlatOwnerFamily(long id, ApplicationUser user, string note)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.DataApprovals.Where(a => a.FlatOwnerFamilyId == id).Any(a => a.ApprovedBy == user.UserName))
                    {
                        throw new Exception("Family member already approved");
                    }

                    var dataApproval = new DataApproval()
                    {
                        FlatOwnerFamilyId = id,
                        ApprovedDate = DateTime.Now,
                        ApprovedBy = user.UserName,
                        ApprovedName = user.Name,
                        Note = note
                    };

                    context.DataApprovals.Add(dataApproval);
                    context.SaveChanges();

                    return dataApproval;
                }
            });
            return taskResult;
        }

        public Task<bool> IsFlatOwnerFamilyApproved(long id, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context.Societies.FirstOrDefault(s => s.Id == societyId);
                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    var family = context.FlatOwnerFamilies.FirstOrDefault(s => s.Id == id);
                    if (family == null)
                    {
                        throw new Exception("Family member not found");
                    }

                    return context.DataApprovals.Count(d => d.FlatOwnerId == id) >= society.ApprovalsCount;
                }
            });
            return taskResult;
        }

        public Task<DataApproval> ApproveVehicle(long id, ApplicationUser user, string note)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    if (context.DataApprovals.Where(a => a.VehicleId == id).Any(a => a.ApprovedBy == user.UserName))
                    {
                        throw new Exception("Vehicle already approved");
                    }

                    var dataApproval = new DataApproval()
                    {
                        VehicleId = id,
                        ApprovedDate = DateTime.Now,
                        ApprovedBy = user.UserName,
                        ApprovedName = user.Name,
                        Note = note
                    };

                    context.DataApprovals.Add(dataApproval);
                    context.SaveChanges();

                    return dataApproval;
                }
            });
            return taskResult;
        }

        public Task<bool> IsVehicleApproved(long id, long societyId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context.Societies.FirstOrDefault(s => s.Id == societyId);
                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    var vehicle = context.Vehicles.FirstOrDefault(s => s.Id == id);
                    if (vehicle == null)
                    {
                        throw new Exception("Vehicle not found");
                    }

                    return context.DataApprovals.Count(d => d.VehicleId == id) >= society.ApprovalsCount;
                }
            });
            return taskResult;
        }

    }
}
