// Learn more about F# at http://fsharp.org
open System

[<EntryPoint>]
let main argv =
    printfn "Starting Gamayun monolith..."
    let config =
        ConfigurationReader.Toml.ReadConfigurationsFromPath
            "/home/ivan/Projects/fsharp/GamayunMonolith/ConfigurationReader/SampleConfigurations"

    let grpcPort = 50052
    use server = new Gamayun.GrpcServer(grpcPort, fun x -> printfn "Got a message from id %d" x.JobId)
            
    0 // return an integer exit code
