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
    internal class ContactTest : BaseFunctionTest
    {
        #region Initiate variables
        internal static IRestResponse? response;
        internal static string? apiPath,
        model = "res.partner";
        internal static int id = 72;
        internal static int? recordId;
        internal static string contactName = "QA Test API Contact";
        internal static string phone = "02119899991";
        internal static string birthday = "2004-10-29";
        internal static string gender = "female";
        internal static int loaiNguonId = 5; // --> Online / Ads
        internal static int loaiNguonDetailId = 3; // --> Chiến dịch tháng 11
        internal static int loaiNguonDetailId2 = 7; // --> www.testads.com
        internal const int team_id = 1;
        internal const int uid = 18;
        internal static string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
        #endregion

        #region Head and Clean
        [SetUp]
        public void SetupData()
        {
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            // Clean up ...
        }
        #endregion

        #region TestMethod
        [Test, Category("BVTN - API Smoke Tests")]
        public void ST001_Post_Create_Contact()
        {
            #region before run: Get all search Contact and delete
            const string inputSearch = "qa test api Contact"; // ivf
            response = GetWebSearchRead(urlSite, model, inputSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            string[]? ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            string recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadContact(urlSite, model, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region Variables declare
            string body = body = @"{"
                                 + "\n" + @"    ""id"": 439,"
                                 + "\n" + @"    ""jsonrpc"": ""2.0"","
                                 + "\n" + @"    ""method"": ""call"","
                                 + "\n" + @"    ""params"": {"
                                 + "\n" + @"        ""args"": [{"
                                 + "\n" + @"                ""__last_update"": false,"
                                 + "\n" + @"                ""image_1920"": false,"
                                 + "\n" + @"                ""is_company"": false,"
                                 + "\n" + @"                ""active"": true,"
                                 + "\n" + @"                ""company_id"": false,"
                                 + "\n" + @"                ""company_type"": ""person"","
                                 + "\n" + @"                ""is_customer"": true,"
                                 + "\n" + @"                ""is_supplier"": false,"
                                 + "\n" +                   "\"name\"" + ": " + "\"" + contactName + "\"" + ","
                                 + "\n" + @"                ""parent_id"": false,"
                                 + "\n" + @"                ""company_name"": false,"
                                 + "\n" + @"                ""employee"": false,"
                                 + "\n" + @"                ""patient_id"": false,"
                                 + "\n" +                   "\"birthday\"" + ": " + "\"" + birthday + "\"" + ","
                                 + "\n" + @"                ""year_of_birth"": 2004,"
                                 + "\n" +                   "\"gender\"" + ": " + "\"" + gender + "\"" + ","
                                 + "\n" + @"                ""sub_department_id"": false,"
                                 + "\n" + @"                ""id_card_no"": false,"
                                 + "\n" + @"                ""service_unit_ids"": [[6, false, []]],"
                                 + "\n" + @"                ""type"": ""contact"","
                                 + "\n" + @"                ""street"": false,"
                                 + "\n" + @"                ""street2"": false,"
                                 + "\n" + @"                ""city"": false,"
                                 + "\n" + @"                ""state_id"": false,"
                                 + "\n" + @"                ""zip"": false,"
                                 + "\n" + @"                ""district_id"": false,"
                                 + "\n" + @"                ""ward_id"": false,"
                                 + "\n" + @"                ""country_id"": 241,"
                                 + "\n" + @"                ""vat"": false,"
                                 + "\n" + @"                ""vip_guest"": false,"
                                 + "\n" + @"                ""vip_code"": false,"
                                 + "\n" + @"                ""date_up_to_vip"": false,"
                                 + "\n" +                   "\"date_cancel_vip\"" + ": " + "\"" + dateNow + "\"" + ","
                                 + "\n" + @"                ""date_up_to_vip_first_time"": false,"
                                 + "\n" + @"                ""function"": false,"
                                 + "\n" +                   "\"phone\"" + ": " + "\"" + phone + "\"" + ","
                                 + "\n" + @"                ""mobile"": false,"
                                 + "\n" + @"                ""sdt_2"": false,"
                                 + "\n" + @"                ""sdt_3"": false,"
                                 + "\n" + @"                ""user_ids"": [],"
                                 + "\n" + @"                ""email"": false,"
                                 + "\n" + @"                ""website"": false,"
                                 + "\n" + @"                ""title"": false,"
                                 + "\n" + @"                ""lang"": ""vi_VN"","
                                 + "\n" + @"                ""category_id"": [[6, false, []]],"
                                 + "\n" + @"                ""sale_customer_service"": false,"
                                 + "\n" + @"                ""child_ids"": [],"
                                 + "\n" + @"                ""call_history_record_ids"": false,"
                                 + "\n" + @"                ""user_id"": false,"
                                 + "\n" + @"                ""team_id"": false,"
                                 + "\n" + @"                ""property_payment_term_id"": false,"
                                 + "\n" + @"                ""property_product_pricelist"": false,"
                                 + "\n" + @"                ""property_supplier_payment_term_id"": false,"
                                 + "\n" + @"                ""property_purchase_currency_id"": false,"
                                 + "\n" + @"                ""property_account_position_id"": false,"
                                 + "\n" + @"                ""company_registry"": false,"
                                 + "\n" + @"                ""ref"": false,"
                                 + "\n" + @"                ""industry_id"": false,"
                                 + "\n" + @"                ""bank_ids"": [],"
                                 + "\n" + @"                ""use_partner_credit_limit"": false,"
                                 + "\n" + @"                ""credit_limit"": 0,"
                                 + "\n" + @"                ""comment"": false,"
                                 + "\n" + @"                ""medical_examination_ids"": [],"
                                 + "\n" + @"                ""care_services_ids"": [],"
                                 + "\n" + @"                ""partner_relation_ids"": [],"
                                 + "\n" + @"                ""lang_custom"": ""vi_VN"","
                                 + "\n" + @"                ""employee_position"": false,"
                                 + "\n" + @"                ""employee_department"": false,"
                                 + "\n" +                   "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                                 + "\n" +                   "\"connext_source_type_detail_id\"" + ": " + loaiNguonDetailId + ","
                                 + "\n" +                   "\"connext_source_type_detail_id_2\"" + ": " + loaiNguonDetailId2 + ","
                                 + "\n" + @"                ""cbnv"": false,"
                                 + "\n" + @"                ""doi_tac"": false,"
                                 + "\n" + @"                ""kh_gioi_thieu"": false,"
                                 + "\n" + @"                ""zns_otp_ids"": [],"
                                 + "\n" + @"                ""personal_identification_document_ids"": []"
                                 + "\n" + @"            }"
                                 + "\n" + @"        ],"
                                 + "\n" + @"        ""model"": ""res.partner"","
                                 + "\n" + @"        ""method"": ""create"","
                                 + "\n" + @"        ""kwargs"": {"
                                 + "\n" + @"            ""context"": {"
                                 + "\n" + @"                ""lang"": ""vi_VN"","
                                 + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                                 + "\n" +                   "\"uid\"" + ": " + uid + ","
                                 + "\n" + @"                ""allowed_company_ids"": [1],"
                                 + "\n" + @"                ""is_customer_individuals_page"": true"
                                 + "\n" + @"            }"
                                 + "\n" + @"        }"
                                 + "\n" + @"    }"
                                 + "\n" + @"}";
            #endregion

            #region Run Tests
            // create Contact
            response = ConnextApi.Create(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            Assert.That((double?)responseJs["jsonrpc"], Is.GreaterThanOrEqualTo(2.0));
            Assert.That((int?)responseJs["id"], Is.GreaterThanOrEqualTo(10));
            Assert.That((int?)responseJs["result"], Is.GreaterThanOrEqualTo(100));
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST002_Get_WebSearchRead_Contact()
        {
            #region Variables declare
            string inputSearch = "qa test api Contact";
            int id = 23;
            string order = "";
            const string contextLastKey = "is_customer_individuals_page";
            const string contextLastKeyValue = "true";
            string domain = "[" + "\"&\"" + ", " + "\"&\"" + ", " + "[" + "\"is_customer\"" + ", " + "\"=\"" + ", " + "true], " + "[" + "\"is_company\"" + ", " + "\"=\"" + ", " + "false], " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", "
                          + "[" + "\"display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"ref\"" + ", " + "\"=\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"external_display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"name\"" + ", " + "\"phone\"" + ", " + "\"birthday_display\"" + ", " + "\"type_customer\"" + ", " + "\"vip_code\"" + ", " + "\"date_up_to_vip_first_time\"" + ", " + "\"date_up_to_vip\"" + ", " + "\"date_cancel_vip\"" + ", " + "\"display_name\"" + ", " + "\"unread_message_count\"" + ", " + "\"address\"" + ", " + "\"patient_id\"" + ", " + "\"recent_examination_date\"" + ", " + "\"days_recent_examination_date\"" + ", " + "\"recent_examination_service\"" + ", " + "\"booking_date\"" + ", " + "\"opportunity_latest_update\"" + ", " + "\"activity_ids\"" + ", " + "\"sale_customer_service\"" + "]";
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
            // get search contact
            response = ConnextApi.WebSearchRead(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));

            // Sort records in Js to verify the return value
            List<JToken>? recordsSort = responseJs["result"]["records"].OrderBy(o => o.SelectToken("id")).ToList();
            //Assert.That((int?)recordsSort[0]["id"], Is.EqualTo(12433));
            Assert.That((string?)recordsSort[0]["name"], Is.EqualTo(contactName));
            Assert.That((string?)recordsSort[0]["phone"], Is.EqualTo(phone));
            Assert.That((string?)recordsSort[0]["birthday_display"], Is.EqualTo("29/10/2004"));
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST003_Delete_WebSearchRead_Contact()
        {
            #region Get search Contact
            const string inputSearch = "qa test api Contact";
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
                          " + "\n" + @"                ""is_customer_individuals_page"": ""true""
                          " + "\n" + @"            }
                          " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // delete search Contact
            response = ConnextApi.Delete(urlSite, model, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            #endregion
        }
        #endregion

        #region Methods
        private static IRestResponse GetWebSearchRead(string urlSite, string model, string inputSearch, string sessionID)
        {
            #region Variables declare
            int id = 23;
            string order = "";
            const string contextLastKey = "is_customer_individuals_page";
            const string contextLastKeyValue = "true";
            string domain = "[" + "\"&\"" + ", " + "\"&\"" + ", " + "[" + "\"is_customer\"" + ", " + "\"=\"" + ", " + "true], " + "[" + "\"is_company\"" + ", " + "\"=\"" + ", " + "false], " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", "
                          + "[" + "\"display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"ref\"" + ", " + "\"=\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"external_display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"name\"" + ", " + "\"phone\"" + ", " + "\"birthday_display\"" + ", " + "\"type_customer\"" + ", " + "\"vip_code\"" + ", " + "\"date_up_to_vip_first_time\"" + ", " + "\"date_up_to_vip\"" + ", " + "\"date_cancel_vip\"" + ", " + "\"display_name\"" + ", " + "\"unread_message_count\"" + ", " + "\"address\"" + ", " + "\"patient_id\"" + ", " + "\"recent_examination_date\"" + ", " + "\"days_recent_examination_date\"" + ", " + "\"recent_examination_service\"" + ", " + "\"booking_date\"" + ", " + "\"opportunity_latest_update\"" + ", " + "\"activity_ids\"" + ", " + "\"sale_customer_service\"" + "]";
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

        private static IRestResponse DeleteWebSearchReadContact(string urlSite, string model, string recordIds, string sessionID)
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
                          " + "\n" + @"                ""is_customer_individuals_page"": ""true""
                          " + "\n" + @"            }
                          " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // delete search Contact
            response = ConnextApi.Delete(urlSite, model, body, sessionID);
            return response;
            #endregion
        }
        #endregion
    }
}
