using System;
using System.Windows;
using System.Windows.Input;
using RestaurantManagementApp.DataAccess;
using RestaurantManagementApp.Services;
using RestaurantManagementApp.Views;

namespace RestaurantManagementApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _parola;
        private readonly UtilizatorDAL _utilizatorDAL;

        public LoginViewModel()
        {
            _utilizatorDAL = new UtilizatorDAL();
            LoginCommand = new RelayCommand(ExecutaLogin);
            ContinuaCaOaspeteCommand = new RelayCommand(ExecutaOaspete);
            DeschideInregistrareCommand = new RelayCommand(ExecutaDeschideInregistrare);
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Parola
        {
            get => _parola;
            set { _parola = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand DeschideInregistrareCommand { get; }
        public ICommand ContinuaCaOaspeteCommand { get; }

        private void ExecutaLogin(object obj)
        {
            
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Parola))
            {
                MessageBox.Show("Te rugăm să introduci email-ul și parola!");
                return;
            }

           
            var utilizator = _utilizatorDAL.Login(Email);

            if (utilizator != null && utilizator.Parola == Parola)
            {
                
                SessionService.UtilizatorCurent = utilizator;
                MessageBox.Show($"Bun venit, {utilizator.Prenume}! Te-ai logat ca {utilizator.Rol}.");

                
                DeschideMeniuSiInchideLogin(obj);
            }
            else
            {
                MessageBox.Show("Email sau parolă incorecte!");
            }
        }

        private void ExecutaOaspete(object obj)
        {
            
            SessionService.UtilizatorCurent = null;
            MessageBox.Show("Continui ca oaspete. Poți vedea meniul, dar nu poți comanda.");

            DeschideMeniuSiInchideLogin(obj);
        }

        private void DeschideMeniuSiInchideLogin(object obj)
        {
            
            var menuView = new MenuView();
            menuView.Show();

            
            if (obj is Window loginWindow)
            {
                loginWindow.Close();
            }
        }
        private void ExecutaDeschideInregistrare(object obj)
        {
            var registerView = new RegisterView();
            registerView.ShowDialog(); 
        }
    }
}