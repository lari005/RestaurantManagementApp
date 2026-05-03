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
        public bool InregistrareClient(Utilizator utilizator)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertUtilizator", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nume", utilizator.Nume);
                cmd.Parameters.AddWithValue("@Prenume", utilizator.Prenume);
                cmd.Parameters.AddWithValue("@Email", utilizator.Email);
                cmd.Parameters.AddWithValue("@Telefon", utilizator.Telefon);

                cmd.Parameters.AddWithValue("@AdresaLivrare", string.IsNullOrEmpty(utilizator.AdresaLivrare) ? (object)DBNull.Value : utilizator.AdresaLivrare);
                cmd.Parameters.AddWithValue("@Parola", utilizator.Parola);
                cmd.Parameters.AddWithValue("@Rol", "Client"); 

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true; 
                }
                catch
                {
                    return false; 
                }
            }
        }
    }
}