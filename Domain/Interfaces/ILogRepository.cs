using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    //1. interface cannot be instantiated
    //2. interface dictates what to be implemented in the derived classes
    //3. interface can be used as a base-reference for any different implementations
    public interface ILogRepository
    {
        void AddLog(Log l); //for sure if i decide to inherit this interface it MUST have implemented this method

        IQueryable<Log> GetLogs(); //...you can come up with more CRUD operations, depending on the scenario 
    }
}
