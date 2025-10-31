using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeDoosan.ViewModel
{
    public partial class WaitVM : ObservableObject
    {
        [ObservableProperty]
        private bool isLoading = true;

        [ObservableProperty]
        private string message = "WAIT";

        [ObservableProperty]
        private double progress = 0;

        [ObservableProperty]
        private bool showProgress = false;
    }
}
