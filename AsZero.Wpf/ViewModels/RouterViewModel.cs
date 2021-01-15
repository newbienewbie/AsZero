using FutureTech.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace AsZero.Wpf.ViewModels
{

    public class RouterViewModel : ViewModelBase
    {
        public RouterViewModel()
        {
            this.CmdNavigateToPage = new RelayCommand<Page>(
                p => {
                    this.CurrentPage = p;
                },
                p => true
            );
        }

        #region
        private Page _currentPage;

        /// <summary>
        /// 当前页面
        /// </summary>
        public Page CurrentPage {
            get => _currentPage;
            set {
                if (_currentPage != value)
                {
                    this._currentPage = value;
                    this.OnPropertyChanged(nameof(CurrentPage)); 
                }
            }
        }
        #endregion

        public ICommand CmdNavigateToPage { get;  }



        public Func<string, Page> MapSourceToPage { get; set; }


        public void NavigateTo(string source)
        {
            if (this.MapSourceToPage == null)
            {
                throw new Exception($"{nameof(MapSourceToPage)}不可为NULL！你是否忘记设置该属性了？");
            }
            var page = MapSourceToPage(source);
            if (this.CmdNavigateToPage.CanExecute(page))
            {
                this.CmdNavigateToPage.Execute(page);
            }
        }
    }
}
