using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SubjectRepository
    {
        private AttendanceContext myContext;

        //we are assuming that the AttendanceContext instance has already be created somewhere else
        public SubjectRepository(AttendanceContext _context)
        {
            myContext = _context;
        }

        public IQueryable<Subject> GetSubjects()
        {
            return myContext.Subjects;
        }


        public Subject GetSubject(string code)
        {
            return myContext.Subjects.SingleOrDefault(x => x.Code == code);
        }
    }
}
