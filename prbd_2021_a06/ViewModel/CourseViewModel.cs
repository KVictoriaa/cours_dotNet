using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using prbd_2021_a06.Properties;
using System.Windows.Input;
using System.Collections.ObjectModel;
using prbd_2021_a06.Model;
using System.Windows;
using System.ComponentModel;

namespace prbd_2021_a06.ViewModel
{
    class CourseViewModel : ViewModelCommon
    {
        
        private ObservableCollection<Course> courses;
        public ObservableCollection<Course> Courses
        {
            get => courses;
            set => SetProperty<ObservableCollection<Course>>(ref courses, value);
        }

        private string filter;
        public string Filter
        {
            get => filter;
            set => SetProperty<string>(ref filter, value, OnRefreshData);
        }
        private User teacher;
        public User Teacher { get => teacher; set => SetProperty(ref teacher, value, OnRefreshData); }

        public CourseViewModel() : base()
        {
            if(!App.CurrentUser.IsTeacher)
            {
                Courses = new ObservableCollection<Course>(App.Context.Courses);

            }
            else
            {
                if (App.CurrentUser.IsTeacher)
                {
                    this.Teacher = CurrentUser;

                    Courses.RefreshFromModel(Teacher.GetReceivedAndVisibleMessages(CurrentUser));

                }
              
            }

            
            Registration = new RelayCommand<Course>((course) => Subscribe(course, true));
            UnRegistration = new RelayCommand<Course>((course) => Subscribe(course, false));

            DisplayCourseDetails = new RelayCommand<Course>(courses => {
                NotifyColleagues(AppContext.MSG_DISPLAY_COURSE, courses);
            });
            ClearFilter = new RelayCommand(() => Filter = "");

            NewCourse = new RelayCommand(() => { NotifyColleagues(AppContext.MSG_NEW_COURSE); });

            App.Register(this, AppContext.MSG_COURSES, () =>
            {
                this.Teacher = CurrentUser;
                Courses = new ObservableCollection<Course>(Teacher.CoursesForTeacher);
            });

            if (App.CurrentUser.IsTeacher)
            {
                this.Teacher = CurrentUser;
            }

                

        }
        public void Subscribe(Course course, bool subscribe)
        {
            if (subscribe)
            {
                var studentCourse = new StudentCourse()
                {
                    Course = course,
                    IsValide = false,
                    IsActif = false,
                    Student = App.CurrentUser
                };
                studentCourse.Subscribe();
            }
            else
            {
                StudentCourse.Unsubscribe(App.CurrentUser.Id, course.Id);
            }

            // Rafraichir la liste àprès une demande d'enregistrement 
            Courses = new ObservableCollection<Course>(App.Context.Courses);
        }
        public ICommand Registration { get; set; }
        public ICommand UnRegistration { get; set; }
        public ICommand DisplayCourseDetails { get; set; }
        public ICommand ClearFilter { get; set; }
        public ICommand NewCourse { get; set; }

        public Visibility PermissionAddCourse
        {
            get
            {
                if (!(App.CurrentUser.IsTeacher))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }
        public Course selectedCourse;
        public Course SelectedCourse
        {
            get
            {

                return selectedCourse;
            }
            set
            {
                selectedCourse = value;
                RaisePropertyChanged(nameof(SelectedCourse));
                RaisePropertyChanged();
            }
        }
        public ICollectionView CourseTeacher => Courses.GetCollectionView(nameof(Courses));

        protected override void OnRefreshData()
        {
            IQueryable<Course> courses = string.IsNullOrEmpty(Filter) ? Course.GetAll() : Course.GetFiltered(Filter);
            if(!App.CurrentUser.IsTeacher)
            {
                var filteredCourses = from c in courses
                                      where c.Title.Contains(
                                      Filter)
                                      select c;
                Courses = new ObservableCollection<Course>(filteredCourses);
            }
            else
            {
                var filteredCoursesTeacher = from c in courses
                                      where c.Title.Contains(
                                        Filter) && c.Teacher.Id == App.CurrentUser.Id
                                      select c;
                Courses = new ObservableCollection<Course>(filteredCoursesTeacher);

            }
            
        }
    }
}
