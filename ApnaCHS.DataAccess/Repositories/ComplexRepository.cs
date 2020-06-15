using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ApnaCHS.AppCommon;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IComplexRepository
    {
        Task<Complex> Create(Complex complex, ApplicationUser currentUser);

        Task<Complex> Read(int key);

        Task Update(Complex complex);

        Task Delete(int id);

        Task<List<Complex>> List(ComplexSearchParams searchParams);

        Task<List<AllIndiaPincode>> ReadArea(int pincode);

        Task UpdateLoginDetails(Complex complex);

    }

    public class ComplexRepository : IComplexRepository
    {
        UserRepository _userRepository = null;

        public ComplexRepository()
        {
            _userRepository = new UserRepository();
        }

        public Task<Complex> Create(Complex complex, ApplicationUser currentUser)
        {
            var taskResult = Task.Run(async () =>
            {
                using (var context = new DbContext())
                {
                    //check NoOfGate
                    //TODO check assets

                    if (context.ComplexList.Any(r => r.RegistrationNo == complex.RegistrationNo))
                    {
                        throw new Exception("Complex already registered with this Registration number.");
                    }
                    //check NoOfSocieties
                    if (complex.NoOfSocieties != complex.Societies.Count)
                    {
                        throw new Exception("No of societies count does not match with entered society list names count");
                    }

                    //check NoOfBuilding
                    if (complex.NoOfBuilding != complex.Facilities.Count(f => f.Type == (byte)EnFacilityType.Building))
                    {
                        throw new Exception("No of building count does not match with entered building list names count");
                    }

                    //check NoOfAmenities
                    if (complex.NoOfAmenities != complex.Facilities.Count(f => f.Type != (byte)EnFacilityType.Building))
                    {
                        throw new Exception("No of amenities count does not match with entered amenity list names count");
                    }

                    //Check duplicate society
                    if (complex.Societies.GroupBy(f => f.Name).Any(f => f.Count() > 1))
                    {
                        throw new Exception("Duplicate names found. All societies should have different name");
                    }

                    //Check duplicate facilities
                    if (complex.Facilities.GroupBy(f => f.Name + "|" + f.Wing).Any(f => f.Count() > 1))
                    {
                        throw new Exception("Duplicate names found. All facilities should have different name");
                    }

                    //check date of DateOfRegistration
                    if (complex.DateOfRegistration < complex.DateOfIncorporation)
                    {
                        throw new Exception("Date of registration cannot be before date of incorporation of the complex");
                    }

                    var facilities = complex
                        .Facilities
                        .Where(f => f.Type == (byte)EnFacilityType.Building)
                        .Select(f => f.Name + '|' + f.Wing)
                        .ToList();

                    foreach (var item in complex.Facilities)
                    {
                        if (item.Type == (byte)EnFacilityType.Building)
                        {
                            item.Name = item.Name.Split('|')[0];
                        }
                    }

                    //copy details from complex to society 
                    foreach (var item in complex.Societies)
                    {
                        item.RegistrationNo = complex.RegistrationNo;
                        item.DateOfIncorporation = complex.DateOfIncorporation;
                        item.DateOfRegistration = complex.DateOfRegistration;
                        item.ContactPerson = complex.ContactPerson;
                        item.PhoneNo = complex.PhoneNo;
                        item.Email = complex.Email;

                        item.BillingCycle = ProgramCommon.BillingCycle;
                        item.DueDays = ProgramCommon.DueDays;
                        item.ApprovalsCount = 1;
                    }

                    foreach (var item in complex.Facilities)
                    {
                        if (item.Type == (byte)EnFacilityType.OpenParking)
                        {
                            item.IsParkingLot = true;
                        }
                    }

                    if (complex.City != null)
                    {
                        complex.CityId = complex.City.Id;
                        complex.City = null;
                    }

                    if (complex.State != null)
                    {
                        complex.StateId = complex.State.Id;
                        complex.State = null;
                    }

                    //assets
                    var assets = new List<SocietyAsset>();
                    assets.AddRange(complex.SocietyAssets);

                    complex.SocietyAssets = null;
                    context.ComplexList.Add(complex);
                    context.SaveChanges();


                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = ProgramCommon.GetFormattedUsername(complex.ContactPerson),
                        Email = complex.Email,
                        Name = complex.ContactPerson,
                        MaxAttempts = ProgramCommon.MaxAttempts,
                        bBlocked = false,
                        bChangePass = true,
                        Deleted = false,
                        CreatedBy = currentUser.UserName,
                        ModifiedBy = currentUser.UserName,
                        PhoneNumber = complex.PhoneNo,
                        IsBack = false,
                        IsDefault = true
                    };

                    //create complex user 
                    await _userRepository.CreateComplexUser(user, complex.Id, currentUser);

                    //create society user
                    foreach (var item in complex.Societies)
                    {
                        await _userRepository.CreateSocietyUser(user, item.Id, currentUser);
                    }

                    //try
                    //{

                    //After saving facilities and societies map them
                    if (complex.Type == (byte)EnComplexType.SingleSociety)
                    {
                        //For single society
                        var society = complex.Societies.First();
                        var facilities1 = complex.Facilities.Where(f => f.Type == (byte)EnFacilityType.Building).ToList();

                        foreach (var item in facilities1)
                        {
                            context.MapsSocietiesToFacilities.Add(new MapSocietiesToFacilities() { SocietyId = society.Id, FacilityId = item.Id });
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        //For group of societies
                        foreach (var item in facilities)
                        {
                            var spilt = item.Split('|');
                            if (spilt.Length >= 3)
                            {
                                var facName = spilt[0];
                                var socName = spilt[1];
                                var wingName = spilt[2];

                                var fac = complex.Facilities.Where(f => f.Name == facName && f.Wing == wingName).First();
                                var soc = complex.Societies.Where(s => s.Name == socName).First();

                                context.MapsSocietiesToFacilities.Add(new MapSocietiesToFacilities() { SocietyId = soc.Id, FacilityId = fac.Id });
                            }
                        }

                        context.SaveChanges();
                    }

                    //add building assets
                    var buildingassets = assets.Where(a => a.Facility != null).ToList();
                    foreach (var item in buildingassets)
                    {
                        //After saving facilities and societies map them
                        if (complex.Type == (byte)EnComplexType.SingleSociety)
                        {
                            var spilt = item.Facility.Name.Split('|');
                            if (spilt.Length >= 2)
                            {
                                var facName = spilt[0];
                                var wingName = item.Facility.Wing;

                                var fac = complex.Facilities.Where(f => f.Name == facName && f.Wing == wingName).First();

                                context.SocietyAssets.Add(new SocietyAsset() { Name = item.Name, FacilityId = fac.Id, ComplexId = complex.Id, Quantity = 1 });
                            }
                        }
                        else
                        {
                            var spilt = item.Facility.Name.Split('|');
                            if (spilt.Length >= 2)
                            {
                                var facName = spilt[0];
                                var wingName = item.Facility.Wing;

                                var fac = complex.Facilities.Where(f => f.Name == facName && f.Wing == wingName).First();

                                context.SocietyAssets.Add(new SocietyAsset() { Name = item.Name, FacilityId = fac.Id, ComplexId = complex.Id, Quantity = 1 });
                            }
                        }
                    }
                    context.SaveChanges();


                    //var buildings = complex.Facilities.Where(f => f.Type == (byte)EnFacilityType.Building).ToList();
                    //for (int i = 0; i < buildings.Count; i++)
                    //{
                    //var building = buildings[i];

                    ////Code to add floors
                    //if (building.NoOfFloors == 0) continue;

                    //int flatPerFloor = building.NoOfFlats / building.NoOfFloors;

                    //for (int j = 0; j < building.NoOfFloors + 1; j++)
                    //{
                    //    //add Floor
                    //    var floor = new Floor()
                    //    {
                    //        Name = (j == 0 ? "Ground Floor" : "Floor " + j.ToString()),
                    //        FloorNumber = j,
                    //        FacilityId = building.Id,
                    //        Type = (j == 0 ? (byte)EnFloorType.Parkings : (byte)EnFloorType.Floor)
                    //    };

                    //    if (j > 0)
                    //    {
                    //        //add Flats
                    //        for (int k = 0; k < flatPerFloor; k++)
                    //        {
                    //            var flat = new Flat()
                    //            {
                    //                Name = ((floor.FloorNumber * 100) + (k + 1)).ToString()
                    //            };

                    //            floor.Flats.Add(flat);
                    //        }
                    //    }

                    //    context.Floors.Add(floor);
                    //    context.SaveChanges();
                    //}
                    //}


                    //}
                    //catch (Exception)
                    //{

                    //}

                    //add complex assets 
                    var complexassets = assets.Where(a => a.Facility == null).ToList();
                    foreach (var item in complexassets)
                    {
                        context.SocietyAssets.Add(new SocietyAsset() { Name = item.Name, ComplexId = complex.Id, Quantity = 1 });
                    }
                    context.SaveChanges();

                    CreateNewArea(complex.Area, complex.Pincode);

                    return complex;
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
                    var existingRecord = context.ComplexList.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Complex not found");
                    }

                    context.ComplexList.Remove(existingRecord);
                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<Complex> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .ComplexList
                        .Include(c => c.City)
                        .Include(c => c.State)
                        .Include(c => c.Facilities)
                        //.Include(c => c.Facilities.Select(f => f.Societies.Select(s => s.Society)))
                        .Include(c => c.Societies)
                        .Include(c => c.SocietyAssets)
                        .Include(c => c.SocietyAssets.Select(s => s.Complex))
                        .Include(c => c.SocietyAssets.Select(s => s.Facility))
                        .Include(c => c.SocietyAssets.Select(s => s.Floor))
                        .Include(c => c.SocietyAssets.Select(s => s.Society))
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Complex not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Complex complex)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.ComplexList.FirstOrDefault(p => p.Id == complex.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Complex detail not found");
                    }

                    if (complex.City != null)
                    {
                        complex.CityId = complex.City.Id;
                        complex.City = null;
                    }

                    if (complex.State != null)
                    {
                        complex.StateId = complex.State.Id;
                        complex.State = null;
                    }
                    existingRecord.Type = complex.Type;
                    existingRecord.Name = complex.Name;
                    existingRecord.Address = complex.Address;
                    existingRecord.Area = complex.Area;
                    existingRecord.DateOfIncorporation = complex.DateOfIncorporation;
                    existingRecord.DateOfRegistration = complex.DateOfRegistration;
                    existingRecord.RegistrationNo = complex.RegistrationNo;
                    existingRecord.NoOfBuilding = complex.NoOfBuilding;
                    existingRecord.NoOfGate = complex.NoOfGate;
                    existingRecord.NoOfAmenities = complex.NoOfAmenities;
                    existingRecord.Pincode = complex.Pincode;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<Complex>> List(ComplexSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .ComplexList
                        .Include(c => c.City)
                        .Include(c => c.State)
                        .Include(c => c.Facilities)
                        //.Include(c => c.Facilities.Select(f => f.Societies.Select(s => s.Society)))
                        .Include(c => c.Societies)
                        .Include(c => c.SocietyAssets);

                    if (searchParams.City.HasValue)
                    {
                        ctx = ctx.Where(c => c.CityId == searchParams.City.Value);
                    }

                    if (searchParams.State.HasValue)
                    {
                        ctx = ctx.Where(c => c.StateId == searchParams.State.Value);
                    }

                    if (searchParams.AmenityType.HasValue)
                    {
                        ctx = ctx.Where(c => c.Facilities.Any(f => f.Type == searchParams.AmenityType.Value));
                    }

                    if (!string.IsNullOrEmpty(searchParams.ComplexName))
                    {
                        ctx = ctx.Where(c => c.Name.Contains(searchParams.ComplexName));
                    }

                    if (!string.IsNullOrEmpty(searchParams.SocietyName))
                    {
                        ctx = ctx.Where(c => c.Societies.Any(f => f.Name.Contains(searchParams.SocietyName)));
                    }

                    return ctx
                        .ToList();
                }
            });
            return taskResult;
        }

        public Task<List<AllIndiaPincode>> ReadArea(int pincode)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    return context
                        .AllIndiaPincodes
                        .Where(p => p.Pincode == pincode)
                        .ToList();
                }

            });
            return taskResult;
        }

        public Task UpdateLoginDetails(Complex complex)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.ComplexList.FirstOrDefault(p => p.Id == complex.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Complex detail not found");
                    }

                    if (complex.City != null)
                    {
                        complex.CityId = complex.City.Id;
                        complex.City = null;
                    }

                    if (complex.State != null)
                    {
                        complex.StateId = complex.State.Id;
                        complex.State = null;
                    }

                    existingRecord.Email = complex.Email;
                    existingRecord.PhoneNo = complex.PhoneNo;
                    existingRecord.ContactPerson = complex.ContactPerson;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task CreateNewArea(string areaName, int pincode)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {

                    var existingRecord = context.AllIndiaPincodes.FirstOrDefault(r => r.Pincode == pincode && r.OfficeName == areaName);

                    if (existingRecord == null)
                    {
                        var existing = context.AllIndiaPincodes.FirstOrDefault(r => r.Pincode == pincode);

                        var allIndiaPincode = new AllIndiaPincode();

                        allIndiaPincode.OfficeName = areaName;
                        allIndiaPincode.Pincode = existing.Pincode;
                        allIndiaPincode.OfficeType = existing.OfficeType;
                        allIndiaPincode.DeliveryStatus = existing.DeliveryStatus;
                        allIndiaPincode.DivisionName = existing.DivisionName;
                        allIndiaPincode.RegionName = existing.RegionName;
                        allIndiaPincode.CircleName = existing.CircleName;
                        allIndiaPincode.Taluk = existing.Taluk;
                        allIndiaPincode.DistrictName = existing.DistrictName;
                        allIndiaPincode.StateName = existing.StateName;
                        allIndiaPincode.Telephone = existing.Telephone;
                        allIndiaPincode.RelatedSuboffice = existing.RelatedSuboffice;
                        allIndiaPincode.RelatedHeadoffice = existing.RelatedHeadoffice;
                        allIndiaPincode.Latitude = existing.Latitude;
                        allIndiaPincode.Longitude = existing.Longitude;

                        context.AllIndiaPincodes.Add(allIndiaPincode);
                        context.SaveChanges();
                    }
                }
            });
            return taskResult;
        }
    }
}
