/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 12:59:51 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 数据库查询时使用的分页类
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
using System.Xml.Serialization;

namespace HD.DAL
{
    [Serializable]
    public class PagerInfo
    {
        private int currenetPageIndex;
        private int pageSize;
        private int recordCount;

        [XmlElement(ElementName = "CurrenetPageIndex")]
        public int CurrenetPageIndex
        {
            get =>
                this.currenetPageIndex;
            set
            {
                this.currenetPageIndex = value;
            }
        }

        [XmlElement(ElementName = "PageSize")]
        public int PageSize
        {
            get =>
                this.pageSize;
            set
            {
                this.pageSize = value;
            }
        }

        [XmlElement(ElementName = "RecordCount")]
        public int RecordCount
        {
            get =>
                this.recordCount;
            set
            {
                this.recordCount = value;
            }
        }
    }
}
