using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    //role of the controller
    //1. Do not interact with the database (i.e. AttendanceContext) directly from inside the Controllers
    //2. Use the controllers as an extra layer on top of the Repository classes to receive the data from the UI
    //3. Use the controllers to validate/santize/filter/... the data you receive
    public class StudentsController : Controller
    {

        private StudentRepository _studentRepository;
        private GroupRepository _groupRepository;
        public StudentsController(StudentRepository studentRepository, GroupRepository groupRepository)
        { 
            _studentRepository= studentRepository;
            _groupRepository= groupRepository;
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


        [HttpGet] //this is called first when the admin selects the user to be edited
        public IActionResult Edit(string id) 
        {
            //get the object pertaining to the student idcard no = id, and pass it to the View, for the admin to be able to edit the details

            var student = _studentRepository.GetStudent(id);
            if (student == null)
            {
                /*
                 * Session: works only on the server side, that you can pass data to and from controllers and it will be available as long as the session
                 * ViewData: works on client side, however it is available to use only from controller to view; with redirection data in it is lost
                 * ViewBag: is like ViewData, only difference is how to set the data; it is set by variable declaration on the fly
                 * TempData: is like ViewData, but it survives ONE redirection
                 * 
                 */
                TempData["error"] = "Id provided doesn't exist";
                return RedirectToAction("Index");
            }
             
                //it will look for a View with name Edit.cshtml inside Views>Students>.... and it will load and pass the object
                return View(student);
        }

        [HttpPost] //this is called after the admin submits the updated changes
        public IActionResult Edit(Student updatedStudent) 
        {
            if (updatedStudent == null)
            {
                TempData["error"] = "Data passed is not valid";
                return Redirect("Error");
            }

            //this is the place where you apply validation

            ModelState.Remove("Group");//it removes the Group navigational property from the list to check

            if (ModelState.IsValid) //triggers any validators which you may have coded
            {
                _studentRepository.UpdateStudent(updatedStudent);
                TempData["message"] = "Student is saved in database";

                //redirecting the user where?
                return RedirectToAction("Index");
            }

            return View(updatedStudent); 
        
        }



        [HttpGet] //when a method is tagged with HttpGet, it means that this method is going to
        //be called when we click on a link to load the page where to input the student details
        public IActionResult Create() {

            IQueryable<Group> myGroups = _groupRepository.GetGroups(); //until here no database call is actually done
            //the view accepts as a model/a data type Student...so we cannot pass an IQueryable<Group>

            //Approach 1 - Creating a ViewModel

            StudentWriteViewModel studentWriteViewModel = new StudentWriteViewModel();
            studentWriteViewModel.Groups = myGroups.ToList(); //it is here that a database call will be made
            
            //Approach 2 - Using ViewBag

            return View(studentWriteViewModel); 
        }

        [HttpPost] //when a method is tagged with HttpPost, it will be called when you submit
        //a form with the data and therefore this method will receive some data
        public IActionResult Create(Student student, IFormFile file) {

            //string name = Request.Form["Name"]; //this is just another approach how you can read the data received

            ModelState.Remove("student.Group");//it removes the Group navigational property from the list to check


            if(_studentRepository.GetStudent(student.IdCard) != null)
            {
                ModelState.AddModelError("student.IdCard", "Student already exists");
                return View(student);
            }

            //the ModelState.IsValid in its default state will validate only any empty fields
            if (ModelState.IsValid) //triggers any validators which you may have coded
            {

                //we can save the image on the webserver

                //1. generate an original/unique filename for the new image Guid
                //2. identify the absolute path where to save the image
                //3. save the image at the absolute path identified using the newly genreated filename
                //4. will save the relative path into the student instance

                //code saving data into the database
                _studentRepository.AddStudent(student);
                TempData["message"] = "Student is saved in database";

                //redirecting the end user where?
                return RedirectToAction("Index");
            }

            StudentWriteViewModel myModel = new StudentWriteViewModel();
            myModel.Student = student;

            myModel.Groups = _groupRepository.GetGroups().ToList();

            TempData["error"] = "Some inputs are incorrect";
            return View(myModel); 
        }

    }
}
