﻿using System.Collections.Generic;
using System.Linq;
using RevitDBExplorer.Domain.DataModel.Accessors;
using RevitDBExplorer.Domain.DataModel.ValueContainers.Base;
using RevitDBExplorer.Domain.DataModel.ValueViewModels;
using RevitDBExplorer.Domain.DataModel.ValueViewModels.Base;
using RevitExplorer.Visualizations.DrawingVisuals;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.DataModel.MemberAccessors
{
    internal abstract class MemberAccessorTyped<TSnoopedObjectType> : IAccessor
    {
        public string UniqueId { get; set; }
        public string DefaultInvocation { get; set; }

        IValueViewModel IAccessor.CreatePresenter(SnoopableContext context, object @object)
        {
            Guard.IsAssignableToType<TSnoopedObjectType>(@object);
            var typedObject = (TSnoopedObjectType)@object;
            return CreatePresenter(context, typedObject);
        }
        public virtual IValueViewModel CreatePresenter(SnoopableContext context, TSnoopedObjectType typedObject) => null;
    }


    internal abstract class MemberAccessorTypedWithDefaultPresenter<TSnoopedObjectType> : MemberAccessorTyped<TSnoopedObjectType>, IAccessorForDefaultPresenter
    {
        public override IValueViewModel CreatePresenter(SnoopableContext context, TSnoopedObjectType @object)
        {            
            return new DefaultPresenter(this);
        }

        ReadResult IAccessorForDefaultPresenter.Read(SnoopableContext context, object @object)
        {
            Guard.IsAssignableToType<TSnoopedObjectType>(@object);      
            var typedObject = (TSnoopedObjectType) @object;          
            return Read(context, typedObject);
        }
        public abstract ReadResult Read(SnoopableContext context, TSnoopedObjectType typedObject);

        IEnumerable<SnoopableObject> IAccessorForDefaultPresenter.Snoop(SnoopableContext context, object @object, IValueContainer state)
        {
            Guard.IsAssignableToType<TSnoopedObjectType>(@object);
            var typedObject = (TSnoopedObjectType) @object;            
            return Snoop(context, typedObject, state) ?? Enumerable.Empty<SnoopableObject>();
        }
        public virtual IEnumerable<SnoopableObject> Snoop(SnoopableContext context, TSnoopedObjectType typedObject, IValueContainer state)
        {
            return state?.Snoop();
        }

        IEnumerable<DrawingVisual> IAccessorForDefaultPresenter.GetVisualization(SnoopableContext context, object @object, IValueContainer state)
        {
            Guard.IsAssignableToType<TSnoopedObjectType>(@object);
            var typedObject = (TSnoopedObjectType)@object;
            return GetVisualization(context, typedObject, state) ?? Enumerable.Empty<DrawingVisual>();
        }
        public virtual IEnumerable<DrawingVisual> GetVisualization(SnoopableContext context, TSnoopedObjectType typedObject, IValueContainer state)
        {
            return state?.GetVisualization();
        }
    }
}