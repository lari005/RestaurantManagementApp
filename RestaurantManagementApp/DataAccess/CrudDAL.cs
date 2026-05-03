using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;

namespace RestaurantManagementApp.DataAccess
{
    public class CrudDAL
    {
        // ------------------ CATEGORII ------------------
        public List<Categorie> GetCategorii()
        {
            List<Categorie> lista = new List<Categorie>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SelectCategorii", conn) { CommandType = CommandType.StoredProcedure };
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Categorie { Id = (int)reader["Id"], Nume = reader["Nume"].ToString() });
                    }
                }
            }
            return lista;
        }

        public bool InsertCategorie(string nume)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertCategorie", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Nume", nume);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateCategorie(int id, string nume)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateCategorie", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nume", nume);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCategorie(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteCategorie", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ------------------ ALERGENI ------------------
        public List<Alergen> GetAlergeni()
        {
            List<Alergen> lista = new List<Alergen>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SelectAlergeni", conn) { CommandType = CommandType.StoredProcedure };
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Alergen { Id = (int)reader["Id"], Nume = reader["Nume"].ToString() });
                    }
                }
            }
            return lista;
        }

        public bool InsertAlergen(string nume)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertAlergen", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Nume", nume);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool UpdateAlergen(int id, string nume)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateAlergen", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nume", nume);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteAlergen(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteAlergen", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}