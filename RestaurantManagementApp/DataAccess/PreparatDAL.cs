using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;
using System.Collections.Generic;
using System.Data;

namespace RestaurantManagementApp.DataAccess
{
    public class PreparatDAL
    {
       
        public List<Preparat> GetToatePreparatele()
        {
            List<Preparat> lista = new List<Preparat>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                
                SqlCommand cmd = new SqlCommand("sp_SelectPreparateDisponibile", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Preparat
                        {
                            Id = (int)reader["Id"],
                            Denumire = reader["Denumire"].ToString(),
                            Pret = (decimal)reader["Pret"],
                            CantitatePortie = (decimal)reader["CantitatePortie"],
                            NumeCategorie = reader["CategorieNume"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}