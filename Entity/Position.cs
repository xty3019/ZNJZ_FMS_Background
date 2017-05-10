/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 12:59:51 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 数据库实体类
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ Dean 2017 All rights reserved 
*****************************************************************/
using System;

namespace HD.Entity
{
    /// <summary>
    /// 实体类Position。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Position:BaseEntity
    {
		/// <summary>
		/// 
		/// </summary>
		public int PositionID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string PositionName { get; set; }
	}
}