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

namespace RestaurantManagementApp.ViewModels
{
    public class CrudViewModel : BaseViewModel
    {
        private readonly CrudDAL _dal = new CrudDAL();

        
        private ObservableCollection<Categorie> _categorii;
        public ObservableCollection<Categorie> Categorii { get => _categorii; set { _categorii = value; OnPropertyChanged(); } }

        private Categorie _categorieSelectata;
        public Categorie CategorieSelectata
        {
            get => _categorieSelectata;
            set
            {
                _categorieSelectata = value;
                OnPropertyChanged();
                if (value != null) NumeCategorie = value.Nume;
            }
        }

        private string _numeCategorie;
        public string NumeCategorie { get => _numeCategorie; set { _numeCategorie = value; OnPropertyChanged(); } }

        public ICommand AdaugaCategorieCommand { get; }
        public ICommand ModificaCategorieCommand { get; }
        public ICommand StergeCategorieCommand { get; }

        private ObservableCollection<Alergen> _alergeni;
        public ObservableCollection<Alergen> Alergeni { get => _alergeni; set { _alergeni = value; OnPropertyChanged(); } }

        private Alergen _alergenSelectat;
        public Alergen AlergenSelectat
        {
            get => _alergenSelectat;
            set
            {
                _alergenSelectat = value;
                OnPropertyChanged();
                if (value != null) NumeAlergen = value.Nume;
            }
        }

        private string _numeAlergen;
        public string NumeAlergen { get => _numeAlergen; set { _numeAlergen = value; OnPropertyChanged(); } }

        public ICommand AdaugaAlergenCommand { get; }
        public ICommand ModificaAlergenCommand { get; }
        public ICommand StergeAlergenCommand { get; }

        public CrudViewModel()
        {
            IncarcaDate();

            AdaugaCategorieCommand = new RelayCommand(o => { _dal.InsertCategorie(NumeCategorie); ResetCategorie(); });
            ModificaCategorieCommand = new RelayCommand(o => { if (CategorieSelectata != null) { _dal.UpdateCategorie(CategorieSelectata.Id, NumeCategorie); ResetCategorie(); } });
            StergeCategorieCommand = new RelayCommand(o => { if (CategorieSelectata != null) { _dal.DeleteCategorie(CategorieSelectata.Id); ResetCategorie(); } });

            AdaugaAlergenCommand = new RelayCommand(o => { _dal.InsertAlergen(NumeAlergen); ResetAlergen(); });
            ModificaAlergenCommand = new RelayCommand(o => { if (AlergenSelectat != null) { _dal.UpdateAlergen(AlergenSelectat.Id, NumeAlergen); ResetAlergen(); } });
            StergeAlergenCommand = new RelayCommand(o => { if (AlergenSelectat != null) { _dal.DeleteAlergen(AlergenSelectat.Id); ResetAlergen(); } });
        }

        private void IncarcaDate()
        {
            Categorii = new ObservableCollection<Categorie>(_dal.GetCategorii());
            Alergeni = new ObservableCollection<Alergen>(_dal.GetAlergeni());
        }

        private void ResetCategorie() { NumeCategorie = ""; IncarcaDate(); }
        private void ResetAlergen() { NumeAlergen = ""; IncarcaDate(); }
    }
}