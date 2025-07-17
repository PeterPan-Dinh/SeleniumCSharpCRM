using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.PredefinedScenarios.BVTN
{
    internal class LeadTest : BaseFunctionTest
    {
        #region Initiate variables
        internal static IRestResponse? response;
        internal static string? apiPath,
        model = "crm.lead";
        internal static int id = 62;
        internal static int? recordId;
        internal static string leadName = "QA Test API Lead";
        internal static string phone = "02119899991";
        internal static int loaiNguonId = 5; // --> Online / Ads
        internal static int loaiNguonDetailId = 3; // --> Chiến dịch tháng 11
        internal static int loaiNguonDetailId2 = 7; // --> www.testads.com
        #endregion

        #region Head and Clean
        [SetUp]
        public void SetupData()
        {
            // before run: Check if record exists, then delete it
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            // Clean up ...
        }
        #endregion

        #region TestMethod
        [Test, Category("BVTN - API Smoke Tests")]
        public void ST001_Post_Create_Lead()
        {
            #region before run: Get all search Lead and delete
            const string inputSearch = "qa test api"; // ivf
            response = GetWebSearchRead(urlSite, model, inputSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            string[]? ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            string recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadLead(urlSite, model, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region Variables declare
            string body = @"{
                          " + "\n" + "\"id\"" + ": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + @"        ""args"": [{
                          " + "\n" + @"                ""subtype"": false,
                          " + "\n" + @"                ""stage_id"": 16,
                          " + "\n" + @"                ""active"": true,
                          " + "\n" + @"                ""company_id"": 1,
                          " + "\n" + "\"name\"" + ": " + "\"" + leadName + "\"" + ","
                            + "\n" + "\"contract_code\"" + ": " + "false" + ","
                            + "\n" + "\"expected_revenue\"" + ": " + 0 + ","
                            + "\n" + "\"automated_probability\"" + ": " + 99.7 + ","
                            + "\n" + "\"probability\"" + ": " + 0 + ","
                            + "\n" + "\"partner_id\"" + ": " + "false" + ","
                            + "\n" + "\"partner_name\"" + ": " + "false" + ","
                            + "\n" + "\"contact_name\"" + ": " + "false" + ","
                            + "\n" + "\"birthday\"" + ": " + "false" + ","
                            + "\n" + "\"year_of_birth\"" + ": " + 0 + ","
                            + "\n" + "\"phone\"" + ": " + "\"" + phone + "\"" + ","
                            + "\n" + "\"sdt_2\"" + ": " + "false" + ","
                            + "\n" + "\"sdt_3\"" + ": " + "false" + ","
                            + "\n" + "\"email_from\"" + ": " + "false" + ","
                            + "\n" + "\"website\"" + ": " + "false" + ","
                            + "\n" + "\"street\"" + ": " + "false" + ","
                            + "\n" + "\"state_id\"" + ": " + "false" + ","
                            + "\n" + "\"district_id\"" + ": " + "false" + ","
                            + "\n" + "\"ward_id\"" + ": " + "false" + ","
                            + "\n" + "\"street2\"" + ": " + "false" + ","
                            + "\n" + "\"city\"" + ": " + "false" + ","
                            + "\n" + "\"zip\"" + ": " + "false" + ","
                            + "\n" + "\"country_id\"" + ": " + 241 + ","
                            + "\n" + "\"patient_type\"" + ": " + "false" + ","
                            + "\n" + "\"lang_id\"" + ": " + "false" + ","
                            + "\n" + "\"medical_examination_day\"" + ": " + "false" + ","
                            + "\n" + "\"status\"" + ": " + "false" + ","
                            + "\n" + "\"lost_reason_id\"" + ": " + "false" + ","
                            + "\n" + "\"service_unit_id\"" + ": " + 2 + ","
                            + "\n" + "\"tag_ids\"" + ": " + "[[6, false, []]]" + ","
                            + "\n" + "\"type\"" + ": " + "\"lead\"" + ","
                            + "\n" + "\"net_revenue\"" + ": " + 0 + ","
                            + "\n" + "\"date_deadline\"" + ": " + "false" + ","
                            + "\n" + "\"priority\"" + ": " + "\"0\"" + ","
                            + "\n" + "\"user_id\"" + ": " + 18 + ","
                            + "\n" + "\"team_id\"" + ": " + "false" + ","
                            + "\n" + "\"contract_id\"" + ": " + "false" + ","
                            + "\n" + "\"medical_examination_date_closed\"" + ": " + "false" + ","
                            + "\n" + "\"medical_examination_due_date\"" + ": " + "false" + ","
                            + "\n" + "\"start_date_get_sample\"" + ": " + "false" + ","
                            + "\n" + "\"end_date_get_sample\"" + ": " + "false" + ","
                            + "\n" + "\"place_of_registration\"" + ": " + "\"" + "noivien" + "\"" + ","
                            + "\n" + "\"package_examination\"" + ": " + "false" + ","
                            + "\n" + "\"lead_properties\"" + ": " + "[]" + ","
                            + "\n" + "\"description\"" + ": " + "false" + ","
                            + "\n" + "\"campaign_id\"" + ": " + "false" + ","
                            + "\n" + "\"medium_id\"" + ": " + "false" + ","
                            + "\n" + "\"source_id\"" + ": " + "false" + ","
                            + "\n" + "\"refer_by\"" + ": " + "false" + ","
                            + "\n" + "\"referred\"" + ": " + "false" + ","
                            + "\n" + "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                            + "\n" + "\"connext_source_type_detail_id\"" + ": " + loaiNguonDetailId + ","
                            + "\n" + "\"connext_source_type_detail_id_2\"" + ": " + loaiNguonDetailId2 + ","
                            + "\n" + "\"cbnv\"" + ": " + "false" + ","
                            + "\n" + "\"kh_gioi_thieu\"" + ": " + "false" + ","
                            + "\n" + "\"title\"" + ": " + "false" + ","
                            + "\n" + "\"function\"" + ": " + "false" + ","
                            + "\n" + "\"mobile\"" + ": " + "false" + ","
                            + "\n" + "\"medical_examination_ids\"" + ": " + "[]"
                            + "\n" + "}" + "\n" + "],"
                            + "\n" + "\"model\"" + ": " + "\"crm.lead\"" + ","
                            + "\n" + "\"method\"" + ": " + "\"create\"" + ","
                            + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 18,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""default_type"": ""lead""
                          " + "\n" + @"            } " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // create Lead
            response = ConnextApi.Create(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            Assert.That((double?)responseJs["jsonrpc"], Is.GreaterThanOrEqualTo(2.0));
            Assert.That((int?)responseJs["id"], Is.GreaterThanOrEqualTo(10));
            Assert.That((int?)responseJs["result"], Is.GreaterThanOrEqualTo(4000));
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST002_Get_WebSearchRead_Lead()
        {
            #region Variables declare
            string inputSearch = "qa test api";
            string order = "create_date DESC";
            const string contextLastKey = "default_type";
            const string contextLastKeyValue = "\"" + "lead" + "\"";
            string domain = "[" + "\"&\"" + ", " + "\"|\"" + ", " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "\"lead\"" + "], " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "false" + "], " + "\"|\", " + "\"|\", " + "\"|\", " + "[" + "\"partner_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email_from\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"contact_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"create_date\"" + ", " + "\"date_open\"" + ", " + "\"contact_name\"" + ", " + "\"street2\"" + ", " + "\"city\"" + ", " + "\"stage_id\"" + ", " + "\"name\"" + ", " + "\"last_telesale_note\"" + ", " + "\"status\"" + ", " + "\"unread_message_count\"" + ", " + "\"tag_ids\"" + ", " + "\"team_id\"" + ", " + "\"user_id\"" + ", " + "\"source_id\"" + ", " + "\"campaign_id\"" + ", " + "\"last_trucchat_note\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"probability\"" + ", " + "\"is_sale_manager\"" + ", " + "\"is_chat_manager\", " + "\"is_team_leader\", " + "\"create_uid\", " + "\"write_date\"" + "]";
            string body = @"{
                          " + "\n" + "\"id\"" + ": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""web_search_read"",
                          " + "\n" + @"        ""args"": [],
                          " + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""limit"": 80,
                          " + "\n" + @"            ""offset"": 0,
                          " + "\n" + "\"order\"" + ": " + "\"" + order + "\"" + ","
                            + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 51,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""bin_size"": true,
                          " + "\n" + "\"" + contextLastKey + "\"" + ": " + contextLastKeyValue
                            + "\n" + @"            },
                          " + "\n" + @"            ""count_limit"": 10001,
                          " + "\n" + "\"domain\"" + ": " + domain
                            + "\n" + "\"fields\"" + ": " + fields
                            + "\n" + @"        }
                          " + "\n" + @"    } " + "\n" +
                          @"}";
            #endregion

            #region Run Tests
            // get search Lead
            response = ConnextApi.WebSearchRead(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            //Assert.That((string?)responseJs["result"]["records"][0]["phone"], Is.EqualTo(phone));
            Assert.That((string?)responseJs["result"]["records"][0]["name"], Is.EqualTo(leadName));
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST003_Delete_WebSearchRead_Lead()
        {
            #region Get search Lead
            const string inputSearch = "qa test api"; // ivf
            response = GetWebSearchRead(urlSite, model, inputSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            recordId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Variables declare
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""unlink"",
                          " + "\n" + "\"args\": " + args + ", "
                            + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 51,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""default_type"": ""lead""
                          " + "\n" + @"            }
                          " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // delete search Lead
            response = ConnextApi.Delete(urlSite, model, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }
        #endregion

        private static IRestResponse GetWebSearchRead(string urlSite, string model, string inputSearch, string sessionID)
        {
            #region Variables declare
            string order = "create_date DESC";
            const string contextLastKey = "default_type";
            const string contextLastKeyValue = "\"" + "lead" + "\"";
            string domain = "[" + "\"&\"" + ", " + "\"|\"" + ", " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "\"lead\"" + "], " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "false" + "], " + "\"|\", " + "\"|\", " + "\"|\", " + "[" + "\"partner_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email_from\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"contact_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"create_date\"" + ", " + "\"date_open\"" + ", " + "\"contact_name\"" + ", " + "\"street2\"" + ", " + "\"city\"" + ", " + "\"stage_id\"" + ", " + "\"name\"" + ", " + "\"last_telesale_note\"" + ", " + "\"status\"" + ", " + "\"unread_message_count\"" + ", " + "\"tag_ids\"" + ", " + "\"team_id\"" + ", " + "\"user_id\"" + ", " + "\"source_id\"" + ", " + "\"campaign_id\"" + ", " + "\"last_trucchat_note\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"probability\"" + ", " + "\"is_sale_manager\"" + ", " + "\"is_chat_manager\", " + "\"is_team_leader\", " + "\"create_uid\", " + "\"write_date\"" + "]";
            string body = @"{
                          " + "\n" + "\"id\"" + ": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""web_search_read"",
                          " + "\n" + @"        ""args"": [],
                          " + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""limit"": 80,
                          " + "\n" + @"            ""offset"": 0,
                          " + "\n" + "\"order\"" + ": " + "\"" + order + "\"" + ","
                            + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 51,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""bin_size"": true,
                          " + "\n" + "\"" + contextLastKey + "\"" + ": " + contextLastKeyValue
                            + "\n" + @"            },
                          " + "\n" + @"            ""count_limit"": 10001,
                          " + "\n" + "\"domain\"" + ": " + domain
                            + "\n" + "\"fields\"" + ": " + fields
                            + "\n" + @"        }
                          " + "\n" + @"    } " + "\n" +
                          @"}";
            #endregion

            #region Run Tests
            response = ConnextApi.WebSearchRead(urlSite, body, model, sessionID);
            return response;
            #endregion
        }

        private static IRestResponse DeleteWebSearchReadLead(string urlSite, string model, string recordIds, string sessionID)
        {
            #region Variables declare
            string args = "[[" + recordIds + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""unlink"",
                          " + "\n" + "\"args\": " + args + ", "
                            + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 51,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""default_type"": ""lead""
                          " + "\n" + @"            }
                          " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // delete search Lead
            response = ConnextApi.Delete(urlSite, model, body, sessionID);
            return response;
            #endregion
        }
    }
}
