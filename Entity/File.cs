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
    /// 实体类File。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class File:BaseEntity
    {
		/// <summary>
		/// 
		/// </summary>
		public int FileID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string FileExt { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? FileSize { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public byte[] FileData { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? FileCreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? FilePID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public byte[] FilePOwner { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public byte[] FilePRole { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public byte[] FilePOther { get; set; }
	}
}