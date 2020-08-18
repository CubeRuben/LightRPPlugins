using Exiled.API.Interfaces;
using System.Security.Policy;

namespace Bleeding
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}
