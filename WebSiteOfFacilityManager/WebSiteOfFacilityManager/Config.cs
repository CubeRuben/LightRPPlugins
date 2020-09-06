using Exiled.API.Interfaces;

namespace WebSiteOfFacilityManager
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public static string WebSiteDataPath { get; set; } = "site/";
    }
}
