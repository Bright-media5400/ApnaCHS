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
    public interface IMaintenanceCostService
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

        Task Restore(int id);
    }

    public class MaintenanceCostService : IMaintenanceCostService
    {
        IMaintenanceCostRepository _maintenanceCostRepository = null;

        public MaintenanceCostService()
        {
            _maintenanceCostRepository = new MaintenanceCostRepository();
        }

        public Task<MaintenanceCost> Create(MaintenanceCost maintenanceCost)
        {
            return _maintenanceCostRepository.Create(maintenanceCost);
        }

        public Task Delete(int id)
        {
            return _maintenanceCostRepository.Delete(id);
        }

        public Task<List<MaintenanceCost>> List(MaintenanceCostSearchParams searchParams)
        {
            return _maintenanceCostRepository.List(searchParams);
        }

        public Task<MaintenanceCost> Read(int key)
        {
            return _maintenanceCostRepository.Read(key);
        }

        public Task Update(MaintenanceCost maintenanceCost)
        {
            return _maintenanceCostRepository.Update(maintenanceCost);
        }

        public Task UpdateEndDate(MaintenanceCost maintenanceCost)
        {
            return _maintenanceCostRepository.UpdateEndDate(maintenanceCost);
        }

        public Task<MaintenanceCost> Approve(int id, string note, ApplicationUser currentUser)
        {
            return _maintenanceCostRepository.Approve(id, note, currentUser);
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, string note, ApplicationUser currentUser)
        {
            return _maintenanceCostRepository.BulkApprove(ids, note, currentUser);
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            return _maintenanceCostRepository.Reject(id, note, currentUser);
        }

        public Task AssignedFlats(int mcid, Flat[] assignedFlats, Flat[] unassignedFlats)
        {
            return _maintenanceCostRepository.AssignedFlats(mcid, assignedFlats, unassignedFlats);
        }

        public Task Restore(int id)
        {
            return _maintenanceCostRepository.Restore(id);
        }
    }
}
