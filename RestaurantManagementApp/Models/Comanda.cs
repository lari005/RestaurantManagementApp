using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantManagementApp.Models
{
    public class Comanda
    {
        public int Id { get; set; }
        public Guid CodUnic { get; set; } 
        public int UtilizatorId { get; set; }
        public DateTime DataComanda { get; set; }
        public string Stare { get; set; } 
        public DateTime? OraEstimativaLivrare { get; set; }

        public decimal CostMancare { get; set; }
        public decimal CostTransport { get; set; }
        public decimal ProcentDiscount { get; set; }
        public decimal CostTotal { get; set; }
        public string ProduseFormatate { get; set; }
        public bool PoateFiAnulata => Stare == "inregistrata";
        public string NumeClient { get; set; }
        public string TelefonClient { get; set; }
        public string AdresaClient { get; set; }
        public string StareNoua { get; set; }
        public List<string> StariDisponibile { get; } = new List<string>
        {
            "inregistrata", "se pregateste", "a plecat la client", "livrata", "anulata"
        };
        public List<Preparat> Preparate { get; set; } = new List<Preparat>();
        public List<Meniu> Meniuri { get; set; } = new List<Meniu>();
    }
}