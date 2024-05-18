﻿using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using RevitDBExplorer.Domain.DataModel.Members;
using RevitDBExplorer.Domain.DataModel.Members.Base;
using RevitDBExplorer.Domain.DataModel.MembersTemplates.Accessors;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.DataModel.MembersTemplates
{
    internal class Element_Templates : IHaveMemberTemplates
    {
        public IEnumerable<ISnoopableMemberTemplate> GetTemplates() =>
        [   
            MemberTemplate<Element>.Create((doc, target) => doc.ActiveView.GetElementOverrides(target.Id), kind: MemberKind.AsArgument),
            MemberTemplate<Element>.Create((doc, target) => doc.GetWorksetId(target.Id), kind: MemberKind.AsArgument),            

#if R2023_MIN       
            MemberTemplate<Element>.WithCustomAC(typeof(AnalyticalToPhysicalAssociationManager), nameof(AnalyticalToPhysicalAssociationManager.HasAssociation), new AnalyticalToPhysicalAssociationManager_HasAssociation(), kind: MemberKind.AsArgument),
            MemberTemplate<Element>.WithCustomAC(typeof(AnalyticalToPhysicalAssociationManager), nameof(AnalyticalToPhysicalAssociationManager.GetAssociatedElementId), new AnalyticalToPhysicalAssociationManager_GetAssociatedElementId(), kind: MemberKind.AsArgument),
#endif

#if R2024_MIN
            MemberTemplate<Element>.Create((doc, target) => AnalyticalToPhysicalAssociationManager.IsAnalyticalElement(doc, target.Id), kind: MemberKind.StaticMethod),
            MemberTemplate<Element>.Create((doc, target) => AnalyticalToPhysicalAssociationManager.IsPhysicalElement(doc, target.Id), kind: MemberKind.StaticMethod),
            MemberTemplate<Element>.WithCustomAC(typeof(AnalyticalToPhysicalAssociationManager), nameof(AnalyticalToPhysicalAssociationManager.GetAssociatedElementIds), new AnalyticalToPhysicalAssociationManager_GetAssociatedElementIds(), kind: MemberKind.AsArgument),
#endif

            MemberTemplate<Element>.Create((document, target) => SolidSolidCutUtils.IsAllowedForSolidCut(target)),
            MemberTemplate<Element>.Create((document, target) => SolidSolidCutUtils.GetCuttingSolids(target)),
            MemberTemplate<Element>.Create((document, target) => SolidSolidCutUtils.GetSolidsBeingCut(target)),

            MemberTemplate<Element>.Create((document, target) => ElementTransformUtils.CanMirrorElement(document, target.Id)),
        ];
    }
}