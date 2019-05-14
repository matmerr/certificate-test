using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.SelfHost;

namespace CertificateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceHostConfig = new HttpSelfHostConfiguration("https://127.0.0.1:34343");

            //serviceHostConfig.ClientCredentialType = HttpClientCredentialType.Certificate;
            serviceHostConfig.ClientCredentialType = HttpClientCredentialType.None;

            serviceHostConfig.MessageHandlers.Add(new TestAuthorizationMetadataHandler());

            var server = new HttpSelfHostServer(serviceHostConfig);
            server.OpenAsync().Wait();

            Console.WriteLine("Press Enter to exit..");
            GC.KeepAlive(server);
            Console.ReadLine();
        }
    }


    class TestAuthorizationMetadataHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cert = request.GetClientCertificate();
            if (cert != null)
            {
                Console.WriteLine(cert);

            } else
            {
                Console.WriteLine("No cert");
            }
            return new HttpResponseMessage();
        }
    }
}
