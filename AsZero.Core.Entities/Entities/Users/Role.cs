using FutureTech.Dal.Entities;

namespace AsZero.Core.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role : GenericEntity<int>
    { 
        public string RoleName{ get;set;}
        public string Note { get; set; }
    }
}
