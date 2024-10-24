using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GroupRepository
    {
        private AttendanceContext myContext;
        public GroupRepository(AttendanceContext _context)
        {
            myContext = _context;
        }


        public IQueryable<Group> GetGroups()
        {
            return myContext.Groups; //Select * From Groups

            //at this stage it doesnt open a connection with the db and executes the sql
        }

        //Hw:
        //AddGroup
        //UpdateGroup
        
    }
}
