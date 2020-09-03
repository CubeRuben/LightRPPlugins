using Exiled.API.Interfaces;

namespace CustomTeslaDamage
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public float TeslaDamage { get; set; } = 2000;
        public float TeslaDamageRange { get; set; } = 200;
    }
}
