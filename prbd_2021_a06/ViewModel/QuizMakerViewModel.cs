using Microsoft.EntityFrameworkCore;
using prbd_2021_a06.Model;
using prbd_2021_a06.Properties;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace prbd_2021_a06.ViewModel
{
    public class QuizMakerViewModel : ViewModelCommon
    {
        private Quiz quizz;
        public Quiz Quizz { get => quizz; set => SetProperty(ref quizz, value); }
        private Course courseQuiz;
        public Course CourseQuiz { get => courseQuiz; set => SetProperty(ref courseQuiz, value); }


        private QuestionQuiz questionQuizz;
        public QuestionQuiz QuestionQuizz { get => questionQuizz; set => SetProperty(ref questionQuizz, value); }

        private ObservableCollection<QuestionQuiz> questionQuizzs;
        public ObservableCollection<QuestionQuiz> QuestionQuizzs
        {
            get { return questionQuizzs; }
            set => SetProperty(ref questionQuizzs, value);

        }
        private ObservableCollection<Question> questionsCourse;
        public ObservableCollection<Question> QuestionsCourse
        {
            get { return questionsCourse; }
            set => SetProperty(ref questionsCourse, value);

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
        public ICommand AddQuestions { get; set; }
        public ICommand DeleteQuestions { get; set; }
        public QuizMakerViewModel() : base()
        {
            Save = new RelayCommand(() => {
                if (Validate())
                {
                    OnSaveQuiz();
                }
            
                },
                ()=> CanSaveAction());
            Cancel = new RelayCommand(OnCancelQuiz, CanCancelAction);
            DeleteCommand = new RelayCommand(OnDeleteQuiz, () => !IsNew);
            AddQuestions = new RelayCommand(() =>
            {
                var quiz = (from q in App.Context.Quizzes
                            where q.Id == Quizz.Id
                            select q).FirstOrDefault();
                if (SelectedQuestion != null)
                {
                    var QuestionsQuizz = new QuestionQuiz();
                    QuestionsQuizz.Question = SelectedQuestion;
                    QuestionsQuizz.Quiz = quiz;
                    QuestionsQuizz.Point = Point;
                    Context.QuestionQuizzes.Add(QuestionsQuizz);
                    QuestionsCourse.Remove(SelectedQuestion);
                }

                Context.SaveChanges();
                
                ReloadListQuestionsQuizz();
            }

            );
            DeleteQuestions = new RelayCommand(() =>
            {
                if (SelectedQuestionQuiz != null)

                {  
                    QuestionsCourse.Add(SelectedQuestionQuiz.Question);
                    Context.QuestionQuizzes.Remove(SelectedQuestionQuiz);

                    Context.SaveChanges();
                }
                ReloadListQuestionsQuizz();
            });
            App.Register(this, AppContext.MSG_QUESTIONQUIZZ_CHANGED, () =>
            {
                ListQuestionsQuizz();
            });
        }
        private bool CanCancelAction()
        {
            return Quizz != null && (IsNew || Context?.Entry(Quizz)?.State == EntityState.Modified);
        }
        private bool CanSaveAction()
        {
            if (IsNew)
                return !string.IsNullOrEmpty(Title);
            return Quizz != null && (Context?.Entry(Quizz)?.State == EntityState.Modified);
        }
        private void OnCancelQuiz()
        {
            if (IsNew)
            {
                NotifyColleagues(AppContext.MSG_CLOSE_TABQUIZZ, Quizz);
            }
            else
            {
                Context.Reload(Quizz);
                RaisePropertyChanged();
            }
        }

        private void OnSaveQuiz()
        {
            Quiz quiz;
            if (!IsNew)
            {
                Console.WriteLine("Save ancien quiz");
                quiz = App.Context.Quizzes.Find(Quizz.Id);
                // Modifier le quiz
                if (quiz != null)
                {
                    quiz.Title = Title;
                    quiz.Debut = StartAt;
                    quiz.Fin = EndAt;
                    App.Context.Quizzes.Update(quiz);
                    App.Context.SaveChanges();
                }
            }
            else
            { // Enregistrer le quiz
                Console.WriteLine("Save");
                var c = App.Context.Courses.FirstOrDefault(co => co.Title.Trim().ToLower().Equals(Course.ToLower().Trim()));
                quiz = new Quiz()
                {
                    Course = c,
                    Debut = StartAt,
                    Fin = EndAt,
                    Title = Title
                };
                App.Context.Quizzes.Add(quiz);
                App.Context.SaveChanges();
                Id = quiz.Id;
                IsNew = false;
               
                //NotifyColleagues(AppContext.MSG_QUIZZ_CHANGED, quiz.Id);
            }

            if (QuestionQuizzs != null && quiz != null && quiz.Id > 0)
            {
                foreach (var questionQuiz in QuestionQuizzs)
                {
                    
                    if (questionQuiz.Id > 0 && !string.IsNullOrEmpty(questionQuiz.Question.Enonce))
                    {
                        var q = App.Context.QuestionQuizzes.Find(questionQuiz.Id);
                        // Add a comment to this line
                        if (q != null)
                        {
                            q.Question.Enonce = questionQuiz.Question.Enonce;
                            App.Context.QuestionQuizzes.Update(q);
                            App.Context.SaveChanges();
                        }

                    }
                    else if (!string.IsNullOrEmpty(questionQuiz.Question.Enonce))
                    { // Cas d'Ajout

                        var qz = new QuestionQuiz()
                        {
                            Quiz = quiz,
                            Question = new Question()
                            {
                                Enonce = questionQuiz.Question.Enonce,
                                Type = Model.Type.One
                            }
                        };

                        App.Context.QuestionQuizzes.Add(qz);
                        App.Context.SaveChanges();
                    }
                }
            }

            NotifyColleagues(AppContext.MSG_QUIZZ_CHANGED, Quizz);
            NotifyColleagues(AppContext.MSG_QUIZZ);
            NotifyColleagues(AppContext.MSG_RENAMEQuizz_TAB, Quizz);
            // Notifier la vue.
        }
        
        private void OnDeleteQuiz()
        {
            
            var quiz = (from q in App.Context.Quizzes
                       where q.Id == Quizz.Id
                       select q).FirstOrDefault();
            quiz.Delete();
            NotifyColleagues(AppContext.MSG_QUIZZ);
            NotifyColleagues(AppContext.MSG_CLOSE_TABQUIZZ, Quizz);
            
        }
        public void LoadQuestionsNewQuiz()
        {

            QuestionsCourse = new ObservableCollection<Question>(CourseQuiz.Questions);
            var Questions = new ObservableCollection<Question>();

            foreach (var question in QuestionsCourse)
            {

                    Questions.Add(question);

            }
            QuestionsCourse = new ObservableCollection<Question>(Questions);

        }
        public void LoadQuestions()
        {
            
            QuestionsCourse = new ObservableCollection<Question>(CourseQuiz.Questions);
            var Questions = new ObservableCollection<Question>();
            var quests = (from u in QuestionQuizzs
                          where u.Quiz.Id.Equals(quizz.Id)
                          select u).Select(a => a.Question.Id).ToList();

            foreach (var question in QuestionsCourse)
                {
                
                if (!quests.Contains(question.Id))
                {
                     Questions.Add(question);
                }
              
            }
            QuestionsCourse = new ObservableCollection<Question>(Questions);
            
        }

        public void Init(Quiz quizz, bool isNew)
        {
            this.Quizz = quizz;
            this.IsNew = isNew;
            Console.WriteLine(IsNew);
            ListQuestionsQuizz();
            if (!isNew)
            {
                CourseQuiz = quizz.Course;
                LoadQuestions();
                //QuestionsCourse = new ObservableCollection<Question>(CourseQuiz.Questions);
            }
            else
            {
                CourseQuiz = quizz.Course;
                LoadQuestionsNewQuiz();
            }
            
            
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
                Validate();
            }
        }
        public string Course
        {
            get { return Quizz?.Course?.Title; }
            set
            {
                Quizz.Course.Title = value;
                RaisePropertyChanged(nameof(Course));
            }
        }
        public DateTime StartAt
        {
            get { return Quizz != null ? Quizz.Debut : DateTime.MinValue; }
            set
            {
                if (Quizz != null)
                    Quizz.Debut = value;
                RaisePropertyChanged(nameof(StartAt));
            }
        }

        public DateTime EndAt
        {
            get { return Quizz != null ? Quizz.Fin : DateTime.MaxValue; }
            set
            {
                if (Quizz != null)
                    //Add a comment to this line
                    Quizz.Fin = value;
                RaisePropertyChanged(nameof(EndAt));
            }
        }

        private int quizzId;
        public int QuizzId
        {
            get => quizzId;
            set => SetProperty<int>(ref quizzId, value);
        }
        public DateTime? Debut
        {
            get { return Quizz?.Debut; }
            set
            {
                if (value != null)
                    Quizz.Debut = value.Value;

                RaisePropertyChanged(nameof(Debut));
                Validate();
            }
        }
        public DateTime? Fin
        {

            get { return Quizz?.Fin; }
            set
            {
                if (value != null)
                    Quizz.Fin = value.Value;
                RaisePropertyChanged(nameof(Fin));
                Validate();
            }
        }

        public IEnumerable<Question> questions;
        public IEnumerable<Question> Questions
        {
            get { return Quizz?.Questions; }
            set
            {
                RaisePropertyChanged(nameof(Questions));
                //Validate();
            }
        }
        private int point;
        public int Point
        {
            get => point;
            set => SetProperty<int>(ref point, value);
        }
        public Question selectedQuestion;
        public Question SelectedQuestion
        {
            get
            {
                
                return selectedQuestion;

            }
            set
            {
                selectedQuestion = value;
                RaisePropertyChanged(nameof(SelectedQuestion));
                RaisePropertyChanged();
            }
        }
        public QuestionQuiz selectedQuestionQuiz;
        public QuestionQuiz SelectedQuestionQuiz
        {
            get
            {

                return selectedQuestionQuiz;

            }
            set
            {
                selectedQuestionQuiz = value;
                RaisePropertyChanged(nameof(SelectedQuestionQuiz));
                RaisePropertyChanged();
            }
        }
        
        public void ListQuestionsQuizz()
        {
          if(IsNew)
            {
                QuestionQuizzs = new ObservableCollection<QuestionQuiz>();
            }
            else
            {
                var quiz = (from q in App.Context.Quizzes
                            where q.Title == Quizz.Title
                            select q).FirstOrDefault();

                QuestionQuizzs = new ObservableCollection<QuestionQuiz>(Quizz.QuestionQuizzes);
            }
            
        }

        public void ReloadListQuestionsQuizz()
        {
            
                var quiz
                = (from q in App.Context.Quizzes
                            where q.Id == Quizz.Id
                            select q).FirstOrDefault();

                QuestionQuizzs = new ObservableCollection<QuestionQuiz>(quiz.QuestionQuizzes);
            

        }
        public Visibility ChangeQuizz
        {
            get
            {
                return( Quizz != null && Id > 0 && Quizz.Debut < DateTime.Now) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public override bool Validate()
        {
            ClearErrors();

            if (IsNew)
            {

                

                if (string.IsNullOrEmpty(Title))
                {
                    AddError(nameof(Title), Resources.Error_Required);
                }

                if (Debut < DateTime.Now)
                    AddError(nameof(Debut), Resources.Error_DateDebut);
                if (Fin < Debut)
                    AddError(nameof(Fin), Resources.Error_DateFin);

            }

            RaiseErrors();
            return !HasErrors;
        }
        protected override void OnRefreshData()
        {
            
        }

    }
}
