﻿using System.Collections.Generic;
using Autodesk.Revit.DB;
using RevitDBExplorer.Domain.RevitDatabaseQuery.FuzzySearch;
using RevitDBExplorer.WPF.Controls;

// (c) Revit Database Explorer https://github.com/NeVeSpl/RevitDBExplorer/blob/main/license.md

namespace RevitDBExplorer.Domain.RevitDatabaseQuery.Parser.Commands
{
    internal class WorksetCmdDefinition : ICommandDefinition, INeedInitializationWithDocument, IOfferArgumentAutocompletion
    {
        private static readonly AutocompleteItem AutocompleteItem = new AutocompleteItem("w: ", "w:[workset]", "select elements from a given workset", AutocompleteItemGroups.Commands);
        private readonly DataBucket<WorksetCmdArgument> dataBucket = new DataBucket<WorksetCmdArgument>(0.61);


        public void Init(Document document)
        {
            dataBucket.Clear();
            foreach (var workset in new FilteredWorksetCollector(document).OfKind(WorksetKind.UserWorkset).ToWorksets())
            {
                dataBucket.Add(new AutocompleteItem(workset.Name, workset.Name, null), new WorksetCmdArgument(workset.Id, workset.Name), workset.Name);
            }
            dataBucket.Rebuild();
        }


        public IAutocompleteItem GetCommandAutocompleteItem() => AutocompleteItem;
        public IEnumerable<IAutocompleteItem> GetAutocompleteItems(string prefix)
        {
            return dataBucket.ProvideAutoCompletion(prefix);
        }


        public IEnumerable<string> GetClassifiers()
        {
            yield return "w";
            yield return "wrk";
            yield return "workset";
        }
        public IEnumerable<string> GetKeywords()
        {
            yield break;
        }
        public bool CanRecognizeArgument(string argument) => false;
        public bool CanParticipateInGenericSearch() => true;


        public ICommand Create(string cmdText, string argument)
        {
            var args = dataBucket.FuzzySearch(argument);
            return new WorksetCmd(cmdText, args);
        }
    }


    internal class WorksetCmdArgument : CommandArgument<WorksetId>
    {
        public WorksetCmdArgument(WorksetId worksetId, string name) : base(worksetId)
        {

            Name = $"new WorksetId({worksetId})";
            Label = name;
        }
    }


    internal class WorksetCmd : Command
    {
        public WorksetCmd(string text, IEnumerable<IFuzzySearchResult> matchedArguments = null) : base(CmdType.Workset, text, matchedArguments, null)
        {
            IsBasedOnQuickFilter = true;
        }
    }
}