// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System.Diagnostics
open System.Reflection

// properties
let outputDir = "./nuget/io/"
let contentDir = outputDir + "content/"
let nuspec = "./nuget/Velyo.IO.Extensions.nuspec"

let accessKey = ReadFileAsString "my.nukey"

// targets
Target "Clean" (fun _ ->
    CleanDir outputDir
)

Target "Copy" (fun _ ->
    let dir = contentDir + "Extensions/"
    !! "src/Velyo.IO.Extensions/*.cs"
        |> Copy dir
)

Target "NuGet" (fun _ ->
    NuGet (fun p -> 
        {p with
            Project = "Velyo.IO.Exensions"
            Version = "1.1"
            WorkingDir = outputDir
            OutputPath = outputDir
            AccessKey = accessKey
            Publish = false}) 
        nuspec
)

// dependencies
"Clean"
    ==> "Copy"
    ==> "NuGet"

// start build
RunTargetOrDefault "NuGet"