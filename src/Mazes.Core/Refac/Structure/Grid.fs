﻿// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.Core.Refac.Structure

type Grid =
    | GridArray2DChoice of GridArray2D
    | GridArrayOfAChoice of GridArrayOfA
    