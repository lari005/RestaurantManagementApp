using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RestaurantManagementApp.Models;
using RestaurantManagementApp.DataAccess;

namespace RestaurantManagementApp.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _nume;
        private string _prenume;
        private string _email;
        private string _telefon;
        private string _adresaLivrare;
        private string _parola;

        public string Nume { get => _nume; set { _nume = value; OnPropertyChanged(); } }
        public string Prenume { get => _prenume; set { _prenume = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string Telefon { get => _telefon; set { _telefon = value; OnPropertyChanged(); } }
        public string AdresaLivrare { get => _adresaLivrare; set { _adresaLivrare = value; OnPropertyChanged(); } }
        public string Parola { get => _parola; set { _parola = value; OnPropertyChanged(); } }

        public ICommand InregistrareCommand { get; }

        public RegisterViewModel()
        {
            InregistrareCommand = new RelayCommand(ExecutaInregistrare);
        }

        private void ExecutaInregistrare(object obj)
        {
            if (string.IsNullOrWhiteSpace(Nume) || string.IsNullOrWhiteSpace(Prenume) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Parola))
            {
                MessageBox.Show("Numele, Prenumele, Email-ul și Parola sunt obligatorii!");
                return;
            }

            var utilizatorNou = new Utilizator
            {
                Nume = this.Nume,
                Prenume = this.Prenume,
                Email = this.Email,
                Telefon = this.Telefon ?? "",
                AdresaLivrare = this.AdresaLivrare,
                Parola = this.Parola
            };

            UtilizatorDAL dal = new UtilizatorDAL();
            bool succes = dal.InregistrareClient(utilizatorNou);

            if (succes)
            {
                MessageBox.Show("Cont creat cu succes! Acum te poți autentifica.");

                if (obj is Window window)
                {
                    window.Close();
                }
            }
            else
            {
                MessageBox.Show("Eroare la creare cont. Acest email este deja folosit!");
            }
        }
    }
}