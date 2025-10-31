using Microsoft.Extensions.DependencyInjection;
using PracticeDoosan.Service;
using PracticeDoosan.View;
using PracticeDoosan.ViewModel;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PracticeDoosan
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            // DI 컨테이너 설정
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Services 등록
            services.AddSingleton<ILoadingService, LoadingService>();

            // ViewModels 등록
            services.AddTransient<MainVM>();

            // Views 등록 (선택사항)
            services.AddTransient<MainView>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // MainWindow 생성 및 표시
            var mainWindow = _serviceProvider.GetRequiredService<MainView>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<MainVM>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
