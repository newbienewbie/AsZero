using FutureTech.Dal.Repository;
using FutureTech.Dal.Services;
using FutureTech.OpResults;
using AsZero.DbContexts;
using AsZero.Core.Entities;
using AsZero.Core.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsZero.Core.Services.Auth
{

    internal class DefaultUserManager : IUserManager
    {
        private readonly AsZeroDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public DefaultUserManager(AsZeroDbContext dbContext, IPasswordHasher passwordHasher)
        {
            this._dbContext = dbContext;
            this._passwordHasher = passwordHasher;
        }


        /// <summary>
        /// 校验用户密码
        /// </summary>
        /// <returns></returns>
        public Task<OpResult<User>> ValidateUserAsync(string account, string password)
        {
            var user = this._dbContext.Users.FirstOrDefault(u => u.Account == account);
            
            // 账号不存在
            if (user == null)
            {
                OpResult<User> res1 = new FailedOpResult<User>("账号或密码错误");      // 可能客户要求提示账号不存在
                return Task.FromResult(res1);
            }

            // 密码不对
            if (user.Password != this._passwordHasher.ComputeHash(password, user.Salt))
            {
                OpResult<User> res2 = new FailedOpResult<User>("用户名或密码错误");   // 可能客户要求提示密码不对

                return Task.FromResult(res2);
            }

            // 账号异常
            if (user.Status == UserStatus.Hold)
            {
                OpResult<User> res3 = new FailedOpResult<User>("用户账户异常，请联系客服！");
                return Task.FromResult(res3);
            }

            OpResult<User> res = new SucceededOpResult<User>(user);
            return Task.FromResult(res);
        }

        /// <summary>
        /// 更新用户密码的密文
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newCipher"></param>
        /// <returns></returns>
        internal async Task<OpResult<User>> ChangePasswordCipherAsync(string account, string newCipher)
        {
            var existed = this._dbContext.Users.FirstOrDefault(u => u.Account == account);
            if (existed == null)
            {
                return new NotFoundOpResult<User>()
                {
                    Message = $"未找到account={account}的用户记录",
                };
            }
            existed.Password = newCipher;
            await this._dbContext.SaveChangesAsync();
            return new SucceededOpResult<User>(existed);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public async Task<OpResult<User>> ChangePasswordAsync(string account, string newPass)
        {
            var existed = this._dbContext.Users.FirstOrDefault(u => u.Account == account);
            if (existed == null)
            {
                return new NotFoundOpResult<User>()
                {
                    Message = $"未找到account={account}的用户记录",
                };
            }
            var encryptedPwd = this._passwordHasher.ComputeHash(newPass, existed.Salt);
            existed.Password = encryptedPwd;
            await this._dbContext.SaveChangesAsync();
            return new SucceededOpResult<User>(existed);
        }


        public async Task<OpResult<User>> CreateUserAsync(CreateUserInput input)
        {
            var u = this._dbContext.Users.FirstOrDefault(e => e.Account == input.Account);
            if (u != null)
            {
                return new FailedOpResult<User>($"{input.Account}的账户已经存在！");
            }

            var salt = Guid.NewGuid().ToString();
            var pass = this._passwordHasher.ComputeHash(input.Password, salt);
            var user = new User
            {
                Account = input.Account,
                Name = input.UserName,
                Password = pass,
                Salt = salt,
                Status = UserStatus.Created,
            };
            this._dbContext.Users.Add(user);
            await this._dbContext.SaveChangesAsync();
            return new SucceededOpResult<User>(user);
        }

        public Task<User> GetUserAsync(string account)
        {
            var d = this._dbContext.Users.FirstOrDefault(u => u.Account == account);
            return Task.FromResult(d);
        }


        public bool UserExists(string accout)
        {
            return this._dbContext.Users.Any(u => u.Account == accout);
        }

        public async Task<OpResult<User>> ActivateUserAsync(string account)
        {
            var existed = this._dbContext.Users.FirstOrDefault(u => u.Account == account);
            if (existed == null)
            {
                return new NotFoundOpResult<User>();
            }
            if (existed.Status == UserStatus.Activated)
            {
                return new FailedOpResult<User>($"当前用户{account}已经是激活状态！");
            }
            existed.Status = UserStatus.Activated;
            await this._dbContext.SaveChangesAsync();
            return new SucceededOpResult<User>(existed);
        }
    }

}
