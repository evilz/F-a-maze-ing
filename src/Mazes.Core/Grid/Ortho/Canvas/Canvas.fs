﻿// Copyright 2020 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Grid.Ortho.Canvas

open System.Text
open Mazes.Core
open Mazes.Core.Array2D
open Mazes.Core.Canvas
open Mazes.Core.Grid.Ortho

type Canvas =
    { Zones : Zone[,] }

    member this.NumberOfRows =
        Array2D.length1 this.Zones

    member this.NumberOfColumns =
        Array2D.length2 this.Zones

    member this.MaxRowIndex =
        maxRowIndex this.Zones

    member this.MaxColumnIndex =
        maxColumnIndex this.Zones

    member this.ExistAt coordinate =
        existAt this.Zones coordinate

    member this.Zone coordinate =
        get this.Zones coordinate

    member this.TotalOfMazeZones =
        this.GetZoneByZone RowsAscendingColumnsAscending (fun (zone : Zone) _ -> zone.IsAPartOfMaze)
        |> Seq.length

    member this.IsZonePartOfMaze coordinate =
        (this.Zone coordinate).IsAPartOfMaze

    member this.GetZoneByZone extractBy filter =
        getItemByItem this.Zones extractBy filter

    member this.NeighborsPartOfMazeOf (coordinate : Coordinate) =
        let neighborCoordinateAt = OrthoCoordinate.neighborCoordinateAt coordinate 
        seq {
            let leftCoordinate = neighborCoordinateAt Left
            if (this.ExistAt leftCoordinate) && (this.Zone leftCoordinate).IsAPartOfMaze then
                yield (leftCoordinate, Left)

            let topCoordinate = neighborCoordinateAt Top
            if (this.ExistAt topCoordinate) && (this.Zone topCoordinate).IsAPartOfMaze then
                yield (topCoordinate, Top)

            let rightCoordinate = neighborCoordinateAt Right
            if (this.ExistAt rightCoordinate) && (this.Zone rightCoordinate).IsAPartOfMaze then
                yield (rightCoordinate, Right)

            let bottomCoordinate = neighborCoordinateAt Bottom
            if (this.ExistAt bottomCoordinate) && (this.Zone bottomCoordinate).IsAPartOfMaze then
                yield (bottomCoordinate, Bottom)
        }

    member this.GetFirstPartOfMazeZone =
        this.GetZoneByZone RowsAscendingColumnsAscending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

    member this.GetLastPartOfMazeZone =
        this.GetZoneByZone RowsDescendingColumnsDescending (fun zone _ -> zone.IsAPartOfMaze)
        |> Seq.head

module Canvas =
    let create numberOfRows numberOfColumns isZonePartOfMaze =
        let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create (isZonePartOfMaze rowIndex columnIndex))
    
        { Zones = zones }

module Convert =

    let charPartOfMaze = '*'
    let charEmpty = '.'
    
    let startLineTag = "Type=Canvas"
    let endLineTag = "end"

    let private zoneToChar zone =
        match zone with
        | PartOfMaze -> charPartOfMaze
        | Empty -> charEmpty

    let private charToZone char =
        match char with
        | c when c = charPartOfMaze -> PartOfMaze 
        | c when c = charEmpty -> Empty
        | _ -> failwith "Canvas character not supported"

    let toString canvas =
        let appendZone (sBuilder : StringBuilder) zone =
            sBuilder.Append(zoneToChar(zone)) |> ignore

        let appendRow (sBuilder : StringBuilder) rowZones =
            rowZones
            |> Array.iter(fun zone -> appendZone sBuilder zone)

            sBuilder.Append('\n') |> ignore

        let sBuilder = StringBuilder()
        sBuilder.Append(startLineTag + "\n") |> ignore
        canvas.Zones
            |> extractByRows
            |> Seq.iter(fun rowZones -> appendRow sBuilder rowZones)

        sBuilder.Append(endLineTag) |> ignore

        sBuilder.ToString()

    let fromString (save : string) =
        let lines = save.Split('\n')
        if lines.[0].StartsWith(startLineTag) then
            let numberOfRows = lines.Length - 2
            let numberOfColumns =
                match lines.[1].StartsWith(endLineTag) with
                | false -> lines.[1].Length
                | true -> 0

            let zones = Array2D.init numberOfRows numberOfColumns (fun rowIndex columnIndex -> Zone.create ((charToZone lines.[rowIndex + 1].[columnIndex]) = PartOfMaze))

            Some { Zones = zones; }            
        else
            None