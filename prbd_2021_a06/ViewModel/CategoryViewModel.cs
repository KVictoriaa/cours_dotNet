using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;

namespace prbd_2021_a06.ViewModel
{
    public class CategoryViewModel : ViewModelCommon
    {
        private ObservableCollection<CategoryCoursesHelper> categoryCourses;
        public ObservableCollection<CategoryCoursesHelper> CategoryCourses
        {
            get => categoryCourses;
            set => SetProperty<ObservableCollection<CategoryCoursesHelper>>(ref categoryCourses, value);
        }
        private int courseId;
        public int CourseId
        {
            get => courseId;
            set => SetProperty<int>(ref courseId, value, OnRefreshData);
        }

        private bool showSave = false;
        public bool ShowSave {
            get => showSave; 
            set => SetProperty<bool>(ref showSave, value);
        }

        private bool showDelete = false;
        public bool ShowDelete {
            get => showDelete;
            set => SetProperty<bool>(ref showDelete, value);
        }


        public ICommand RegisterCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public CategoryViewModel()
        {
            RegisterCommand = new RelayCommand(() => SaveItem());
            DeleteCommand = new RelayCommand<int>((id) => DeleteItem(id));
            CancelCommand = new RelayCommand(() => OnRefreshData());
        }

        public void SaveItem()
        {
            foreach (var item in CategoryCourses)
            {
                // Cas d'enregistrement
                if (item.IdCategory == 0 && !string.IsNullOrEmpty(item.CategoryName))
                {
                    var category = new Category(item.CategoryName);
                    category.Course = App.Context.Courses.Find(item.IdCourse);
                    category.AddNew();
                    OnRefreshData();
                }


            }
            NotifyColleagues(AppContext.MSG_CATEGORY);
            NotifyColleagues(AppContext.MSG_CATEGORY_CHANGE);
        }

        public void DeleteItem(int id) {
            var item = App.Context.Categories.Find(id);
            if (item != null) {
                item.Remove();


                OnRefreshData();
            }
            NotifyColleagues(AppContext.MSG_CATEGORY);
            NotifyColleagues(AppContext.MSG_CATEGORY_CHANGE );
        }

         

        protected override void OnRefreshData()
        {
            var cs = App.Context.Courses.ToList();
            var filter = App.Context.Categories
                .Where(c => c.Course.Id == courseId)
                .OrderBy(c => c.Title)
                .Select(c => new CategoryCoursesHelper()
                {
                    CategoryName = c.Title,
                    IdCategory = c.Id,
                    IdCourse = c.Course.Id,
                    NumberOfQuestion = c.CategoryQuestions != null ? c.CategoryQuestions.Count() : 0
                }).ToList();

            filter.Add(new CategoryCoursesHelper() { CategoryName = "", IdCourse = courseId });

            CategoryCourses = new ObservableCollection<CategoryCoursesHelper>(filter);
        }

    }
    public class CategoryCoursesHelper : ViewModelCommon
    {
        private int idCategory;
        public int IdCategory
        {
            get => idCategory;
            set => SetProperty<int>(ref idCategory, value);
        }

        private int idCourse;
        public int IdCourse
        {
            get => idCourse;
            set => SetProperty<int>(ref idCourse, value);
        }

        private string categoryName;
        public string CategoryName
        {
            get => categoryName;
            set => SetProperty<string>(ref categoryName, value);
        }

        private int? numberOfQuestion;
        public int? NumberOfQuestion
        {
            get => numberOfQuestion;
            set => SetProperty<int?>(ref numberOfQuestion, value);
        }

        protected override void OnRefreshData()
        {
            //throw new NotImplementedException();
        }
    }
}
