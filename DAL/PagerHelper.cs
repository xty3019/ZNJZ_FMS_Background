/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 13:16:05 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ Dean 2017 All rights reserved 
*****************************************************************/
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace HD.DAL
{
    public class PagerHelper
    {
        private bool bool_0;
        private bool bool_1;
        private int int_0;
        private int int_1;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;

        public PagerHelper(string tableName, string strwhere)
        {
            this.bool_0 = false;
            this.string_1 = "*";
            this.string_2 = string.Empty;
            this.int_0 = 10;
            this.int_1 = 1;
            this.bool_1 = false;
            this.string_3 = string.Empty;
            this.string_0 = tableName;
            this.bool_0 = true;
            this.string_3 = strwhere;
        }

        public PagerHelper(string tableName, bool isDoCount, string fieldNameToSort)
        {
            this.bool_0 = false;
            this.string_1 = "*";
            this.string_2 = string.Empty;
            this.int_0 = 10;
            this.int_1 = 1;
            this.bool_1 = false;
            this.string_3 = string.Empty;
            this.string_0 = tableName;
            this.bool_0 = isDoCount;
            this.string_2 = fieldNameToSort;
        }

        public PagerHelper(string tableName, bool isDoCount, string fieldsToReturn, string fieldNameToSort, int pageSize, int pageIndex, bool isDescending, string strwhere)
        {
            this.bool_0 = false;
            this.string_1 = "*";
            this.string_2 = string.Empty;
            this.int_0 = 10;
            this.int_1 = 1;
            this.bool_1 = false;
            this.string_3 = string.Empty;
            this.string_0 = tableName;
            this.bool_0 = isDoCount;
            this.string_1 = fieldsToReturn;
            this.string_2 = fieldNameToSort;
            this.int_0 = pageSize;
            this.int_1 = pageIndex;
            this.bool_1 = isDescending;
            this.string_3 = strwhere;
        }

        public int GetCount()
        {
            if (!this.bool_0)
            {
                throw new ArgumentException("要返回总数统计，DoCount属性一定为true");
            }
            string query = this.method_0();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return (int)database.ExecuteScalar(sqlStringCommand);
        }

        public IDataReader GetDataReader()
        {
            if (this.bool_0)
            {
                throw new ArgumentException("要返回记录集，DoCount属性一定为false");
            }
            string query = this.method_0();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteReader(sqlStringCommand);
        }

        public DataSet GetDataSet()
        {
            if (this.bool_0)
            {
                throw new ArgumentException("要返回记录集，DoCount属性一定为false");
            }
            string query = this.method_0();
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteDataSet(sqlStringCommand);
        }

        private string method_0()
        {
            string str = "";
            if (this.bool_0)
            {
                str = string.Format("select count(*) as Total from [{0}] ", this.string_0);
                if (!string.IsNullOrEmpty(this.string_3))
                {
                    str = str + string.Format("Where {0} ", this.string_3);
                }
                return str;
            }
            string str2 = string.Empty;
            string str3 = string.Empty;
            if (this.bool_1)
            {
                str2 = "<(select min";
                str3 = string.Format(" order by [{0}] desc", this.string_2);
            }
            else
            {
                str2 = ">(select max";
                str3 = string.Format(" order by [{0}] asc", this.string_2);
            }
            str = string.Format("select top {0} {1} from [{2}] ", this.int_0, this.string_1, this.string_0);
            if (this.int_1 == 1)
            {
                if (!string.IsNullOrEmpty(this.string_3))
                {
                    str = str + string.Format(" Where {0} ", this.string_3);
                }
                return (str + str3);
            }
            if (!string.IsNullOrEmpty(this.string_3))
            {
                return (str + string.Format(" Where [{0}] {1} ([{0}]) from (select top {2} [{0}] from [{3}] where {5} {4} ) as tblTmp) and {5} {4}", new object[] { this.string_2, str2, (this.int_1 - 1) * this.int_0, this.string_0, str3, this.string_3 }));
            }
            return (str + string.Format(" Where [{0}] {1} ([{0}]) from (select top {2} [{0}] from [{3}] {4} ) as tblTmp) {4}", new object[] { this.string_2, str2, (this.int_1 - 1) * this.int_0, this.string_0, str3 }));
        }

        public string FieldNameToSort
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public string FieldsToReturn
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public bool IsDescending
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool IsDoCount
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

        public int PageIndex
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public string StrWhere
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public string TableName
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}
