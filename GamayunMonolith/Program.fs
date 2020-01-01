// Learn more about F# at http://fsharp.org
open System

[<EntryPoint>]
let main argv =
    printfn "Starting Gamayun monolith..."
    let config =
        ConfigurationReader.Toml.ReadConfigurationsFromPath
            "/home/ivan/Projects/fsharp/GamayunMonolith/ConfigurationReader/SampleConfigurations"        
    0 // return an integer exit code
