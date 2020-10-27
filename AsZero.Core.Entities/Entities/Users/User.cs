using FutureTech.Dal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AsZero.Core.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User: GenericEntity<int>
    {
        /// <summary>
        /// 账户名
        /// </summary>
        [Column("account")]
        [MaxLength(255)]
        public string Account { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("user_name")]
        public string Name { get; set; }

        /// <summary>
        ///  密码，需加密后保存
        /// </summary>
        /// <value></value>
        [Column("pass")]
        [MaxLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// 对密码进行加密的盐，每个账户的盐都各不相同
        /// </summary>
        /// <value></value>
        [Column("salt")]
        [MaxLength(255)]
        public string Salt { get; set; }

        /// <summary>
        /// 当前用户的状态
        /// </summary>
        [Column("status")]
        public UserStatus Status { get; set; }

        public IList<UserRoles> UserRoles { get; set; }
    }


    public enum UserStatus
    {
        /// <summary>
        /// 已创建，未激活
        /// </summary>
        Created = 0,
        /// <summary>
        /// 已激活
        /// </summary>
        Activated = 1,
        /// <summary>
        /// 异常，HOLD ，需要致电客服
        /// </summary>
        Hold = -1,
    }

}
