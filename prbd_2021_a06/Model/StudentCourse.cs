using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace prbd_2021_a06.Model {
    public enum IsValide { True}
    public enum IsActif { False }
    public class StudentCourse : EntityBase<Context> {
        [Key]
        public int Id { get; set; }
        public Boolean IsValide { get; set; }
        public Boolean IsActif { get; set; }
        public virtual User Student { get; set; }
        public virtual Course Course { get; set; }

        public virtual ICollection<AnswerQuestions> AnswerQuestions { get; set; } = new HashSet<AnswerQuestions>();
        public virtual ICollection<Note> Notes { get; set; } = new HashSet<Note>();


        public StudentCourse (
            )
        {
        }



        public static StudentCourse UpdateStatus(int studentCourse) {
            var studentCourses = Context.StudentCourses.Find(studentCourse);
            studentCourses.IsActif = !studentCourses.IsActif;
            Context.Update(studentCourses);
            Context.SaveChanges();

            return studentCourses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentCourse"></param>
        public static void UnRegister(int studentCourse) {
            var studentCourses = Context.StudentCourses.Find(studentCourse);
            Context.Remove(studentCourses);
            Context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="courseId"></param>
        public static void Register(int studentId, int courseId ) {
            // Si le nombre d'étudiant est atteint
            if (Context.StudentCourses.Count(sc => sc.Course.Id == courseId)
                >= Context.Courses.FirstOrDefault(c => c.Id == courseId).MaxStudent)
                return;
            // Check if user is not already subscribe in the course
            var isIn = Context.StudentCourses.Any(sc => sc.Student.Id == studentId && sc.Course.Id == courseId);
            if (!isIn) {
                var student = Context.Users.Find(studentId);
                var course = Context.Courses.Find(courseId);
                var studentcourse = new StudentCourse() { Course = course, Student = student, IsActif = true, IsValide = true };
                Context.StudentCourses.Add(studentcourse);
                Context.SaveChanges();
            }
           
        }


    }
}
