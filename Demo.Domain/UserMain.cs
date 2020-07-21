using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Domain
{
    public class UserMain
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public int RoleId { get; set; }
    }
}
