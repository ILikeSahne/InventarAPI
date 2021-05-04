using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace InventarAPI
{
    class Command
    {
        /// <summary>
        /// The first 2 bytes after the RSA Setup indicate the type of Command
        /// This is the first byte
        /// </summary>
        public byte Number { get; set; }
        /// <summary>
        /// The First 2 bytes after the RSA Setup indicate the type of Command
        /// This is the second byte
        /// </summary>
        public byte Number2 { get; set; }

        /// <summary>
        /// Saves values
        /// </summary>
        /// <param name="_number">First indication byte</param>
        /// <param name="_number2">Second indication byte</param>
        public Command(byte _number, byte _number2)
        {
            Number = _number;
            Number2 = _number2;
        }

        /// <summary>
        /// Returns true if the first 2 bytes after the RSA Setup match up with the bytes of the Command
        /// </summary>
        /// <param name="_data">The first bytes after the RSA Setup</param>
        /// <returns>Returns true if the bytes match</returns>
        public bool IsCommand(byte[] _data)
        {
            return _data[0] == Number && _data[1] == Number2;
        }

        /// <summary>
        /// Sends a Command plus the Command type
        /// </summary>
        /// <param name="_helper">Is used to send bytes secured</param>
        /// <returns>Returns an Error if the Command couldn't be send</returns>
        public Error SendCommand(RSAHelper _helper)
        {
            SendCommandType(_helper);
            Error e = SendCommandData(_helper);
            if (!e)
            {
                return new Error(ErrorType.COMMAND_ERROR, CommandErrorType.COMMAND_DATA_ERROR, e);
            }
            byte[] data = _helper.ReadByteArray();
            e = HandleResponse((data[1] << 8) + data[0]);
            if (!e)
            {
                return new Error(ErrorType.COMMAND_ERROR, CommandErrorType.COMMAND_RESPONSE_ERROR, e);
            }
            return Error.NO_ERROR;
        }

        /// <summary>
        /// Send the type of the Command
        /// </summary>
        /// <param name="_helper">Is used to send bytes secured</param>
        public void SendCommandType(RSAHelper _helper)
        {
            byte[] _data = { Number, Number2 };
            _helper.WriteByteArray(_data);
        }

        /// <summary>
        /// Sends the Data of the Command
        /// </summary>
        /// <param name="_helper">Is used to send bytes secured</param>
        /// <returns>Returns an Error if there was a problem with the Command Data</returns>
        public virtual Error SendCommandData(RSAHelper _helper)
        {
            return Error.NO_ERROR;
        }

        /// <summary>
        /// The Server sends two bytes, indicating if there was a problem
        /// </summary>
        /// <param name="_id">The 2 bytes stored in an integer</param>
        /// <returns>Returns an Error, if there was a problem</returns>
        public Error HandleResponse(int _id)
        {
            Console.WriteLine(_id);
            string[] responses = GetResponses();
            for (int i = 0; i < responses.Length; i++)
            {
                if (_id == i)
                    return new Error(ErrorType.COMMAND_ERROR, responses[i]);
            }
            return new Error(ErrorType.COMMAND_ERROR, CommandErrorType.UNKNOWN_RESPONSE_ERROR);
        }

        /// <summary>
        /// Converts the Error into an integer
        /// </summary>
        /// <param name="_response">The response to convert</param>
        /// <returns>The ID of the Error</returns>
        public int ResponseToID(object _response)
        {
            string response = _response.ToString();
            string[] responses = GetResponses();
            for (int i = 0; i < responses.Length; i++)
            {
                if (responses[i] == response)
                    return i;
            }
            return 0;
        }

        /// <summary>
        /// Returns a list of possible responses
        /// </summary>
        /// <returns>The responses as a string list</returns>
        public virtual string[] GetResponses()
        {
            string[] data = new string[1];
            data[0] = "NO_ERROR";
            return data;
        }
    }

    enum CommandErrorType
    {
        SYNTAX_ERROR,
        COMMAND_DATA_ERROR,
        COMMAND_RESPONSE_ERROR,
        UNKNOWN_RESPONSE_ERROR
    }
}
