using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using prbd_2021_a06.Properties;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using prbd_2021_a06.Model;
using System.ComponentModel;
using System.Collections;
using System.Windows.Input;

namespace prbd_2021_a06.ViewModel
{
    public class QuizzViewModel : ViewModelCommon
    {
        
        protected override void OnRefreshData()
        {
            // IQueryable<Quiz> quizzes = Quizzes.GetAll()
            if (Course == null) return;
            Course = Course.GetByTitle(Course.Title);
            LoadQuizzes();
            RaisePropertyChanged();
        }

        private ObservableCollection<Quiz> quizzes = new ObservableCollection<Quiz>();
        public ObservableCollection<Quiz> Quizzes
        {
            get => quizzes;
            set => SetProperty(ref quizzes, value);
        }

        private Course course;
        public Course Course
        {
            get => course;
            set => SetProperty(ref course, value, OnRefreshData);
        }
        

        private IList selectedItems = new ArrayList();
        public IList SelectedItems
        {
            get => selectedItems;
            set => SetProperty(ref selectedItems, value);
        }

        public ICollectionView QuizzesView => Quizzes.GetCollectionView(nameof(DateTime), ListSortDirection.Descending);
        public ICommand DisplayQuizzDetails { get; set; }
        public ICommand DisplayQuizzStudent { get; set; }

        private void LoadQuizzes()
        {
            if (Course == null) return;
            var ids = SelectedItems.Cast<Quiz>().Select(q => q.Id).ToList();
            //Récupère ta liste de Quizz
            //Quizzes.RefreshFromModel(Course.GetQuizzes(Course));
            Quizzes = new ObservableCollection<Quiz>(Course.Quizzes);
             
            
            if(!(App.CurrentUser.IsTeacher))
            {
               
                 var Quizze = new ObservableCollection<Quiz>();
                foreach(var q in Quizzes)
                {
                    
                    if (q.Debut < DateTime.Now || q.Debut == DateTime.Now)
                    {
                        Quizze.Add(q);
                    }
                }

                Quizzes = new ObservableCollection<Quiz>(Quizze);
            }
            SelectedItems.Clear();
            foreach (var q in Quizzes.Where(q => ids.Contains(q.Id)))
                SelectedItems.Add(q);
        }
        public void Init(Course course)
        {
            this.Course = course;
            LoadQuizzes();
        }
        public Visibility PermissionAddQuizz
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
        //public Visibility AnswerQuizz
        //{
        //    get
        //    {
        //        if (!(App.CurrentUser.IsTeacher) && (Course.GetQuizBytime.Debut > DateTime.Now))
        //        {
        //            return Visibility.Collapsed;
        //        }
        //        return Visibility.Visible;
        //    }
        //}
        public bool IsEnabled
        {
            get
            {

                return false;
            }
        }
        public QuizzViewModel() : base()
        {
            //Quizzes = new ObservableCollection<Quiz>(App.Context.Quizzes);
            DisplayQuizzDetails = new RelayCommand<Quiz>(quizzes => {
                NotifyColleagues(AppContext.MSG_DISPLAY_QUIZZ, quizzes);
            });
            NewQuizz = new RelayCommand<Course>((course) => { NotifyColleagues(AppContext.MSG_NEW_Quizz,Course); });

            DisplayQuizzStudent = new RelayCommand<Quiz>(quizzes => {
                
                NotifyColleagues(AppContext.MSG_DISPLAY_QUIZZSTUDENT, quizzes);
            });
        }
        public ICommand NewQuizz { get; set; }
       
    }
}
