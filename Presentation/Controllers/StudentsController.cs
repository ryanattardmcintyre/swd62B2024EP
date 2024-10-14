using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
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

         
    }
}
