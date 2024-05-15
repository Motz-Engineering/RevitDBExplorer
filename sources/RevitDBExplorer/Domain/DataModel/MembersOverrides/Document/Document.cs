﻿using System.Collections.Generic;
using Autodesk.Revit.DB;
using RevitDBExplorer.Domain.DataModel.Members;
using RevitDBExplorer.Domain.DataModel.Members.Base;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.DataModel.MembersOverrides
{
    internal class Document_Overrides : IHaveMembersOverrides
    {
        public IEnumerable<IMemberOverride> GetOverrides() =>
        [
            MemberOverride<Document>.ByFunc((doc, document) => Document.GetDocumentVersion(document)),
        ];
    }
}
