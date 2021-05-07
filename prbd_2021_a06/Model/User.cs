using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public enum Role
    {
        Student,Teacher
    }
    public class User : EntityBase <Context>
    {
        [Key]
        public int Id { get; set;}
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; } = Role.Student;
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();

        [NotMapped]
        public IEnumerable<Course> CoursesForStudent { get => StudentCourses.Select(sc => sc.Course); }
        public virtual ICollection<Course> CoursesForTeacher { get; set; } = new HashSet<Course>();
        public User() { }
        public User(
            string LastName,
            string FirstName, 
            string Password, 
            string Email,
            Role Role = Role.Student
        
        )
        {
           
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Password = Password;
            this.Email = Email;
            this.Role = Role;
        }

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
        public bool IsTeacher
        {
            get => this.Role == Role.Teacher;
        }

        public bool IsStudent
        {
            get => this.Role == Role.Student;
        }

        [NotMapped]
        public bool IsValideStudents
        {
            get
            {
                return (from s in StudentCourses where s.IsValide == true select s).Any();
            }
        }
    }
    
}
