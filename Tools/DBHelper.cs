using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Tools
{
    /// <summary>
    /// 数据库连接工厂类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>

        public static int ExecuteCommand(string sql)
        {
            DbCommand _cmd = GetCommand(sql);
            return _cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 带参数执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string sql, DbParameter[] pars)
        {
            DbCommand _cmd = GetCommand(sql, pars);
            return _cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        private static MySqlConnection GetConnect()
        {

            MySqlConnection _conn = new MySqlConnection(ConfigHelper.GetConfigString("Data:DefaultConnection:ConnectionString"));
            _conn.Open();
            return _conn;
        }
        private static MySqlCommand GetCommand(string sql,params DbParameter[] parameters)
        {
            MySqlCommand _cmd = new MySqlCommand(sql);
            _cmd.CommandType = CommandType.Text;
            _cmd.Connection = GetConnect();
            if (parameters != null)
            {
                foreach (MySqlParameter parm in parameters)
                    _cmd.Parameters.Add(parm);
            }
            return _cmd;
        }

        public static DataTable GetDataTable(string sql, params DbParameter[] parameters)
        {
            DbDataAdapter _sda = new MySqlDataAdapter(GetCommand(sql, parameters));
            DataTable _dt = new DataTable();
            _sda.Fill(_dt);
            return _dt;
        }
    }
}
