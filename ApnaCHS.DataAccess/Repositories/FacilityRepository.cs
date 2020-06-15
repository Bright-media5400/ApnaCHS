using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApnaCHS.Entities;
using ApnaCHS.Common;
using System.Data.Entity;
using ApnaCHS.AppCommon;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IFacilityRepository
    {
        Task<Facility[]> Create(Facility[] facilities, long? societyId);

        Task<Facility> Read(int key);

        Task Update(Facility facility);

        Task Delete(int id);

        Task<List<Facility>> List(FacilitySearchParams searchParams);

        Task<List<Facility>> SocietyWiseList(FacilitySearchParams searchParams);

        Task<List<Facility>> LoadFlats(FacilitySearchParams searchParams);

        Task<int> FlatCount(long facilityId);

        Task<int> FloorCount(long facilityId);

        Task<List<Facility>> LoadParkings(FacilitySearchParams searchParams);

        Task LinkSocieties(long[] linkSocities, long[] unlinkSocities, long facilityId);
    }
    public class FacilityRepository : IFacilityRepository
    {
        ISocietyRepository societyRepository = null;
        IFlatParkingRepository flatParkingRepository = null;

        public FacilityRepository()
        {
            societyRepository = new SocietyRepository();
            flatParkingRepository = new FlatParkingRepository();
        }

        public Task<Facility[]> Create(Facility[] facilities, long? societyId)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    foreach (var facility in facilities)
                    {
                        if (facility.Complex != null)
                        {
                            facility.ComplexId = facility.Complex.Id;
                            facility.Complex = null;
                        }

                        if (context.Facilities.Any(f => f.Name == facility.Name && f.Wing == facility.Wing && f.ComplexId == facility.ComplexId))
                        {
                            throw new Exception("Facility with same name found in the complex");
                        }

                        if (facility.Type == (byte)EnFacilityType.RowHouses)
                        {
                            //Row Houses

                            //isWing = false;
                            //isNoOfFloors = false;
                            //isNoOfFlats = true;
                            //isNoOfLifts = false;
                            //isNoOfParkinglots = false;
                        }
                        else if (facility.Type == (byte)EnFacilityType.ParkingTower)
                        {
                            //Parking Tower
                            //isWing = true;
                            //isNoOfFloors = true;
                            //isNoOfFlats = false;
                            //isNoOfLifts = true;
                            //isNoOfParkinglots = true;

                            facility.IsParkingLot = true;

                            context.Facilities.Add(facility);
                            context.SaveChanges();

                            int parkingsPerFloor = facility.NoOfParkinglots / facility.NoOfFloors;

                            for (int j = 0; j < facility.NoOfFloors + 1; j++)
                            {
                                //add Floor
                                var floor = new Floor()
                                {
                                    Name = (j == 0 ? "Ground Floor" : "Floor " + j.ToString()),
                                    FloorNumber = j,
                                    FacilityId = facility.Id,
                                    Type = (byte)EnFloorType.Parkings
                                };

                                context.Floors.Add(floor);
                                context.SaveChanges();

                                if (parkingsPerFloor > 0)
                                {
                                    var flatParking = new FlatParking()
                                    {
                                        Facility = new Facility() { Id = facility.Id },
                                    };

                                    await flatParkingRepository.Create(flatParking, parkingsPerFloor);
                                }
                            }

                            if (facility.NoOfLifts > 0)
                            {
                                for (int i = 1; i <= facility.NoOfLifts; i++)
                                {
                                    context.SocietyAssets.Add(new SocietyAsset() { Name = "Lift " + i, FacilityId = facility.Id, ComplexId = facility.ComplexId, Quantity = 1 });
                                }
                                context.SaveChanges();
                            }
                        }
                        else if (facility.Type == (byte)EnFacilityType.OpenParking)
                        {
                            facility.IsParkingLot = true;

                            int noOf2Parkinglots = 0;
                            int noOf4Parkinglots = 0;
                            int noOf24Parkinglots = 0;

                            if (!string.IsNullOrEmpty(facility.Wing))
                            {
                                var arr = facility.Wing.Split(':');
                                if (arr.Length == 3)
                                {
                                    noOf2Parkinglots = Convert.ToInt32(arr[0]);
                                    noOf4Parkinglots = Convert.ToInt32(arr[1]);
                                    noOf24Parkinglots = Convert.ToInt32(arr[2]);
                                }

                                facility.Wing = null;
                            }

                            context.Facilities.Add(facility);
                            context.SaveChanges();

                            //Open Parking

                            //For two wheeler
                            if (noOf2Parkinglots > 0)
                            {
                                var flatParking = new FlatParking()
                                {
                                    Facility = new Facility() { Id = facility.Id },
                                    Type = (byte)EnParkingType.TwoWheeler
                                };

                                await flatParkingRepository.Create(flatParking, noOf2Parkinglots);
                            }

                            //For four wheeler
                            if (noOf4Parkinglots > 0)
                            {
                                var flatParking = new FlatParking()
                                {
                                    Facility = new Facility() { Id = facility.Id },
                                    Type = (byte)EnParkingType.FourWheeler
                                };

                                await flatParkingRepository.Create(flatParking, noOf4Parkinglots);
                            }

                            //For two/four wheeler
                            if (noOf24Parkinglots > 0)
                            {
                                var flatParking = new FlatParking()
                                {
                                    Facility = new Facility() { Id = facility.Id },
                                    Type = (byte)EnParkingType.TwoFourWheeler
                                };

                                await flatParkingRepository.Create(flatParking, noOf24Parkinglots);
                            }

                        }
                        else if (facility.Type == (byte)EnFacilityType.Building ||
                            facility.Type == (byte)EnFacilityType.ClubHouse ||
                            facility.Type == (byte)EnFacilityType.Gym ||
                            facility.Type == (byte)EnFacilityType.CommunityHall ||
                            facility.Type == (byte)EnFacilityType.CommercialSpace ||
                            facility.Type == (byte)EnFacilityType.School ||
                            facility.Type == (byte)EnFacilityType.Hospital)
                        {
                            //isWing = true;
                            //isNoOfFloors = true;
                            //isNoOfFlats = true;
                            //isNoOfLifts = true;
                            //isNoOfParkinglots = false;
                            context.Facilities.Add(facility);
                            context.SaveChanges();

                            int flatPerFloor = facility.NoOfFlats / facility.NoOfFloors;

                            for (int j = 0; j < facility.NoOfFloors + 1; j++)
                            {
                                //add Floor
                                var floor = new Floor()
                                {
                                    Name = (j == 0 ? "Ground Floor" : "Floor " + j.ToString()),
                                    FloorNumber = j,
                                    FacilityId = facility.Id,
                                    Type = (byte)EnFloorType.Floor
                                };

                                if (j > 0)
                                {
                                    //add Flats
                                    for (int k = 0; k < flatPerFloor; k++)
                                    {
                                        var flat = new Flat()
                                        {
                                            Name = ((floor.FloorNumber * 100) + (k + 1)).ToString()
                                        };

                                        floor.Flats.Add(flat);
                                    }
                                }

                                context.Floors.Add(floor);
                                context.SaveChanges();
                            }

                            if (facility.NoOfLifts > 0)
                            {
                                for (int i = 1; i <= facility.NoOfLifts; i++)
                                {
                                    context.SocietyAssets.Add(new SocietyAsset() { Name = "Lift " + i, FacilityId = facility.Id, ComplexId = facility.ComplexId, Quantity = 1 });
                                }
                                context.SaveChanges();
                            }

                        }
                        else if (facility.Type == (byte)EnFacilityType.Garden ||
                            facility.Type == (byte)EnFacilityType.SwimmingPool ||
                            facility.Type == (byte)EnFacilityType.PlayGround ||
                            facility.Type == (byte)EnFacilityType.PlayArea ||
                            facility.Type == (byte)EnFacilityType.Temple ||
                            facility.Type == (byte)EnFacilityType.Mosque ||
                            facility.Type == (byte)EnFacilityType.Curch ||
                            facility.Type == (byte)EnFacilityType.Gurudwada)
                        {
                            //do nothing
                            context.Facilities.Add(facility);
                            context.SaveChanges();

                            //scope.isWing = false;
                            //scope.isNoOfFloors = false;
                            //scope.isNoOfFlats = false;
                            //scope.isNoOfLifts = false;
                            //scope.isNoOfParkinglots = false;
                        }

                        //Map facility to society#
                        if (societyId.HasValue)
                        {
                            context.MapsSocietiesToFacilities.Add(new MapSocietiesToFacilities() { SocietyId = societyId.Value, FacilityId = facility.Id });
                        }
                        context.SaveChanges();
                    }

                    return facilities;
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
                    var existingRecord = context.Facilities.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Facility not found");
                    }

                    context.Facilities.Remove(existingRecord);
                    context.SaveChanges();

                }
            });
            return taskresult;
        }

        public Task<Facility> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Facilities
                        .Include(c => c.Complex)
                        .Include(c => c.Complex.City)
                        .Include(c => c.Complex.State)
                        .Include(c => c.Societies.Select(s => s.Society))
                        .Include(c => c.Societies.Select(s => s.Facility))
                        .Include(c => c.Floors)
                        .Include(c => c.FlatParkings)
                        .Include(c => c.SocietyAssets)
                        .Include(c => c.SocietyAssets.Select(s => s.Complex))
                        .Include(c => c.SocietyAssets.Select(s => s.Facility))
                        .Include(c => c.SocietyAssets.Select(s => s.Floor))
                        .Include(c => c.SocietyAssets.Select(s => s.Society))
                        //.Include(c => c.FlatParkings.Select(f => f.ParkingType))
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Facility not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Facility facility)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.Facilities.FirstOrDefault(p => p.Id == facility.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Facility detail not found");
                    }
                    if (facility.Complex != null)
                    {
                        existingRecord.ComplexId = facility.Complex.Id;
                        existingRecord.Complex = null;
                    }

                    var flatcount = await FlatCount(facility.Id);
                    if (facility.NoOfFlats < flatcount)
                    {
                        throw new Exception("Cannot update. More then " + facility.NoOfFlats.ToString() + " flats already added.");
                    }

                    var floorcount = await FloorCount(facility.Id);
                    if (facility.NoOfFloors < floorcount - 1) // - 1 for the ground floor
                    {
                        throw new Exception("Cannot update. More then " + facility.NoOfFloors.ToString() + " floors already added.");
                    }

                    existingRecord.Name = facility.Name;
                    existingRecord.Wing = facility.Wing;

                    //TODO Before updating NoOfFloors check total floors already added and then update
                    existingRecord.NoOfFloors = facility.NoOfFloors;

                    //TODO Before updating NoOfFlats check total flats already added and then update
                    existingRecord.NoOfFlats = facility.NoOfFlats;

                    //TODO Before updating NoOfLifts check total lifts already added and then update
                    existingRecord.NoOfLifts = facility.NoOfLifts;

                    //TODO Before updating NoOfParkinglots check total parking lots already added and then update
                    existingRecord.NoOfParkinglots = facility.NoOfParkinglots;

                    //cannot change facility if it is parking lot.
                    //existingRecord.IsParkingLot = facility.IsParkingLot;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Facility>> List(FacilitySearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var qry = context
                        .Facilities
                        .Include(c => c.Complex)
                        .Include(c => c.SocietyAssets)

                        .AsQueryable();

                    if (searchParams.ComplexId.HasValue)
                    {
                        qry = qry
                            .Where(f => f.ComplexId == searchParams.ComplexId.Value);
                    }

                    return qry
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<Facility>> SocietyWiseList(FacilitySearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var qry = context
                        .MapsSocietiesToFacilities
                        .Include(c => c.Facility)
                        .Include(c => c.Facility.Complex)
                        .Where(m => m.SocietyId == searchParams.SocietyId);

                    return qry
                        .Select(m => m.Facility)
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<Facility>> LoadFlats(FacilitySearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var fac = context
                        .MapsSocietiesToFacilities
                        .Where(m => m.SocietyId == searchParams.SocietyId)
                        .Select(m => m.FacilityId)
                        .ToList();

                    var qry = context
                        .Facilities
                        .Where(f => fac.Contains(f.Id))
                        .Include(f => f.Complex)
                        .Include(f => f.Floors)
                        .Include(f => f.Floors.Select(o => o.Flats))
                        .AsQueryable();

                    return qry
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<int> FlatCount(long facilityId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var flats = (from f in context.Facilities
                                 join flr in context.Floors on f.Id equals flr.FacilityId
                                 join fls in context.Flats on flr.Id equals fls.FloorId
                                 where f.Id == facilityId
                                 select fls)
                                 .Count();

                    return flats;
                }
            });
            return taskResult;
        }

        public Task<int> FloorCount(long facilityId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var floor = (from f in context.Facilities
                                 join flr in context.Floors on f.Id equals flr.FacilityId
                                 //join fls in context.Flats on flr.Id equals fls.FloorId
                                 where f.Id == facilityId
                                 select flr)
                                 .Count();

                    return floor;
                }
            });
            return taskResult;
        }

        public Task<List<Facility>> LoadParkings(FacilitySearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var parkingsTypes = new byte[] { (byte)EnFacilityType.Building, (byte)EnFacilityType.OpenParking, (byte)EnFacilityType.ParkingTower };

                    var flat = context
                        .Flats
                        .Include(f => f.Floor.Facility.Societies)
                        .FirstOrDefault(f => f.Id == searchParams.FlatId);

                    if (flat == null) return null;

                    var socId = flat.Floor.Facility.Societies.First().SocietyId;

                    var fac = context
                        .MapsSocietiesToFacilities
                        .Where(m => m.SocietyId == socId)
                        .Select(m => m.FacilityId)
                        .ToList();

                    var qry = context
                        .Facilities
                        .Where(f => fac.Contains(f.Id) &&
                            (f.Type == (byte)EnFacilityType.Building ||
                            f.Type == (byte)EnFacilityType.OpenParking ||
                            f.Type == (byte)EnFacilityType.ParkingTower))

                        .Include(f => f.FlatParkings)
                        .Include(f => f.Floors)
                        .Include(f => f.Floors.Select(o => o.FlatParkings));
                    //.AsQueryable();

                    return qry
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task LinkSocieties(long[] linkSocieties, long[] unlinkSocieties, long facilityId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .Facilities
                        .FirstOrDefault(p => p.Id == facilityId);

                    if (existing == null)
                    {
                        throw new Exception("Facility not found");
                    }

                    if (unlinkSocieties.Length > 0)
                    {
                        foreach (var item in unlinkSocieties)
                        {
                            var fac = context
                                .MapsSocietiesToFacilities
                                .FirstOrDefault(f => f.FacilityId == facilityId && f.SocietyId == item);

                            if (fac != null)
                            {
                                context.MapsSocietiesToFacilities.Remove(fac);
                            }
                        }

                        context.SaveChanges();
                    }

                    if (linkSocieties.Length > 0)
                    {
                        foreach (var item in linkSocieties)
                        {
                            var fac = context
                                .MapsSocietiesToFacilities
                                .FirstOrDefault(f => f.FacilityId == facilityId && f.SocietyId == item);

                            if (fac == null)
                            {
                                context.MapsSocietiesToFacilities.Add(new MapSocietiesToFacilities() { FacilityId = facilityId, SocietyId = item });
                            }
                        }

                        context.SaveChanges();
                    }

                }
            });
            return taskResult;
        }
    }
}


