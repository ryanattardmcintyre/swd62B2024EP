using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStudentRepository
    {
        IQueryable<Student> GetStudents();
        void AddStudent(Student student);

        void DeleteStudent(string idCard);
        void UpdateStudent(Student student);
        Student GetStudent(string idCard);
    }
}
