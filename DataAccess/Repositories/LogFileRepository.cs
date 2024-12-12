using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace DataAccess.Repositories
{
    public class LogFileRepository : ILogRepository
    {
        private string _fileName="";
        public LogFileRepository(string filename) { _fileName = filename; }
        public void AddLog(Log l)
        {
            if (System.IO.File.Exists(_fileName) == false)
            {
                using (var myFile = System.IO.File.CreateText(_fileName))
                {
                    List<Log> logs = new List<Log>();
                    logs.Add(l);

                    //serialize the list
                    string fileContents = JsonConvert.SerializeObject(logs);

                    //saving the file
                    myFile.Write(fileContents);
                }
            }
            else //if file already exists
            {
                var logs = GetLogs().ToList();
                logs.Add(l);

                //serialize the list
                string fileContents = JsonConvert.SerializeObject(logs);

                //saving the file
                System.IO.File.WriteAllText(_fileName, fileContents);
            }
        }

        public IQueryable<Log> GetLogs()
        {  List<Log> logs = new List<Log>();
            string fileContents = "";

            if (System.IO.File.Exists(_fileName) == false)
            {
                return logs.AsQueryable();
            }

            using (var myFile = System.IO.File.OpenText(_fileName))
            {
                fileContents = myFile.ReadToEnd();
            }

            if (string.IsNullOrEmpty(fileContents))
            {
                return logs.AsQueryable();
            }
            else
            {
                logs = JsonConvert.DeserializeObject<List<Log>>(fileContents);
                return logs.AsQueryable();
            }

        }
    }
}
