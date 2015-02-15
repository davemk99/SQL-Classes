using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public long insertQuery(string tableName,Dictionary<string,object> insertDic)
        {
            long lastInsertId;
            string insert="INSERT INTO `"+tableName+"` ";
            string ID = "SELECT LAST_INSERT_ID();";

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
                MySqlCommand command=new MySqlCommand(ID,mConnect);
                lastInsertId = long.Parse(command.ExecuteScalar().ToString());
                

            }
              catch (Exception ex)
            {
                
                throw ex; 
            }
            insertDic.Clear();
            insertDic = null;
            return lastInsertId;





        }

        public object selectSingle(string selectString)
        {
            MySqlDataReader reader;
            mCommand=new MySqlCommand(selectString,mConnect);
            reader=mCommand.ExecuteReader();
            object myObj;
            try
            {
                reader.Read();
                myObj = reader.GetValue(0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
           reader.Close();
            return myObj;

        }

        public void updateQuery(string tableName,Dictionary<string,object> dicUpdate,Dictionary<string,object> terms  )
        {
            string update = "UPDATE `" + tableName + "` SET ";
            string termsQuery=" WHERE ";

          
            
            long id = 0;
            int count = dicUpdate.Count;
            for (int i = 0; i < count; i++)
            {
                if (i != count - 1)
                {
                    update += "`" + dicUpdate.ElementAt(i).Key.ToString() + "`='" +
                              dicUpdate.ElementAt(i).Value.ToString() + "',";
                }
                else
                {
                  update+=  "`" + dicUpdate.ElementAt(i).Key.ToString() + "`='" +
                              dicUpdate.ElementAt(i).Value.ToString() + "'";
                }

            }
            int counts = terms.Count;
            for (int i = 0; i <counts ; i++)
            {
                if (i != count - 1)
                {
                    termsQuery += "`" + terms.ElementAt(i).Key.ToString() + "`='" + terms.ElementAt(i).Value.ToString() +
                                  "' AND ";
                }
                else
                {
                    termsQuery += "`" + terms.ElementAt(i).Key.ToString() + "`='" + terms.ElementAt(i).Value.ToString() +
                                "' ; ";
                }
            }
            update += termsQuery;
       
            mCommand=new MySqlCommand(update,mConnect);
            mCommand.ExecuteNonQuery();  
          
          
      

        }

        public void destroyConnection()
        {
            mConnect.Close();

        }

       
        



    }
}
