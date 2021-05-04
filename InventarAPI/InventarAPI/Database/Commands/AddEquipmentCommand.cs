using System;
using System.Collections.Generic;
using System.Text;

namespace InventarAPI
{
    class AddEquipmentCommand : Command
    {
        /// <summary>
        /// Holds the name of the Database and the Login details of the User
        /// </summary>
        public DatabaseUser DatabaseUser { get; set; }
        /// <summary>
        /// The Equipment to add
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Saves values
        /// </summary>
        /// <param name="_user">Holds the name of the Database and the Login details of the User</param>
        /// <param name="_e">The Equipment to add</param>
        public AddEquipmentCommand(DatabaseUser _user, Equipment _e) : base(10, 10)
        {
            DatabaseUser = _user;
            Equipment = _e;
        }

        /// <summary>
        /// Sends the DatabaseName, the User Login and the Equipment to add, to the Server
        /// </summary>
        /// <param name="_helper">Is used to send bytes secured</param>
        /// <returns>Returns an Error if the Command couldn't be send</returns>
        public override Error SendCommandData(RSAHelper _helper)
        {
            try
            {
                ASCIIEncoding an = new ASCIIEncoding();
                _helper.WriteByteArray(an.GetBytes(DatabaseUser.DatabaseName));
                _helper.WriteByteArray(an.GetBytes(DatabaseUser.Username));
                _helper.WriteByteArray(an.GetBytes(DatabaseUser.Password));
                byte[] eq = Equipment.ToByteArray();
                _helper.WriteByteArray(eq);
            }
            catch (Exception e)
            {
                return new Error(ErrorType.COMMAND_ERROR, AddEquipmentCommandError.EQUIPMENT_DATA_CORRUPTED, e);
            }
            return Error.NO_ERROR;
        }

        /// <summary>
        /// Returns a list of responses from the enum AddEquipmentCommandError
        /// </summary>
        /// <returns>Returns all the responses as a string array</returns>
        public override string[] GetResponses()
        {
            return Enum.GetNames(typeof(AddEquipmentCommandError));
        }
    }

    enum AddEquipmentCommandError
    {
        NO_ERROR,
        DATABASE_DOESNT_EXIST,
        INVALID_NUMBER_ERROR,
        INVALID_SECOND_NUMBER_ERROR,
        INVALID_CURRENT_NUMBER_ERROR,
        INVALID_ACTIVATION_DATE_ERROR,
        INVALID_NAME_ERROR,
        INVALID_SERIAL_NUMBER_ERROR,
        INVALID_COST_ERROR,
        INVALID_BOOK_VALUE_ERROR,
        INVALID_CURRENCY_ERROR,
        INVALID_KFZ_LICENSE_PLATE_ERROR,
        INVALID_ROOM_ERROR,
        INVALID_ROOM_NAME_ERROR,
        EQUIPMENT_DATA_CORRUPTED
    }
}
