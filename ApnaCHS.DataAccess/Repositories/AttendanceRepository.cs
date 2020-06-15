using ApnaCHS.Common;
using ApnaCHS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApnaCHS.DataAccess.Repositories
{
    public interface IAttendanceRepository
    {
        Task<Attendance> Create(Attendance attendance);

        Task<Attendance> Read(int key);

        Task Update(Attendance attendance);

        Task Delete(int id);

        Task<List<Attendance>> List(AttendanceSearchParams searchParams);
    }
    public class AttendanceRepository : IAttendanceRepository
    {
        public Task<Attendance> Create(Attendance attendance)
       {
           var taskResult = Task.Run(() =>
           {
               using (var context = new DbContext())
               {
                   if (attendance.SecurityStaff != null)
                   {
                       attendance.SecurityStaffId = attendance.SecurityStaff.Id;
                       attendance.SecurityStaff = null;
                   }
                   if (attendance.SocietyStaff != null)
                   {
                       attendance.SocietyStaffId = attendance.SocietyStaff.Id;
                       attendance.SocietyStaff = null;
                   }
                   if (attendance.ShiftType != null)
                   {
                       attendance.ShiftTypeId = attendance.ShiftType.Id;
                       attendance.ShiftType = null;
                   }
                   context.AttendanceList.Add(attendance);
                   context.SaveChanges();

                   return attendance;
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
                    var existingRecord = context.AttendanceList.FirstOrDefault(p => p.Id == id);
                    if (existingRecord == null)
                    {
                        throw new Exception("Data not found");
                    }

                    context.AttendanceList.Remove(existingRecord);

                    context.SaveChanges();
                }
            });
            return taskresult;
        }

        public Task<Attendance> Read(int id)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existing = context.AttendanceList.FirstOrDefault(p => p.Id == id);

                    if (existing == null)
                    {
                        throw new Exception("Data not found");
                    }
                    return existing;
                }

            });
            return taskResult;
        }

        public Task Update(Attendance attendance)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var existingRecord = context.AttendanceList.FirstOrDefault(p => p.Id == attendance.Id);

                    if (existingRecord == null)
                    {
                        throw new Exception("Data detail not found");
                    }
                    if (attendance.SecurityStaff != null)
                    {
                        attendance.SecurityStaffId = attendance.SecurityStaff.Id;
                        attendance.SecurityStaff = null;
                    }
                    if (attendance.SocietyStaff != null)
                    {
                        attendance.SocietyStaffId = attendance.SocietyStaff.Id;
                        attendance.SocietyStaff = null;
                    }
                    if (attendance.ShiftType != null)
                    {
                        attendance.ShiftTypeId = attendance.ShiftType.Id;
                        attendance.ShiftType = null;
                    }
                    existingRecord.Day = attendance.Day;
                    existingRecord.InTime= attendance.InTime;
                    existingRecord.OutTime = attendance.OutTime;
             
                    context.SaveChanges();
                }
            });
            return taskResult;
        }
        public Task<List<Attendance>> List(AttendanceSearchParams searchParams)
        {
            var taskResult = Task.Run(() =>
            {
                using (var context = new DbContext())
                {
                    var ctx = context.AttendanceList.ToList();

                    return ctx;
                }
            });
            return taskResult;
        }
    }
}
