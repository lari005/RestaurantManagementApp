using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.DataAccess;
using RestaurantManagementApp.Services; 
namespace RestaurantManagementApp.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly PreparatDAL _preparatDAL = new PreparatDAL();
        private string _textCautare;
        private ObservableCollection<Preparat> _preparateAfisate;
        private List<Preparat> _toatePreparatele;

        private Preparat _preparatSelectat;
        public Preparat PreparatSelectat
        {
            get => _preparatSelectat;
            set { _preparatSelectat = value; OnPropertyChanged(); }
        }

        private int _cantitateSelectata = 1;
        public int CantitateSelectata
        {
            get => _cantitateSelectata;
            set { _cantitateSelectata = value; OnPropertyChanged(); }
        }

        
        public bool EsteClientLogat => SessionService.EsteClient;

        public ICommand AdaugaInCosCommand { get; }

        public MenuViewModel()
        {
            IncarcaDatele();
            AdaugaInCosCommand = new RelayCommand(ExecutaAdaugaInCos);
        }

        public string TextCautare
        {
            get => _textCautare;
            set
            {
                _textCautare = value;
                OnPropertyChanged();
                ExecutaFiltrare();
            }
        }

        public ObservableCollection<Preparat> PreparateAfisate
        {
            get => _preparateAfisate;
            set { _preparateAfisate = value; OnPropertyChanged(); }
        }

        private void IncarcaDatele()
        {
            _toatePreparatele = _preparatDAL.GetToatePreparatele();
            PreparateAfisate = new ObservableCollection<Preparat>(_toatePreparatele);
        }

        private void ExecutaFiltrare()
        {
            if (string.IsNullOrWhiteSpace(TextCautare))
            {
                PreparateAfisate = new ObservableCollection<Preparat>(_toatePreparatele);
            }
            else
            {
                string term = TextCautare.ToLower();

                var filtrat = _toatePreparatele.Where(p =>
                    (p.Denumire != null && p.Denumire.ToLower().Contains(term)) ||
                    (p.Alergeni != null && p.Alergeni.Any(a => a.ToLower().Contains(term)))
                ).ToList();

                PreparateAfisate = new ObservableCollection<Preparat>(filtrat);
            }
        }
        private void ExecutaAdaugaInCos(object obj)
        {
            if (PreparatSelectat == null)
            {
                MessageBox.Show("Te rugăm să selectezi un preparat din listă!");
                return;
            }

            if (CantitateSelectata <= 0 || CantitateSelectata > PreparatSelectat.CantitateTotala)
            {
                MessageBox.Show("Cantitate invalidă sau stoc insuficient!");
                return;
            }

            for (int i = 0; i < CantitateSelectata; i++)
            {
                SessionService.ComandaCurenta.Preparate.Add(PreparatSelectat);
            }

            MessageBox.Show($"Au fost adăugate {CantitateSelectata} porții de {PreparatSelectat.Denumire} în coș!");

            CantitateSelectata = 1;
        }
    }
}