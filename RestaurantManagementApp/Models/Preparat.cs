using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models
{
    public class Preparat
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public decimal Pret { get; set; }
        public decimal CantitatePortie { get; set; } 
        public decimal CantitateTotala { get; set; } 
        public int CategorieId { get; set; }
        public string NumeCategorie { get; set; } 

        
        public List<string> Alergeni { get; set; } = new List<string>();
        public List<string> Imagini { get; set; } = new List<string>();

        public bool IsDisponibil => CantitateTotala > 0; 
    }
}