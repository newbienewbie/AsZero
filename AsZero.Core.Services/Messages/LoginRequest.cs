using MediatR;
using System.Security.Claims;

namespace AsZero.Core.Services.Messages
{
    public class LoginRequest : IRequest<LoginResponse>
    { 
        /// <summary>
        /// 账号名
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }


    public class LoginResponse
    {
        /// <summary>
        /// 是否成功？
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 如果成功了，该字段执行该用户的Principal
        /// </summary>
        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// 成功或者失败的提示
        /// </summary>
        public string Tip { get; set; }
    }

}
