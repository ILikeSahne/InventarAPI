using System;
using System.Text;

namespace InventarAPI
{
    class APITest
    {
        private const string domain = "80.123.26.217";
        private const int port = 10001;

        static void Main(string[] args)
        {
            InventarAPI api = new InventarAPI(domain, port);
            Error e = api.OpenConnection();
            if(!e)
            {
                e.PrintAllErrors();
                api.CloseConnection();
            }
            else
            {
                InventarAPI.WriteLine("Connection established!");
            }
            string name = Console.ReadLine();
            Equipment eq = new Equipment(0, "12345", "99", "434", "29/04/2020", name, "ABC123", 50, 10, '€', "No", "BT8/1-55", "Bauteil 8 Lehrsaal 813B");
            Error cmdError = api.AddEquipment("testdb", "olaf", "12345678", eq);
            if (!cmdError)
            {
                cmdError.PrintAllErrors();
                api.CloseConnection();
            }
            else
            {
                InventarAPI.WriteLine("Added Equipment: {0}", e);
            }
            api.CloseConnection();
            Console.ReadKey();
        }
    }
}
