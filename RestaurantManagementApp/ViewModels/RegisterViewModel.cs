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
        public List<string> RoluriDisponibile { get; } = new List<string> { "Client", "Angajat" };

        private string _rolSelectat = "Client"; 
        public string RolSelectat
        {
            get => _rolSelectat;
            set { _rolSelectat = value; OnPropertyChanged(); }
        }
        private string _codAngajat;
        public string CodAngajat
        {
            get => _codAngajat;
            set { _codAngajat = value; OnPropertyChanged(); }
        }
        public ICommand InregistrareCommand { get; }

        public RegisterViewModel()
        {
            InregistrareCommand = new RelayCommand(ExecutaInregistrare);
        }

        private void ExecutaInregistrare(object obj)
        {
            if (string.IsNullOrWhiteSpace(Nume) || string.IsNullOrWhiteSpace(Prenume) ||
                string.IsNullOrWhiteSpace(AdresaLivrare) || string.IsNullOrWhiteSpace(Parola))
            {
                MessageBox.Show("Numele, Prenumele, Adresa și Parola sunt obligatorii!");
                return;
            }

            string emailFinal = this.Email;
            
            if (RolSelectat == "Angajat")
            {
                if (CodAngajat != "061005")
                {
                    MessageBox.Show("Cod de securitate incorect!");
                    return;
                }

               
                string numeCurat = Nume.Replace(" ", "").ToLower();
                string prenumeCurat = Prenume.Replace(" ", "").ToLower();
                emailFinal = $"{numeCurat}.{prenumeCurat}@angajat.ro";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(emailFinal))
                {
                    MessageBox.Show("Te rugăm să introduci o adresă de email personală!");
                    return;
                }
            }

            var utilizatorNou = new Utilizator
            {
                Nume = this.Nume,
                Prenume = this.Prenume,
                Email = emailFinal,
                Telefon = this.Telefon ?? "",
                AdresaLivrare = this.AdresaLivrare,
                Parola = this.Parola,
                Rol = this.RolSelectat
            };

            UtilizatorDAL dal = new UtilizatorDAL();
            bool succes = dal.InregistrareClient(utilizatorNou);

            if (succes)
            {
                if (RolSelectat == "Angajat")
                {
                    MessageBox.Show($"Cont de angajat creat!\nATENȚIE: Te vei autentifica folosind email-ul: {emailFinal}");
                }
                else
                {
                    MessageBox.Show("Cont de client creat cu succes!");
                }

                if (obj is Window window) window.Close();
            }
            else
            {
                MessageBox.Show("Eroare! Email-ul există deja sau datele sunt invalide.");
            }
        }
    }
}