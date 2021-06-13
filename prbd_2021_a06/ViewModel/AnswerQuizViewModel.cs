using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;
using Type = prbd_2021_a06.Model.Type;

namespace prbd_2021_a06.ViewModel
{
    public class AnswerQuizViewModel : ViewModelCommon
    {
        private Quiz quizz;
        public Quiz Quizz { get => quizz; set => SetProperty(ref quizz, value); }

        private AnswerQuestions answerQuestions;
        public AnswerQuestions AnswerQuestions { get => answerQuestions; set => SetProperty(ref answerQuestions, value); }
        private Question question { get; set; }
        public Question Question
        {
            get => question;
            set
            {
                question = value;
                RaisePropertyChanged(nameof(Question));
            }
        }

        private bool isNew;
        public bool IsNew
        {
            get => isNew;
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew), nameof(IsExisting));
            }
        }
        public ICommand SaveQuestion { get; set; }
        public ICommand CancelQuestion { get; set; }
        private ObservableCollection<QuestionQuiz> questionQuizzs;
        public ObservableCollection<QuestionQuiz> QuestionQuizzs
        {
            get { return questionQuizzs; }
            set => SetProperty(ref questionQuizzs, value);
            
        }
        private ObservableCollection<Proposition> propositions;
        public ObservableCollection<Proposition> Propositions
        {
            get { return propositions; }
            set => SetProperty(ref propositions, value);
            
        }
        public bool IsExisting { get => !isNew; }

        public AnswerQuizViewModel() : base()
        {
            SaveQuestion = new RelayCommand(() =>
            {
               
               var questionQuiz = (from u in App.Context.QuestionQuizzes
                                                where u.Question.Id.Equals(Question.Id) && u.Quiz.Id == quizz.Id
                                                select u).FirstOrDefault();



                var studentCourse = (from u in App.Context.StudentCourses
                            where u.Student.Equals(App.CurrentUser) 
                            select u).FirstOrDefault();

                foreach (var qq in QuestionQuizzs)
                {
                    var q = qq.Question;
                    var anwers = (from a in App.Context.AnswerQuestions
                                  where a.StudentCourse.Id == studentCourse.Id &&
                                  //a.Proposition.Id == proposition.Id &&
                                  a.QuestionQuiz.Id == qq.Id
                                  select a).ToList();
                    if(anwers != null)
                    {
                        foreach (var a in anwers)
                        {
                            App.Context.AnswerQuestions.Remove(a);
                        }
                    }
                    

                    foreach (var proposition in q.Propositions)
                    {
                        if (proposition.IsCheck)
                        {
                            
                            var answer = new AnswerQuestions();
                            answer.Proposition = proposition;
                            answer.StudentCourse = studentCourse;
                            answer.QuestionQuiz = qq;
                            App.Context.AnswerQuestions.Add(answer);

                            if ( q.Type == Type.One) 
                            {
                                if (proposition.IsCorrect)
                                {
                                    Console.WriteLine(proposition.Body);
                                    Console.WriteLine(qq.Point);
                                    Console.WriteLine("One");
                                    answer.Point = qq.Point;
                                }
                                else
                                {
                                    Console.WriteLine(proposition.Body);
                                    Console.WriteLine("not correct");
                                    answer.Point = 0;
                                }
                            }

                            else
                            {

                                var propositions = q.Propositions.ToList();
                                int answersTrue = 0;
                                int answersFalse = 0;
                                foreach (var p in propositions)
                                {
                                    if (p.IsCheck)
                                    {
                                        if (p.IsCorrect)
                                        {
                                            ++answersTrue;

                                        }
                                        else
                                        {
                                            ++answersFalse;
                                        }
                                    }

                                    Console.WriteLine("answerTrue:" + answersTrue);
                                    Console.WriteLine("answerFasle:" + answersFalse);
                                }
                                var propositionsCorrect = q.Propositions.Where(p => p.IsCorrect == true).ToList().Count;
                                var PointManyQuestions = qq.Point * Math.Max((answersTrue - answersFalse), 0) / propositionsCorrect;

                                answer.Point = PointManyQuestions / propositionsCorrect;
                                Console.WriteLine("correct prop" + propositionsCorrect);
                                Console.WriteLine("max" + Math.Max((answersTrue - answersFalse), 0));
                                Console.WriteLine("point tot" + qq.Point * Math.Max((answersTrue - answersFalse), 0));
                                Console.WriteLine("point question" + qq.Point);
                            }
                        }


                    }
                    
                   
                  

                }
                
                Context.SaveChanges();
                
            });
            CancelQuestion = new RelayCommand(CancelAction

                );
            
        }

        public void Init(Quiz quizz, bool isNew)
        {
            this.Quizz = quizz;
            this.IsNew = isNew;
            QuestionQuizzs = new ObservableCollection<QuestionQuiz>(Quizz.QuestionQuizzes);
            
            Question = quizz.GetQuestion(quizz);
            loadAnswerQuestions();
            RaisePropertyChanged();
        }

        public bool IsEnabled
        {
            get
            {
                return (Quizz != null) ? Quizz.Fin > DateTime.Now: true;

            }
        }
        public Visibility CorrectPropos
        {
            get
            {
                return (Quizz != null && Quizz.Fin < DateTime.Now) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void CancelAction()
        {
            
                Console.WriteLine("Close");
                NotifyColleagues(AppContext.MSG_CLOSE_TABQUIZZ_ETUDIANT, Quizz);
            
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
            }
        }
        //Récupère l'état de réponse
        public void loadAnswerQuestions()
        {
            var answers = (from a in App.Context.AnswerQuestions
                          where a.StudentCourse.Student.Id.Equals(App.CurrentUser.Id)
                          select a).ToList();
            foreach (var q in QuestionQuizzs)
            {
                foreach (var p in answers)
                {
                    if(q.Id == p.QuestionQuiz.Id)
                    {
                        foreach (var proposition in q.Question.Propositions)
                        {
                            if (proposition.Id == p.Proposition.Id)
                            {
                                proposition.IsCheck = true;
                            }
                        }
                    }
                }
            }
        }
        
        protected override void OnRefreshData()
        {
            
        }
    }
    
    
}
