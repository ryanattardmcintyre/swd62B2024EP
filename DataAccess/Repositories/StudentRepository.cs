using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Dependency Injection - is a design pattern which optimizes the way how instances/objects are created
    //                     - it is making the code more efficient because at the end of the day only 1 instance is created
    //                     - the instance (e.g. of AttendanceContext type) is not even created here


    //different types of DI
    //line 25: an example of Constructor Injection
    //Method Injection - requires you to type [FromServices]+ type of instance you need in every method you  need it in
    //Property Injection - is when you turn the field into a property and assign the instance thru the property not the constructor


    //What is a repository?
    //it is a collection of CRUD operations to manage the data
    public class StudentRepository
    {
        private AttendanceContext myContext;
        public StudentRepository(AttendanceContext _context) {
            myContext = _context;
        }

        //Reading
        public IQueryable<Student> GetStudents()
        {
            return myContext.Students;
        }

        public Student GetStudent(string idCard)
        {    
            return myContext.Students.SingleOrDefault(x=>x.IdCard == idCard);
        }

        //Create
        public void AddStudent(Student student) {
           
            myContext.Add(student);
            myContext.SaveChanges();

        }

        //Delete
        public void DeleteStudent(string idCard) {
            if (GetStudent(idCard) != null)
            {
                myContext.Students.Remove(GetStudent(idCard));  
                myContext.SaveChanges();
            }
        }

        //Update

        public void UpdateStudent(Student student) { 
         var myOriginalStudent = GetStudent(student.IdCard);
            myOriginalStudent.LastName = student.LastName;
            myOriginalStudent.Name = student.Name;
            myOriginalStudent.GroupFK = student.GroupFK;
            //when you need to alter the value of the foreign you don't do it on the navigational property

            myContext.SaveChanges();
        }
    }
}
