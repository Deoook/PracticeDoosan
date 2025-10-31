using PracticeDoosan.View;
using PracticeDoosan.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PracticeDoosan.Service
{
    public class LoadingService : ILoadingService
    {
        private WaitView _waitView;
        private WaitVM _waitViewModel;

        public void Show(string message = "WAIT")
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_waitView != null)
                {
                    return; // 이미 표시 중
                }

                _waitViewModel = new WaitVM
                {
                    IsLoading = true,
                    Message = message,
                    ShowProgress = false
                };

                _waitView = new WaitView
                {
                    DataContext = _waitViewModel,
                    Owner = Application.Current.MainWindow
                };

                _waitView.Show();
            });
        }

        public void UpdateMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_waitViewModel != null)
                {
                    _waitViewModel.Message = message;
                }
            });
        }

        public void UpdateProgress(double progress)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_waitViewModel != null)
                {
                    _waitViewModel.ShowProgress = true;
                    _waitViewModel.Progress = progress;
                }
            });
        }

        public void Hide()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _waitView?.Close();
                _waitView = null;
                _waitViewModel = null;
            });
        }
    }
}
