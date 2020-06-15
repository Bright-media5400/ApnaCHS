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
    public interface IAttendanceService
    {
        Task<Attendance> Create(Attendance attendance);

        Task<Attendance> Read(int key);

        Task Update(Attendance attendance);

        Task Delete(int id);

        Task<List<Attendance>> List(AttendanceSearchParams searchParams);

    }
    public class AttendanceService : IAttendanceService
    {
         IAttendanceRepository _attendanceRepository = null;

         public AttendanceService()
        {
            _attendanceRepository = new AttendanceRepository();
        }

      public Task<Attendance> Create(Attendance attendance)
        {
            return _attendanceRepository.Create(attendance);
        }

        public Task Delete(int id)
        {
            return _attendanceRepository.Delete(id);
        }

        public Task<List<Attendance>> List(AttendanceSearchParams searchParams)
        {
            return _attendanceRepository.List(searchParams);
        }

        public Task<Attendance> Read(int key)
        {
            return _attendanceRepository.Read(key);
        }

        public Task Update(Attendance attendance)
        {
            return _attendanceRepository.Update(attendance);
        }
    }
}
