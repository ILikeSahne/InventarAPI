using System;
using System.Text;

namespace InventarAPI
{
    class APITest
    {
        private const string domain = "ilikesahne.ddns.net";
        private const int port = 10001;

        static void Main(string[] args)
        {
            InventarAPI api = new InventarAPI(domain, port);
            APIError e = api.OpenConnection();
            if(!e)
            {
                e.PrintError();
            } else
            {
                InventarAPI.WriteLine("Connection established!");
            }
        }
    }
}
