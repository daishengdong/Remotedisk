using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RemoteDiskServer.CommonStaticVar;

namespace RemoteDiskServer.SQLHandle
{
    class SQLHandler
    {
        private static string ConnectStr = CommonStaticVariables.connectString;
        private static string cmdStr = string.Empty;
        private static SqlConnection connection = null;
        private static SqlCommand cmd = null;
        private static SqlDataAdapter da = null;
        private static SqlDataReader dr = null;
        private static DataTable dt = null;

        private static void ConnectionNew()
        {
            connection = new SqlConnection(ConnectStr);
        }

        private static void ConnectionOpen()
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void ConnectionClose()
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CmdNew()
        {
            try
            {
                if (connection != null && cmdStr != string.Empty)
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                        cmd = new SqlCommand(cmdStr, connection);
                    }
                    else
                    {
                        cmd = new SqlCommand(cmdStr, connection);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CmdExecuteNonQuery()
        {
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void CmdStrSet(string p_cmdStr)
        {
            cmdStr = p_cmdStr;
        }

        private static void DaNew()
        {
            try
            {
                if (da != null)
                {
                    da.Dispose();
                    da = new SqlDataAdapter(cmdStr, connection);
                }
                else
                {
                    da = new SqlDataAdapter(cmdStr, connection);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void DrSet()
        {
            try
            {
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static bool DrRead(string selectCmdStr)
        {
            ConnectionNew();
            ConnectionOpen();
            CmdStrSet(selectCmdStr);
            CmdNew();
            DrSet();
            bool read = dr.Read();
            DrClose();
            ConnectionClose();
            return read;
        }

        private static void DrClose()
        {
            dr.Close();
        }

        private static void DtNew()
        {
            try
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = new DataTable();
                }
                else
                {
                    dt = new DataTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void OperateRecord(string cmdStr)
        {
            ConnectionNew();
            ConnectionOpen();
            CmdStrSet(cmdStr);
            CmdNew();
            CmdExecuteNonQuery();
            ConnectionClose();
        }

        private static void FillData(string selectCmdStr)
        {
            ConnectionNew();
            ConnectionOpen();
            CmdStrSet(selectCmdStr);
            DaNew();
            DtNew();
            da.Fill(dt);
            ConnectionClose();
        }

        public static DataTable getData(string selectCmdStr)
        {
            FillData(selectCmdStr);
            return dt;
        }
    }
}