﻿// Copyright 2020-Present Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Maze

open System
open Mazes.Core.Refac
open Mazes.Core.Refac.Structure

type Maze =
    {
        NDStruct : NDimensionalStructure
    }

module Maze =

    let openMaze m (entrance, exit) =
        NDimensionalStructure.openCell m.NDStruct entrance
        NDimensionalStructure.openCell m.NDStruct exit

    let toMaze nDStruct =
        { NDStruct = nDStruct }

    let braid (rngSeed : int) (ratio : float) (deadEnds : NCoordinate seq) (maze : Maze) =
        let rng = Random(rngSeed)

        let linkToNotAlreadyLinkedNeighbor deadEndCoordinate =
            let notLinkedNeighbors =
                NDimensionalStructure.connectedWithNeighbors maze.NDStruct false deadEndCoordinate
                |> Seq.toArray
            if not (notLinkedNeighbors |> Array.isEmpty) then
                NDimensionalStructure.updateConnectionState maze.NDStruct Open deadEndCoordinate notLinkedNeighbors.[rng.Next(notLinkedNeighbors.Length)]

        deadEnds
        |> Seq.iter(fun deadEnd ->
            if rng.NextDouble() <= ratio then linkToNotAlreadyLinkedNeighbor deadEnd)

        maze