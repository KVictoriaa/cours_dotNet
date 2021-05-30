using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using PRBD_Framework;
using prbd_2021_a06.ViewModel;

namespace prbd_2021_a06.Model
{
    public enum Role {
        Student, Teacher
    }
    public class User : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
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

        ) {

            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Password = Password;
            this.Email = Email;
            this.Role = Role;
        }

        public override string ToString() {
            return LastName + " " + FirstName;
        }
        public bool IsTeacher {
            get => this.Role == Role.Teacher;
        }

        public bool IsStudent {
            get => this.Role == Role.Student;
        }

        [NotMapped]
        public bool IsValideStudents {
            get {
                return (from s in StudentCourses where s.IsValide == true select s).Any();
            }
        }

        public static IQueryable<StudentHelper> GetAllStudentNotRegister(string filter, int courseId)
        {
            var filtered = Context.Users.Where(u =>
                            u.Role == Role.Student &&
                            (!u.StudentCourses.Any(sc => sc.Course.Id == courseId) ||
                            u.StudentCourses.Any(sc => sc.Course.Id == courseId && !sc.IsActif))
                           && (u.FirstName.Trim().ToLower().Contains(filter.ToLower().Trim())
                           || u.LastName.Trim().ToLower().Contains(filter.ToLower().Trim())
                           )).OrderBy(u => u.FirstName).Select(s => new StudentHelper()
                           {
                               StudentId = s.Id,
                               StudentName = $"{s.FirstName} {s.LastName}"
                           })
                           ;

            return filtered;
        }
        public IQueryable<Course> GetReceivedAndVisibleMessages(User observer)
        {
            var questions =
                Context.Courses.Where(m =>
                    m.Teacher == observer);
            return questions;

        }
    }


}
