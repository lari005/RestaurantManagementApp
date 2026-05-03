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
    }
}