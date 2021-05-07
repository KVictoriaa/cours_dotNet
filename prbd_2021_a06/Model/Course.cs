using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Windows;
using PRBD_Framework;

namespace prbd_2021_a06.Model
{
    public class Course : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxStudent { get; set; }
        public virtual User Teacher { get; set; } 
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
        public virtual ICollection<Quiz> Quizzes { get; set; } = new HashSet<Quiz>();

        [NotMapped]
        public IEnumerable<User> Students { get => StudentCourses.Select(sc => sc.Student); }
        [NotMapped]
        public int NbActifStudents 
        {
            get
            {
                return (from s in StudentCourses where s.IsActif == true select s).Count();
            } 
        }

        [NotMapped]
        public int NbPendingStudents
        {
            get
            {
                return (from s in StudentCourses where s.IsValide == false select s).Count();
            }
        }
        [NotMapped]
        public int NbInactifStudents 
        {
            get 
            {
                return (from s in StudentCourses where s.IsActif == false select s).Count();
            }
        }
        
        [NotMapped]
        public Visibility PermissionRegistration 
        {
            get
            {
                if (App.CurrentUser.IsTeacher || (!(App.CurrentUser.IsTeacher) && GetStudentCourseByUser != null) ) 
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        [NotMapped]
        public StudentCourse GetStudentCourseByUser     
        {
            get
            {
                return (
                    from s in StudentCourses
                    where s.Student.Id.Equals(App.CurrentUser.Id) && 
                    s.Course.Id.Equals(this.Id) select s
                    ).FirstOrDefault();
            }
        }
        public Visibility IsPending
        {
            get
            {
                if (App.CurrentUser.IsTeacher || (!(App.CurrentUser.IsTeacher) && GetStudentCourseByUser == null) ||
                   (!(App.CurrentUser.IsTeacher) && GetStudentCourseByUser != null && (this.GetStudentCourseByUser.IsValide)))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        public Visibility PermissionAddCourse 
        { 
            get
            {
                if(!(App.CurrentUser.IsTeacher))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        
        public Course(string Title,
            string Description,
            int MaxStudent
            )
        {
            this.Title = Title;
            this.Description = Description;
            this.MaxStudent = MaxStudent;
            

           
           
        }
        public static IQueryable<Course> GetAll()
        {
            return Context.Courses.OrderBy(m => m.Title);
        }

        public static IQueryable<Course> GetFiltered(string Filter)
        {
            var filtered = from c in Context.Courses
                           where c.Title.Contains(Filter) 
                           orderby c.Title
                           select c;
            return filtered;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
