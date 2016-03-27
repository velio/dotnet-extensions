// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open System.Diagnostics
open System.Reflection

// properties
let nugetDir    = "./nuget/"
let nugetSpec   = nugetDir + "Velyo.Web.Extensions.nuspec"
let sourceDir   = "./src/Velyo.Web.Extensions/"
let outputDir   = "./build/"
let version     = "1.1"

let accessKey   = ReadFileAsString "my.nukey"

// NuGet build target
Target "NuGet" (fun _ ->
    NuGet (fun p -> 
        {p with
            Project = "Velyo.Web.Exensions"
            Version = version
            WorkingDir = sourceDir
            OutputPath = outputDir
            AccessKey = accessKey
            Publish = false}) 
        nugetSpec
)

// start build
RunTargetOrDefault "NuGet"