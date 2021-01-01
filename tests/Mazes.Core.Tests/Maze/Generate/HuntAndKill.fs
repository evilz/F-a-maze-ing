﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

module Mazes.Core.Tests.Maze.Generate.HuntAndKill

open FsUnit
open Xunit
open Mazes.Core.Canvas.Array2D.Shape
open Mazes.Core.Canvas.ArrayOfA.Shape
open Mazes.Core.Grid.Array2D.Ortho
open Mazes.Core.Grid.ArrayOfA.Polar
open Mazes.Core.Maze.Generate

[<Fact>]
let ``Given a ortho grid 5 by 10, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let orthoGrid =
        (Rectangle.create 5 10)
        |> OrthoGrid.CreateFunction
    
    // act
    let maze = orthoGrid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        " _ _ _ _ _ _ _ _ _ _ \n" +
        "|  _|    _|  _|     |\n" +
        "|_  |_|_ _  |  _| |_|\n" +
        "|  _ _ _|  _| |_ _  |\n" +
        "| |  _ _ _ _|_ _  | |\n" +
        "|_ _ _ _ _ _ _ _ _|_|\n"

    maze.Grid.ToString |> should equal expectedMaze

[<Fact>]
let ``Given a polar disc grid with 5 rings, when generating a maze with the Hunt and Kill algorithm (rng 1), then the output should be like the expected output`` () =
    // arrange
    let grid =
        (Disc.create 5 1.0 3)
        |> PolarGrid.createGridFunction
    
    // act
    let maze = grid |> HuntAndKill.createMaze 1

    // assert
    let expectedMaze =
        "¦ | ¦ ¦\n" +
        "¦¨¦‾|‾¦‾|‾|‾¦\n" +
        "¦‾¦‾|¨|‾¦¨|‾¦‾|¨|¨|¨|¨|‾¦\n" +
        "¦‾¦‾|‾¦¨|¨|‾¦‾|¨|¨|‾¦‾|¨¦‾|¨|‾|¨|‾¦‾|¨¦‾¦‾¦‾|‾|¨¦\n" +
        "¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨|¨¦‾¦¨¦‾¦¨|¨¦¨|¨¦¨¦‾¦‾¦‾¦¨|¨¦¨¦‾¦\n" +
        " ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾ ‾\n"
        
    maze.Grid.ToString |> should equal expectedMaze

    let map = maze.createMap maze.Grid.GetFirstPartOfMazeZone
    map.ConnectedNodes |> should equal maze.Grid.TotalOfMazeCells