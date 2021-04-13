﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

open System.Collections.Generic
open Mazes.Core.Refac

type NDimensionalStructure =
    private
        {
            Structure : Dictionary<Dimension, Grid>
            CoordinateConnections : CoordinateConnections
            Obstacles : Obstacles
        }