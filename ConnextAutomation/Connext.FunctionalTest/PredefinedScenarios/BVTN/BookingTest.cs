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
    internal class BookingTest : BaseFunctionTest
    {
        #region Initiate variables
        internal static IRestResponse? response;
        internal static string? apiPath,
        model = "sale.order";
        internal const string modelLeadOpp = "crm.lead";
        internal const string modelContact = "res.partner";
        internal static int? recordId;
        internal const string opportunityName = "QA Test API Cơ hội";
        internal const string contactName = "QA Test API Contact";
        internal const string phone = "02119899991";
        internal const string birthday = "2004-10-29";
        internal const string gender = "female";
        internal static int loaiNguonId = 5; // --> Online / Ads
        internal static int loaiNguonDetailId = 3; // --> Chiến dịch tháng 11
        internal static int loaiNguonDetailId2 = 7; // --> www.testads.com
        internal const int team_id = 1;
        internal const int uid = 18;
        internal string date_order = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Ex: format 2024-10-30 03:00:17
        #endregion

        #region Head and Clean
        [SetUp]
        public void SetupData()
        {
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            // variables
            const string inputSearch = "qa test api";

            #region Delete all search Contact
            response = GetWebSearchReadContact(urlSite, modelContact, inputSearch + " Contact", sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            string[]? ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            string recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadContact(urlSite, modelContact, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region Delete all search Opportunity
            response = GetWebSearchReadLead(urlSite, inputSearch + " Cơ hội", sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadLead(urlSite, modelLeadOpp, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion
        }
        #endregion

        #region TestMethod
        [Test, Category("BVTN - API Smoke Tests")]
        public void ST001_Post_Create_Booking()
        {
            #region before run: Get all search Booking and delete
            const string inputSearch = "qa test api";
            const string fieldSearch = "name"; 
            var response = GetWebSearchRead(urlSite, model, inputSearch + " Contact", fieldSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            string[]? ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            string recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadBooking(urlSite, model, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region before run: Get all search Contact and delete
            response = GetWebSearchReadContact(urlSite, modelContact, inputSearch + " Contact", sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadContact(urlSite, modelContact, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region before run: Get all search Lead and delete
            response = GetWebSearchReadLead(urlSite, inputSearch + " Cơ hội", sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadLead(urlSite, modelLeadOpp, recordIds, sessionID);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
            #endregion

            #region Create Contact (Dialog Booking)
            response = PostCreateContactDialogOpp(urlSite, modelContact, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            var recordContactId = int.Parse(responseJs["result"].ToString());
            #endregion

            #region Create Opportunity (name create)
            response = PostCreateOpportunityName(urlSite, modelLeadOpp, recordContactId, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            var recordOppId = int.Parse(responseJs["result"].ToString());
            #endregion

            #region Variables declare (Booking)
            string body = @"{"
                          + "\n" + "\"id\"" + ": " + 105 + ","
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""args"": [{"
                          + "\n" + "\"partner_id\"" + ": " + recordContactId + "," // ex: 12601
                          + "\n" + "\"opportunity_id\"" + ": " + recordOppId + "," // ex: 23063
                          + "\n" + "\"date_order\"" + ": " + "\"" + date_order + "\"" + ","
                          + "\n" + "\"booking_type\"" + ": " + "\"" + "examination" + "\"" + ","
                          //+ "\n" + @"                ""embryo_top_name"": false,"
                          //+ "\n" + @"                ""embryo_date"": false,"
                          + "\n" + @"                ""validity_date"": false,"
                          + "\n" + @"                ""service_unit"": false,"
                          //+ "\n" + @"                ""product_ids"": [[6, false, []]],"
                          + "\n" + @"                ""booking_status"": false,"
                          + "\n" + @"                ""consulting_status"": false,"
                          + "\n" + @"                ""doctor_id"": false,"
                          + "\n" + @"                ""show_update_pricelist"": false,"
                          + "\n" + @"                ""pricelist_id"": 1,"
                          + "\n" + @"                ""company_id"": 1,"
                          + "\n" + @"                ""payment_term_id"": false,"
                          + "\n" + @"                ""sh_sale_ticket_ids"": [[6, false, []]],"
                          + "\n" + @"                ""description"": false,"
                          + "\n" + @"                ""order_line"": [],"
                          //+ "\n" + @"                ""medical_record_id"": false,"
                          + "\n" + @"                ""note"": ""<p>Điều khoản &amp; Điều kiện: <a href=\""https://qa-odoo.connext.biz/terms\"" target=\""_blank\"" rel=\""noreferrer noopener\"">https://qa-odoo.connext.biz/terms</a></p>"","
                          + "\n" + @"                ""sale_order_option_ids"": [],"
                          + "\n" +                   "\"user_id\"" + ": " + uid + ","
                          + "\n" +                   "\"team_id\"" + ": " + team_id + ","
                          + "\n" + @"                ""client_order_ref"": false,"
                          + "\n" + @"                ""tag_ids"": [[6, false, []]],"
                          + "\n" + @"                ""show_update_fpos"": false,"
                          + "\n" + @"                ""fiscal_position_id"": false,"
                          + "\n" +                   "\"partner_invoice_id\"" + ": " + recordContactId + "," // ex: 12601
                          + "\n" + @"                ""project_id"": false,"
                          + "\n" +                   "\"warehouse_id\"" + ": " + 1 + ","
                          + "\n" + @"                ""picking_policy"": ""direct"","
                          + "\n" + @"                ""commitment_date"": false,"
                          + "\n" + @"                ""origin"": false,"
                          + "\n" + @"                ""campaign_id"": false,"
                          + "\n" + @"                ""medium_id"": false,"
                          + "\n" + @"                ""source_id"": false,"
                          + "\n" +                   "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                          + "\n" +                   "\"connext_source_type_detail_id\"" + ": " + loaiNguonDetailId + ","
                          + "\n" +                   "\"connext_source_type_detail_id_2\"" + ": " + loaiNguonDetailId2 + ","
                          + "\n" + @"                ""cbnv"": false,"
                          + "\n" + @"                ""refer_by"": false,"
                          + "\n" + @"                ""kh_gioi_thieu"": false,"
                          + "\n" + @"                ""is_addnew_treament_card"": false,"
                          + "\n" + @"                ""treatment_card_id"": false,"
                          + "\n" + @"                ""treatment_card_code"": false,"
                          + "\n" + @"                ""treatment_card_date"": false,"
                          + "\n" + @"                ""treatment_id"": false,"
                          + "\n" + @"                ""treatment_card_qty"": 1,"
                          + "\n" + @"                ""treatment_qty"": 0,"
                          + "\n" + @"                ""medical_examination_ids"": []"
                          + "\n" + @"            }"
                          + "\n" + @"        ],"
                          + "\n" + @"        ""model"": ""sale.order"","
                          + "\n" + @"        ""method"": ""create"","
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" +                   "\"uid\"" + ": " + uid + ","
                          + "\n" + @"                ""allowed_company_ids"": [1]"
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    } "
                          + "\n" + @"}";
            #endregion

            #region Run Tests
            // create Booking
            response = ConnextApi.Create(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            recordId = int.Parse(responseJs["result"].ToString());
            Assert.That(responseJs.Count, Is.EqualTo(3));
            Assert.That((double?)responseJs["jsonrpc"], Is.GreaterThanOrEqualTo(2.0));
            Assert.That((int?)responseJs["id"], Is.GreaterThanOrEqualTo(10));
            Assert.That((int?)responseJs["result"], Is.GreaterThanOrEqualTo(100));
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST002_Get_WebSearchRead_Booking()
        {
            #region Variables declare
            const string inputSearch = "qa test api Contact";
            const string fieldSearch = "name";
            int id = 124;
            string order = "date_order DESC";
            string domain = "[" + "\"|\"" + ", " +"\"|\"" + ", " + "[" + "\"" + fieldSearch + "\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"client_order_ref\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"partner_id\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"date_order\"" + ", " + "\"booking_type\"" + ", " + "\"partner_id\"" + ", " + "\"name\"" + ", "
                          + "\"actual_examination_date\"" + ", " + "\"create_date\"" + ", " + "\"booking_time_start\"" + ", " + "\"booking_time_end\"" + ", " + "\"commitment_date\"" + ", " + "\"expected_date\"" + ", " + "\"user_id\"" + ", " + "\"doctor_id\"" + ", " + "\"booking_status\"" + ", " + "\"team_id\"" + ", " + "\"tag_ids\"" + ", " + "\"company_id\"" + ", " + "\"amount_untaxed\"" + ", " + "\"amount_tax\"" + ", " + "\"amount_total\"" + ", " + "\"state\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"invoice_status\"" + ", " + "\"message_needaction\"" + ", " + "\"currency_id\"" + ", " + "\"service_unit\"" + "]";
            string body = @"{
                          " + "\n" +       "\"id\"" + ": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" +           "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""web_search_read"",
                          " + "\n" + @"        ""args"": [],
                          " + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""limit"": 80,
                          " + "\n" + @"            ""offset"": 0,
                          " + "\n" +               "\"order\"" + ": " + "\"" + order + "\"" + ","
                            + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                            + "\n" +                   "\"uid\"" + ": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" +                   "\"bin_size\"" + ": " + "true"
                            + "\n" + @"            },
                          " + "\n" + @"            ""count_limit"": 10001,
                          " + "\n" +               "\"domain\"" + ": " + domain
                            + "\n" +               "\"fields\"" + ": " + fields
                            + "\n" + @"        }
                          " + "\n" + @"    } " + "\n" +
                          @"}";
            #endregion

            #region Run Tests
            // get search Booking
            response = ConnextApi.WebSearchRead(urlSite, body, model, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            recordId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());

            // Sort records in Js to verify the return value
            List<JToken>? recordsSort = responseJs["result"]["records"].OrderBy(o => o.SelectToken("id")).ToList();
            Assert.That((int?)recordsSort[0]["id"], Is.EqualTo(recordId));
            Assert.That((string?)recordsSort[0]["create_date"], Does.Contain(DateTime.Now.ToString("yyyy-MM-dd")));
            Assert.That((string?)recordsSort[0]["date_order"], Does.Contain(DateTime.Now.ToString("yyyy-MM-dd")));
            Assert.That(recordsSort[0]["partner_id"].Any(jt => jt.Value<string>().Contains(contactName + " - " + phone + " - " + birthday.Replace("2004-10-29", "29/10/2004"))), Is.True);
            #endregion
        }

        [Test, Category("BVTN - API Smoke Tests")]
        public void ST003_Delete_WebSearchRead_Booking()
        {
            #region Get search Booking
            const string inputSearch = "qa test api Contact";
            const string fieldSearch = "name";
            response = GetWebSearchRead(urlSite, model, inputSearch, fieldSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            recordId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Variables declare (Booking)
            const int id = 125;
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
                          " + "\n" + "\"uid\"" + ": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1]
                          " + "\n" + @"            }
                          " + "\n" + @"        }
                          " + "\n" + @"    }
                          " + "\n" + @"}";
            #endregion

            #region Run Tests
            // delete search Booking
            response = ConnextApi.Delete(urlSite, model, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }
        #endregion

        #region Methods
        private static IRestResponse PostCreateContactDialogOpp(string urlSite, string model, string sessionID)
        {
            #region Variables declare
            string body = @"{"
                          + "\n" + "\"id\"" + ": " + 161 + ","
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
                          + "\n" + "\"name\"" + ": " + "\"" + contactName + "\"" + ","
                          + "\n" + @"                ""parent_id"": false,"
                          + "\n" + @"                ""company_name"": false,"
                          + "\n" + @"                ""employee"": false,"
                          + "\n" + @"                ""patient_id"": false,"
                          + "\n" + "\"birthday\"" + ": " + "\"" + birthday + "\"" + ","
                          + "\n" + "\"year_of_birth\"" + ": " + birthday.Replace("2004-10-29", "2004") + ","
                          + "\n" + "\"gender\"" + ": " + "\"" + gender + "\"" + ","
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
                          + "\n" + @"                ""date_cancel_vip"": ""2025-02-08"","
                          + "\n" + @"                ""date_up_to_vip_first_time"": false,"
                          + "\n" + @"                ""function"": false,"
                          + "\n" + "\"phone\"" + ": " + "\"" + phone + "\"" + ","
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
                          + "\n" + @"                ""call_history_record_ids"": [],"
                          + "\n" + "\"user_id\"" + ": " + uid + ","
                          + "\n" + "\"team_id\"" + ": " + team_id + ","
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
                          + "\n" + "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                          + "\n" + "\"connext_source_type_detail_id\"" + ": " + loaiNguonDetailId + ","
                          + "\n" + "\"connext_source_type_detail_id_2\"" + ": " + loaiNguonDetailId2 + ","
                          + "\n" + @"                ""cbnv"": false,"
                          + "\n" + @"                ""doi_tac"": false,"
                          + "\n" + @"                ""kh_gioi_thieu"": false,"
                          + "\n" + @"                ""zns_otp_ids"": [],"
                          + "\n" + @"                ""personal_identification_document_ids"": []"
                          + "\n" + @"            }"
                          + "\n" + @"        ],"
                          + "\n" + "\"model\"" + ": " + "\"" + model + "\"" + ","
                          + "\n" + @"        ""method"": ""create"","
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" + "\"uid\"" + ": " + uid + ","
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""res_partner_search_mode"": ""customer"","
                          + "\n" + "\"default_name\"" + ": " + "\"" + contactName + "\"" + ","
                          + "\n" + @"                ""default_street"": false,"
                          + "\n" + @"                ""default_is_company"": false,"
                          + "\n" + @"                ""default_company_name"": false,"
                          + "\n" + @"                ""default_street2"": false,"
                          + "\n" + @"                ""default_city"": false,"
                          + "\n" + @"                ""default_title"": false,"
                          + "\n" + @"                ""default_state_id"": false,"
                          + "\n" + @"                ""default_zip"": false,"
                          + "\n" + @"                ""default_country_id"": 241,"
                          + "\n" + @"                ""default_function"": false,"
                          + "\n" + "\"default_phone\"" + ": " + "\"" + phone + "\"" + ","
                          + "\n" + @"                ""default_mobile"": false,"
                          + "\n" + @"                ""default_email"": false,"
                          + "\n" + "\"default_user_id\"" + ": " + uid + ","
                          + "\n" + "\"default_team_id\"" + ": " + team_id + ","
                          + "\n" + @"                ""default_website"": false,"
                          + "\n" + @"                ""default_lang"": false,"
                          + "\n" + @"                ""show_vat"": true"
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" + @"}";
            #endregion

            #region Run Tests
            response = ConnextApi.Create(urlSite, body, model, sessionID);
            return response;
            #endregion
        }

        private static IRestResponse PostCreateOpportunityName(string urlSite, string model, int recordContactId, string sessionID)
        {
            #region Variables declare
            string body = @"{"
                          + "\n" +       "\"id\"" + ": " + 182 + ","
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""args"": [{"
                          + "\n" + @"                ""subtype"": false,"
                          + "\n" +                   "\"stage_id\"" + ": " + 16 + ","
                          + "\n" + @"                ""active"": true,"
                          + "\n" + @"                ""company_id"": 1,"
                          + "\n" +                   "\"name\"" + ": " + "\"" + opportunityName + "\"" + ","
                          + "\n" + @"                ""contract_code"": false,"
                          + "\n" + @"                ""expected_revenue"": 0,"
                          + "\n" + @"                ""automated_probability"": 99.7,"
                          + "\n" + @"                ""probability"": 0,"
                          + "\n" +                   "\"partner_id\"" + ": " + recordContactId + "," // ex: 9375
                          + "\n" + @"                ""partner_name"": false,"
                          + "\n" +                   "\"contact_name\"" + ": " + "\"" + contactName + "\"" + ","
                          + "\n" + @"                ""birthday"": false,"
                          + "\n" + @"                ""year_of_birth"": 0,"
                          + "\n" +                   "\"phone\"" + ": " + "\"" + phone + "\"" + ","
                          + "\n" + @"                ""sdt_2"": false,"
                          + "\n" + @"                ""sdt_3"": false,"
                          + "\n" + @"                ""email_from"": false,"
                          + "\n" + @"                ""website"": false,"
                          + "\n" + @"                ""street"": false,"
                          + "\n" + @"                ""state_id"": false,"
                          + "\n" + @"                ""district_id"": false,"
                          + "\n" + @"                ""ward_id"": false,"
                          + "\n" + @"                ""street2"": false,"
                          + "\n" + @"                ""city"": false,"
                          + "\n" + @"                ""zip"": false,"
                          + "\n" + @"                ""country_id"": 241,"
                          + "\n" + @"                ""patient_type"": false,"
                          + "\n" + @"                ""lang_id"": 86,"
                          + "\n" + @"                ""medical_examination_day"": false,"
                          + "\n" + @"                ""status"": false,"
                          + "\n" + @"                ""lost_reason_id"": false,"
                          + "\n" + @"                ""service_unit_id"": 2,"
                          + "\n" + @"                ""tag_ids"": [[6, false, []]],"
                          + "\n" + @"                ""type"": ""opportunity"","
                          + "\n" + @"                ""net_revenue"": 0,"
                          + "\n" + @"                ""date_deadline"": false,"
                          + "\n" + @"                ""priority"": ""0"","
                          + "\n" +                   "\"user_id\"" + ": " + uid + ","
                          + "\n" +                   "\"team_id\"" + ": " + "false" + "," // team_id "false"
                          + "\n" + @"                ""contract_id"": false,"
                          + "\n" + @"                ""medical_examination_date_closed"": false,"
                          + "\n" + @"                ""medical_examination_due_date"": false,"
                          + "\n" + @"                ""start_date_get_sample"": false,"
                          + "\n" + @"                ""end_date_get_sample"": false,"
                          + "\n" + @"                ""place_of_registration"": ""noivien"","
                          + "\n" + @"                ""package_examination"": false,"
                          + "\n" + @"                ""lead_properties"": [],"
                          + "\n" + @"                ""description"": false,"
                          + "\n" + @"                ""campaign_id"": false,"
                          + "\n" + @"                ""medium_id"": false,"
                          + "\n" + @"                ""source_id"": false,"
                          + "\n" + @"                ""refer_by"": false,"
                          + "\n" + @"                ""referred"": false,"
                          + "\n" +                   "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                          + "\n" +                   "\"connext_source_type_detail_id\"" + ": " + loaiNguonDetailId + ","
                          + "\n" +                   "\"connext_source_type_detail_id_2\"" + ": " + loaiNguonDetailId2 + ","
                          + "\n" + @"                ""cbnv"": false,"
                          + "\n" + @"                ""kh_gioi_thieu"": false,"
                          + "\n" + @"                ""title"": false,"
                          + "\n" + @"                ""function"": false,"
                          + "\n" + @"                ""mobile"": false,"
                          + "\n" + @"                ""medical_examination_ids"": []"
                          + "\n" + @"            }"
                          + "\n" + @"        ],"
                          + "\n" +           "\"model\"" + ": " + "\"" + model + "\"" + ","
                          + "\n" + @"        ""method"": ""create"","
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" +                   "\"uid\"" + ": " + uid + ","
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""default_type"": ""opportunity"","
                          + "\n" +                   "\"default_name\"" + ": " + "\"" + opportunityName + "\""
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" + @"}";
            #endregion

            #region Run Tests
            response = ConnextApi.Create(urlSite, body, model, sessionID);
            return response;
            #endregion
        }

        private static IRestResponse GetWebSearchRead(string urlSite, string model, string inputSearch, string fieldSearch, string sessionID)
        {
            #region Variables declare
            int id = 124;
            const string order = "date_order DESC";
            string domain = "[" + "\"|\"" + ", " + "\"|\"" + ", " + "[" + "\"" + fieldSearch + "\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"client_order_ref\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"partner_id\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"date_order\"" + ", " + "\"booking_type\"" + ", " + "\"partner_id\"" + ", " + "\"name\"" + ", "
                          + "\"actual_examination_date\"" + ", " + "\"create_date\"" + ", " + "\"booking_time_start\"" + ", " + "\"booking_time_end\"" + ", " + "\"commitment_date\"" + ", " + "\"expected_date\"" + ", " + "\"user_id\"" + ", " + "\"doctor_id\"" + ", " + "\"booking_status\"" + ", " + "\"team_id\"" + ", " + "\"tag_ids\"" + ", " + "\"company_id\"" + ", " + "\"amount_untaxed\"" + ", " + "\"amount_tax\"" + ", " + "\"amount_total\"" + ", " + "\"state\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"invoice_status\"" + ", " + "\"message_needaction\"" + ", " + "\"currency_id\"" + ", " + "\"service_unit\"" + "]";
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
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                            + "\n" + "\"uid\"" + ": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + "\"bin_size\"" + ": " + "true"
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

        private static IRestResponse GetWebSearchReadContact(string urlSite, string model, string inputSearch, string sessionID)
        {
            #region Variables declare
            int id = 379;
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
            const int id = 389;
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
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                            + "\n" + "\"uid\": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1],
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

        private static IRestResponse GetWebSearchReadLead(string urlSite, string inputSearch, string sessionID)
        {
            #region Variables declare
            int id = 354;
            string order = "";
            const string unknown1 = "default_type";
            const string unknown1Value = "\"opportunity\"" + ",";
            const string unknown2 = "default_team_id";
            const int unknown2Value = team_id;
            string domain = "[" + "\"&\"" + ", " + "\"&\"" + ", " + "\"&\"" + ", " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "\"opportunity\"" + "], " + "[" + "\"subtype\"" + ", " + "\"!=\"" + ", " + "\"contract\"" + "], " + "[" + "\"subtype\"" + ", " + "\"!=\"" + ", " + "\"examination\"" + "], " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", "
                          + "[" + "\"partner_id\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"partner_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email_from\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"contact_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"date_open\"" + ", " + "\"stage_id\"" + ", " + "\"name\"" + ", " + "\"contact_name\"" + ", "
                          + "\"status\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"is_sale_manager\"" + ", " + "\"is_chat_manager\"" + ", " + "\"is_team_leader\"" + ", " + "\"user_id\"" + ", " + "\"team_id\"" + ", " + "\"expected_revenue\"" + ", " + "\"write_date\"" + ", " + "\"campaign_id\"" + ", " + "\"source_id\"" + ", " + "\"activity_calendar_event_id\"" + "]";
            string body = @"{
                          " + "\n" + "\"id\"" + ": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + modelLeadOpp + "\"" + ","
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
                          " + "\n" + "\"" + unknown1 + "\"" + ": " + unknown1Value
                            + "\n" + "\"" + unknown2 + "\"" + ": " + unknown2Value
                            + "\n" + @"            },
                          " + "\n" + @"            ""count_limit"": 10001,
                          " + "\n" + "\"domain\"" + ": " + domain
                            + "\n" + "\"fields\"" + ": " + fields
                            + "\n" + @"        }
                          " + "\n" + @"    } " + "\n" +
                          @"}";
            #endregion

            #region Run tests
            response = ConnextApi.WebSearchRead(urlSite, body, modelLeadOpp, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse DeleteWebSearchReadLead(string urlSite, string model, string recordIds, string sessionID)
        {
            #region Variables declare
            const int id = 358;
            string args = "[[" + recordIds + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" +           "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""unlink"",
                          " + "\n" +           "\"args\": " + args + ", "
                            + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" +                   "\"uid\"" + ": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" + @"                ""default_type"": ""opportunity"",
                          " + "\n" +                   "\"default_team_id\"" + ": " + team_id
                            + "\n" + @"            }
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

        private static IRestResponse DeleteWebSearchReadBooking(string urlSite, string model, string recordIds, string sessionID)
        {
            #region Variables declare
            const int id = 125;
            string args = "[[" + recordIds + "]]";
            string body = @"{
                          " + "\n" +       "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" +           "\"model\"" + ": " + "\"" + model + "\"" + ","
                            + "\n" + @"        ""method"": ""unlink"",
                          " + "\n" +           "\"args\": " + args + ", "
                            + "\n" + @"        ""kwargs"": {
                          " + "\n" + @"            ""context"": {
                          " + "\n" + @"                ""lang"": ""vi_VN"",
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" +                   "\"uid\"" + ": " + uid + ","
                            + "\n" + @"                ""allowed_company_ids"": [1]
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
