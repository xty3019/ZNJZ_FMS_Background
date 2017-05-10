/**************************************************************** 
 * 作    者：黄鼎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2017-05-10 13:36:03 
 * 当前版本：1.0.0.0
 *  
 * 描述说明： 
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

namespace HD.Entity
{
    public class SimpleUser : BaseEntity
    {
        /// <summary>
        /// 用户登录名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public virtual string UserRealName { get; set; }

        /// <summary>
        /// 用户登录密码
        /// </summary>
        public virtual string UserPassword { get; set; }
    }
}
