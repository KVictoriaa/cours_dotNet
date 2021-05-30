using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;

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

                
                foreach (var proposition in Question.Propositions)
                {
                    var answer = new AnswerQuestions();
                    answer.Proposition = proposition;
                    answer.StudentCourse = studentCourse;
                    answer.QuestionQuiz = questionQuiz;
                    App.Context.AnswerQuestions.Add(answer);
                     
                }
              
                Context.SaveChanges();
                
            });
            }

        public void Init(Quiz quizz, bool isNew)
        {
            this.Quizz = quizz;
            Console.WriteLine(Quizz.Id);
            this.IsNew = isNew;
            var list = App.Context.QuestionQuizzes.Where(sc => sc.Quiz.Id == Quizz.Id).Select(sc => new QuestionQuiz()
            {
                Quiz = sc.Quiz,
                Question = sc.Question,
               
             });
            
            QuestionQuizzs = new ObservableCollection<QuestionQuiz>(list);
            
            Question = quizz.GetQuestion(quizz);
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
            }
        }
        
        protected override void OnRefreshData()
        {
            throw new NotImplementedException();
        }
    }
    
    
}
