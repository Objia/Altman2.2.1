﻿using System.Collections.Generic;
using System.Data;
using Altman.Data;
using PluginFramework;

namespace Altman.Service
{
    /// <summary>
    /// 数据库类，主要包含数据库的相关操作（继承IHostDb接口）
    /// </summary>
    public class Db : IHostDb
    {
        public bool CheckTable(string tableName)
        {
            return Data.Db.CheckTable(tableName);
        }

        public bool InitTable(string tableName, string[] definition)
        {
            return Data.Db.InitTable(tableName, definition);
        }

        public DataTable GetDataTable(string tableName)
        {
            return Data.Db.GetDataTable(tableName);
        }

        public bool Insert(string tableName, Dictionary<string, object> data)
        {
            return Data.Db.Insert(tableName, data);
        }

        public bool Update(string tableName, Dictionary<string, object> data, KeyValuePair<string,object> where)
        {
            return Data.Db.Updata(tableName, data, where);
        }

        public bool Delete(string tableName, KeyValuePair<string, object> where)
        {
            return Data.Db.Delete(tableName, where);
        }
    }
}
