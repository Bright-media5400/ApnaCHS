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
    public interface IVehicleService
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
    public class VehicleService : IVehicleService
    {
        IVehicleRepository _vehicleRepository = null;

        public VehicleService()
        {
            _vehicleRepository = new VehicleRepository();
        }

        public Task<Vehicle> Create(Vehicle vehicle)
        {
            return _vehicleRepository.Create(vehicle);
        }

        public Task Delete(int id)
        {
            return _vehicleRepository.Delete(id);
        }

        public Task<List<Vehicle>> List(VehicleSearchParams searchParams)
        {
            return _vehicleRepository.List(searchParams);
        }

        public Task<Vehicle> Read(int key)
        {
            return _vehicleRepository.Read(key);
        }

        public Task Update(Vehicle vehicle)
        {
            return _vehicleRepository.Update(vehicle);
        }

        public Task<Vehicle> Approve(long id, long societyId, string note, ApplicationUser current)
        {
            return _vehicleRepository.Approve(id, societyId, note, current);
        }

        public Task<List<ApprovalReply>> BulkApprove(int[] ids, long societyId, string note, ApplicationUser currentUser)
        {
            return _vehicleRepository.BulkApprove(ids, societyId, note, currentUser);
        }

        public Task Reject(int id, string note, ApplicationUser currentUser)
        {
            return _vehicleRepository.Reject(id, note, currentUser);
        }
    }
}
