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

        public CourseViewModel() : base()
        {
            Courses = new ObservableCollection<Course>(App.Context.Courses);
          
            //Registration = new RelayCommand();

            DisplayCourseDetails = new RelayCommand<Course>(courses => {
                NotifyColleagues(AppContext.MSG_DISPLAY_COURSE, courses);
            });
            ClearFilter = new RelayCommand(() => Filter = "");

            NewCourse = new RelayCommand(() => { NotifyColleagues(AppContext.MSG_NEW_COURSE); });

           

            //this.isTeacher = App.CurrentUser.IsTeacher

        }
        public ICommand Registration { get; set; }
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
        protected override void OnRefreshData()
        {
            IQueryable<Course> courses = string.IsNullOrEmpty(Filter) ? Course.GetAll() : Course.GetFiltered(Filter);
            var filteredCourses = from c in courses where c.Title.Contains(
                                    Filter)    
                                  select c;
            Courses = new ObservableCollection<Course>(filteredCourses);
        }
    }
}
