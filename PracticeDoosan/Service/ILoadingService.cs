using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeDoosan.Service
{
    public interface ILoadingService
    {
        void Show(string message = "WAIT");
        void UpdateMessage(string message);
        void UpdateProgress(double progress);
        void Hide();
    }
}
