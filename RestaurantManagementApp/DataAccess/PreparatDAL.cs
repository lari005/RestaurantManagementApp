using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;
using System.Data;
using System.Windows; // Necesar pentru a afișa erorile în pop-up în caz de probleme

namespace RestaurantManagementApp.DataAccess
{
    public class PreparatDAL
    {
        /// <summary>
        /// Extrage toate preparatele din baza de date pentru a fi afișate în Meniul Clientului / Oaspetelui.
        /// </summary>
        public List<Preparat> GetToatePreparatele()
        {
            List<Preparat> lista = new List<Preparat>();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_SelectPreparateDisponibile", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPrep = (int)reader["Id"];

                            lista.Add(new Preparat
                            {
                                Id = idPrep,
                                Denumire = reader["Denumire"].ToString(),
                                Pret = (decimal)reader["Pret"],
                                CantitatePortie = (decimal)reader["CantitatePortie"],
                                // CITIRE STOC: Această linie rezolvă starea de "indisponibil"
                                CantitateTotala = (decimal)reader["CantitateTotala"],
                                NumeCategorie = reader["CategorieNume"].ToString(),
                                // IMAGINI: Această linie încarcă lista de fotografii cerută în barem
                                Imagini = GetImaginiPentruPreparat(idPrep)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // În caz de eroare SQL, aplicația nu se mai blochează, ci îți spune clar ce lipsește
                MessageBox.Show("Eroare la încărcarea meniului: " + ex.Message, "Eroare Bază de Date", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return lista;
        }

        /// <summary>
        /// Metodă ajutătoare privată care citește toate căile imaginilor asociate unui anumit preparat.
        /// </summary>
        private List<string> GetImaginiPentruPreparat(int preparatId)
        {
            List<string> imagini = new List<string>();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT CaleImagine FROM ImaginiPreparate WHERE PreparatId = @PrepId", conn);
                    cmd.Parameters.AddWithValue("@PrepId", preparatId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            imagini.Add(reader["CaleImagine"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Dacă o imagine are probleme, lăsăm restul aplicației să ruleze fără să înghețe
                System.Diagnostics.Debug.WriteLine("Eroare la citirea imaginii: " + ex.Message);
            }

            return imagini;
        }

        /// <summary>
        /// Extrage preparatele al căror stoc este critic (mai mic sau egal cu limita definită în Config).
        /// Folosită pentru secțiunea de rapoarte/alerte a Angajaților.
        /// </summary>
        public List<Preparat> GetPreparateEpuizare(decimal limita)
        {
            List<Preparat> lista = new List<Preparat>();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_SelectPreparateEpuizare", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LimitaCantitate", limita);

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
                                CantitateTotala = (decimal)reader["CantitateTotala"],
                                NumeCategorie = reader["CategorieNume"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea alertelor de stoc: " + ex.Message, "Eroare Panou Angajat", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return lista;
        }
    }
}