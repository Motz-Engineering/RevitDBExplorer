﻿using System.Diagnostics.CodeAnalysis;
using RevitDBExplorer.Domain.DataModel.ValueContainers.Base;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.DataModel.Accessors
{
    internal readonly ref struct ReadResult
    {
        public required string Label { get; init; }
        public string AccessorName { get; init; }
        public required bool CanBeSnooped { get; init; } = false;
        public bool CanBeVisualized { get; init; } = false;
        public IValueContainer State { get; init; }


        public ReadResult()
        {
            
        }


        [SetsRequiredMembers]
        public ReadResult(string label, string accessorName, bool canBeSnooped, IValueContainer state = null)
        {
            Label = label;
            AccessorName = accessorName;
            CanBeSnooped = canBeSnooped;
            State = state;
        }

        [SetsRequiredMembers]
        public ReadResult(string label, string accessorName, bool canBeSnooped = false, bool canBeVisualized = false, IValueContainer state = null)
        {
            Label = label;
            AccessorName = accessorName;
            CanBeSnooped = canBeSnooped;
            CanBeVisualized = canBeVisualized;
            State = state;
        }

        public static ReadResult Forbidden => new ReadResult("<access denied / forbidden>", null, false, null);
    }
}