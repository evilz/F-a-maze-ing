﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Output.File

open CommandLine

[<Literal>]
let verb = "o-file"

[<Verb(verb, isDefault = false, HelpText = "Output to the file system")>]
type OutputFile = {
    [<Option('p', "path", Required = true, HelpText = "The full path of the file")>] path : string
}

let handleVerb data (options : Parsed<OutputFile>) =
    System.IO.File.WriteAllText(options.Value.path, data)