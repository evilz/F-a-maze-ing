﻿namespace Mazes.Lib

open Mazes.Lib.Cell

type Grid = {
    Cells : Cell[,]
    NumberOfRows : int
    NumberOfColumns : int
}

module Grid =

    let hasCells grid =
        grid.Cells.Length > 0

    let minRowIndex =
        0

    let maxRowIndex grid =
        grid.NumberOfRows - 1

    let minColumnIndex =
        0

    let maxColumnIndex grid =
        grid.NumberOfColumns - 1
        
    module Cell =
        let existAt rowIndex columnIndex grid =
            minRowIndex <= rowIndex &&
            rowIndex <= (maxRowIndex grid) &&
            minColumnIndex <= columnIndex &&
            columnIndex <= (maxColumnIndex grid)

        let getCell rowIndex columnIndex grid =
            grid.Cells.[rowIndex, columnIndex]

        let isTopALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid
            if cell.WallTop.WallType = Border then
                true
            else
                // the top cell is not part of the maze
                (existAt (rowIndex - 1) columnIndex grid) && (getCell (rowIndex - 1) columnIndex grid).CellType = NotPartOfMaze
            
        let isRightALimit rowIndex columnIndex grid =            
            let cell = getCell rowIndex columnIndex grid
            if cell.WallRight.WallType = Border then
                true
            else
                // the right cell is not part of the maze
                (existAt rowIndex (columnIndex + 1) grid) && (getCell rowIndex (columnIndex + 1) grid).CellType = NotPartOfMaze

        let getConstructorCell rowsLength columnLength rowIndex columnIndex =
            let wallTop =
                match rowIndex with
                | 0 -> { WallType = Border; WallPosition = WallPosition.Top }
                | _ -> { WallType = Normal; WallPosition = WallPosition.Top }
                    
            let wallRight =
                match columnIndex with
                | columnIndex when columnIndex = columnLength - 1 -> { WallType = Border; WallPosition = WallPosition.Right }
                | _ -> { WallType = Normal; WallPosition = WallPosition.Right }
                            
            let WallBottom =
                match rowIndex with
                | rowIndex when rowIndex = rowsLength - 1 -> { WallType = Border; WallPosition = WallPosition.Bottom }
                | _ -> { WallType = Normal; WallPosition = WallPosition.Bottom }
                
            let WallLeft =
                match columnIndex with
                | 0 -> { WallType = Border; WallPosition = WallPosition.Left }
                | _ -> { WallType = Normal; WallPosition = WallPosition.Left }
            
            {
                CellType = PartOfMaze
                WallTop = wallTop
                WallRight = wallRight
                WallBottom = WallBottom
                WallLeft = WallLeft
            }

    module Wall =
        let updateWallAtPosition wallPosition wallType rowIndex columnIndex grid =
            match wallPosition with
            | Top ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallTop = { WallType = wallType; WallPosition = WallPosition.Top } }
                grid.Cells.[rowIndex - 1, columnIndex] <- { grid.Cells.[rowIndex - 1, columnIndex] with WallBottom = { WallType = wallType; WallPosition = WallPosition.Bottom } }
            | Right ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallRight = { WallType = wallType; WallPosition = WallPosition.Right } }
                grid.Cells.[rowIndex, columnIndex + 1] <- { grid.Cells.[rowIndex, columnIndex + 1] with WallLeft = { WallType = wallType; WallPosition = WallPosition.Left } }
            | Bottom ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallBottom = { WallType = wallType; WallPosition = WallPosition.Bottom } }
                grid.Cells.[rowIndex + 1, columnIndex] <- { grid.Cells.[rowIndex + 1, columnIndex] with WallTop = { WallType = wallType; WallPosition = WallPosition.Top } }
            | Left ->
                grid.Cells.[rowIndex, columnIndex] <- { grid.Cells.[rowIndex, columnIndex] with WallLeft = { WallType = wallType; WallPosition = WallPosition.Left } }
                grid.Cells.[rowIndex, columnIndex - 1] <- { grid.Cells.[rowIndex, columnIndex - 1] with WallRight = { WallType = wallType; WallPosition = WallPosition.Right } }

    let private getRow numberOfRows numberOfColumns rowIndex =
        [| for i in 0 .. numberOfColumns - 1 -> Cell.getConstructorCell numberOfRows numberOfColumns rowIndex i |]

    let create numberOfRows numberOfColumns =
        let cells = array2D [ for i in 0 .. numberOfRows - 1 -> (getRow numberOfRows numberOfColumns i) ]
                
        { Cells = cells; NumberOfRows = numberOfRows; NumberOfColumns = numberOfColumns }