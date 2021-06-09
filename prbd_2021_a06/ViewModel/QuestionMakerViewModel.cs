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
using prbd_2021_a06.Properties;
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
       
        
        private ObservableCollection<CategoryQuestion> categoryQuestions = new ObservableCollection<CategoryQuestion>();
        public ObservableCollection<CategoryQuestion> CategoryQuestions
        {
            get => categoryQuestions;
            set => SetProperty(ref categoryQuestions, value);
        }
        private ObservableCollection<Category> category = new ObservableCollection<Category>();
        public ObservableCollection<Category> Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }
        private ObservableCollection<Category> categoriesSelectQuestion;
        public ObservableCollection<Category> CategoriesSelectQuestion
        {
            get => categoriesSelectQuestion;
            set => SetProperty(ref categoriesSelectQuestion, value);
        }
        private string filter;
        public string Filter
        {
            get => filter;
            set => SetProperty<string>(ref filter, value, OnRefreshData);
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
        public ICommand NewCommand { get; set; }
        public ICommand AllCategories { get; set; }
        public ICommand None { get; set; }
        public ICommand CategoryChecked { get; set; }


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
                {
                    Console.WriteLine("not new");
                    /*Question = (from u in App.Context.Questions
                                where u.Enonce.Equals(Enonce) && u.Course.Id == course.Id
                                select u).FirstOrDefault();*/
                    App.Context.Propositions.RemoveRange(Question.Propositions);

                }

                List<string> listOfNames = new List<string>(propositionsString.Split(Environment.NewLine));
                foreach (var proposition in listOfNames)
                {
                    Proposition prop = new Proposition();
                    prop.Question = Question;

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
                foreach (var c in Category)
                {
                    if (c.IsChecked)
                    {
                        CategoryQuestion categoryQuestion = new CategoryQuestion();
                        categoryQuestion.Question = Question;
                        categoryQuestion.Category = c;
                        App.Context.CategoryQuestions.Add(categoryQuestion);
                    }
                }

                Context.SaveChanges();
                
                EditMode = false;
                NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS,Question.Enonce);
            },
            
            () => EditMode );

            CancelCommand = new RelayCommand(() => {
                Context.Entry(Question).Reload();
                EditMode = false;
                //Validate();
                RaisePropertyChanged();
            },
            () => EditMode);

           /* RefreshCommand = new RelayCommand(() => {
                Questions = new ObservableCollectionFast<Question>(Course.Questions);
            },
            () => ReadMode);*/

            NewCommand = new RelayCommand(() => {
                isNew = true;
                EditMode = true;
                Question = new Question("", Type);
                Question.Course = Course;
               
            },
            () => ReadMode);

            Register<String>(this, AppContext.MSG_REFRESH_QUESTIONS, _ => {
                Questions = new ObservableCollectionFast<Question>(Course.Questions);
            });
            //EditMode = false;
            DeleteCommand = new RelayCommand(() =>
            {
                
                    Question.Delete();
                    App.Context.SaveChanges();
                    Questions = new ObservableCollectionFast<Question>(course.Questions);
                    //NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS);
                
            },
            () => EditMode);
            AllCategories = new RelayCommand(() =>
            {

                var Categs = new ObservableCollection<Category>(); //je creer une nouvelle liste de catégorie que je mets à jour avec ma liste de catégorie.

                foreach (var categories in Category)
                {
                    categories.IsChecked  = true;
                    Categs.Add(categories);
                    
                }
                
                Category = new ObservableCollection<Category>(Categs);
                Questions = new ObservableCollectionFast<Question>(course.Questions);
                
              
            });
          
            //CategoryQuestion();
            None = new RelayCommand(() =>
            {
                var Categs = new ObservableCollection<Category>(); //je creer une nouvelle liste de catégorie que je mets à jour avec ma liste de catégorie.

                foreach (var categories in Category)
                {
                    categories.IsChecked = false;
                    Categs.Add(categories);

                }
               
                Category = new ObservableCollection<Category>(Categs);
                Questions = new ObservableCollectionFast<Question>();
            });
            CategoryChecked = new RelayCommand(() =>
            {
                var ques = new ObservableCollectionFast<Question>();
                var courseQuestion = new ObservableCollectionFast<Question>(course.Questions);
                foreach(var c in Category)
                {
                    if(c.IsChecked)
                    {
                        foreach(var q in courseQuestion)
                        {
                            var catego = new ObservableCollection<Category>(q.Categories);
                            if(catego.Contains(c))
                            {
                                ques.Add(q);
                            }
                        }
                    }
                }
                Questions = new ObservableCollectionFast<Question>(ques);
            });
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

        private bool editMode;
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
            Category = new ObservableCollection<Category>(Course.Categories);
            
               
        }
        //private void CancelAction()
        //{
        //    if (imageHelper.IsTransitoryState)
        //    {
        //        imageHelper.Cancel();
        //    }
        //    if (IsNew)
        //    {
        //        NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Member);
        //    }
        //    else
        //    {
        //        Context.Reload(Member);
        //        RaisePropertyChanged();
        //    }
        //}
        //private void DeleteAction()
        //{
        //    CancelAction();
            
        //    Question.Delete();
        //    NotifyColleagues(AppMessages.MSG_MEMBER_CHANGED, Member);
        //    NotifyColleagues(AppMessages.MSG_CLOSE_TAB, Member);
        //}
        public string Enonce
        {
            get { return Question?.Enonce; }
            set
            {
                Question.Enonce = value;
                //EditMode = true;
                RaisePropertyChanged(nameof(Enonce));
                NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS,Question.Enonce);
                Validate();
            }
        }
       
        public ObservableCollection<Proposition> propositions;
        public ObservableCollection<Proposition> Propositions
        {
            get { return propositions; }
            set
            {
               
               Question.Propositions = value;
              //  PropositionsString = string.Join(",", Question.Propositions);
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
                
                RaisePropertyChanged(nameof(PropositionsString));
                
                NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS, Question.Enonce);
                
                var Props = new ObservableCollection<Proposition>();
                List<string> listOfNames = new List<string>(propositionsString.Split(Environment.NewLine));
                foreach (var proposition in listOfNames)
                {
                    Proposition prop = new Proposition();
                    prop.Question = Question;

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

                    Props.Add(prop);
                }
                Question.Propositions = Props;
                Validate();
            }
        }
        public Type type ;
        public Type Type

        {
            get {return type; }
            set
            {
                Question.Type = value;
                //EditMode = true;
                RaisePropertyChanged(nameof(Type));
                NotifyColleagues(AppContext.MSG_REFRESH_QUESTIONS, Question.Enonce);
                //RaisePropertyChanged();
                Validate();
            }
        }


        private Question selectedQuestion;
        public Question SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {

                EditMode = true;
                selectedQuestion = value;
                question = selectedQuestion;

                if (question != null)
                {
                    propositions = new ObservableCollection<Proposition>(Question.Propositions);
                    List<string> bodyList = new List<string>();
                    foreach (var proposition in question.Propositions)
                    {
                        if (proposition.IsCorrect)
                        {
                            var body = proposition.Body.Insert(0, "*");
                            bodyList.Add(body);
                        }
                        else
                        {
                            bodyList.Add(proposition.Body);
                        }

                    }

                    propositionsString = String.Join(System.Environment.NewLine, bodyList);
                    var cat = new ObservableCollection<Category>(question.Categories);
                    var catego = new ObservableCollection<Category>();
                    foreach (var cate in Category)
                    {
                        cate.IsChecked = cat.Contains(cate);
                        catego.Add(cate);
                    }
                    CategoriesSelectQuestion = new ObservableCollection<Category>(catego);

                    type = Question.Type;
                    

                }
                RaisePropertyChanged(nameof(SelectedQuestion));
               
                RaisePropertyChanged();
                

            }
        }

        protected override void OnRefreshData()
        {
            //CategoryQuestion();
            //EditMode = false;
            RaisePropertyChanged();
            
            

        }
        public override bool Validate()
        {
            ClearErrors();

            var user = (from u in App.Context.Questions
                        where u.Enonce.Equals(Enonce)
                        select u).FirstOrDefault();



            if (string.IsNullOrEmpty(Enonce))
            {
                AddError(nameof(Enonce), Resources.Error_Required);
            }

            List<string> addAnswers = new List<string>(PropositionsString.Split(Environment.NewLine));
            int cpt = 0;
            foreach (var answer in addAnswers)
            {
                Proposition proposition = null;
                if (answer.Contains("*"))
                {
                    cpt++;
                }
            }

            if (string.IsNullOrEmpty(PropositionsString))
            {
                AddError(nameof(PropositionsString), Resources.Error_Required);
            }
            else if (cpt == 1 && Question.Type == Type.Many)
                    AddError(nameof(PropositionsString), Resources.Error_PropositionMany);
            else if (cpt > 1 && Question.Type == Type.One  )
                AddError(nameof(PropositionsString), Resources.Error_PropositionOne);


            RaiseErrors();
            return !HasErrors;
        }


    }

}
