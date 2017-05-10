/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 12:59:51 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 实现数据库查询接口的基类
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ Dean 2017 All rights reserved 
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HD.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.OleDb;

namespace HD.DAL
{
    public abstract class BaseDAL<T> : IBaseDAL<T> where T : BaseEntity, new()
    {
        private bool bool_0;
        protected string primaryKey;
        protected string selectedFields;
        protected string sortField;
        protected string tableName;

        public BaseDAL()
        {
            this.sortField = "ID";
            this.selectedFields = " * ";
            this.bool_0 = true;
        }

        public BaseDAL(string tableName, string primaryKey)
        {
            this.sortField = "ID";
            this.selectedFields = " * ";
            this.bool_0 = true;
            this.tableName = tableName;
            this.primaryKey = primaryKey;
        }

        protected virtual T DataReaderToEntity(IDataReader dr)
        {
            T local = Activator.CreateInstance<T>();
            foreach (PropertyInfo info in local.GetType().GetProperties())
            {
                try
                {
                    if (dr[info.Name].ToString() != "")
                    {
                        info.SetValue(local, dr[info.Name] ?? "", null);
                    }
                }
                catch
                {
                }
            }
            return local;
        }

        public bool DeleteByCondition(string condition)
        {
            return this.DeleteByCondition(condition, null);
        }

        public bool DeleteByCondition(string condition, DbTransaction trans)
        {
            return this.DeleteByCondition(condition, trans, null);
        }

        public bool DeleteByCondition(string condition, DbTransaction trans, IDbDataParameter[] paramList)
        {
            string query = string.Format("DELETE FROM {0} WHERE {1} ", this.tableName, condition);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            if (paramList != null)
            {
                sqlStringCommand.Parameters.AddRange(paramList);
            }
            if (trans != null)
            {
                return (database.ExecuteNonQuery(sqlStringCommand, trans) > 0);
            }
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteByKey(string key)
        {
            string condition = string.Format("{0} = @ID", this.primaryKey);
            SqlParameter parameter = new SqlParameter("@ID", key);
            return this.DeleteByCondition(condition, null, new SqlParameter[] { parameter });
        }

        public bool DeleteByKey(string key, DbTransaction trans)
        {
            string condition = string.Format("{0} = '{1}'", this.primaryKey, key);
            return this.DeleteByCondition(condition, trans);
        }

        public List<T> Find(string condition)
        {
            string str = string.Format("Select {0} From {1} Where ", this.selectedFields, this.tableName) + condition + string.Format(" Order by {0} {1}", this.sortField, this.bool_0 ? "DESC" : "ASC");
            return this.method_0(str, null);
        }

        public List<T> Find(string condition, IDbDataParameter[] paramList)
        {
            string str = string.Format("Select {0} From {1} Where ", this.selectedFields, this.tableName) + condition + string.Format(" Order by {0} {1}", this.sortField, this.bool_0 ? "DESC" : "ASC");
            return this.method_0(str, paramList);
        }

        public List<T> Find(string condition, PagerInfo info)
        {
            List<T> list = new List<T>();
            DatabaseFactory.CreateDatabase();
            info.RecordCount = new PagerHelper(this.tableName, condition).GetCount();
            PagerHelper helper2 = new PagerHelper(this.tableName, false, this.selectedFields, this.sortField, info.PageSize, info.CurrenetPageIndex, this.bool_0, condition);
            using (IDataReader reader = helper2.GetDataReader())
            {
                while (reader.Read())
                {
                    list.Add(this.DataReaderToEntity(reader));
                }
            }
            return list;
        }

        public T FindByID(int key)
        {
            return this.FindByID(key.ToString());
        }

        public T FindByID(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            string query = string.Format("Select {0} From {1} Where ({2} = @ID)", this.selectedFields, this.tableName, this.primaryKey);
            SqlParameter parameter = new SqlParameter("@ID", key);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            sqlStringCommand.Parameters.Add(parameter);
            T local3 = default(T);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    local3 = this.DataReaderToEntity(reader);
                }
            }
            return local3;
        }

        public T FindFirst()
        {
            string query = string.Format("Select top 1 * From {0} Order by {1} ASC", this.tableName, this.sortField);
            T local = default(T);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    local = this.DataReaderToEntity(reader);
                }
            }
            return local;
        }

        public T FindLast()
        {
            string query = string.Format("Select top 1 * From {0} Order by {1} DESC", this.tableName, this.sortField);
            T local = default(T);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    local = this.DataReaderToEntity(reader);
                }
            }
            return local;
        }

        public T FindSingle(string condition)
        {
            T local = default(T);
            List<T> list = this.Find(condition);
            if (list.Count > 0)
            {
                local = list[0];
            }
            return local;
        }

        public T FindSingle(string condition, IDbDataParameter[] paramList)
        {
            T local = default(T);
            List<T> list = this.Find(condition, paramList);
            if (list.Count > 0)
            {
                local = list[0];
            }
            return local;
        }

        public List<T> GetAll()
        {
            string str = string.Format("Select {0} From {1}", this.selectedFields, this.tableName) + string.Format(" Order by {0} {1}", this.sortField, this.bool_0 ? "DESC" : "ASC");
            return this.method_0(str, null);
        }

        public List<T> GetAll(PagerInfo info)
        {
            List<T> list = new List<T>();
            string strwhere = "";
            DatabaseFactory.CreateDatabase();
            info.RecordCount = new PagerHelper(this.tableName, strwhere).GetCount();
            PagerHelper helper2 = new PagerHelper(this.tableName, false, this.selectedFields, this.sortField, info.PageSize, info.CurrenetPageIndex, this.bool_0, strwhere);
            using (IDataReader reader = helper2.GetDataReader())
            {
                while (reader.Read())
                {
                    list.Add(this.DataReaderToEntity(reader));
                }
            }
            return list;
        }

        public DataSet GetAllToDataSet(PagerInfo info)
        {
            new DataSet();
            string strwhere = "";
            info.RecordCount = new PagerHelper(this.tableName, strwhere).GetCount();
            PagerHelper helper2 = new PagerHelper(this.tableName, false, this.selectedFields, this.sortField, info.PageSize, info.CurrenetPageIndex, this.bool_0, strwhere);
            return helper2.GetDataSet();
        }

        protected DataTable GetDataTableBySql(string sql)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(sql);
            return database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        protected virtual Hashtable GetHashByEntity(T obj)
        {
            Hashtable hashtable = new Hashtable();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                object obj2 = properties[i].GetValue(obj, null);
                obj2 = (obj2 == null) ? DBNull.Value : obj2;
                if (!hashtable.ContainsKey(properties[i].Name))
                {
                    hashtable.Add(properties[i].Name, obj2);
                }
            }
            return hashtable;
        }

        public int GetMaxID()
        {
            string query = string.Format("SELECT MAX({0}) AS MaxID FROM {1}", this.primaryKey, this.tableName);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            object obj2 = database.ExecuteScalar(sqlStringCommand);
            if (Convert.IsDBNull(obj2))
            {
                return 0;
            }
            return Convert.ToInt32(obj2);
        }

        public int GetRecordCount()
        {
            string query = string.Format("Select Count(*) from {0} ", this.tableName);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
        }

        public int GetRecordCount(string condition)
        {
            string query = string.Format("Select Count(*) from {0} WHERE {1} ", this.tableName, condition);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return Convert.ToInt32(database.ExecuteScalar(sqlStringCommand));
        }

        public List<T> imethod_0(string idString)
        {
            string condition = string.Format("{0} in({1})", this.primaryKey, idString);
            return this.Find(condition);
        }

        public bool Insert(T obj)
        {
            return this.Insert(obj, null);
        }

        public bool Insert(Hashtable recordField, DbTransaction trans)
        {
            return this.Insert(recordField, this.tableName, trans);
        }

        public bool Insert(T obj, DbTransaction trans)
        {
            //ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");
            Hashtable hashByEntity = this.GetHashByEntity(obj);
            return this.Insert(hashByEntity, trans);
        }

        public bool Insert(Hashtable recordField, string targetTable, DbTransaction trans)
        {
            bool flag = false;
            string str = "";
            string str2 = "";
            if ((recordField != null) && (recordField.Count >= 1))
            {
                SqlParameter[] values = new SqlParameter[recordField.Count];
                IEnumerator enumerator = recordField.Keys.GetEnumerator();
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    string str3 = enumerator.Current.ToString();
                    str = str + string.Format("[{0}],", str3);
                    str2 = str2 + string.Format("@{0},", str3);
                    object obj2 = recordField[enumerator.Current.ToString()];
                    obj2 = obj2 ?? DBNull.Value;
                    if ((obj2 is DateTime) && (Convert.ToDateTime(obj2) <= Convert.ToDateTime("1753-1-1")))
                    {
                        obj2 = DBNull.Value;
                    }
                    values[i] = new SqlParameter("@" + str3, obj2);
                }
                str = str.Trim(new char[] { ',' });
                str2 = str2.Trim(new char[] { ',' });
                string query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", targetTable, str, str2);
                Database database = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                sqlStringCommand.Parameters.AddRange(values);
                if (trans != null)
                {
                    flag = database.ExecuteNonQuery(sqlStringCommand, trans) > 0;
                }
                else
                {
                    flag = database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
            }
            return flag;
        }

        public int Insert2(T obj)
        {
            //ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");
            Hashtable hashByEntity = this.GetHashByEntity(obj);
            return this.Insert2(hashByEntity, null);
        }

        public int Insert2(Hashtable recordField, DbTransaction trans)
        {
            return this.Insert2(recordField, this.tableName, trans);
        }

        public int Insert2(Hashtable recordField, string targetTable, DbTransaction trans)
        {
            int num = -1;
            string str = "";
            string str2 = "";
            if ((recordField != null) && (recordField.Count >= 1))
            {
                SqlParameter[] values = new SqlParameter[recordField.Count];
                IEnumerator enumerator = recordField.Keys.GetEnumerator();
                for (int i = 0; enumerator.MoveNext(); i++)
                {
                    string str3 = enumerator.Current.ToString();
                    str = str + str3 + ",";
                    str2 = str2 + string.Format("@{0},", str3);
                    object obj2 = recordField[enumerator.Current.ToString()];
                    values[i] = new SqlParameter("@" + str3, obj2);
                }
                str = str.Trim(new char[] { ',' });
                str2 = str2.Trim(new char[] { ',' });
                string query = string.Format("INSERT INTO {0} ({1}) VALUES ({2});SELECT SCOPE_IDENTITY()", targetTable, str, str2);
                Database database = DatabaseFactory.CreateDatabase();
                DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
                sqlStringCommand.Parameters.AddRange(values);
                if (trans != null)
                {
                    num = Convert.ToInt32(database.ExecuteScalar(sqlStringCommand, trans).ToString());
                }
                else
                {
                    num = Convert.ToInt32(database.ExecuteScalar(sqlStringCommand).ToString());
                }
            }
            return num;
        }

        public bool IsExistKey(Hashtable recordTable)
        {
            SqlParameter[] values = new SqlParameter[recordTable.Count];
            IEnumerator enumerator = recordTable.Keys.GetEnumerator();
            string str = "";
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                string str2 = enumerator.Current.ToString();
                str = str + string.Format(" {0} = @{1} AND", str2, str2);
                string str3 = recordTable[enumerator.Current.ToString()].ToString();
                values[i] = new SqlParameter(string.Format("@{0}", str2), str3);
            }
            str = str.Substring(0, str.Length - 3);
            string query = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", this.tableName, str);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            sqlStringCommand.Parameters.AddRange(values);
            return (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public bool IsExistKey(string fieldName, object key)
        {
            Hashtable recordTable = new Hashtable {
                {
                    fieldName,
                    key
                }
            };
            return this.IsExistKey(recordTable);
        }

        public bool IsExistRecord(string condition)
        {
            string query = string.Format("Select Count(*) from {0} WHERE {1} ", this.tableName, condition);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return (Convert.ToInt32(database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        private List<T> method_0(string string_0, IDbDataParameter[] idbDataParameter_0)
        {
            T item = default(T);
            List<T> list = new List<T>();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(string_0);
            if (idbDataParameter_0 != null)
            {
                sqlStringCommand.Parameters.AddRange(idbDataParameter_0);
            }
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    item = this.DataReaderToEntity(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public DataTable SqlTable(string sql)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(sql);
            return database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public string SqlValueList(string sql)
        {
            StringBuilder builder = new StringBuilder();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(sql);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    builder.AppendFormat("{0},", reader[0].ToString());
                }
            }
            return builder.ToString().Trim(new char[] { ',' });
        }

        public bool Update(T obj, string primaryKeyValue)
        {
            return this.Update(obj, primaryKeyValue, null);
        }

        public bool Update(CommandType commandType, string sql)
        {
            return (DatabaseFactory.CreateDatabase().ExecuteNonQuery(commandType, sql) > 0);
        }

        public bool Update(CommandType commandType, string sql, DbTransaction trans)
        {
            return (DatabaseFactory.CreateDatabase().ExecuteNonQuery(trans, CommandType.Text, sql) > 0);
        }

        public bool Update(int id, Hashtable recordField, DbTransaction trans)
        {
            return this.Update(id, recordField, this.tableName, trans);
        }

        public bool Update(string id, Hashtable recordField, DbTransaction trans)
        {
            return this.Update(id, recordField, this.tableName, trans);
        }

        public bool Update(T obj, string primaryKeyValue, DbTransaction trans)
        {
            //ArgumentValidation.CheckForNullReference(obj, "传入的对象obj为空");
            Hashtable hashByEntity = this.GetHashByEntity(obj);
            return this.Update(primaryKeyValue, hashByEntity, trans);
        }

        public bool Update(int id, Hashtable recordField, string targetTable, DbTransaction trans)
        {
            return this.Update(id.ToString(), recordField, targetTable, trans);
        }

        public bool Update(string id, Hashtable recordField, string targetTable, DbTransaction trans)
        {
            string str = "";
            object obj2 = null;
            string str2 = "";
            if ((recordField == null) || (recordField.Count < 1))
            {
                return false;
            }
            SqlParameter[] values = new SqlParameter[recordField.Count];
            int index = 0;
            IEnumerator enumerator = recordField.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                str = enumerator.Current.ToString();
                obj2 = recordField[enumerator.Current.ToString()];
                obj2 = obj2 ?? DBNull.Value;
                if ((obj2 is DateTime) && (Convert.ToDateTime(obj2) <= Convert.ToDateTime("1753-1-1")))
                {
                    obj2 = DBNull.Value;
                }
                str2 = str2 + string.Format("[{0}] = @{0},", str);
                values[index] = new SqlParameter(string.Format("@{0}", str), obj2);
                index++;
            }
            string query = string.Format("UPDATE {0} SET {1} WHERE {2} = @ID ", targetTable, str2.Substring(0, str2.Length - 1), this.primaryKey);
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            sqlStringCommand.Parameters.AddRange(values);
            if (!sqlStringCommand.Parameters.Contains("@ID"))
            {
                SqlParameter parameter = new SqlParameter("@ID", id);
                sqlStringCommand.Parameters.Add(parameter);
            }
            bool flag2 = false;
            if (trans != null)
            {
                flag2 = database.ExecuteNonQuery(sqlStringCommand, trans) > 0;
            }
            else
            {
                flag2 = database.ExecuteNonQuery(sqlStringCommand) > 0;
            }
            return flag2;
        }

        public bool IsDescending
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string PrimaryKey
        {
            get
            {
                return this.primaryKey;
            }
        }

        protected string SelectedFields
        {
            get
            {
                return this.selectedFields;
            }
            set
            {
                this.selectedFields = value;
            }
        }

        public string SortField
        {
            get
            {
                return this.sortField;
            }
            set
            {
                this.sortField = value;
            }
        }

        public string TableName
        {
            get
            {
                return this.tableName;
            }
        }
    }
}
