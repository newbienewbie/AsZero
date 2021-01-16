using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsZero.Core.Entities;
using AsZero.Core.Services.Auth;
using AsZero.Core.Services.Messages;
using FutureTech.OpResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AsZero.Core.Services.MessageHandlers
{
    public class CreateUserInputHandler : IRequestHandler<CreateUserRequest, OpResult<User>>
    {
        private readonly IUserManager _userMgr;
        private readonly ILogger<CreateUserInputHandler> _logger;

        public CreateUserInputHandler(IUserManager userMgr, ILogger<CreateUserInputHandler> logger)
        {
            this._userMgr = userMgr;
            this._logger = logger;
        }

        public async Task<OpResult<User>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var input = request.Input;
                var res = await _userMgr.CreateUserAsync(input);
                return res;
            }
            catch (Exception ex){
                this._logger.LogError($"处理用户创建消息失败:{ex.Message}\r\n{ex.StackTrace}");
                return new FailedOpResult<User>(ex.Message);
            }
        }
    }
}
