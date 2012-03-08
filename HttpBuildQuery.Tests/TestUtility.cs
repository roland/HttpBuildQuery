// -----------------------------------------------------------------------
// <copyright file="TestUtility.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace HttpBuildQuery.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Collections.Specialized;

    public class TestUtility
    {
        public static bool AreQueryStringsEqual(string qs1, string qs2)
        {
            bool equal = true;
            NameValueCollection nvcqs1 = HttpUtility.ParseQueryString("?" + qs1);
            NameValueCollection nvcqs2 = HttpUtility.ParseQueryString("?" + qs2);

            var nvcqs1keys = nvcqs1.AllKeys;
            var nvcqs2keys = nvcqs2.AllKeys;
            var allKeys = nvcqs1keys.Union(nvcqs2keys).Distinct();
            equal = equal &&
                allKeys.All(key => nvcqs1keys.Contains(key) && nvcqs2keys.Contains(key)) &&
                allKeys.All(key => nvcqs1[key] == nvcqs2[key]);

            return equal;
        }
    }
}
