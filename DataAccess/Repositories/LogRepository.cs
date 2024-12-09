using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogRepository
    {
        private AttendanceContext myContext;
        public LogRepository(AttendanceContext _context)
        {
            myContext = _context;
        }

        public void AddLog(Log l)
        {
            myContext.Logs.Add(l);
            myContext.SaveChanges();
        }
    }
}
