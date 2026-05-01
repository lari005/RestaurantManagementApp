using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models
{
    public class Meniu
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
        public int CategorieId { get; set; }
        public decimal PretCalculat { get; set; }

       
        public List<Preparat> PreparateComponente { get; set; } = new List<Preparat>();

        public bool IsDisponibil { get; set; } 
    }
}