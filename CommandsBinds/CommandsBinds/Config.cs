using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace CommandsBinds
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
