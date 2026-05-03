using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using RestaurantManagementApp.Models;
namespace RestaurantManagementApp.DataAccess
{
    public class ComandaDAL
    {
        public bool PlaseazaComanda(Comanda comanda)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
               
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    SqlCommand cmdComanda = new SqlCommand("sp_InsertComanda", conn, transaction);
                    cmdComanda.CommandType = CommandType.StoredProcedure;

                    cmdComanda.Parameters.AddWithValue("@CodUnic", Guid.NewGuid());
                    cmdComanda.Parameters.AddWithValue("@UtilizatorId", comanda.UtilizatorId);
                    cmdComanda.Parameters.AddWithValue("@DataOraComanda", DateTime.Now);
                    cmdComanda.Parameters.AddWithValue("@Stare", "inregistrata");
                    cmdComanda.Parameters.AddWithValue("@CostTransportAplicat", comanda.CostTransport);
                    cmdComanda.Parameters.AddWithValue("@ProcentDiscountAplicat", comanda.ProcentDiscount);

                    int comandaId = Convert.ToInt32(cmdComanda.ExecuteScalar());

                    
                    var preparateGrupate = comanda.Preparate
                        .GroupBy(p => p.Id)
                        .Select(g => new {
                            Preparat = g.First(),
                            Cantitate = g.Count()
                        }).ToList();

                    foreach (var item in preparateGrupate)
                    {
                        SqlCommand cmdPreparat = new SqlCommand("sp_InsertComandaPreparat", conn, transaction);
                        cmdPreparat.CommandType = CommandType.StoredProcedure;

                        cmdPreparat.Parameters.AddWithValue("@ComandaId", comandaId);
                        cmdPreparat.Parameters.AddWithValue("@PreparatId", item.Preparat.Id);
                        cmdPreparat.Parameters.AddWithValue("@CantitateBucati", item.Cantitate);
                        cmdPreparat.Parameters.AddWithValue("@PretIstoricBucata", item.Preparat.Pret);

                        cmdPreparat.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        
        public List<Comanda> GetComenziClient(int utilizatorId)
        {
            List<Comanda> comenzi = new List<Comanda>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_SelectComenziClient", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizatorId", utilizatorId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Comanda c = new Comanda
                        {
                            Id = (int)reader["Id"],
                            CodUnic = (Guid)reader["CodUnic"],
                            Stare = reader["Stare"].ToString(),
                            DataComanda = (DateTime)reader["DataOraComanda"],
                            CostTransport = (decimal)reader["CostTransportAplicat"],
                            ProcentDiscount = (decimal)reader["ProcentDiscountAplicat"]
                        };

                        if (reader["OraEstimativaLivrare"] != DBNull.Value)
                            c.OraEstimativaLivrare = (DateTime)reader["OraEstimativaLivrare"];

                        comenzi.Add(c);
                    }
                }

                foreach (var c in comenzi)
                {
                    SqlCommand cmdDet = new SqlCommand("sp_SelectDetaliiComanda", conn);
                    cmdDet.CommandType = CommandType.StoredProcedure;
                    cmdDet.Parameters.AddWithValue("@ComandaId", c.Id);

                    decimal costMancareCalculat = 0;
                    List<string> listaProduse = new List<string>(); 

                    using (SqlDataReader r2 = cmdDet.ExecuteReader())
                    {
                        while (r2.Read())
                        {
                            string numeProdus = r2["Denumire"].ToString();
                            int cantitate = (int)r2["CantitateBucati"];
                            decimal pretIstoric = (decimal)r2["PretIstoricBucata"];

                            costMancareCalculat += (cantitate * pretIstoric);
                            listaProduse.Add($"{cantitate}x {numeProdus}");
                        }
                    }

                    c.CostMancare = costMancareCalculat;
                    decimal valoareDiscount = costMancareCalculat * (c.ProcentDiscount / 100);
                    c.CostTotal = costMancareCalculat - valoareDiscount + c.CostTransport;

                    c.ProduseFormatate = string.Join(", ", listaProduse);
                }
            }
            return comenzi;
        }

        
        public bool AnuleazaComanda(int comandaId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStareComanda", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComandaId", comandaId);
                cmd.Parameters.AddWithValue("@StareNoua", "anulata");

                conn.Open();
                int randuriModificate = cmd.ExecuteNonQuery();
                return randuriModificate > 0;
            }
        }
        public List<Comanda> GetComenziPentruAngajat(bool doarActive)
        {
            List<Comanda> comenzi = new List<Comanda>();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string procName = doarActive ? "sp_SelectComenziActive" : "sp_SelectToateComenzile";
                SqlCommand cmd = new SqlCommand(procName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Comanda c = new Comanda
                        {
                            Id = (int)reader["Id"],
                            CodUnic = (Guid)reader["CodUnic"],
                            Stare = reader["Stare"].ToString(),
                            DataComanda = (DateTime)reader["DataOraComanda"],
                            CostTransport = (decimal)reader["CostTransportAplicat"],
                            ProcentDiscount = (decimal)reader["ProcentDiscountAplicat"],
                            NumeClient = reader["Nume"].ToString() + " " + reader["Prenume"].ToString(),
                            TelefonClient = reader["Telefon"].ToString(),
                            AdresaClient = reader["AdresaLivrare"].ToString()
                        };

                        if (reader["OraEstimativaLivrare"] != DBNull.Value)
                            c.OraEstimativaLivrare = (DateTime)reader["OraEstimativaLivrare"];

                        comenzi.Add(c);
                    }
                }

               
                foreach (var c in comenzi)
                {
                    SqlCommand cmdDet = new SqlCommand("sp_SelectDetaliiComanda", conn);
                    cmdDet.CommandType = CommandType.StoredProcedure;
                    cmdDet.Parameters.AddWithValue("@ComandaId", c.Id);

                    decimal costMancareCalculat = 0;
                    List<string> listaProduse = new List<string>();

                    using (SqlDataReader r2 = cmdDet.ExecuteReader())
                    {
                        while (r2.Read())
                        {
                            string numeProdus = r2["Denumire"].ToString();
                            int cantitate = (int)r2["CantitateBucati"];
                            decimal pretIstoric = (decimal)r2["PretIstoricBucata"];

                            costMancareCalculat += (cantitate * pretIstoric);
                            listaProduse.Add($"{cantitate}x {numeProdus}");
                        }
                    }

                    c.CostMancare = costMancareCalculat;
                    decimal valoareDiscount = costMancareCalculat * (c.ProcentDiscount / 100);
                    c.CostTotal = costMancareCalculat - valoareDiscount + c.CostTransport;
                    c.ProduseFormatate = string.Join(", ", listaProduse);
                }
            }
            return comenzi;
        }

        public bool ModificaStareComanda(int comandaId, string stareNoua)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateStareComanda", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ComandaId", comandaId);
                cmd.Parameters.AddWithValue("@StareNoua", stareNoua);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}