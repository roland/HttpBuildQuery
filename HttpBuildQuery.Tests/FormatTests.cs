using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HttpBuildQuery.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FormatTests
    {
        [TestMethod]
        public void T00_Structs()
        {
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("q=1", HttpBuildQueryHelper.FormatValue(1)));
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("q=a", HttpBuildQueryHelper.FormatValue("a")));
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("id=2", HttpBuildQueryHelper.FormatValue(2, "id")));
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("id=1", HttpBuildQueryHelper.FormatValue(true, "id")));
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("id=0", HttpBuildQueryHelper.FormatValue(false, "id")));
        }

        [TestMethod]
        public void T01_Test_Dictionaries()
        {
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("pid=3", HttpBuildQueryHelper.FormatDictionary(
                  new Dictionary<string, object>() { { "pid", 3 } }
            )));

            Assert.IsTrue(TestUtility.AreQueryStringsEqual("pid=3", HttpBuildQueryHelper.FormatDictionary(
                new Dictionary<string, object>() 
                { 
                    { "pid", 3 }
                }
            )));

            var customD1 = new Dictionary<string, object>()
            {
                { "param1", 5 },
                { "param2", 6 }
            };
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("param1=5&param2=6", HttpBuildQueryHelper.FormatDictionary(customD1)));
        }

        [TestMethod]
        public void T02_Test_Arrays_And_Lists()
        {
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("values[0]=1&values[1]=2", HttpBuildQueryHelper.FormatList(new object[] { 1, 2 }, "values")));
        }

        [TestMethod]
        public void T03_Test_Boolean()
        {
            var customD3 = new Dictionary<string, object>()
            {
                { "param1", true },
                { "param2", false }
            };
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("param1=1&param2=0", HttpBuildQueryHelper.FormatDictionary(customD3)));
        }

        [TestMethod]
        public void T04_Test_Embedded_Structures()
        {
            var customD2 = new Dictionary<string, object>()
            {
                { "param1", new Dictionary<string, object>() { { "a", 1 }, {"b", 2 } } },
                { "param2", new object[] { "c", "d", new Dictionary<string, object>() { { "e", 1} } } }
            };

            Assert.IsTrue(TestUtility.AreQueryStringsEqual("param1[a]=1&param1[b]=2&param2[0]=c&param2[1]=d&param2[2][e]=1", HttpBuildQueryHelper.FormatDictionary(customD2)));
        }

        [TestMethod]
        public void T05_Test_Nulls()
        {
            var d1 = new Dictionary<string, object>()
            {
                { "param1", null }
            };

            Assert.IsTrue(TestUtility.AreQueryStringsEqual("", HttpBuildQueryHelper.FormatDictionary(d1)));
        }
    }
}
