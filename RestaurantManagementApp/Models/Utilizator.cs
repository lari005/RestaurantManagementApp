using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementApp.Models
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email { get; set; } 
        public string Telefon { get; set; }
        public string AdresaLivrare { get; set; }
        public string Parola { get; set; }
        public string Rol { get; set; } 
    }
}
