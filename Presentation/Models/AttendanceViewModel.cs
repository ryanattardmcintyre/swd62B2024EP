using Domain.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Presentation.Models
{
    public class AttendanceViewModel
    {
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; } //1, 2, 3, 4, ...

        public string Group { get; set; }

        public List<PresencesViewModel> Presences { get; set; } //F, T, T, T, F

        public AttendanceViewModel()
        {
            Presences = new List<PresencesViewModel>(); //an empty intializaed list, until we get the data we want from the db
        }
    }

    public class PresencesViewModel
    {
        public int Id { get; set; }
        public bool Present { get; set; }
    }
}
