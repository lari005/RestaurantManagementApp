using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace RestaurantManagementApp.DataAccess
{
    public static class DatabaseHelper
    {
        
        private static readonly string connectionString =
            ConfigurationManager.ConnectionStrings["RestaurantConnection"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}