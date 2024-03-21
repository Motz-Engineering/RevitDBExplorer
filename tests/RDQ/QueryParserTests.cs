﻿using System.Linq;
using Autodesk.Revit.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevitDBExplorer.Domain.RevitDatabaseQuery;
using RevitTestLibrary;

namespace RevitDBExplorer.Tests
{
    [TestClass]    
    public class QueryParserTests
    {
        [RevitTestMethod]
        [DataRow("visible")]
        [DataRow("visible ,")]
        [DataRow("visible; ")]
        [DataRow(" , visible , ")]
        public void CanParseSingleCommand(RevitContext revitContext, string query)
        {           
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("visible", result[0].Text);
            Assert.AreEqual(1, result.Count());
        }

        [RevitTestMethod]
        [DataRow("visible, mark = 1")]
        [DataRow("visible, mark = 1 ,")] 
        [DataRow(",visible, mark = 1 ,")]
        public void CanParseTwoCommands(RevitContext revitContext, string query)
        {
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("visible", result[0].Text);
            Assert.AreEqual("mark = 1", result[1].Text);
            Assert.AreEqual(2, result.Count());
        }

        [RevitTestMethod]
        [DataRow("mark = 1,00,view")]
        [DataRow(",mark = 1,00,view,")]   
        public void CanParseDecimalWithComma(RevitContext revitContext, string query)
        {
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("mark = 1,00", result[0].Text);
            Assert.AreEqual("view", result[1].Text);
            Assert.AreEqual(2, result.Count());
        }

        [RevitTestMethod]
        [DataRow("mark = 1,00,12345")]
        [DataRow(",mark = 1,00,12345,")]
        public void CanParseDecimalWithCommaFollowedByInteger(RevitContext revitContext, string query)
        {
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("mark = 1,00", result[0].Text);
            Assert.AreEqual("12345", result[1].Text);
            Assert.AreEqual(2, result.Count());
        }

        [RevitTestMethod]
        [DataRow(",mark = 1,00m,view,")]
        public void CanParseDecimalWithCommaAndUnit(RevitContext revitContext, string query)
        {
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("mark = 1,00m", result[0].Text);
            Assert.AreEqual("view", result[1].Text);
            Assert.AreEqual(2, result.Count());
        }

        [RevitTestMethod]
        [DataRow(",mark = 1,00m,12234,")]
        public void CanParseDecimalWithCommaAndUnitFollowedByInteger(RevitContext revitContext, string query)
        {
            var result = QueryParser.Parse(query).ToList();
            Assert.AreEqual("mark = 1,00m", result[0].Text);
            Assert.AreEqual("12234", result[1].Text);
            Assert.AreEqual(2, result.Count());
        }
    }
}