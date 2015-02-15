using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SQL_Classes
{
    public class MySQL
    {
        private string mConString;
        private string mServerName;
        private string mDBName;
        private string mPassword;
        private string mUserID;
        private MySqlConnection mConnect;
        private MySqlDataAdapter mAdapter;
        private MySqlCommand mCommand;

      
        public MySQL(string servername,string DBName,string UserID,string Pass)
        {
            mServerName = servername;
            mDBName = DBName;
            mUserID = UserID;
            mPassword = Pass;
            mConString = "SERVER=" + mServerName + ";" + "DATABASE=" +
        mDBName + ";" + "UID=" + mUserID + ";" + "PASSWORD=" + mPassword + ";";
            mConnect=new MySqlConnection(mConString);
            mConnect.Open();
            


        }

        public DataTable selectDataTable(string selectString)
        {
            
         mAdapter=new MySqlDataAdapter(selectString,mConnect);
            DataTable dT=new DataTable();
            mAdapter.Fill(dT);
            return  dT;
            dT = null;
}

        public long insertQuery(string tableName,Dictionary<string,object> insertDic)
        {
            long lastInsertId;
            string insert="INSERT INTO `"+tableName+"`";
            string selectLast="SELECT LAST_INSERT_ID();"
            int count = insertDic.Count;
            string firstPart="(";
            string lastPart="(";
            for (int i = 0; i < count; i++)
            {
                if (i != count - 1)
                {
                    firstPart += insertDic.Keys.ToString();
                    lastPart += insertDic.Values.ToString();

                }
                else
                {
                    firstPart += insertDic.Keys.ToString() + ")";
                    lastPart += insertDic.Values.ToString() + ");";
                }

            }
            insert += firstPart + lastPart;
            try
            {



                mCommand = new MySqlCommand();
                mCommand.CommandText = insert;
                mCommand.ExecuteNonQuery();
                insert = "";
                firstPart = "";
                lastPart = "";
                mCommand.CommandText = selectLast;
                lastInsertId = (long) mCommand.ExecuteScalar();
                return lastInsertId;
            }
              catch (Exception ex)
            {
                
                throw ex; 
            }





        }





    }
}
