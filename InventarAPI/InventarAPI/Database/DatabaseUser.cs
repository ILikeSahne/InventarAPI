using System;
using System.Collections.Generic;
using System.Text;

namespace InventarAPI
{
    class DatabaseUser
    {
        /// <summary>
        /// Name of the Database
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Username of the User of the Database
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password of the User of the Database
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Sets all values to an empty string
        /// </summary>
        public DatabaseUser() : this("", "", "") { }

        /// <summary>
        /// Saves Values
        /// </summary>
        /// <param name="_db">Name of the Database</param>
        /// <param name="_user">Username of the User of the Database</param>
        /// <param name="_pw">Password of the User of the Database</param>
        public DatabaseUser(string _db, string _user, string _pw)
        {
            DatabaseName = _db;
            Username = _user;
            Password = _pw;
        }
    }
}
