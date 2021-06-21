using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace POS_New
{
    class ClassDB
    {
        public string GetConnection()
        {
            string cn = "server = localhost;username = root; password = ; database = pos_db";
            return cn;
        }
    }
}
