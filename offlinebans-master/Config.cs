using Exiled.API.Interfaces;

namespace OfflineBans
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}