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
    public interface IFlatParkingRepository
    {
        Task<FlatParking> Create(FlatParking flatParking, int? count);

        Task Create(int totalParkings, long? floorId, long? facilityId);

        Task<FlatParking> Read(int key);

        Task Update(FlatParking flatParking);

        Task Delete(int id);

        Task<List<FlatParking>> List(FlatParkingSearchParams searchParams);

        Task Assign(long flatid, long[] parkingids);
    }
    public class FlatParkingRepository : IFlatParkingRepository
    {
        public Task<FlatParking> Create(FlatParking flatParking, int? count)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var cnt = (!count.HasValue || count.Value == 0 || count.Value == 1) ? 1 : count.Value;

                    if (flatParking.Floor != null)
                    {
                        //On floor parking
                        var floor = context.Floors.First(f => f.Id == flatParking.Floor.Id);
                        var parkings = context.FlatParkings.Count(f => f.FloorId == flatParking.Floor.Id);

                        for (int i = 0; i < cnt; i++)
                        {
                            var newF = new FlatParking();

                            newF.FloorId = flatParking.Floor.Id;
                            newF.FacilityId = flatParking.Facility.Id;
                            newF.SqftArea = flatParking.SqftArea;
                            newF.Type = flatParking.Type;

                            if (cnt != 1)
                            {
                                var no = (floor.FloorNumber * 100) + parkings + (i + 1);
                                newF.Name = (no).ToString();
                                newF.ParkingNo = no;
                            }

                            context.FlatParkings.Add(newF);
                            flatParking.Id = newF.Id;
                        }

                        context.SaveChanges();
                        return flatParking;
                    }
                    else
                    {
                        //Open Parking
                        var parkings = context.FlatParkings.Count(f => f.FloorId == flatParking.Facility.Id);

                        for (int i = 0; i < cnt; i++)
                        {
                            var newF = new FlatParking();

                            newF.FacilityId = flatParking.Facility.Id;
                            newF.SqftArea = flatParking.SqftArea;
                            newF.Type = flatParking.Type;

                            if (cnt != 1)
                            {
                                var no = parkings + (i + 1);
                                newF.Name = (no).ToString();
                                newF.ParkingNo = no;
                            }

                            context.FlatParkings.Add(newF);
                            flatParking.Id = newF.Id;
                        }

                        context.SaveChanges();
                        return flatParking;
                    }
                }
            });
            return taskResult;
        }

        public Task Create(int totalParkings, long? floorId, long? facilityId)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    for (int i = 0; i < totalParkings; i++)
                    {
                        var flatParking = new FlatParking();
                        flatParking.Name = string.Format("{0}-{1}", "P ", i + 1);

                        if (floorId.HasValue)
                        {
                            flatParking.FloorId = floorId.Value;
                        }

                        if (facilityId.HasValue)
                        {
                            flatParking.FacilityId = facilityId.Value;
                        }

                        context.FlatParkings.Add(flatParking);
                    }                    
                    context.SaveChanges();
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
                    var existingRecord = context.FlatParkings.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("FlatParking not found");
                    }

                    existingRecord.FlatId = null;
                    //context.FlatParkings.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }
        
        public Task<FlatParking> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .FlatParkings
                        .Include(f => f.Facility)
                        .Include(f => f.Floor)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Flat Parking not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }
        
        public Task Update(FlatParking flatParking)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.FlatParkings.FirstOrDefault(p => p.Id == flatParking.Id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Flat Parking not found");
                    }
                                                           
                    existingRecord.Name = flatParking.Name;
                    existingRecord.SqftArea = flatParking.SqftArea;
                    existingRecord.Type = flatParking.Type;
                    existingRecord.ParkingNo = flatParking.ParkingNo;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }
        
        public Task<List<FlatParking>> List(FlatParkingSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .FlatParkings
                        .Include(f => f.Facility)
                        .Include(f => f.Floor)
                        .ToList();

                    return ctx;
                }
            });
            return taskResult;
        }

        public Task Assign(long flatid, long[] parkingids)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var flat = context.Flats.FirstOrDefault(p => p.Id == flatid);
                    if (flat == null)
                    {
                        throw new Exception("Flat detail not found");
                    }

                    foreach (var parkingid in parkingids)
                    {
                        var parking = context
                            .FlatParkings
                            .FirstOrDefault(p => p.Id == parkingid);

                        if (parking == null)
                        {
                            if (parkingids.Length == 1)
                            {
                                throw new Exception("Flat Parking detail not found");
                            }
                            else
                            {
                                continue;
                            }
                        }


                        if (parking.FlatId.HasValue)
                        {
                            if (parkingids.Length == 1)
                            {
                                throw new Exception("Cannot assign. Parking assigned to another flat.");
                            }
                            else
                            {
                                continue;
                            }
                        }

                        parking.FlatId = flatid;                       
                    }

                    context.SaveChanges();
                }
            });
            return taskResult;
        }
    }
}
