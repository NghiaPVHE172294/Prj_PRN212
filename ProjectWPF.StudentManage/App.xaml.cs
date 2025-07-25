using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectWPF.DTO.Models;
using ProjectWPF.Repository.Repositories;
using ProjectWPF.Service.Services;
using Microsoft.Extensions.Configuration;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ProjectWPF.StudentManage;
using ProjectWPF.StudentManage.ViewModels;

namespace ProjectWPF.StudentManage
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // DbContext
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContextFactory<MyProjectContext>(options =>
                        options.UseSqlServer(connectionString));

                    // Repository
                    services.AddScoped<ISinhVienRepository, SinhVienRepository>();
                    services.AddScoped<IGiangVienRepository, GiangVienRepository>();
                    services.AddScoped<IKhoaRepository, KhoaRepository>();
                    services.AddScoped<IMonRepository, MonRepository>();
                    services.AddScoped<IKetQuaRepository, KetQuaRepository>();
                    services.AddScoped<IAccountRepository, AccountRepository>();

                    // Service
                    services.AddScoped<ISinhVienService, SinhVienService>();
                    services.AddScoped<IGiangVienService, GiangVienService>();
                    services.AddScoped<IKhoaService, KhoaService>();
                    services.AddScoped<IMonService, MonService>();
                    services.AddScoped<IKetQuaService, KetQuaService>();
                    services.AddScoped<IAccountService, AccountService>();

                    // View
                    services.AddTransient<LoginView>();
                    services.AddTransient<SinhVienView>();
                    services.AddTransient<GiangVienView>();
                    services.AddTransient<KhoaView>();
                    services.AddTransient<MonView>();
                    services.AddTransient<DiemView>();
                    services.AddTransient<AboutView>();
                    services.AddTransient<GiangVienHome>();
                    services.AddTransient<StuHome>();

                    // Main
                    services.AddSingleton<MainWindow>();
                });

            AppHost = builder.Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            StartAppFlow();
            base.OnStartup(e);
        }

        public void StartAppFlow()
        {
            var accountService = AppHost!.Services.GetRequiredService<IAccountService>();
            var loginVM = new LoginViewModel(accountService);

            var loginView = AppHost.Services.GetRequiredService<LoginView>();
            loginView.DataContext = loginVM;

            var result = loginView.ShowDialog();

            if (result == true && loginVM.IsLoginSuccess)
            {
                switch (loginVM.Role)
                {
                    case "ADMIN":
                        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
                        mainWindow.UserRole = loginVM.Role;
                        mainWindow.Closed += (s, e) => StartAppFlow(); // <-- quay lại login khi window bị đóng
                        mainWindow.Show();
                        break;

                    case "GV":
                        // Lấy mã số giảng viên từ loginVM (Username hoặc thuộc tính phù hợp)
                        var maGv = loginVM.MaSo;
                        var gvWindow = new GiangVienHome(maGv);
                        gvWindow.Closed += (s, e) => StartAppFlow();
                        gvWindow.Show();
                        break;

                    case "SV":
                        var maSo = loginVM.MaSo;
                        var svWindow = new StuHome(maSo);
                        svWindow.Closed += (s, e) => StartAppFlow();
                        svWindow.Show();
                        break;

                    default:
                        MessageBox.Show($"Không xác định được quyền: {loginVM.Role}", "Lỗi");
                        Shutdown();
                        break;
                }
            }
            else
            {
                Shutdown();
            }
        }

    }
}
