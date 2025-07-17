using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.PredefinedScenarios
{
    internal class WorldfoneSouthTelecom_ConnectorAPI : BaseFunctionTest
    {
        #region Initiate variables
        internal static IRestResponse? response;
        internal static string? apiPath;
        internal const string model = "connext.phone.record";
        internal static string secret = "2065af1fb0a2361521db70ce4b3b3516"; // for old db: d01ddb0933102538d21fc09c4ed9f3d2
        internal static string pbx_customer_code = "C0666";
        internal static string dateNow = DateTime.Now.ToString("yyyyMMddTHHmmss");
        internal static string starttime = dateNow;
        internal static string dateNowAddSeconds = DateTime.Now.AddSeconds(30).ToString("yyyyMMddTHHmmss");
        internal static string dateNowAddSeconds2 = DateTime.Now.AddSeconds(120).ToString("yyyyMMddTHHmmss");
        internal static string answertime = dateNowAddSeconds;
        internal static string endtime = dateNowAddSeconds2;
        internal static string datereceived = dateNowAddSeconds2;
        internal static Random rnd = new Random();
        internal static int calUIdRandomLastNumber = rnd.Next(0000010, 9999999);
        internal static string calluuid = "1000000001." + calUIdRandomLastNumber;
        internal static string childcalluuid = "1000000001." + calUIdRandomLastNumber + 1;
        internal static string direction = "outbound"; // outbound: noi bo ben trong goi ra ben ngoai; inbound: nguoc lai
        internal static string calltype = "Outbound non-ACD"; // 'Inbound ACD', 'Inbound non-ACD', 'Outbound ACD', 'Outbound non-ACD'
        internal const string internalCode = "104"; // ma (So noi bo)
        internal static string callernumber = internalCode;
        internal static string agentname = internalCode;
        internal static string dnis = internalCode;
        internal static string destinationnumber = "0384145749";
        #endregion

        #region TestMethod
        [Test, Category("DKHN - API Smoke Tests")]
        public void ST001_Dialing()
        {
            #region Variables declare
            apiPath = "worldfone/callback?pbx_customer_code="+ pbx_customer_code + "&secret=" + secret + "&callstatus=Dialing&calluuid=" + calluuid + "&direction=" + direction + "&callernumber=" + callernumber + "&destinationnumber=" + destinationnumber + "&agentname=" + agentname + "&starttime=" + starttime + "&dnis=" + dnis + "&calltype=" + calltype + "&version=4";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // GET - Dialing
                response = ConnextApi.WorldfoneSouthTelecome(urlSite, apiPath, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            //if (urlSite.Contains("qa-horus") || urlSite.Contains("qa-bvtn")){}
            else Console.WriteLine("No run test case on this site!!!");
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST002_DialAnswer()
        {
            #region Variables declare
            apiPath = "worldfone/callback?pbx_customer_code=" + pbx_customer_code + "&secret=" + secret + "&callstatus=DialAnswer&calluuid=" + calluuid + "&childcalluuid=" + childcalluuid + "&callernumber=" + callernumber + "&destinationnumber=" + destinationnumber + "&answertime=" + answertime + "&version=4&direction=" + direction + "&extension=109&dialid=";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // GET - Dial Answer
                response = ConnextApi.WorldfoneSouthTelecome(urlSite, apiPath, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST003_CDR()
        {
            #region Variables declare
            apiPath = "worldfone/callback?pbx_customer_code=" + pbx_customer_code + "&secret=" + secret + "&callstatus=HangUp&calluuid=" + calluuid + "&datereceived=" + datereceived + "&causetxt=16&context=bGl0ZQ%3D%3D&direction=" + direction + "&dialid=&version=4";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // GET - CDR
                response = ConnextApi.WorldfoneSouthTelecome(urlSite, apiPath, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST004_HangUp()
        {
            #region Variables declare
            string getDateNow = DateTime.Now.ToString("yyyyMMdd");
            apiPath = "worldfone/callback?pbx_customer_code=" + pbx_customer_code + "&secret=" + secret + "&callstatus=CDR&calluuid=" + calluuid + "&starttime=" + starttime + "&answertime=" + answertime + "&endtime=" + endtime + "&billduration=21&totalduration=30&disposition=ANSWERED&monitorfilename=%2Fmnt%2Fsipcloud10_1%2FC0666%2Fcallout%2F" + getDateNow + "%2FSIP%2FC0666109-0027eee0-109-1693962961.2892654.mp3&direction=" + direction + "&dialid=&hangup_by=&version=4";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn")) 
            {
                // GET - HangUp
                response = ConnextApi.WorldfoneSouthTelecome(urlSite, apiPath, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST005_Get_PhoneRecord()
        {
            #region Variables declare
            Thread.Sleep(3000);
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd 00:00:00"); // yyyy-MM-dd HH:mm:ss -> 2024-10-30 13:05:06
            string body = @"{"
                          + "\n" + @"    ""id"": 314,"
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""model"": ""connext.phone.record"","
                          + "\n" + @"        ""method"": ""web_search_read"","
                          + "\n" + @"        ""args"": [],"
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""limit"": 80,"
                          + "\n" + @"            ""offset"": 0,"
                          + "\n" + @"            ""order"": ""start_time DESC"","
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" + @"                ""uid"": 51,"
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""bin_size"": true"
                          + "\n" + @"            },"
                          + "\n" + @"            ""count_limit"": 10001,"
                          + "\n" +               "\"domain\"" + ": " + "[[" + "\"" + "start_time" + "\"" + ", " + "\"" + ">=" + "\"" + ", " + "\"" + dateNow + "\"" + "]]," 
                          + "\n" + @"            ""fields"": [""start_time"", ""end_time"", ""caller_number"", ""destination_number"", ""disposition"", ""bill_duration_sec"", ""mp3_link"", ""customer_res_partner_id"", ""res_user_id"", ""note""]"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" + @"}";
            #endregion

            #region Run Tests
            // get phone record
            response = ConnextApi.WebSearchRead(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            Assert.That((string?)responseJs["result"]["records"][0]["start_time"], Does.Contain(DateTime.Now.ToString("yyyy-MM-dd")));
            Assert.That((string?)responseJs["result"]["records"][0]["end_time"], Does.Contain(DateTime.Now.ToString("yyyy-MM-dd")));
            Assert.That((string?)responseJs["result"]["records"][0]["caller_number"], Is.EqualTo("104"));
            Assert.That((string?)responseJs["result"]["records"][0]["destination_number"], Is.EqualTo(destinationnumber).Or.EqualTo(destinationnumber.Replace("414", "4 14").Replace("574", "5 74")));
            Assert.That((string?)responseJs["result"]["records"][0]["disposition"], Is.EqualTo("ANSWERED"));
            Assert.That(responseJs["result"]["records"][0]["res_user_id"].Any(jt => jt.Value<string>().Contains("Trần Duy Lộc")), Is.True);
            #endregion
        }
        #endregion
    }
}
