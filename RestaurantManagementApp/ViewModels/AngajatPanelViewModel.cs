using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.DataAccess;
using RestaurantManagementApp.Services;

namespace RestaurantManagementApp.ViewModels
{
    public class AngajatPanelViewModel : BaseViewModel
    {
        private ObservableCollection<Comanda> _comenzi;
        public ObservableCollection<Comanda> Comenzi
        {
            get => _comenzi;
            set { _comenzi = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Preparat> _preparateEpuizate;
        public ObservableCollection<Preparat> PreparateEpuizate
        {
            get => _preparateEpuizate;
            set { _preparateEpuizate = value; OnPropertyChanged(); }
        }

        private bool _doarActive = true; 
        public bool DoarActive
        {
            get => _doarActive;
            set
            {
                _doarActive = value;
                OnPropertyChanged();
                IncarcaComenzi(); 
            }
        }

        public ICommand SalveazaStareCommand { get; }
        public ICommand DeschideCrudCommand { get; }

        public AngajatPanelViewModel()
        {
            SalveazaStareCommand = new RelayCommand(ExecutaSalvareStare);
            DeschideCrudCommand = new RelayCommand(ExecutaDeschideCrud);
            IncarcaComenzi();
            IncarcaPreparateEpuizate();
        }

        private void IncarcaComenzi()
        {
            ComandaDAL dal = new ComandaDAL();
            var lista = dal.GetComenziPentruAngajat(DoarActive);

            foreach (var c in lista)
            {
                c.StareNoua = c.Stare;
            }

            Comenzi = new ObservableCollection<Comanda>(lista);
        }

        private void IncarcaPreparateEpuizate()
        {
            PreparatDAL dal = new PreparatDAL();
            decimal limita = ConfigService.GetLimitaEpuizare_C(); 
            var lista = dal.GetPreparateEpuizare(limita);
            PreparateEpuizate = new ObservableCollection<Preparat>(lista);
        }

        private void ExecutaSalvareStare(object obj)
        {
            if (obj is Comanda comanda)
            {
                if (comanda.Stare == "livrata" || comanda.Stare == "anulata")
                {
                    MessageBox.Show("Comenzile livrate sau anulate nu mai pot fi modificate!");
                    return;
                }

                ComandaDAL dal = new ComandaDAL();
                if (dal.ModificaStareComanda(comanda.Id, comanda.StareNoua))
                {
                    MessageBox.Show("Starea comenzii a fost actualizată cu succes!");
                    IncarcaComenzi();
                }
                else
                {
                    MessageBox.Show("A apărut o eroare la actualizarea stării.");
                }
            }
        }
        private void ExecutaDeschideCrud(object obj)
        {
            var crudView = new RestaurantManagementApp.Views.CrudView();
            crudView.ShowDialog();
        }
    }
}
