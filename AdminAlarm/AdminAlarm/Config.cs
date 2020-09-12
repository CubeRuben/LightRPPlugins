using Exiled.API.Interfaces;

namespace AdminAlarm
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
