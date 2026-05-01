using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Linq;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.DataAccess;
using System.Collections.Generic;

namespace RestaurantManagementApp.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly PreparatDAL _preparatDAL = new PreparatDAL();
        private string _textCautare;
        private ObservableCollection<Preparat> _preparateAfisate;
        private List<Preparat> _toatePreparatele;

        public MenuViewModel()
        {
            IncarcaDatele();
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
                
                var filtrat = _toatePreparatele.Where(p =>
                    p.Denumire.ToLower().Contains(TextCautare.ToLower())).ToList();
                PreparateAfisate = new ObservableCollection<Preparat>(filtrat);
            }
        }
    }
}