﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitDBExplorer.Domain.DataModel.Accessors;
using RevitDBExplorer.Domain.DataModel.ValueContainers.Base;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.DataModel.MemberAccessors
{
    internal abstract class MemberAccessorByType<TSnoopedObjectType> : MemberAccessorTypedWithDefaultPresenter<TSnoopedObjectType> where TSnoopedObjectType : class
    {
        public sealed override ReadResult Read(SnoopableContext context, TSnoopedObjectType @object)
        {           
            string label = GetLabel(context.Document, @object);
            bool canBeSnooped = CanBeSnoooped(context.Document, @object);
            bool canBeVisualized = false;

            return new ReadResult(label, "[ByType] " + GetType().GetCSharpName(), canBeSnooped, canBeVisualized);
        }
        protected abstract bool CanBeSnoooped(Document document, TSnoopedObjectType value);
        protected abstract string GetLabel(Document document, TSnoopedObjectType value);

        public sealed override IEnumerable<SnoopableObject> Snoop(SnoopableContext context, TSnoopedObjectType @object, IValueContainer state)
        {
            return this.Snooop(context.Document, @object);
        }
        protected virtual IEnumerable<SnoopableObject> Snooop(Document document, TSnoopedObjectType value) => Enumerable.Empty<SnoopableObject>();       
    }


    internal abstract class MemberAccessorByTypeLambda<TSnoopedObjectType> : MemberAccessorByType<TSnoopedObjectType> where TSnoopedObjectType : class
    {
        public Overrides Override { get; } = new Overrides();

        protected override bool CanBeSnoooped(Document document, TSnoopedObjectType value)
        {
            ArgumentNullException.ThrowIfNull(Override.CanBeSnooped);
            return Override.CanBeSnooped(document, value);
        }
        protected override string GetLabel(Document document, TSnoopedObjectType value)
        {
            ArgumentNullException.ThrowIfNull(Override.GetLabel);
            return Override.GetLabel(document, value);
        }
        protected override IEnumerable<SnoopableObject> Snooop(Document document, TSnoopedObjectType value)
        {
            if (Override.Snoop == null) return null;
            return Override.Snoop(document, value);
        }


        public class Overrides
        {
            public Func<Document, TSnoopedObjectType, bool> CanBeSnooped { get; set; }
            public Func<Document, TSnoopedObjectType, string> GetLabel { get; set; }            
            public Func<Document, TSnoopedObjectType, IEnumerable<SnoopableObject>> Snoop { get; set; }
        }
    }
}