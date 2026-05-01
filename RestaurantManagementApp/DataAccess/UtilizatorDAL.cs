using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;
using System.Data;

namespace RestaurantManagementApp.DataAccess
{
    public class UtilizatorDAL
    {
        
        public Utilizator Login(string email)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
               
                SqlCommand cmd = new SqlCommand("sp_SelectUtilizatorDupaEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilizator
                        {
                            Id = (int)reader["Id"],
                            Nume = reader["Nume"].ToString(),
                            Prenume = reader["Prenume"].ToString(),
                            Email = reader["Email"].ToString(),
                            Telefon = reader["Telefon"].ToString(),
                            AdresaLivrare = reader["AdresaLivrare"].ToString(),
                            Parola = reader["Parola"].ToString(), 
                            Rol = reader["Rol"].ToString()
                        };
                    }
                }
            }
            return null; 
        }
    }
}