using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Presentation.ActionFilters;
using Presentation.Models;

namespace Presentation.Controllers
{
    //role of the controller
    //1. Do not interact with the database (i.e. AttendanceContext) directly from inside the Controllers
    //2. Use the controllers as an extra layer on top of the Repository classes to receive the data from the UI
    //3. Use the controllers to validate/santize/filter/... the data you receive

    [Authorize]
    public class StudentsController : Controller
    {

        private StudentRepository _studentRepository;
        private GroupRepository _groupRepository;
        private LogRepository _logRepository;
        public StudentsController(StudentRepository studentRepository, GroupRepository groupRepository, LogRepository logRepository)
        { 
            _studentRepository= studentRepository;
            _groupRepository= groupRepository;
            _logRepository= logRepository;
        }

        [AllowAnonymous]
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
        public IActionResult Edit(Student updatedStudent, IFormFile file,[FromServices] IWebHostEnvironment host) 
        {
            if (updatedStudent == null)
            {
                TempData["error"] = "Data passed is not valid";
                return Redirect("Error");
            }

            //this is the place where you apply validation

            ModelState.Remove("Group");//it removes the Group navigational property from the list to check
            ModelState.Remove("ImagePath");
            ModelState.Remove("file");

            #region File Handling

            if (file != null)
            {
                string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                //C:\Users\attar\source\repos\swd62B2024EP\swd62B2024EP\Presentation\wwwroot\images
                string absolutePath = host.WebRootPath + "\\images\\" + uniqueFilename;

                using (var f = System.IO.File.Create(absolutePath))
                {
                    file.CopyTo(f);
                }

                string relativePath = "\\images\\" + uniqueFilename;

                //delete the old image
                var oldStudent = _studentRepository.GetStudent(updatedStudent.IdCard);
                if (string.IsNullOrEmpty(oldStudent.ImagePath) == false)
                {
                        string absolutePathOfOldImage = host.WebRootPath + oldStudent.ImagePath;
                        System.IO.File.Delete(absolutePathOfOldImage);
                }
                
                updatedStudent.ImagePath = relativePath;
            }
            else
            {
                var oldStudent = _studentRepository.GetStudent(updatedStudent.IdCard);
                updatedStudent.ImagePath = oldStudent.ImagePath;
            }
            #endregion

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
        public IActionResult Create(Student student, IFormFile file, [FromServices] IWebHostEnvironment host) {

            try
            {
    
        

                #region Creating the ViewModel

                StudentWriteViewModel myModel = new StudentWriteViewModel();
                myModel.Student = student;
                myModel.Groups = _groupRepository.GetGroups().ToList();

                #endregion

                #region Validations
                ModelState.Remove("file");
                ModelState.Remove("student.Group");//it removes the Group navigational property from the list to check


                if (_studentRepository.GetStudent(student.IdCard) != null)
                {
                    ModelState.AddModelError("student.IdCard", "Student already exists");
                    return View(myModel);
                }

                #endregion

                #region File Handling

                if (file != null)
                {
                    string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                    //C:\Users\attar\source\repos\swd62B2024EP\swd62B2024EP\Presentation\wwwroot\images
                    string absolutePath = host.WebRootPath + "\\images\\" + uniqueFilename;

                    using (var f = System.IO.File.Create(absolutePath))
                    {
                        file.CopyTo(f);
                    }

                    string relativePath = "\\images\\" + uniqueFilename;
                    student.ImagePath = relativePath;
                }
                #endregion

                #region Addition of a student to db
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
                #endregion

                TempData["error"] = "Some inputs are incorrect";
                return View(myModel);
            }
            catch (OutOfMemoryException ex)
            {
                _logRepository.AddLog(new Log()
                {
                    Message = ex.Message,
                    User = User.Identity.Name,
                    Timestamp = DateTime.Now,
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                //set up a message for the user
                TempData["error"] = "Try uploading a smaller file";

                StudentWriteViewModel myModel = new StudentWriteViewModel();
                myModel.Student = student;
                myModel.Groups = _groupRepository.GetGroups().ToList();

                return View(myModel);
            }
            catch (SqlException ex)
            {
                //log the error
                _logRepository.AddLog(new Log()
                {
                    Message = ex.Message,
                    User = User.Identity.Name,
                    Timestamp = DateTime.Now,
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
                });

                //set up a message for the user
                TempData["error"] = "Connection with the database was lost. Try again later";

                return Redirect("Error");


            }
            catch (Exception ex)
            {
                //log the error
                _logRepository.AddLog(new Log()
                {
                 Message = ex.Message,
                  User = User.Identity.Name,
                   Timestamp = DateTime.Now,
                    IpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()                
                });

                //set up a message for the user
                TempData["error"] = "Error occurred. Try again later";
                //redirect the user
                return RedirectToAction("Error", "Home"); // Redirect to Error action
            }
        }



        //create a get & post only if you intend to display a page where the user kinds of confirm the deletion
        public IActionResult Delete(string id, [FromServices] IWebHostEnvironment host)
        {
            var oldStudent = _studentRepository.GetStudent(id);

            _studentRepository.DeleteStudent(id);

            if(oldStudent.ImagePath != null)
            {
                string absolutePath = host.WebRootPath + oldStudent.ImagePath;
                if(System.IO.File.Exists(absolutePath))
                        System.IO.File.Delete(absolutePath);
            }

            TempData["message"] = "Deleted successfully";
            return RedirectToAction("Index");
        }



    }
}
