using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantManagementApp.Models;

namespace RestaurantManagementApp.Services
{
    public static class SessionService
    {
        
        public static Utilizator UtilizatorCurent { get; set; }
        public static bool EsteClient => UtilizatorCurent != null && UtilizatorCurent.Rol == "Client";
        public static bool EsteAngajat => UtilizatorCurent != null && UtilizatorCurent.Rol == "Angajat";
        public static Comanda ComandaCurenta { get; set; } = new Comanda();
    }
}