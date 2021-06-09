using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using prbd_2021_a06.Model;
using prbd_2021_a06.Properties;
using PRBD_Framework;
namespace prbd_2021_a06.ViewModel
{
    class CourseViewModelDetails : ViewModelCommon
    {
        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value); }

        private bool isNew;
        public bool IsNew {
            get => isNew;
            set {
                isNew = value;
                
                RaisePropertyChanged(nameof(IsNew), nameof(IsExisting));
                    }
        }
        public bool IsExisting { get => !isNew; }
        public QuestionMakerViewModel QuestionMaker { get; private set; } = new QuestionMakerViewModel();

        public QuizzViewModel QuizzViewCourse { get; private set; } = new QuizzViewModel();
        
        public string Title
        {
            get { return Course?.Title; }
            set
            {
                Console.WriteLine(Title);
                Course.Title = value;
                RaisePropertyChanged(nameof(Title));
                NotifyColleagues(AppContext.MSG_TITLECOURSE_CHANGED, Course);
                Validate();
            }
        }

        public string Description
        {
            get { return Course?.Description; }
            set
            {
                Course.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }
       public int MaxStudent
        {
            get { return Convert.ToInt32(Course?.MaxStudent); }
            set
            {
                Course.MaxStudent = Convert.ToInt32(value);
                RaisePropertyChanged(nameof(MaxStudent));
                Validate();
            }
        }
        public string Teacher
        {
            get { return Course?.Teacher.LastName; }
            set
            {
                Course.Teacher.LastName = value;
                RaisePropertyChanged(nameof(Teacher));
                //Validate();
            }
        }

        public CourseViewModelDetails() : base()
        {
            if(Validate())
            {
                Save = new RelayCommand(SaveAction, CanSaveAction);
            }
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            Delete = new RelayCommand(DeleteAction, () => !IsNew);
            

        }
        public void Init(Course course, bool isNew)
        {

            this.Course = course;
            this.BindOneWay(nameof(Course), QuestionMaker, nameof(QuestionMaker.Course));
            //passer en fonction du cours
            this.BindOneWay(nameof(Course), QuizzViewCourse, nameof(QuizzViewCourse.Course));
            this.IsNew = isNew;
            RaisePropertyChanged();

        }
        public Visibility Permission
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
       
        private void SaveAction()
        {
            if (IsNew)
            {
                Context.Add(Course);
                IsNew = false;
            }
            Context.SaveChanges();
            OnRefreshData();
            NotifyColleagues(AppContext.MSG_COURSE_CHANGED, Course);
            NotifyColleagues(AppContext.MSG_COURSES);
            NotifyColleagues(AppContext.MSG_RENAME_TAB, Course);
        }

        private bool CanSaveAction()
        {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            return Course != null && (Context?.Entry(Course)?.State == EntityState.Modified);
        }

        private void CancelAction()
        {
            if (IsNew)
            {
                NotifyColleagues(AppContext.MSG_CLOSE_TAB, Course);
            }
            else
            {
                Context.Reload(Course);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction()
        {
            return Course != null && (IsNew || Context?.Entry(Course)?.State == EntityState.Modified);
        }
        private void DeleteAction()
        {
            CancelAction();
            Course.Delete();
            //NotifyColleagues(AppContext.MSG_COURSE_CHANGED, Course);
            NotifyColleagues(AppContext.MSG_COURSES);
            NotifyColleagues(AppContext.MSG_CLOSE_TAB, Course);
        }


        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Delete { get; set; }
        public override bool Validate()
        {
            ClearErrors();

            var course = (from u in App.Context.Courses
                        where u.Title.Equals(Title)
                        select u).FirstOrDefault();

            if(IsNew)
            {
                if (string.IsNullOrEmpty(Title))
                {
                    AddError(nameof(Title), Resources.Error_Required);
                }


                if (MaxStudent == 0)
                {
                    AddError(nameof(MaxStudent), Resources.Error_MaxStudent);
                }
            }
            
            
            

            RaiseErrors();
            return !HasErrors;
        }
        protected override void OnRefreshData()
        {
           
        }
    }
}
