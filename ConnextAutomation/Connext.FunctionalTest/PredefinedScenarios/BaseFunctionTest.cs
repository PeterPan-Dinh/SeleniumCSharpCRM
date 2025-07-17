using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.PredefinedScenarios
{
    [TestFixture]
    internal class BaseFunctionTest
    {
        internal static string? projectName = null;
        internal static string? siteName = null;
        internal static string? username = null;
        internal static string? password = null;

        internal static string? apiKey = null;
        internal static string? urlSite = null;
        internal static string? db = null;
        internal static string? sessionID = null;
        internal static string? instanceName = null;

        [SetUp]
        public void Setup()
        {
            Config.configurationFile();
            projectName = Config.projectName;
            siteName = Config.siteName;
            username = Config.username;
            password = Config.password;
            apiKey = Config.apikey;
            urlSite = Config.url;
            db = Config.db;
            instanceName = Config.instanceName;

            if (sessionID == null)
            {
                IRestResponse? getAuth = ConnextApi.PostSessionWeb(urlSite, db, username, password);
                sessionID = getAuth.Cookies.SingleOrDefault(x => x.Name == "session_id").Value;
            }
        }

        [TearDown]
        public virtual void Cleanup()
        {
            // Clean up
        }
    }
}
