using Microsoft.Owin;
using Owin;
using Serilog;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Musicalog.Api.Startup))]

namespace Musicalog.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {        }
    }
}
