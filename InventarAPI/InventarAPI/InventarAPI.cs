using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InventarAPI
{
    class InventarAPI
    {
        private string domain;
        private int port;

        private TcpClient client;
        private NetworkStream stream;

        private RSAHelper rsaHelper;

        /// <summary>
        /// Saves values
        /// </summary>
        /// <param name="_domain">Domain of the Server, can also be an IP-Adress</param>
        /// <param name="_port">Port of the Server</param>
        public InventarAPI(string _domain, int _port)
        {
            domain = _domain;
            port = _port;
        }

        /// <summary>
        /// Opens a Connection to the Client
        /// </summary>
        /// <returns>Is true if there was no Error, if there is one it returns the Error and the Exception</returns>
        public Error OpenConnection()
        {
            try
            {
                client = new TcpClient();
                IAsyncResult result =  client.BeginConnect(Dns.GetHostAddresses(domain)[0], port, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                client.EndConnect(result);
                if (!success)
                {
                    return new Error(ErrorType.API_ERROR, APIErrorType.CONNECTION_TIMEOUT);
                }
                stream = client.GetStream();
                Error e = SetupRSA();
                if (!e)
                    return new Error(ErrorType.API_ERROR, APIErrorType.RSA_ERROR, e);
                return Error.NO_ERROR;
            }
            catch (Exception e)
            {
                return new Error(ErrorType.API_ERROR, APIErrorType.CONNECTION_ERROR, e);
            }
        }

        /// <summary>
        /// Closes the Connection
        /// </summary>
        public void CloseConnection()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }
        }

        /// <summary>
        /// Adds a new Equipment to a Database
        /// </summary>
        /// <param name="_databaseName">The Name of the Database</param>
        /// <param name="_user">The Username</param>
        /// <param name="_pw">The Password</param>
        /// <param name="_e">The Equipment to add</param>
        /// <returns>Returns an Errro if there was a problem with the Command</returns>
        public Error AddEquipment(string _databaseName, string _user, string _pw, Equipment _e)
        {
            AddEquipmentCommand e = new AddEquipmentCommand(new DatabaseUser(_databaseName, _user, _pw), _e);
            Error error = e.SendCommand(rsaHelper);
            if(!error)
                return new Error(ErrorType.API_ERROR, APIErrorType.COMMAND_FAILED, error);
            return Error.NO_ERROR;
        }

        /// <summary>
        /// Setup RSA communication
        /// </summary>
        /// <returns>Is true if there was no Error, if there is one it returns the Error and the Exception</returns>
        private Error SetupRSA()
        {
            rsaHelper = new RSAHelper(stream);
            Error e = rsaHelper.SetupClient();
            if(!e)
            {
                return new Error(ErrorType.RSA_ERROR, APIErrorType.RSA_ERROR, e);
            }
            return Error.NO_ERROR;
        }

        /// <summary>
        /// Only writes to Console when in DEBUG mode
        /// </summary>
        /// <param name="_s">Message to write</param>
        /// <param name="_args">Objects to place in {} placeholders</param>
        public static void WriteLine(string _s, params object[] _args)
        {
#if DEBUG
            Console.WriteLine(_s, _args);
#endif
        }
    }

    enum APIErrorType
    {
        CONNECTION_ERROR,
        CONNECTION_TIMEOUT,
        RSA_ERROR,
        COMMAND_FAILED,
        EQUIPMENT_INVAlID
    }
}
