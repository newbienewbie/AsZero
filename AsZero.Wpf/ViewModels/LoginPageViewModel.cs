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

namespace AsZero.Wpf.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IServiceProvider _sp;
        private readonly RouterViewModel _routes;
        private readonly IPrincipalAccessor _principalAccessor;

        public User User { get; private set; }

        public LoginPageViewModel(IServiceProvider sp, RouterViewModel routes, IPrincipalAccessor principalAccessor)
        {
            this._sp = sp;
            this._routes = routes;
            this._principalAccessor = principalAccessor;

            this.CmdValidateUser = new AsyncRelayCommand<PasswordBox>(
                async o => {
                    OpResult<User> res = null;
                    ClaimsPrincipal principal;
                    using (var scope = this._sp.CreateScope())
                    {
                        var sp = scope.ServiceProvider;
                        var _userMgr = sp.GetRequiredService<IUserManager>();
                        res = await _userMgr.ValidateUserAsync(this.Account, o.Password);
                        if (res?.Success == true)
                        {
                            this.Tips = String.Empty;
                            this.User = res.Data;
                            this.HasSignedIn = true;

                            principal = await _userMgr.LoadPrincipalAsync(this.Account, true);
                            this._principalAccessor.SetCurrentPrincipal(principal);
                            var loginWin = sp.GetRequiredService<LoginWindow>();
                            loginWin.Hide();
                            var main = sp.GetRequiredService<MainWindow>();
                            main.Show();
                        }
                        else
                        {
                            this._principalAccessor.SetCurrentPrincipal(null);
                            this.Tips = res.Message;
                            this.User = null;
                            this.HasSignedIn = false;
                            MessageBox.Show("登录失败");
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
                    OpResult<User> res = null;
                    using (var scope = this._sp.CreateScope())
                    {
                        var sp = scope.ServiceProvider;
                        var _userMgr = sp.GetRequiredService<IUserManager>();
                        res = await _userMgr.CreateUserAsync(input);
                    }
                    if (res.Success)
                    {
                        MessageBox.Show("用户创建成功！");
                    }
                    else
                    {
                        MessageBox.Show("用户创建失败！");
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

        public bool IsAdmin
        {
            get
            {
                var principal = this._principalAccessor.GetCurrentPrincipal();
                if (principal?.HasClaim(ClaimTypes.Role, "管理员") == true)
                {
                    return true;
                }
                return false;
            }
        }

        public ICommand CmdValidateUser { get; }
        public ICommand CmdCreateUser { get; }

    }
}
