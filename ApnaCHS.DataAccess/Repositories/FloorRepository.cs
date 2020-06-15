using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.Common;
using System.Data.Entity;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IFloorRepository
    {
        Task<Floor> Create(Floor floor);

        Task CreateMultiple(Floor[] floors);

        Task<Floor> Read(int key);

        Task Update(Floor floor);

        Task Delete(int id);

        Task<List<Floor>> List(FloorSearchParams searchParams);

        Task<List<Floor>> SocietyWiseList(FloorSearchParams searchParams);

    }
    public class FloorRepository : IFloorRepository
    {
        public Task<Floor> Create(Floor floor)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    var facility = context.Facilities.FirstOrDefault(f => f.Id == floor.Facility.Id);
                    if (facility == null) { throw new Exception("Facility not found"); }

                    //get total flats in the facility
                    //- 1 is for the ground floor which is not considered in floor count
                    var floorcount = context.Floors.Count(f => f.FacilityId == floor.Facility.Id) - 1;

                    //+ 1 is for the new added floor
                    if (floorcount + 1 > facility.NoOfFloors)
                    {
                        throw new Exception("Max " + facility.NoOfFloors.ToString() + " allowed");
                    }

                    //next floor number for the facility
                    var floorNumber = context.Floors.Any(f => f.FacilityId == floor.Facility.Id)
                        ? context.Floors.Where(f => f.FacilityId == floor.Facility.Id).Max(f => f.FloorNumber) + 1
                        : 0;
                    floor.FloorNumber = floorNumber;

                    if (floor.Facility != null)
                    {
                        floor.FacilityId = floor.Facility.Id;
                        floor.Facility = null;
                    }

                    context.Floors.Add(floor);
                    context.SaveChanges();

                    return floor;
                }
            });
            return taskResult;
        }

        public Task CreateMultiple(Floor[] floors)
        {
            var taskResult = Task.Run(async() =>
            {
                using (var context = new DbContext())
                {
                    foreach (var item in floors)
                    {
                        var i = 0;
                        foreach (var flat in  item.Flats)
                        {
                            flat.Name = ((item.FloorNumber * 100) + (i + 1)).ToString();
                            i++;
                        }

                        i = 0;
                        foreach (var flatparking in item.FlatParkings)
                        {
                            flatparking.Name = string.Format("{0}", i + 1);
                            i++;
                        }

                        await Create(item);
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
                    var existingRecord = context.Floors.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Floor not found");
                    }

                    context.Floors.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<Floor> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                                   .Floors
                                   .Include(f => f.Facility)
                                   .Include(f => f.FlatParkings)
                                   //.Include(f => f.FlatParkings.Select(p => p.ParkingType))
                                   .Include(f => f.Flats)
                                   .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Floor not found");
                    }
                    return existing;
                }
            });
            return taskResult;
        }

        public Task Update(Floor floor)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Floors.FirstOrDefault(p => p.Id == floor.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Floor detail not found");
                    }

                    existingRecord.Name = floor.Name;
                    existingRecord.Type = floor.Type;
                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Floor>> List(FloorSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var qry = context
                        .Floors
                        .Include(f => f.Facility)
                        .Include(f => f.FlatParkings)
                        //.Include(f => f.FlatParkings.Select(p => p.ParkingType))
                        .Include(f => f.Flats)
                        .AsQueryable();

                    if (searchParams.FacilityId.HasValue)
                    {
                        qry = qry
                            .Where(f => f.FacilityId == searchParams.FacilityId.Value);
                    }

                    return qry
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<Floor>> SocietyWiseList(FloorSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = (from msf in context.MapsSocietiesToFacilities
                               join fl in context.Facilities on msf.FacilityId equals fl.Id
                               join fr in context.Floors on fl.Id equals fr.FacilityId
                               where msf.SocietyId == searchParams.SocietyId.Value
                               select fr)
                                .Include(f => f.Facility)
                                .Include(f => f.FlatParkings)
                                .Include(f => f.Flats)
                                .AsQueryable();

                    return ctx
                        .ToList();
                }
            });
            return taskResult;
        }

    }
}
