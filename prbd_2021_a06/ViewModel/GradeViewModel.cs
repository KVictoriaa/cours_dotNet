using prbd_2021_a06.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2021_a06.ViewModel
{
    public class GradeViewModel : ViewModelCommon
    {
        private ObservableCollection<Note> note = new ObservableCollection<Note>();
        public ObservableCollection<Note> Note
        {
            get => note;
            set => SetProperty(ref note, value);
        }
        private ObservableCollection<StudentCourse> studentCourses = new ObservableCollection<StudentCourse>();
        public ObservableCollection<StudentCourse> StudentCourses
        {
            get => studentCourses;
            set => SetProperty(ref studentCourses, value);
        }
        private ObservableCollection<AnswerQuestions> quizzes = new ObservableCollection<AnswerQuestions>();
        public ObservableCollection<AnswerQuestions> Quizzes
        {
            get => quizzes;
            set => SetProperty(ref quizzes, value);
        }
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value, OnRefreshData); }
        private User student;
        public User Student { get => student; set => SetProperty(ref student, value, OnRefreshData); }
        public GradeViewModel() : base()
        {

        }
        public void Init(Course course)
        {
            this.Course = course;
            StudentCourses = new ObservableCollection<StudentCourse>(Course.StudentCourses);
            Quizzes = new ObservableCollection<AnswerQuestions>();
            LoadGrade();
        }
            
        public void LoadGrade()
            {
                if (App.CurrentUser.IsTeacher)
                {
                
                StudentCourses = new ObservableCollection<StudentCourse>(Course.StudentCourses);

            }
            else
            {
                var Studs = new ObservableCollection<StudentCourse>();
                foreach (var student in StudentCourses)
                {
                    Console.WriteLine(student.Student.LastName);
                    if (student.Student.Id == App.CurrentUser.Id)
                    {
                        
                        Console.WriteLine(student.Student);
                        Studs.Add(student);
                    }
                    Console.WriteLine(Studs.Count);
                }
                StudentCourses = new ObservableCollection<StudentCourse>(Studs);

            }
        }

        protected override void OnRefreshData()
        {
            
        }
    }
}
