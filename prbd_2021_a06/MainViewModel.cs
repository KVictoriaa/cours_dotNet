using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using prbd_2021_a06.Model;
using PRBD_Framework;

namespace prbd_2021_a06 { 
    public class MainViewModel : ViewModelBase<Context> {

        
        
        private string filter;
        public string Filter {
            get => filter;
            set => SetProperty(ref filter, value, ApplyFilterAction);
        }

        public ICommand ClearFilter { get; set; }

        public MainViewModel() : base() {
          
            ClearFilter = new RelayCommand(() => Filter = "");
        }

        private void ApplyFilterAction() {
          
        }

        protected override void OnRefreshData() {
            // pour plus tard
        }
    }
}
