﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitDBExplorer.Domain.DataModel;
using RevitDBExplorer.Domain.RevitDatabaseQuery.Filters;
using RevitDBExplorer.Domain.RevitDatabaseQuery.Parser;
using RevitDBExplorer.Domain.RevitDatabaseQuery.Parser.Commands;
using VisibleInViewFilter = RevitDBExplorer.Domain.RevitDatabaseQuery.Filters.VisibleInViewFilter;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.RevitDatabaseQuery
{
    internal static class RevitDatabaseQueryService
    {
        public static void Init()
        {
            CommandParser.Init();          
        }


        public static Result ParseAndExecute(Document document, string query)
        {
            if (document is null) return new Result(null, new List<ICommand>(), new SourceOfObjects());  
                      
            CommandParser.LoadDocumentSpecificData(document);
            var commands = QueryParser.Parse(query);
            commands.SelectMany(x => x.Arguments).OfType<ParameterArgument>().ToList().ForEach(x => x.ResolveStorageType(document));

            var pipe = new List<QueryItem>();
            pipe.AddRange(VisibleInViewFilter.Create(commands, document));
            pipe.AddRange(ElementTypeFilter.Create(commands));
            pipe.AddRange(ElementIdFilter.Create(commands));
            pipe.AddRange(ClassFilter.Create(commands));
            pipe.AddRange(CategoryFilter.Create(commands));
            pipe.AddRange(StructuralTypeFilter.Create(commands));
            pipe.AddRange(LevelFilter.Create(commands));
            pipe.AddRange(RoomFilter.Create(commands, document));
            pipe.AddRange(RuleFilter.Create(commands, document));
            pipe.AddRange(ParameterFilter.Create(commands));

            var collector = new FilteredElementCollector(document);
            var collectorSyntax = "new FilteredElementCollector(document)";

            foreach (var filter in pipe)
            {
                if (filter.Filter != null)
                {
                    collector.WherePasses(filter.Filter);
                    collectorSyntax += Environment.NewLine + "    " + filter.CollectorSyntax;
                }
            }
            collectorSyntax += Environment.NewLine + "    .ToElements();";

            bool atLeastOneFilter = pipe.Any();

            return new Result(collectorSyntax, commands, new SourceOfObjects(new RemoteExecutor(collector, document, atLeastOneFilter)));
        }

        public record Result(string GeneratedCSharpSyntax, IList<ICommand> Commands, SourceOfObjects SourceOfObjects);

        public class RemoteExecutor : IAmSourceOfEverything
        {
            private readonly FilteredElementCollector collector;
            private readonly Document document;
            private readonly bool atLeastOneFilter;

            public RemoteExecutor(FilteredElementCollector collector, Document document, bool atLeastOneFilter)
            {              
                this.collector = collector;
                this.document = document;
                this.atLeastOneFilter = atLeastOneFilter;
            }


            public IEnumerable<SnoopableObject> Snoop(UIApplication app)
            {
                if (document == null) return null;
                if (atLeastOneFilter == false) return null;
                var snoopableObjects = collector.ToElements().Select(x => new SnoopableObject(document, x));
                return snoopableObjects;
            }
        }
    }
}