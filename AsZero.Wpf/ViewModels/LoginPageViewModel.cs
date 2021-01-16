using FutureTech.Mvvm;
using FutureTech.OpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using AsZero.Core.Services.Auth;
using AsZero.Core.Entities;
using AsZero.Wpf.Views;
using MediatR;
using AsZero.Core.Services.Messages;

namespace AsZero.Wpf.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IPrincipalAccessor _principalAccessor;
        private readonly IServiceProvider _sp;

        public User User { get; private set; }

        public LoginPageViewModel(IPrincipalAccessor principalAccessor, IServiceProvider serviceprovider)
        {
            this._principalAccessor = principalAccessor;
            this._sp = serviceprovider;

            this.CmdValidateUser = new AsyncRelayCommand<PasswordBox>(
                async o => {
                    using (var scope = this._sp.CreateScope())
                    {
                        var sp = scope.ServiceProvider;
                        var mediator =  sp.GetRequiredService<IMediator>();
                        var loginResp = await mediator.Send(new LoginRequest {
                            Account = this.Account,
                            Password = o.Password,
                        });
                        this._principalAccessor.SetCurrentPrincipal(loginResp.Principal);

                        var loginWin = sp.GetRequiredService<LoginWindow>();
                        var mainWin = sp.GetRequiredService<MainWindow>();
                        if (loginResp.Status)
                        {
                            loginWin.Hide();
                            mainWin.Show();
                        }
                        else
                        {
                            this.Tips = "登录失败！";
                        }
                    }
                },
                o => true
            );

            this.CmdCreateUser = new AsyncRelayCommand<PasswordBox>(
                async o =>
                {
                    var input = new CreateUserInput()
                    {
                        Account = this.Account,
                        Password = o.Password,
                        Status = UserStatus.Created,
                        UserName = this.Account,
                    };
                    var req = new CreateUserRequest { 
                        Input = input,
                    };
                    using (var scope = this._sp.CreateScope())
                    {
                        var sp = scope.ServiceProvider;
                        var mediator = sp.GetRequiredService<IMediator>();

                        OpResult<User> res = await mediator.Send(req);
                        if (res.Success)
                        {
                            MessageBox.Show("用户创建成功！");
                        }
                        else
                        {
                            MessageBox.Show("用户创建失败！");
                        }
                    }
                },
                o => true
            );
        }

        private string _account;
        public string Account
        {
            get => _account;
            set
            {
                if (this._account != value)
                {
                    _account = value;
                    this.OnPropertyChanged(nameof(Account));
                }
            }
        }


        private string _tips;
        public string Tips { 
            get => _tips; 
            set {
                if (this._tips != value)
                { 
                    this._tips = value;
                    this.OnPropertyChanged(nameof(Tips));
                }
            }
        }

        private bool _hasSignedIn = false;
        public bool HasSignedIn {
            get => _hasSignedIn;
            set {
                if (this._hasSignedIn != value)
                {
                    this._hasSignedIn = value;
                    this.OnPropertyChanged(nameof(HasSignedIn));
                }
            }
        }

        public ICommand CmdValidateUser { get; }
        public ICommand CmdCreateUser { get; }

    }
}
