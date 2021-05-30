using prbd_2021_a06.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRBD_Framework;

namespace prbd_2021_a06.ViewModel
{
    public class PropositionViewModel : ViewModelCommon
    {
        private ObservableCollection<Proposition> propositions = new ObservableCollection<Proposition>();
        public ObservableCollection<Proposition> Propositionns
        {
            get => propositions;
            set => SetProperty(ref propositions, value);
        }


        public PropositionViewModel() : base()
        {
            Propositionns = new ObservableCollectionFast<Proposition>(App.Context.Propositions);
        }
        protected override void OnRefreshData()
        {
            
        }
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
            //set { EditMode = !value; }
        }
        /*private void LoadProposition()
        {
            if (Question == null) return;

            var ids = SelectedQuestions.Cast<Question>().Select(m => m.Id).ToList();
            //Recupère la liste des cours;
            Questions.RefreshFromModel(Course.GetReceivedAndVisibleMessages(Course));

        }*/


    }
}
