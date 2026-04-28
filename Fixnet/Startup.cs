using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fixnet.Startup))]

namespace Fixnet
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(
            typeof(IUserIdProvider),
            () => new SignalRUserIdProvider());

            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);

            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };


            app.MapSignalR(hubConfiguration);
        }
    }
}