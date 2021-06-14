using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Intra_Block.COM
{
    public class IntraService : Intra.IntraBase
    {
        private readonly ILogger<IntraService> Logger;
        
        public IntraService(ILogger<IntraService> logger)
        {
            Logger = logger;
        }
        
        public override Task<InsertionResponse> Insert(InsertionRequest request, ServerCallContext context)
        {
            return Task.FromResult(new InsertionResponse()
            {
                Successful = true
            });
        }

        public override Task<RetrievalResponse> Retrieve(RetrievalRequest request, ServerCallContext context)
        {
            return Task.FromResult(new RetrievalResponse()
            {
                Value = "Hello world"
            });
        }
    }
}