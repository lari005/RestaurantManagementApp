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
        public string PrimaImagine
        {
            get
            {
                if (Imagini != null && Imagini.Count > 0)
                {
                    string caleRelativa = Imagini[0].TrimStart('/');

                    string caleAbsoluta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, caleRelativa);

                    if (System.IO.File.Exists(caleAbsoluta))
                    {
                        return caleAbsoluta;
                    }

                    return Imagini[0];
                }
                return null;
            }
        }
        public bool IsDisponibil => CantitateTotala > 0; 
    }
}