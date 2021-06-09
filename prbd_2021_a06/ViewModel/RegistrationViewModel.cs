using prbd_2021_a06.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace prbd_2021_a06.ViewModel
{
    public class RegistrationViewModel : ViewModelCommon
    {

        private ObservableCollection<StudentCourseHelper> studentcourses;
        private ObservableCollection<StudentHelper> students = new ObservableCollection<StudentHelper>();
        public ObservableCollection<StudentCourseHelper> Studentcourses
        {
            get => studentcourses;
            set => SetProperty<ObservableCollection<StudentCourseHelper>>(ref studentcourses, value);
        }


        private ObservableCollection<StudentCourseHelper> registerStudentCourseSelected = new ObservableCollection<StudentCourseHelper>();
        private ObservableCollection<StudentHelper> unRegisterStudentCoursesSelected = new ObservableCollection<StudentHelper>();
        public ObservableCollection<StudentCourseHelper> RegisterStudentCoursesSelected
        {
            get => registerStudentCourseSelected;
            set => SetProperty<ObservableCollection<StudentCourseHelper>>(ref registerStudentCourseSelected, value);
        }

        public ObservableCollection<StudentHelper> UnRegisterStudentCoursesSelected
        {
            get => unRegisterStudentCoursesSelected;
            set => SetProperty<ObservableCollection<StudentHelper>>(ref unRegisterStudentCoursesSelected, value);
        }

        private bool showLeft = false;
        public bool ShowLeft
        {
            get => showLeft;
            set => SetProperty<bool>(ref showLeft, value);
        }

        private bool showDoubleLeft = false;
        public bool ShowDoubleLeft
        {
            get => showDoubleLeft;
            set => SetProperty<bool>(ref showDoubleLeft, value);
        }

        private bool showRight = false;
        public bool ShowRight
        {
            get => showRight;
            set => SetProperty<bool>(ref showRight, value);
        }

        private bool showDoubleRight = false;
        public bool ShowDoubleRight
        {
            get => showDoubleRight;
            set => SetProperty<bool>(ref showDoubleRight, value);
        }


        public ObservableCollection<StudentHelper> Students
        {
            get => students;
            set => SetProperty<ObservableCollection<StudentHelper>>(ref students, value);
        }
        private int courseId;
        public int CourseId
        {
            get => courseId;
            set => SetProperty<int>(ref courseId, value, InitList);
        }
        private string filter;
        public string Filter
        {
            get => filter;
            set => SetProperty<string>(ref filter, value, OnRefreshData);
        }



        public void RefrechLeft()
        {
            ShowLeft = unRegisterStudentCoursesSelected != null && unRegisterStudentCoursesSelected.Count == 1;
            ShowDoubleLeft = unRegisterStudentCoursesSelected != null && unRegisterStudentCoursesSelected.Count > 1;
        }
        public void RefrechRight()
        {
            ShowRight = RegisterStudentCoursesSelected != null && RegisterStudentCoursesSelected.Count == 1;
            ShowDoubleRight = RegisterStudentCoursesSelected != null && RegisterStudentCoursesSelected.Count > 1;
        }


        public RegistrationViewModel() : base()
        {

            //Registration = new RelayCommand();


            ClearFilterCommand = new RelayCommand(() => Filter = "");

            ChangeStatusCommand = new RelayCommand<int>(studentCourseId => OnChangeStatus(studentCourseId));





            //this.isTeacher = App.CurrentUser.IsTeacher

        }

        public ICommand ClearFilterCommand { get; set; }
        public ICommand ChangeStatusCommand { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="unregister"></param>
        public void OnPermute(bool unregister)
        {
            // supprimer les utilisateur à cours données
            if (unregister && RegisterStudentCoursesSelected.Count > 0)
            {
                foreach (var studentCourse in RegisterStudentCoursesSelected)
                {
                    StudentCourse.UnRegister(studentCourse.StudentCourseId);
                }
                RegisterStudentCoursesSelected.Clear();
                RegisterStudentCoursesSelected = RegisterStudentCoursesSelected;
                RefrechRight();
            }
            else if (UnRegisterStudentCoursesSelected.Count > 0)
            { // ajouter les utilisateurs selectionnés au cours
                foreach (var student in UnRegisterStudentCoursesSelected)
                {
                    StudentCourse.Register(student.StudentId, CourseId);
                }
                UnRegisterStudentCoursesSelected.Clear();
                UnRegisterStudentCoursesSelected = UnRegisterStudentCoursesSelected;
                RefrechLeft();
            }
            InitList();

        }

        public Visibility PermissionSeeView
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


        private void SetStudentCourseHelpers()
        {
            var list = App.Context.StudentCourses.Where(sc => sc.Course.Id == CourseId).Select(sc => new StudentCourseHelper()
            {
                CourseId = sc.Course.Id,
                StudentName = $"{sc.Student.FirstName} {sc.Student.LastName}",
                ButtonName = sc.IsActif ? "Desactivate" : "Activate",
                IsActif = sc.IsActif ? "Activate" : "Pending",
                StudentCourseId = sc.Id
            });

            Studentcourses = new ObservableCollection<StudentCourseHelper>(list);
        }

        private void InitList()
        {
            // notifier la liste des student register (gauche)
            SetStudentCourseHelpers();
            // notifier la liste des student unregister (droite)
            OnRefreshData();
        }

        public void OnChangeStatus(int studentCourseId)
        {
            var st = StudentCourse.UpdateStatus(studentCourseId);
            // notifier la liste des student register (gauche)
            SetStudentCourseHelpers();
            // notifier la liste des student unregister (droite)
            OnRefreshData();
        }


        protected override void OnRefreshData()
        {
            IQueryable<StudentHelper> filterStudents = User.GetAllStudentNotRegister(string.IsNullOrEmpty(Filter) ? "" : Filter, CourseId);

            Students.Clear();
            //  var l = filterStudents.ToList();
            Students = new ObservableCollection<StudentHelper>(filterStudents);
        }

    }

    public class StudentCourseHelper : ViewModelCommon
    {
        private string isActif;
        private string buttonName;
        private string studentName;
        private int courseId;

        private int studentCourseId;




        public string IsActif
        {
            get => isActif;
            set => SetProperty<string>(ref isActif, value);
        }

        public string ButtonName
        {
            get => buttonName;
            set => SetProperty<string>(ref buttonName, value);
        }
        public virtual string StudentName
        {
            get => studentName;
            set => SetProperty<string>(ref studentName, value);
        }
        public virtual int CourseId
        {
            get => CourseId;
            set => SetProperty<int>(ref courseId, value);
        }

        public virtual int StudentCourseId
        {
            get => studentCourseId;
            set => SetProperty<int>(ref studentCourseId, value);
        }

        protected override void OnRefreshData()
        {

        }
    }


    public class StudentHelper : ViewModelCommon
    {
        private string studentName;
        private int studentId;

        public virtual string StudentName
        {
            get => studentName;
            set => SetProperty<string>(ref studentName, value);
        }
        public virtual int StudentId
        {
            get => studentId;
            set => SetProperty<int>(ref studentId, value);
        }

        protected override void OnRefreshData()
        {

        }
    }
}





