using System;
using System.Net;
using System.Threading.Tasks;
using Viasoft.Core.WebHost;

namespace Viasoft.Qualidade.RNC.Core.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            await ViasoftCoreWebHost.Main<Startup>(args);
        }
    }
}