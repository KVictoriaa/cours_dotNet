using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prbd_2021_a06.Model;
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
                RaisePropertyChanged(nameof(isNew));
                    }
        }
        
        public string Title
        {
            get { return Course?.Title; }
            set
            {
                Course.Title = value;
                RaisePropertyChanged(nameof(Title));
                NotifyColleagues(AppContext.MSG_TITLECOURSE_CHANGED, Course);
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
       /*public int MaxStudent
        {
            get { return (int)(Course?.MaxStudent); }
            set
            {
                Course.MaxStudent = value;
                RaisePropertyChanged(nameof(MaxStudent));
            }
        }*/
        public string Teacher
        {
            get { return Course?.Teacher.LastName; }
            set
            {
                Course.Teacher.LastName = value;
                RaisePropertyChanged(nameof(Teacher));
            }
        }

        public CourseViewModelDetails() : base()
        {

        }
        public void Init(Course course, bool isNew)
        {
            this.Course = course;
            this.IsNew = isNew;
            RaisePropertyChanged();

        }
        protected override void OnRefreshData()
        {
           
        }
    }
}
