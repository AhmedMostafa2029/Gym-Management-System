using GymSystem.BLL.ViewModels.AnalyticsViewModels;

namespace GymSystem.BLL.ViewModels.AccountViewModels
{
    public class LoginPageViewModel
    {
        public LoginViewModel Login { get; set; } = new();

        public AnalyticsViewModel Analytics { get; set; } = new();
    }
}