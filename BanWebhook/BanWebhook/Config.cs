using Exiled.API.Interfaces;

namespace BanWebhook
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public string WebHookURL { get; set; } = "";

        public string PathOfAdminBansCount { get; set; } = "statistics/adminsBanCounts.txt";
    }
}
