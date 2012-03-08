// -----------------------------------------------------------------------
// <copyright file="ConvertTests.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace HttpBuildQuery.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Country
    {
        public string Name { get; set; }
        public string[] States { get; set; }
    }

    public class Company
    {
        public string Name { get; set; }
        public Country[] Countries { get; set; }
    }

    public class Employee
    {
        public Company Company { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int[] Supervisors { get; set; }
    }

    [TestClass]
    public class ConvertTests
    {
        [TestMethod]
        public void T00_Test_Employee()
        {
            var e1 = new Employee();
            e1.ID = 1;
            e1.Name = "John";
            var e1Obj = (Dictionary<string, object>)HttpBuildQueryHelper.Convert(e1);
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("ID=1&Name=John", HttpBuildQueryHelper.FormatDictionary(e1Obj)));

            var e2 = new Employee();
            e2.ID = 1;
            e2.Supervisors = new[] { 1, 2 };
            var e2Obj = (Dictionary<string, object>)HttpBuildQueryHelper.Convert(e2);
            Assert.IsTrue(TestUtility.AreQueryStringsEqual("ID=1&Supervisors[0]=1&Supervisors[1]=2", HttpBuildQueryHelper.FormatDictionary(e2Obj)));

            var e3 = new Employee();
            e3.ID = 1;
            e3.Supervisors = new[] { 1, 2 };
            e3.Company = new Company()
            {
                Name = "A1",
                Countries = new Country[] { 
                    new Country() { Name = "US", States = new string[] { "AL", "IN" } }
                }
            };
            var e3Obj = (Dictionary<string, object>)HttpBuildQueryHelper.Convert(e3);
            string e3qs1 = "ID=1&Supervisors[0]=1&Supervisors[1]=2&Company[Name]=A1&Company[Countries][0][Name]=US&Company[Countries][0][States][0]=AL&Company[Countries][0][States][1]=IN";
            string e3qs2 = HttpBuildQueryHelper.FormatDictionary(e3Obj);

            Assert.IsTrue(TestUtility.AreQueryStringsEqual(e3qs1, e3qs2));
        }
    }
}
