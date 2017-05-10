/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 12:59:51 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 数据库查询接口
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ Dean 2017 All rights reserved 
*****************************************************************/
using HD.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.DAL
{
    public interface IBaseDAL<T> where T : BaseEntity
    {
        bool DeleteByCondition(string condition);
        bool DeleteByCondition(string condition, DbTransaction trans);
        bool DeleteByCondition(string condition, DbTransaction trans, IDbDataParameter[] paramList);
        bool DeleteByKey(string key);
        bool DeleteByKey(string key, DbTransaction trans);
        List<T> Find(string condition);
        List<T> Find(string condition, IDbDataParameter[] paramList);
        List<T> Find(string condition, PagerInfo info);
        T FindByID(int key);
        T FindByID(string key);
        T FindFirst();
        T FindLast();
        T FindSingle(string condition);
        T FindSingle(string condition, IDbDataParameter[] paramList);
        List<T> GetAll();
        List<T> GetAll(PagerInfo info);
        DataSet GetAllToDataSet(PagerInfo info);
        int GetMaxID();
        int GetRecordCount();
        int GetRecordCount(string condition);
        List<T> imethod_0(string idString);
        bool Insert(T obj);
        bool Insert(T obj, DbTransaction trans);
        int Insert2(T obj);
        bool IsExistKey(Hashtable recordTable);
        bool IsExistKey(string fieldName, object key);
        bool IsExistRecord(string condition);
        DataTable SqlTable(string sql);
        string SqlValueList(string sql);
        bool Update(T obj, string primaryKeyValue);
        bool Update(CommandType commandType, string sql);
        bool Update(T obj, string primaryKeyValue, DbTransaction trans);
        bool Update(CommandType commandType, string sql, DbTransaction trans);
    }
}
