namespace ConfigurationReader

open System.IO

module Toml =
    let private readTomlFile (file : string) =
        let fileParentDirectory = Path.GetDirectoryName file
        let expandVariables (s : string) =
            s.Replace("${CFG_FILE_PATH}", fileParentDirectory)
        let checkTableFields (table : Nett.TomlTable) =
            table.Keys.Contains("task_executor")
            && table.Keys.Contains("arguments") && table.Keys.Contains("cron")

        let deserializeTable (table : Nett.TomlTable) =
            let taskExecutor =
                table.Get<string>("task_executor") |> expandVariables
            let arguments = table.Get<string>("arguments") |> expandVariables
            let cron = table.Get<string>("cron") |> expandVariables
            { Arguments = arguments
              TaskExecutor = taskExecutor
              Cron = cron }

        let toml = Nett.Toml.ReadFile(file)
        toml.Rows
        |> Seq.map (fun x -> (x.Key, x.Value.Get<Nett.TomlTable>()))
        |> Seq.filter (fun (_, config) -> checkTableFields config) //TODO: LOG WRONG CONFIGURATIONS
        |> Seq.map (fun (name, config) -> (name, deserializeTable config))

    let private readConfigurationFile file =
        "",
        { TaskExecutor = ""
          Arguments = ""
          Cron = "" }

    let private getConfigurationFiles root =
        Directory.GetFiles(root, "*.gconf.toml", SearchOption.AllDirectories)

    //entire error handling is missing here
    let ReadConfigurationsFromPath rootPath =
        let configurations =
            getConfigurationFiles rootPath
            |> Seq.collect readTomlFile
            |> Map.ofSeq
        configurations
