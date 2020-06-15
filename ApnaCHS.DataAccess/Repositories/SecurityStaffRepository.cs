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
    public interface ISecurityStaffRepository
    {
        Task<SecurityStaff> Create(SecurityStaff securityStaff);

        Task<SecurityStaff> Read(int key);

        Task Update(SecurityStaff securityStaff);

        Task UpdateLastWorkingDay(SecurityStaff securityStaff);

        Task Delete(int id);

        Task<List<SecurityStaff>> List(SecurityStaffSearchParams searchParams);
    }


   public class SecurityStaffRepository : ISecurityStaffRepository
    {
       public Task<SecurityStaff> Create(SecurityStaff securityStaff)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   var society = context
                                .Societies
                                .FirstOrDefault(s => s.Id == securityStaff.Society.Id);

                   if (society == null)
                   {
                       throw new Exception("Society not found");
                   }

                   if (securityStaff.JoiningDate < society.DateOfIncorporation)
                   {
                       throw new Exception("Joining date cannot be before date of incorporation of the society");
                   }

                   if (securityStaff.DateOfBirth.HasValue && securityStaff.JoiningDate < securityStaff.DateOfBirth.Value)
                   {
                       throw new Exception("Joining since date cannot be before Date of Birth");
                   }

                   if (securityStaff.Society != null)
                   {
                       securityStaff.SocietyId = securityStaff.Society.Id;
                       securityStaff.Society = null;
                   }
                   if (securityStaff.ShiftType != null)
                   {
                       securityStaff.ShiftTypeId = securityStaff.ShiftType.Id;
                       securityStaff.ShiftType = null;
                   }
                   context.SecurityStaffList.Add(securityStaff);
                   context.SaveChanges();

                   return securityStaff;
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
                   var existingRecord = context.SecurityStaffList.FirstOrDefault(p => p.Id == id);
                   if (existingRecord == null)
                   {
                       throw new Exception("Security Staff not found");
                   }

                   context.SecurityStaffList.Remove(existingRecord);

                   context.SaveChanges();
               }
           });
           return taskresult;
       }
       
       public Task<SecurityStaff> Read(int id)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   var existing = context
                       .SecurityStaffList
                       .Include(s => s.ShiftType)
                       .Include(s => s.Society)
                       .FirstOrDefault(p => p.Id == id);

                   if (existing == null)
                   {
                       throw new Exception("Security Staff not found");
                   }
                   return existing;
               }

           });
           return taskResult;
       }
       
       public Task Update(SecurityStaff securityStaff)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   var existingRecord = context.SecurityStaffList.FirstOrDefault(p => p.Id == securityStaff.Id);

                   if (existingRecord == null)
                   {
                       throw new Exception("Security Staff detail not found");
                   }

                   var society = context
                                .Societies
                                .FirstOrDefault(s => s.Id == existingRecord.Society.Id);

                   if (society == null)
                   {
                       throw new Exception("Society not found");
                   }

                   if (securityStaff.JoiningDate < society.DateOfIncorporation)
                   {
                       throw new Exception("Joining date cannot be before date of incorporation of the society");
                   }

                   if (securityStaff.DateOfBirth.HasValue && securityStaff.JoiningDate < securityStaff.DateOfBirth.Value)
                   {
                       throw new Exception("Joining since date cannot be before Date of Birth");
                   }


                   if (securityStaff.Society != null)
                   {
                       securityStaff.SocietyId = securityStaff.Society.Id;
                       securityStaff.Society = null;
                   }
                   if (securityStaff.ShiftType != null)
                   {
                       securityStaff.ShiftTypeId = securityStaff.ShiftType.Id;
                       securityStaff.ShiftType = null;
                   }
                   existingRecord.Name = securityStaff.Name;
                   existingRecord.PhoneNo = securityStaff.PhoneNo;
                   existingRecord.AadharCardNo = securityStaff.AadharCardNo;
                   existingRecord.Photo = securityStaff.Photo;
                   existingRecord.DateOfBirth = securityStaff.DateOfBirth;
                   existingRecord.Address = securityStaff.Address;
                   existingRecord.NativeAddress=securityStaff.NativeAddress;
                   existingRecord.JoiningDate = securityStaff.JoiningDate;
                   
                   context.SaveChanges();
               }
           });
           return taskResult;
       }

       public Task UpdateLastWorkingDay(SecurityStaff securityStaff)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   var existingRecord = context.SecurityStaffList.FirstOrDefault(p => p.Id == securityStaff.Id);

                   if (existingRecord == null)
                   {
                       throw new Exception("Security Staff detail not found");
                   }
                   if (existingRecord.JoiningDate > securityStaff.LastWorkingDay)
                   {
                       throw new Exception("Last working day cannot be before date of joining");
                   }
                   if (securityStaff.Society != null)
                   {
                       securityStaff.SocietyId = securityStaff.Society.Id;
                       securityStaff.Society = null;
                   }
                   existingRecord.LastWorkingDay = securityStaff.LastWorkingDay;

                   context.SaveChanges();
               }
           });
           return taskResult;
       }
       
       public Task<List<SecurityStaff>> List(SecurityStaffSearchParams searchParams)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   var ctx = context
                       .SecurityStaffList
                       .Include(s => s.ShiftType)
                       .Include(s => s.Society);
                   if (searchParams.SocietyId.HasValue)
                   {
                       ctx = ctx.Where(c => c.SocietyId == searchParams.SocietyId.Value);
                   }

                   return ctx.ToList();;
               }
           });
           return taskResult;
       }
    }
}
