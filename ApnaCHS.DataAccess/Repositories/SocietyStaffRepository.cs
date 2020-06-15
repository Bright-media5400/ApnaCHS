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
    public interface ISocietyStaffRepository
    {
        Task<SocietyStaff> Create(SocietyStaff societyStaff);

        Task<SocietyStaff> Read(int key);

        Task Update(SocietyStaff societyStaff);

        Task UpdateLastWorkingDay(SocietyStaff societyStaff);

        Task Delete(int id);

        Task<List<SocietyStaff>> List(SocietyStaffSearchParams searchParams);
    }

    public class SocietyStaffRepository : ISocietyStaffRepository
    {
        public Task<SocietyStaff> Create(SocietyStaff societyStaff)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var society = context
                                 .Societies
                                 .FirstOrDefault(s => s.Id == societyStaff.Society.Id);

                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    if (societyStaff.JoiningDate < society.DateOfIncorporation)
                    {
                        throw new Exception("Joining date cannot be before date of incorporation of the society");
                    }

                    if (societyStaff.DateOfBirth.HasValue && societyStaff.JoiningDate < societyStaff.DateOfBirth.Value)
                    {
                        throw new Exception("Joining since date cannot be before Date of Birth");
                    }

                    if (societyStaff.StaffType != null)
                    {
                        societyStaff.StaffTypeId = societyStaff.StaffType.Id;
                        societyStaff.StaffType = null;
                    }
                    if (societyStaff.Society != null)
                    {
                        societyStaff.SocietyId = societyStaff.Society.Id;
                        societyStaff.Society = null;
                    }
                    context.SocietyStaffList.Add(societyStaff);
                    context.SaveChanges();

                    return societyStaff;
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
                    var existingRecord = context.SocietyStaffList.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Society Staff not found");
                    }

                    context.SocietyStaffList.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<SocietyStaff> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context
                        .SocietyStaffList
                        .Include(s => s.StaffType)
                        .Include(s => s.Society)
                        .FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Society Staff not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(SocietyStaff societyStaff)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .SocietyStaffList
                        .Include(s => s.StaffType)
                        .Include(s => s.Society)
                        .FirstOrDefault(p => p.Id == societyStaff.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society Staff detail not found");
                    }

                    var society = context
                                 .Societies
                                 .FirstOrDefault(s => s.Id == existingRecord.Society.Id);

                    if (society == null)
                    {
                        throw new Exception("Society not found");
                    }

                    if (societyStaff.JoiningDate < society.DateOfIncorporation)
                    {
                        throw new Exception("Joining date cannot be before date of incorporation of the society");
                    }

                    if (societyStaff.DateOfBirth.HasValue && societyStaff.JoiningDate < societyStaff.DateOfBirth.Value)
                    {
                        throw new Exception("Joining since date cannot be before Date of Birth");
                    }

                    if (societyStaff.StaffType != null)
                    {
                        societyStaff.StaffTypeId = societyStaff.StaffType.Id;
                        societyStaff.StaffType = null;
                    }
                    if (societyStaff.Society != null)
                    {
                        societyStaff.SocietyId = societyStaff.Society.Id;
                        societyStaff.Society = null;
                    }
                    existingRecord.Name = societyStaff.Name;
                    existingRecord.PhoneNo = societyStaff.PhoneNo;
                    existingRecord.AadharCardNo = societyStaff.AadharCardNo;
                    existingRecord.Photo = societyStaff.Photo;
                    existingRecord.DateOfBirth = societyStaff.DateOfBirth;
                    existingRecord.Address = societyStaff.Address;
                    existingRecord.NativeAddress = societyStaff.NativeAddress;
                    existingRecord.JoiningDate = societyStaff.JoiningDate;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task UpdateLastWorkingDay(SocietyStaff societyStaff)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context
                        .SocietyStaffList
                        .Include(s => s.StaffType)
                        .Include(s => s.Society)
                        .FirstOrDefault(p => p.Id == societyStaff.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Society Staff detail not found");
                    }
                    if (existingRecord.JoiningDate > societyStaff.LastWorkingDay)
                    {
                        throw new Exception("Last working day cannot be before date of joining");
                    }
                    if (societyStaff.Society != null)
                    {
                        societyStaff.SocietyId = societyStaff.Society.Id;
                        societyStaff.Society = null;
                    }
                    existingRecord.LastWorkingDay = societyStaff.LastWorkingDay;

                    context.SaveChanges();
                }
            });
            return taskResult;
        }

        public Task<List<SocietyStaff>> List(SocietyStaffSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context
                        .SocietyStaffList
                        .Include(s => s.StaffType)
                        .Include(s => s.Society);

                    if (searchParams.SocietyId.HasValue)
                    {
                        ctx = ctx.Where(c => c.SocietyId == searchParams.SocietyId.Value);
                    }


                    return ctx.ToList();
                }
            });
            return taskResult;
        }
    }
}
