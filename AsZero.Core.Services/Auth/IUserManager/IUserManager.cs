using FutureTech.OpResults;
using AsZero.Core.Entities;
using System.Threading.Tasks;

namespace AsZero.Core.Services.Auth
{
    public interface IUserManager
    {
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="accout"></param>
        /// <returns></returns>
        bool UserExists(string accout);

        /// <summary>
        /// 根据账号获取用户实体
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(string account);


        /// <summary>
        /// 验证用户的账户和密码是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<OpResult<User>> ValidateUserAsync(string account, string password);

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OpResult<User>> CreateUserAsync(CreateUserInput input);

        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OpResult<User>> ActivateUserAsync(string account);

        /// <summary>
        /// 更新用户的密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        Task<OpResult<User>> ChangePasswordAsync(string account, string newPass);
    }
}
