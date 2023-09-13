using System;
using System.Collections.Generic;
using University.Interfaces;

namespace University.Models
{
    public class Student : IEntity
    {
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; } = null;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string PlaceOfResidence { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public virtual ICollection<Course>? Courses { get; set; } = null;
        public virtual ICollection<Exam>? Exams { get; set; } = null;

        public string GetKey()
        {
            return StudentId;
        }
    }
}
