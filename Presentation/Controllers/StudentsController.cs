using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    //role of the controller
    //1. Do not interact with the database (i.e. AttendanceContext) directly from inside the Controllers
    //2. Use the controllers as an extra layer on top of the Repository classes to receive the data from the UI
    //3. Use the controllers to validate/santize/filter/... the data you receive
    public class StudentsController : Controller
    {

        private StudentRepository _studentRepository;
        public StudentsController(StudentRepository studentRepository)
        { 
            _studentRepository= studentRepository;
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Index() {

            //IQueryable<Student>

            var list = _studentRepository.GetStudents();
            return View(list); //as soon as View(list) executes, an outcome similar .ToList() will happen
        
        }


        /*public IActionResult Create(Student newStudent)
        {
            //1. a check: does this student exist with the email received
            var student = _studentRepository.GetStudent(newStudent.IdCard);
            if (student != null)
            {
                _studentRepository.AddStudent(newStudent);
            }

            return Content("Done");
        }*/


        [HttpGet] //this is called first when the admin selects the user to be edited
        public IActionResult Edit(string id) 
        {
            //get the object pertaining to the student idcard no = id, and pass it to the View, for the admin to be able to edit the details


            return View(); 
        }

        [HttpPost] //this is called after the admin submits the updated changes
        public IActionResult Edit(Student updatedStudent) 
        { return View(); }

         
    }
}
