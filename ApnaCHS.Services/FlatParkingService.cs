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
    public interface IFlatParkingService
    {
        Task<FlatParking> Create(FlatParking flatParking, int? count);

        Task Create(int totalParkings, long? floorId, long? facilityId);

        Task<FlatParking> Read(int key);

        Task Update(FlatParking flatParking);

        Task Delete(int id);

        Task<List<FlatParking>> List(FlatParkingSearchParams searchParams);

        Task Assign(long flatid, long[] parkingids);
    }
    public class FlatParkingService : IFlatParkingService
    {
        IFlatParkingRepository _flatParkingRepository = null;

        public FlatParkingService()
        {
            _flatParkingRepository = new FlatParkingRepository();
        }

        public Task<FlatParking> Create(FlatParking flatParking, int? count)
        {
            return _flatParkingRepository.Create(flatParking, count);
        }

        public Task Create(int totalParkings, long? floorId, long? facilityId)
        {
            return _flatParkingRepository.Create(totalParkings, floorId, facilityId);
        }

        public Task Delete(int id)
        {
            return _flatParkingRepository.Delete(id);
        }

        public Task<List<FlatParking>> List(FlatParkingSearchParams searchParams)
        {
            return _flatParkingRepository.List(searchParams);
        }

        public Task<FlatParking> Read(int key)
        {
            return _flatParkingRepository.Read(key);
        }

        public Task Update(FlatParking flatParking)
        {
            return _flatParkingRepository.Update(flatParking);
        }

        public Task Assign(long flatid, long[] parkingids)
        {
            return _flatParkingRepository.Assign(flatid, parkingids);
        }
    }
}
