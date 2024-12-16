using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DataAccess.Repositories
{
    public class StudentXmlRepository : IStudentRepository
    {
        private string _pathToFile;

        //what are we going to gain here?
        //that when this StudentXmlRepository will be registered as a service implementation of the IStudentsRepository it will be a simple one
        public StudentXmlRepository(IConfiguration config) {
            //goal: to get the name of the xml file from the appsettings.json   

            _pathToFile = config["xmlPath"];
        }
        public void AddStudent(Student student)
        {

            var myList = GetStudents().ToList();

            myList.Add(student);


            SerializeToXml<List<Student>>(myList, _pathToFile);
        }

        public IQueryable<Student> GetStudents()
        {

            if (System.IO.File.Exists(_pathToFile) == false)
            {
                return new List<Student>().AsQueryable();
            }

           List<Student> students =   DeserializeXmlFile<List<Student>>(_pathToFile);

            return students.AsQueryable();
        }


        public T DeserializeXmlFile<T>(string filePath)
        {
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Students"));

            // Ignore namespaces
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = null; // Disable resolving external DTDs
            using (var reader = XmlReader.Create(filePath, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Strip namespaces
                return (T)serializer.Deserialize(reader);
            }
        }

        static void SerializeToXml<T>(T data, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Position = 0;
                XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("Students"));
                serializer.Serialize(fs, data);
            }
        }

        public void DeleteStudent(string idCard)
        {
            var list = GetStudents().ToList();
            var student = list.SingleOrDefault(x=>x.IdCard == idCard);
            list.Remove(student);

            SerializeToXml<List<Student>>(list, _pathToFile);

        }

        public void UpdateStudent(Student student)
        {
            var list = GetStudents().ToList();
            var studentToBeUpdated = 
                list.SingleOrDefault(x => x.IdCard == student.IdCard);
             
            studentToBeUpdated.LastName = student.LastName;
            studentToBeUpdated.Name = student.Name;

            SerializeToXml<List<Student>>(list, _pathToFile);
        }

        public Student GetStudent(string idCard)
        {
            var list = GetStudents().ToList();
            var studentToBeReturned =
                list.SingleOrDefault(x => x.IdCard == idCard);

            return studentToBeReturned;

        }
    }
}
