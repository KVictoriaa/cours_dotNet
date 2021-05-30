using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using prbd_2021_a06.Model;
using prbd_2021_a06.View;
using PRBD_Framework;
using Type = prbd_2021_a06.Model.Type;

namespace prbd_2021_a06.ViewModel
{
    public class QuestionMakerViewModel : ViewModelCommon
    {
        private ObservableCollectionFast<Question> questions;
        public ObservableCollectionFast<Question> Questions
        {
            get { return questions; }
            set => SetProperty(ref questions, value);
        }
       
        private ObservableCollection<CategoryQuestionHelper> categoryQuestions = new ObservableCollection<CategoryQuestionHelper>();
        public ObservableCollection<CategoryQuestionHelper> CategoryQuestions
        {
            get => categoryQuestions;
            set => SetProperty(ref categoryQuestions, value);
        }
        
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

        private Course course;
        public Course Course { get => course; set => SetProperty(ref course, value, OnRefreshData); }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveOneCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand DeleteMessages { get; set; }
        public ICommand NewCommand { get; set; }

        public QuestionMakerViewModel() : base()
        {
            
            SaveOneCommand = new RelayCommand(() => {
               
                if (isNew)
                {
                    Question.Enonce = Enonce;
                    Question.Type = Type;
                    Question.Course = Course;

                    App.Context.Questions.Add(Question);
                    App.Context.SaveChanges();
                    Question = (from u in App.Context.Questions
                                where u.Enonce.Equals(Enonce) && u.Course.Id == course.Id
                                select u).FirstOrDefault();
                    IsNew = false;
                }
                else
                {   Console.WriteLine(IsNew);
                    Question = (from u in App.Context.Questions
                                where u.Enonce.Equals(Enonce) && u.Course.Id == course.Id
                                select u).FirstOrDefault();
                    App.Context.Propositions.RemoveRange(Question.Propositions);
                    
                }

                List<string> listOfNames = new List<string>(propositionsString.Split(Environment.NewLine));
                foreach (var proposition in listOfNames)
                {
                    Proposition prop = new Proposition();
                    prop.Question = Question;
                    Console.WriteLine(listOfNames.Count);
                    if (proposition.Contains("*"))
                    {
                        prop.Body = proposition.Replace("*", "");
                        prop.IsCorrect = true;
                    }
                    else
                    {
                        prop.Body = proposition;
                        prop.IsCorrect = false;
                    }

                    App.Context.Propositions.Add(prop);
                }

                Context.SaveChanges();
                
                EditMode = false;
                NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS,Question.Enonce);
            },
            
            () => EditMode );

            CancelCommand = new RelayCommand(() => {
                Context.Entry(Question).Reload();
                EditMode = false;
                Validate();
                RaisePropertyChanged();
            },
            () => EditMode);

           /* RefreshCommand = new RelayCommand(() => {
                Questions = new ObservableCollectionFast<Question>(Course.Questions);
            },
            () => ReadMode);*/

            NewCommand = new RelayCommand(() => {
                Question = new Question("", Type);
                Question.Course = Course;
                
                isNew = true;
            },
            () => ReadMode);

            Register<String>(this, AppContext.MSG_REFRESH_QUESTIONS, _ => {
                Questions = new ObservableCollectionFast<Question>(Course.Questions);
            });
            EditMode = false;

        }
       
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged(nameof(IsNew), nameof(IsExisting));
            }
        }

        public bool IsExisting { get => !isNew; }

        private bool editMode = false;
        public bool EditMode
        {
            get { return editMode; }
            set
            {
                editMode = value;
                RaisePropertyChanged(nameof(EditMode), nameof(ReadMode));
            }
        }

        public bool ReadMode
        {
            get { return !EditMode; }
            set { EditMode = !value; }
        }
        //Initialisation des données
        public void Init(Course course)
        {

            this.Course = course;
            Questions = new ObservableCollectionFast<Question>(Course.Questions);
            RaisePropertyChanged();
               
        }
        public string Enonce
        {
            get { return Question?.Enonce; }
            set
            {
                Question.Enonce = value;
                EditMode = true;
                RaisePropertyChanged(nameof(Enonce));
                //Validate();
            }
        }
       
        public ObservableCollection<Proposition> propositions;
        public ObservableCollection<Proposition> Propositions
        {
            get { return propositions; }
            set
            {
               
               Question.Propositions = value;
                PropositionsString = string.Join(",", Question.Propositions);
                EditMode = true;
                RaisePropertyChanged(nameof(Propositions));
                //Validate();
            }
        }
        public string propositionsString;
        public string PropositionsString
        {
            get { return propositionsString; }
            set
            {

                propositionsString = value;
                EditMode = true;
                RaisePropertyChanged(nameof(PropositionsString));
                //Validate();
            }
        }
        public Type type ;
        public Type Type

        {
            get {return type; }
            set
            {
                Question.Type = value;
                EditMode = true;
                RaisePropertyChanged(nameof(Type));
                //Validate();
            }
        }


        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                selectedQuestion = value;
                question = selectedQuestion;
                
                propositions  = new ObservableCollection<Proposition>(Question.Propositions);
                List<string> bodyList = new List<string>();
                foreach (var proposition in question.Propositions)
                {
                    bodyList.Add(proposition.Body);
                }

                propositionsString = String.Join(System.Environment.NewLine, bodyList);
                
                type = Question.Type;
                RaisePropertyChanged(nameof(SelectedQuestion));
               
                RaisePropertyChanged();
                
            }
        }

       /* public void CategoryQuestion()
        {
            CategoryQuestions = new ObservableCollection<CategoryQuestionHelper>();
            var list = App.Context.CategoryQuestions.Where(sc => sc.Questions.Id == Question.Id).Select(sc => new CategoryQuestionHelper()
            {
                
                QuestionId = sc.Questions.Id,
                CategoryTitle = $"{sc.Categories.Title}",
                //QuestionId = sc.Id,

            });
            CategoryQuestions = new ObservableCollection<CategoryQuestionHelper>(list);

        }*/

        protected override void OnRefreshData()
        {
            //CategoryQuestion();
            EditMode = false;
            RaisePropertyChanged();
        }
    }
    public class CategoryQuestionHelper : ViewModelCommon
    {
        private string categoryTitle;
        private int questionId;
        private int quizzId;

        public virtual string CategoryTitle
        {
            get => categoryTitle;
            set => SetProperty<string>(ref categoryTitle, value);
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
