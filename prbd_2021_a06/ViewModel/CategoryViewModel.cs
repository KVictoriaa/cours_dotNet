using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;

namespace prbd_2021_a06.ViewModel {
    public class CategoryViewModel : ViewModelCommon {
        private ObservableCollection<CategoryQuestionsHelper> categoryQuestions ;
        public ObservableCollection<CategoryQuestionsHelper> CategoryQuestions {
            get => categoryQuestions;
            set => SetProperty<ObservableCollection<CategoryQuestionsHelper>>(ref categoryQuestions , value);
        }
        private int courseId;
        public int CourseId {
            get => courseId;
            set => SetProperty<int>(ref courseId, value, OnRefreshData);
        }
        public ICommand RegisterCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public CategoryViewModel(){
            RegisterCommand = new RelayCommand(() => SaveItem());
            DeleteCommand = new RelayCommand<int>((id) => DeleteItem(id));
            CancelCommand = new RelayCommand(() => OnRefreshData());
        }

        public void SaveItem() {
            foreach (var item in CategoryQuestions) {
                // Cas d'enregistrement
                if(item.Id==0  && !string.IsNullOrEmpty(item.CategoryName)) {
                     new Category(item.CategoryName).AddNew();
                    OnRefreshData();
                }
                
            }
        }

        public void DeleteItem(int id) {
            var item = App.Context.Categories.Find(id);
            if (item != null) {
                item.Remove();
                OnRefreshData();
            }
        }

        protected override void OnRefreshData() {
            var filter = App.Context.Categories
                .Where(c => c.CategoryQuestions.Any(cq => cq.Questions.Course.Id == courseId) || c.CategoryQuestions == null)
                .OrderBy(c => c.Title)
                .Select(c => new CategoryQuestionsHelper() {
                    CategoryName = c.Title,
                    Id = c.Id,
                    NumberOfQuestion = c.CategoryQuestions != null ? c.CategoryQuestions.Count() : 0
                }).ToList();

            filter.Add(new CategoryQuestionsHelper() { CategoryName = "" });

            CategoryQuestions = new ObservableCollection<CategoryQuestionsHelper>(filter);
        }

    }
    public class CategoryQuestionsHelper: ViewModelCommon {
        private int id;
        public int Id {
            get => id;
            set => SetProperty<int>(ref id, value);
        }

        private string categoryName;
        public string CategoryName {
            get => categoryName;
            set => SetProperty<string>(ref categoryName, value);
        }

        private int numberOfQuestion;
        public int NumberOfQuestion {
            get => numberOfQuestion;
            set => SetProperty<int>(ref numberOfQuestion, value);
        }

        protected override void OnRefreshData() {
            throw new NotImplementedException();
        }
    }
}
