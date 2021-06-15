using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Core;
using Intra_Block.Cache;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Intra_Block.COM
{
    public class IntraService : Intra.IntraBase
    {
        private readonly ILogger<IntraService> Logger;
        private readonly Cache.Cache Cache;
        private readonly Administratum Administratum;

        public IntraService(ILogger<IntraService> logger, Cache.Cache cache, Administratum administratum)
        {
            Logger = logger;
            Cache = cache;
            Administratum = administratum;
        }

        public override Task<GenericResponse> Insert(InsertionRequest request, ServerCallContext context)
        {
            Logger.LogInformation($"{context.Host} inserting \"{request.Key}\" into the cache");

            try
            {
                Cache.Insert(request.Key, request.Value, request.Expiry);

                return Task.FromResult(new GenericResponse()
                {
                    Successful = true
                });
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);

                return Task.FromResult(new GenericResponse()
                {
                    Successful = false,
                    Message = e.Message
                });
            }
        }

        public override Task<RetrievalResponse> Retrieve(RetrievalRequest request, ServerCallContext context)
        {
            Logger.LogInformation($"{context.Host} requesting \"{request.Key}\" from the cache");

            try
            {
                return Task.FromResult(new RetrievalResponse()
                {
                    Value = Cache.Retrieve(request.Key)
                });
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        public override Task<GenericResponse> Remove(RemoveRequest request, ServerCallContext context)
        {
            Logger.LogInformation($"{context.Host} removing \"{request.Key}\" from the cache");
            
            try
            {
                Cache.Exterminatus(request.Key);
                
                return Task.FromResult(new GenericResponse()
                {
                    Successful = true
                });
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                
                return Task.FromResult(new GenericResponse()
                {
                    Successful = false,
                    Message = e.Message
                });
            }
            throw new NotImplementedException();
        }

        public override Task<ReportResponse> Report(ReportRequest request, ServerCallContext context)
        {
            Logger.LogInformation($"{context.Host} requesting a report");

            var report = Administratum.GatherReport();

            return Task.FromResult(new ReportResponse()
            {
                NumberOfEntries = (ulong)Cache.Keys().Count,
                Uptime = (ulong)(DateTime.Now - Process.GetCurrentProcess().StartTime).Ticks,
                CurrentMemoryUsage = (ulong)Process.GetCurrentProcess().PrivateMemorySize64,
                Averages = new Averages
                {
                    ReapsPerMinute = report.Averages.ReapsPerMinute,
                    RequestsPerMinute = report.Averages.RequestsPerMinute,
                    TimeToInsert = report.Averages.TimeToInsert,
                    TimeToRetrieve = report.Averages.TimeToRetrieve,
                    RequestCompletion = report.Averages.RequestCompletion
                }
            });
        }
    }
}