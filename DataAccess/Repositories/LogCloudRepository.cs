using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogCloudRepository : ILogRepository
    {
        public void AddLog(Log l)
        {
            throw new NotImplementedException(); //cloud is off topic. will do these in Programming for the Cloud
        }

        public IQueryable<Log> GetLogs()
        {
            throw new NotImplementedException();
        }
    }


    public class LogEmailRepository : ILogRepository
    {
        public void AddLog(Log l)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> GetLogs()
        {
            throw new NotImplementedException();
        }
    }
}
