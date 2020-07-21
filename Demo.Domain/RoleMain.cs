using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo.Domain
{
    public class RoleMain
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
