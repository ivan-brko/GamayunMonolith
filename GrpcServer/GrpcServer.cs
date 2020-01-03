using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Gamayun
{
    class InnerGrpcServer : Result.ResultBase
    {
        public InnerGrpcServer(Action<TaskResult> callback)
        {
            Callback = callback;
        }

        public Action<TaskResult> Callback { get; }

        public override Task<EmptyResponse> ReportResult(TaskResult request, ServerCallContext context)
        {
            Callback(request);
            return Task.FromResult(new EmptyResponse());
        }
    }

    public class GrpcServer : IDisposable
    {
        private readonly Server server;

        public GrpcServer(int port, Action<TaskResult> callback)
        {
            Console.WriteLine($"Starting a new grpc server, listening on localhost::{port}");
            server = new Server
            {
                Services = { Result.BindService(new InnerGrpcServer(callback)) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public void Dispose() => server.ShutdownAsync().Wait();
    }


}
