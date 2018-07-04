﻿using System.Collections.Generic;
using System.Data;

namespace PluginFramework
{
    /// <summary>
    /// 针对数据库的一些操作
    /// </summary>
    public interface IHostDb
    {
        //db
        bool CheckTable(string tableName);
        bool InitTable(string tableName, string[] definition);
        DataTable GetDataTable(string tableName);
        bool Insert(string tableName, Dictionary<string, object> data);
        bool Update(string tableName, Dictionary<string, object> data, KeyValuePair<string, object> where);
        bool Delete(string tableName, KeyValuePair<string, object> where);
    }
}
