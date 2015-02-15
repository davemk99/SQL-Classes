using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

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

      
        public  MySQL(string servername,string DBName,string UserID,string Pass)
        {
            MySqlConnectionStringBuilder builder=new MySqlConnectionStringBuilder();
            mServerName = servername;
            mDBName = DBName;
            mUserID = UserID;
            mPassword = Pass;


            string Constring = "SERVER="+mServerName+";DATABASE="+mDBName+";Uid="+mUserID+";PASSWORD="+mPassword;



            MessageBox.Show(Constring);
            mConnect=new MySqlConnection(Constring);
            mConnect.Open();
            
           




        }

        public MySQL(string Constrng)
        {
            mConnect=new MySqlConnection(Constrng);
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

        public void insertQuery(string tableName,Dictionary<string,object> insertDic)
        {
            long lastInsertId;
            string insert="INSERT INTO `"+tableName+"` ";
           
            int count = insertDic.Count;
            string firstPart="(";
            string lastPart=" VALUES (";
            for (int i = 0; i < count; i++)
            {
                if (i != count - 1)
                {
                    firstPart += insertDic.ElementAt(i).Key.ToString()+",";
                    lastPart += "'"+insertDic.ElementAt(i).Value.ToString()+"',";

                }
                else
                {
                    firstPart += insertDic.ElementAt(i).Key.ToString() + ")";
                    lastPart +="'"+ insertDic.ElementAt(i).Value.ToString() + "');";
                }

            }
            insert += firstPart + lastPart;
            try
            {


           
                mCommand = new MySqlCommand(insert,mConnect);
         
                mCommand.ExecuteNonQuery();
                insert = "";
                firstPart = "";
                lastPart = "";
               
            }
              catch (Exception ex)
            {
                
                throw ex; 
            }
            insertDic.Clear();
            insertDic = null;





        }

       
        



    }
}
