using System.Data.SqlClient;

namespace ClothingStoreAutomation
{
    public class DbConnection
    {
        public static SqlConnection con =
            new SqlConnection(
              @"Server=CEDAY;Database=Week1;User Id=ceyda;Password=SİFRENİZ;"

        public static int AdminID;

        public static string AdminName;
    }
}