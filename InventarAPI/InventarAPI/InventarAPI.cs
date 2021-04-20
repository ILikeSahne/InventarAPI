﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
        public APIError OpenConnection()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(Dns.GetHostAddresses(domain)[0], port);
                client = new TcpClient();
                client.Connect(endPoint);
                stream = client.GetStream();
                APIError e = SetupEncryption();
                if (!e)
                    return e;
                return new APIError(APIErrorType.NO_ERROR, null);
            }
            catch (Exception e)
            {
                return new APIError(APIErrorType.CONNECTION_ERROR, e);
            }
        }

        /// <summary>
        /// Setup RSA Encryption, Client side
        /// </summary>
        /// <returns></returns>
        private APIError SetupEncryption()
        {
            rsaHelper = new RSAHelper(stream);
            RSAError e = rsaHelper.SetupClient();
            ASCIIEncoding en = new ASCIIEncoding();
            rsaHelper.WriteByteArray(en.GetBytes("Hallo, dies ist ein Test, es sollte eigentlich funktionieren. Zumindest hoffe ich das, bitte bitte Bitte!!!!"));
            return new APIError(APIErrorType.NO_ERROR, null);
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

    class APIError
    {
        /// <summary>
        /// Type of Error
        /// </summary>
        public APIErrorType Type { get; }
        /// <summary>
        /// Thrown Exception
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Saves values
        /// </summary>
        /// <param name="_type">Type of Error</param>
        /// <param name="_e">Thrown Exception</param>
        public APIError(APIErrorType _type, Exception _e)
        {
            Type = _type;
            Exception = _e;
        }

        /// <summary>
        /// Writes the Error to the Console (only when in DEBUG mode)
        /// </summary>
        public void PrintError()
        {
            InventarAPI.WriteLine(ToString());
        }

        /// <summary>
        /// Returns the Error as a String:
        ///     "TypeOfError: ExceptionMessage"
        /// </summary>
        /// <returns>"TypeOfError: ExceptionMessage"</returns>
        public override string ToString()
        {
            if (Exception != null)
                return Type + ": " + Exception.Message;
            else
                return Type.ToString();
        }

        /// <summary>
        /// Returns true if there was no Error, otherwise it returns false
        /// </summary>
        /// <param name="e">ServerError</param>
        public static implicit operator bool(APIError e)
        {
            return e.Type == APIErrorType.NO_ERROR;
        }
    }

    enum APIErrorType
    {
        NO_ERROR,
        CONNECTION_ERROR
    }
}
