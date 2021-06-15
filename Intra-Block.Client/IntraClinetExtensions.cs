using System;
using Intra_Block.COM;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Intra_Block.Client
{
    public static class IntraClientExtensions
    {
        public static IntraClientBuilder AddIntraClient(this IServiceCollection services, Uri host) => new IntraClientBuilder(services, host);
        public static UsingIntraClientBuilder UseIntraClient(this IApplicationBuilder app) => new UsingIntraClientBuilder(app);
    }

    public class IntraClientBuilder
    {
        public IntraClientBuilder(IServiceCollection services, Uri host)
        {
            services.AddGrpcClient<Intra.IntraClient>(options => { options.Address = host; });
            services.AddTransient<IntraCache>();
        }
    }

    public class UsingIntraClientBuilder
    {
        public UsingIntraClientBuilder(IApplicationBuilder app)
        {
        }
    }
}