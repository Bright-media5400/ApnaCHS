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
    public interface IFloorService
    {
        Task<Floor> Create(Floor floor);

        Task CreateMultiple(Floor[] floors);

        Task<Floor> Read(int key);

        Task Update(Floor floor);

        Task Delete(int id);

        Task<List<Floor>> List(FloorSearchParams searchParams);

        Task<List<Floor>> SocietyWiseList(FloorSearchParams searchParams);
    }

    public class FloorService : IFloorService
    {

        IFloorRepository _floorRepository = null;

        public FloorService()
        {
            _floorRepository = new FloorRepository();
        }

        public Task<Floor> Create(Floor floor)
        {
            return _floorRepository.Create(floor);
        }

        public Task CreateMultiple(Floor[] floors)
        {
            return _floorRepository.CreateMultiple(floors);
        }

        public Task Delete(int id)
        {
            return _floorRepository.Delete(id);
        }

        public Task<List<Floor>> List(FloorSearchParams searchParams)
        {
            return _floorRepository.List(searchParams);
        }

        public Task<List<Floor>> SocietyWiseList(FloorSearchParams searchParams)
        {
            return _floorRepository.SocietyWiseList(searchParams);
        }

        public Task<Floor> Read(int key)
        {
            return _floorRepository.Read(key);
        }

        public Task Update(Floor floor)
        {
            return _floorRepository.Update(floor);
        }
    }
}


