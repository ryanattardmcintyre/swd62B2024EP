using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AttendanceRepository
    {
        //CRUD

        private AttendanceContext myContext; 
        public AttendanceRepository(AttendanceContext _context)
        {
            myContext = _context;
        }

        public void AddAttendance(Attendance attendance)
        {
            attendance.Timestamp= DateTime.Now;

            myContext.Attendances.Add(attendance);
            myContext.SaveChanges();

        }

        public void AddAttendances(List<Attendance> attendances)
        {
            foreach (var attendance in attendances)
            {
                attendance.Timestamp = DateTime.Now;
                myContext.Attendances.Add(attendance);
            }
            myContext.SaveChanges(); //is being called only once at the of the Add operation...
        }


        public   IQueryable<Attendance> GetAttendances(string subjectCode, string groupCode) {

            return myContext.Attendances.Where(x => x.SubjectFK == subjectCode
                                                    && x.Student.GroupFK == groupCode
                                                    );
        }


    }
}
