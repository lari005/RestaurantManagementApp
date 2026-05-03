using RestaurantManagementApp.DataAccess;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManagementApp.ViewModels
{
    public class CosViewModel : BaseViewModel
    {
        public ObservableCollection<Preparat> ProduseInCos { get; set; }

        private decimal _costMancare;
        public decimal CostMancare
        {
            get => _costMancare;
            set { _costMancare = value; OnPropertyChanged(); }
        }

        private decimal _costTransport;
        public decimal CostTransport
        {
            get => _costTransport;
            set { _costTransport = value; OnPropertyChanged(); }
        }

        private decimal _discountAplicat;
        public decimal DiscountAplicat
        {
            get => _discountAplicat;
            set { _discountAplicat = value; OnPropertyChanged(); }
        }

        private decimal _costTotal;
        public decimal CostTotal
        {
            get => _costTotal;
            set { _costTotal = value; OnPropertyChanged(); }
        }
        private string _adresaLivrareFinala;
        public string AdresaLivrareFinala
        {
            get => _adresaLivrareFinala;
            set { _adresaLivrareFinala = value; OnPropertyChanged(); }
        }
        public ICommand PlaseazaComandaCommand { get; }
        public ICommand StergeDinCosCommand { get; }

        public CosViewModel()
        {
            
            ProduseInCos = new ObservableCollection<Preparat>(SessionService.ComandaCurenta.Preparate);
            CalculeazaTotaluri();

            PlaseazaComandaCommand = new RelayCommand(ExecutaPlasare);
            StergeDinCosCommand = new RelayCommand(ExecutaStergere);
            AdresaLivrareFinala = SessionService.UtilizatorCurent?.AdresaLivrare;
        }

        private void CalculeazaTotaluri()
        {
            CostMancare = ProduseInCos.Sum(p => p.Pret);

            if (CostMancare == 0)
            {
                CostTransport = 0; DiscountAplicat = 0; CostTotal = 0;
                return;
            }

            
            decimal limitaTransport_a = ConfigService.GetLimitaTransport_A();
            decimal costTransport_b = ConfigService.GetCostTransport_B();
            decimal sumaMinimaDiscount_y = ConfigService.GetSumaMinimaDiscount_Y();
            decimal procentDiscount_w = ConfigService.GetProcentDiscount_W();

            
            CostTransport = CostMancare < limitaTransport_a ? costTransport_b : 0;

            
            DiscountAplicat = CostMancare >= sumaMinimaDiscount_y ? (CostMancare * procentDiscount_w / 100) : 0;

            
            CostTotal = CostMancare - DiscountAplicat + CostTransport;
        }

        private void ExecutaStergere(object obj)
        {
            if (obj is Preparat preparat)
            {
                ProduseInCos.Remove(preparat);
                SessionService.ComandaCurenta.Preparate.Remove(preparat);
                CalculeazaTotaluri(); 
            }
        }

        private void ExecutaPlasare(object obj)
        {
            if (ProduseInCos.Count == 0)
            {
                MessageBox.Show("Coșul tău este gol!");
                return;
            }

            
            if (string.IsNullOrWhiteSpace(AdresaLivrareFinala))
            {
                MessageBox.Show("Te rugăm să introduci o adresă de livrare pentru comandă!");
                return;
            }

            var comandaNoua = new Comanda
            {
                UtilizatorId = SessionService.UtilizatorCurent.Id,
                CostTransport = CostTransport, 
                ProcentDiscount = (DiscountAplicat > 0) ? ConfigService.GetProcentDiscount_W() : 0,
                Preparate = ProduseInCos.ToList()
            };

            ComandaDAL comandaDAL = new ComandaDAL();
            bool success = comandaDAL.PlaseazaComanda(comandaNoua);

            if (success)
            {
                MessageBox.Show($"Comanda a fost plasată cu succes și va fi livrată la adresa:\n{AdresaLivrareFinala}");

                ProduseInCos.Clear();
                SessionService.ComandaCurenta.Preparate.Clear();
                CalculeazaTotaluri();
            }
            else
            {
                MessageBox.Show("Eroare la procesarea comenzii în baza de date.");
            }
        }
    }
}