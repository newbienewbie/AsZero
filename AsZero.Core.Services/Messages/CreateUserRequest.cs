using AsZero.Core.Entities;
using AsZero.Core.Services.Auth;
using FutureTech.OpResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsZero.Core.Services.Messages
{
    public class CreateUserRequest: IRequest<OpResult<User>>
    {
        public CreateUserInput Input { get; set; }
    }

}
