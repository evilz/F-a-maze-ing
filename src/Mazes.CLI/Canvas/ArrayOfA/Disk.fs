﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.CLI.Canvas.ArrayOfA.Disk

open CommandLine
open Mazes.Core.Canvas.ArrayOfA.Shape

[<Literal>]
let verb = "s-disk"

[<Verb(verb, isDefault = false, HelpText = "Disk shape")>]
type ShapeDisk = {
    [<Option('r', "rings", Required = true, HelpText = "The number of rings.")>] rings : int
    [<Option('w', "ratio", Required = true, HelpText = "Width height ratio." )>] ratio : float
    [<Option('c', "center", Required = true, HelpText = "Number of cells for the central ring." )>] center : int
}

let handleVerb (options : Parsed<ShapeDisk>) =
    Disk.create options.Value.rings options.Value.ratio options.Value.center