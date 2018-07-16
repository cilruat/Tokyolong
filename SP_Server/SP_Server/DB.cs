using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Diagnostics;

namespace SP_Server
{
    public class DB
    {
        /*readonly private User m_user = null;
        protected SqlConnection m_cnnDB;

        public DB(User user)
        {
            m_cnnDB = new SqlConnection();
            _SetDBConInfo();
            m_user = user;
        }

        void _SetDBConInfo()
        {
            //string conn = "server=192.168.25.13;";
            string conn = "server=DESKTOP-P3RO7M5;";
            conn += "uid=sa;pwd=q1w2e3r4;";
            conn += "Initial Catalog=SP;";
            conn += "Max Pool Size=5000;Min Pool Size=50";

            m_cnnDB.ConnectionString = conn;
        }

        public void Close()
        {
            m_cnnDB.Close();
        }

        public bool Login(int tableNum)
        {
            return true;

            m_cnnDB.Close();

            SqlCommand cmdSQL = new SqlCommand();

            try
            {
                m_cnnDB.Open();

                cmdSQL.CommandText = "INSERT INTO [User](Num) values(@Num)";
                cmdSQL.Connection = m_cnnDB;
                cmdSQL.CommandTimeout = 120;
                cmdSQL.Parameters.AddWithValue("@Num", tableNum);
                cmdSQL.ExecuteNonQuery();

                m_cnnDB.Close();
            }
            catch(System.Exception e)
            {
                m_cnnDB.Close();
                StackFrame stackFrame = new StackFrame(1, true);
                m_user.mainFrm.BeginInvoke(m_user.mainFrm.WriteLogInstance,
                            new object[] { e.ToString(),
                        stackFrame.GetMethod().Name, stackFrame.GetFileName(),
                        stackFrame.GetFileLineNumber().ToString() });                

                return false;
            }

            return true;
        }*/
    }
}