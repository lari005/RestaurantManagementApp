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
    public class ClientPanelViewModel : BaseViewModel
    {
        private ObservableCollection<Comanda> _comenziClient;
        public ObservableCollection<Comanda> ComenziClient
        {
            get => _comenziClient;
            set { _comenziClient = value; OnPropertyChanged(); }
        }

        public ICommand AnuleazaComandaCommand { get; }

        public ClientPanelViewModel()
        {
            IncarcaComenzi();
            AnuleazaComandaCommand = new RelayCommand(ExecutaAnulare);
        }

        private void IncarcaComenzi()
        {
            if (SessionService.UtilizatorCurent != null)
            {
                ComandaDAL dal = new ComandaDAL();
                var lista = dal.GetComenziClient(SessionService.UtilizatorCurent.Id);
                ComenziClient = new ObservableCollection<Comanda>(lista);
            }
        }

        private void ExecutaAnulare(object obj)
        {
            if (obj is Comanda comanda)
            {
               
                if (!comanda.PoateFiAnulata)
                {
                    MessageBox.Show("Această comandă nu mai poate fi anulată deoarece a intrat deja în preparare sau a fost livrată.");
                    return;
                }

                var rezultat = MessageBox.Show("Ești sigur că vrei să anulezi această comandă?", "Confirmare Anulare", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (rezultat == MessageBoxResult.Yes)
                {
                    ComandaDAL dal = new ComandaDAL();
                    if (dal.AnuleazaComanda(comanda.Id))
                    {
                        MessageBox.Show("Comanda a fost anulată cu succes.");
                        IncarcaComenzi(); 
                    }
                    else
                    {
                        MessageBox.Show("A apărut o eroare la anularea comenzii.");
                    }
                }
            }
        }
    }
}