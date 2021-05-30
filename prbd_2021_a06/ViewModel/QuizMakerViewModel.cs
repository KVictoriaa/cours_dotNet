using Microsoft.EntityFrameworkCore;
using prbd_2021_a06.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2021_a06.ViewModel
{
    public class QuizMakerViewModel : ViewModelCommon
    {
        private Quiz quizz;
        public Quiz Quizz { get => quizz; set => SetProperty(ref quizz, value); }

        private QuestionQuiz questionQuizz;
        public QuestionQuiz QuestionQuizz { get => questionQuizz; set => SetProperty(ref questionQuizz, value); }

        private ObservableCollection<QuestionQuizHelper> questionQuizzs;
        public ObservableCollection<QuestionQuizHelper> QuestionQuizzs
        {
            get { return questionQuizzs; }
            //set => SetProperty<ObservableCollectionFast<Question>>(ref questions, value);
            set => SetProperty(ref questionQuizzs, value);
            /*{
                questions = value;
                RaisePropertyChanged(nameof(Questions), nameof(QuestionMarker));
            }*/
        }
        private bool isNew;
        public bool IsNew
        {
            get => isNew;
            set
            {
                isNew = value;
                //RaisePropertyChanged(nameof(isNew));
                RaisePropertyChanged(nameof(IsNew), nameof(IsExisting));
            }
        }
        public bool IsExisting { get => !isNew; }
        public ICommand DeleteCommand { get; set; }
        public ICommand Save { get; set; }
        public ICommand Cancel { get; set; }
        public QuizMakerViewModel() : base()
        {
            Save = new RelayCommand(SaveAction, CanSaveAction);
            Cancel = new RelayCommand(CancelAction, CanCancelAction);
            DeleteCommand = new RelayCommand(DeleteAction, () => !IsNew);
        }
        private void SaveAction()
        {
            if (IsNew)
            {
                Context.Add(Quizz);
                IsNew = false;
            }
            Context.SaveChanges();
            OnRefreshData();
            NotifyColleagues(AppContext.MSG_Quizz_CHANGED, Quizz);
            NotifyColleagues(AppContext.MSG_Quizz);
            NotifyColleagues(AppContext.MSG_RENAMEQuizz_TAB, Quizz);
        }

        private bool CanSaveAction()
        {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            return Quizz != null && (Context?.Entry(Quizz)?.State == EntityState.Modified);
        }

        private void CancelAction()
        {
            if (IsNew)
            {
                NotifyColleagues(AppContext.MSG_CLOSE_TAB, Quizz);
            }
            else
            {
                Context.Reload(Quizz);
                RaisePropertyChanged();
            }
        }

        private bool CanCancelAction()
        {
            return Quizz != null && (IsNew || Context?.Entry(Quizz)?.State == EntityState.Modified);
        }
        private void DeleteAction()
        {
            CancelAction();
            //if (File.Exists(PicturePath))
            // File.Delete(PicturePath);
            //QuestionQuizz.Delete();
            Quizz.Delete();
            NotifyColleagues(AppContext.MSG_Quizz_CHANGED, Quizz);
            NotifyColleagues(AppContext.MSG_CLOSE_TAB, Quizz);
        }
        public void Init(Quiz quizz, bool isNew)
        {
            this.Quizz = quizz;
            Console.WriteLine(Quizz.Id);
            this.IsNew = isNew;
            var list = App.Context.QuestionQuizzes.Where(sc => sc.Quiz.Id == Quizz.Id).Select(sc => new QuestionQuizHelper()
            {
                QuizzId = sc.Quiz.Id,
                QuestionName = $"{sc.Question.Enonce}",
                QuestionId = sc.Id,

            });


            QuestionQuizzs = new ObservableCollection<QuestionQuizHelper>(list);
            RaisePropertyChanged();
        }

        public int Id
        {
            get { return Convert.ToInt32(Quizz?.Id); }
            set
            {
                Quizz.Id = Convert.ToInt32(value);
                RaisePropertyChanged(nameof(Id));
                //NotifyColleagues(AppContext.MSG_TITLEQUIZZ_CHANGED, Quizz);
            }
        }
        
        public string Title
        {
            get { return Quizz?.Title; }
            set
            {
                Quizz.Title = value;
                RaisePropertyChanged(nameof(Title));
                NotifyColleagues(AppContext.MSG_TITLEQUIZZ_CHANGED, Quizz);
            }
        }
        public string Course
        {
            get { return Quizz?.Course.Title; }
            set
            {
                Quizz.Course.Title = value;
                RaisePropertyChanged(nameof(Course));
                //NotifyColleagues(AppContext.MSG_TITLEQUIZZ_CHANGED, Quizz);
            }
        }

        private int quizzId;
        public int QuizzId
        {
            get => quizzId;
            set => SetProperty<int>(ref quizzId, value);
        }
        

        public IEnumerable<Question> questions;
        public IEnumerable<Question> Questions
        {
            get { return Quizz?.Questions; }
            set
            {

                //Quizz.Questions.= value;
               // PropositionsString = string.Join(",", SelectedQuestion.Propositions);
                //Console.WriteLine(PropositionsString);
                //EditMode = true;
                RaisePropertyChanged(nameof(Questions));
                //Validate();
            }
        }


        protected override void OnRefreshData()
        {
           
        }
    }
    public class QuestionQuizHelper : ViewModelCommon
    {
        private string questionName;
        private int questionId;
        private int quizzId;

        public virtual string QuestionName
        {
            get => questionName;
            set => SetProperty<string>(ref questionName, value);
        }
        public virtual int QuestionId
        {
           get => questionId;
           set => SetProperty<int>(ref questionId, value);
        }
        public virtual int QuizzId
        {
            get => QuizzId;
            set => SetProperty<int>(ref quizzId, value);
        }

        protected override void OnRefreshData()
        {

        }
    }
}
