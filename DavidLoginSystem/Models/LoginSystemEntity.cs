using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DavidLoginSystem.Models
{
    public partial class User : IComparable
    {
        /// <summary>
        /// 验证密码
        /// </summary>
        [Display(Name = "再次输入密码：")]
        [Required(ErrorMessage = "重新输入的密码不能为空"), DataType(DataType.Password)]
        public string Password2 { get; set; }

        /// <summary>
        /// 重载Compare函数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            User user = obj as User;
            if (user == null)
                return -1;

            return this.Id.CompareTo(user.Id);
        }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user == null)
                return false;
            else
            {
                return (this.Id.CompareTo(user.Id) == 0) && (this.UserName.Trim().ToLower().CompareTo(user.UserName.Trim().ToLower()) == 0);
            }           
        }
    }
}