using FutureTech.Dal.Entities;

namespace AsZero.Core.Entities
{
    /// <summary>
    /// 用户-角色配置表
    /// </summary>
    public class UserRoles : GenericEntity<int>
    { 
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }

}
