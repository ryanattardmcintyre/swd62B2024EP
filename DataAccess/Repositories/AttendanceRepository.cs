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

        public   IQueryable<Attendance> GetAttendances(string subjectCode, DateTime dt, string groupCode) {

            return myContext.Attendances.Where(x => x.SubjectFK == subjectCode
                                                    && x.Timestamp.Day == dt.Day
                                                    && x.Timestamp.Hour == dt.Hour
                                                    && x.Timestamp.Minute == dt.Minute
                                                    && x.Student.GroupFK == groupCode
                                                    );

            /* //same outcome as above however using raw LINQ
            var list = from a in myContext.Attendances
                       join s in myContext.Students on a.StudentFK equals s.IdCard
                       where s.GroupFK == groupCode && a.Timestamp.Day == dt.Day 
                              && a.Timestamp.Hour == dt.Hour
                                                    && a.Timestamp.Minute == dt.Minute
                                                    && a.SubjectFK == subjectCode
                       select a;

            */

        }


    }
}
