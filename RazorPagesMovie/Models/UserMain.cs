using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class UserMain
    {
        public int Id { get; set; }

        [DisplayName("用户名")]
        public string UserName { get; set; }

        [DisplayName("用户登录名")]
        public string UserLoginName { get; set; }

        [DisplayName("密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("创建时间")]
        [DataType(DataType.Date)]
        public DateTime CreateTime { get; set; }

        [DisplayName("最后一次修改时间")]
        [DataType(DataType.Date)]
        public DateTime UpdateTime { get; set; }
    }
}
