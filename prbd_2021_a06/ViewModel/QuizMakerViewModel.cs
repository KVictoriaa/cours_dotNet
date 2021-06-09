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
        public QuizMakerViewModel() : base()
        {
            Save = new RelayCommand(OnSaveQuiz, CanSaveAction);
            Cancel = new RelayCommand(OnCancelQuiz, CanCancelAction);
            //DeleteCommand = new RelayCommand(DeleteAction, () => !IsNew);
            AddQuestions = new RelayCommand(() =>
            {

                if (SelectedQuestion != null)
                {
                    var QuestionsQuizz = new QuestionQuiz();
                    QuestionsQuizz.Question = SelectedQuestion;
                    QuestionsQuizz.Quiz = Quizz;
                    QuestionsQuizz.Point = Point;
                    Context.QuestionQuizzes.Add(QuestionsQuizz);
                    QuestionsCourse.Remove(SelectedQuestion);
                }

                Context.SaveChanges();
                ListQuestionsQuizz();
            }

            );
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
            if (Id > 0)
            {
                quiz = App.Context.Quizzes.Find(Id);
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
            }

            if (QuestionQuizzs != null && quiz != null && quiz.Id > 0)
            {
                foreach (var questionQuiz in QuestionQuizzs)
                {
                    // cas d'une modification
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


            // Notifier la vue.
        }

        private void OnDeleteQuiz()
        {
            if (Id > 0)
            {
                var quiz = App.Context.Quizzes.Find(Id);
                if (quiz != null)
                {
                    App.Context.Quizzes.Remove(quiz);
                    App.Context.SaveChanges();
                }
                //Add a comment to this line
            }
        }
        public void LoadQuestions()
        {
            //if (quizz.Id > 0)
            //{
            //    CourseQuiz = quizz.Course;

            //QuestionsCourse = new ObservableCollection<Question>(CourseQuiz.Questions);
            var Questions = new ObservableCollection<Question>();
            foreach (var q in QuestionQuizzs)
            {
                foreach (var question in QuestionsCourse)
                {

                    if (question.Id != q.Question.Id)
                    {
                        Questions.Add(question);
                    }
                }
            }
            QuestionsCourse = new ObservableCollection<Question>(Questions);
            //}
        }

        public void Init(Quiz quizz, bool isNew)
        {
            this.Quizz = quizz;
            this.IsNew = isNew;
            ListQuestionsQuizz();
            if (quizz.Id > 0)
            {
                CourseQuiz = quizz.Course;

                QuestionsCourse = new ObservableCollection<Question>(CourseQuiz.Questions);
            }
            LoadQuestions();
            
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
        public void ListQuestionsQuizz()
        {
            
            QuestionQuizzs = new ObservableCollection<QuestionQuiz>(Quizz.QuestionQuizzes);
        }
        public Visibility ChangeQuizz
        {
            get
            {
                return (Quizz != null && Quizz.Debut < DateTime.Now) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        protected override void OnRefreshData()
        {
            
        }

    }
}
