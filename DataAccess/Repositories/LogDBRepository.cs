using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    /// <summary>
    /// LogDBRepository logs the messages passed into a database
    /// </summary>
    public class LogDBRepository: ILogRepository
    {
        private AttendanceContext myContext;
        public LogDBRepository(AttendanceContext _context) //constructor injection
        {
            myContext = _context;
        }

        public void AddLog(Log l)
        {
            myContext.Logs.Add(l);
            myContext.SaveChanges();
        }

        public IQueryable<Log> GetLogs()
        {
            return myContext.Logs;
        }
    }
}
