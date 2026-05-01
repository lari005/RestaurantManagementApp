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


        public List<Preparat> Preparate { get; set; } = new List<Preparat>();
        public List<Meniu> Meniuri { get; set; } = new List<Meniu>();
    }
}