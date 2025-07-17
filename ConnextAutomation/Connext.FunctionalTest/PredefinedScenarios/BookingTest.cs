using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.PredefinedScenarios
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
        internal const int loaiNguonId = 16; // 58 = Offline / Giới thiệu / CBNV - BVĐK HN; 36 = Offline / Giới thiệu / C2C ; 16 = Offline / Tự đến
        internal const int cbnv = 130; // 130 = (bthuy)
        internal const int team_id = 18;
        internal const int uid = 51;
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
        [Test, Category("DKHN - API Smoke Tests")]
        public void ST001_Post_Create_Booking()
        {
            #region before run: Get all search Booking and delete
            const string inputSearch = "qa test api"; // ivf
            const string fieldSearch = "wife_infor"; // IVF - wife_infor / wife_phone / husband_infor / husband_phone
            response = GetWebSearchRead(urlSite, model, inputSearch + " Contact", fieldSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            string[]? ids = responseJs.Children<JProperty>().Descendants().OfType<JProperty>().Where(a => a.Name == "id").Select(a => a.Value.ToString()).ToArray();
            string recordIds = string.Join(", ", ids); // put all ids in one line
            if (recordIds != null && recordIds != "")
            {
                response = DeleteWebSearchReadOpportunity(urlSite, model, recordIds, sessionID);
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
            response = PostCreateOpportunityName(urlSite, modelLeadOpp, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            var recordOppId = int.Parse(responseJs["result"][0].ToString());
            #endregion

            #region Variables declare (Booking)
            string body = @"{"
                          + "\n" + "\"id\"" + ": " + 60 + ","
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""args"": [{"
                          + "\n" + "\"partner_id\"" + ": " + recordContactId + "," // ex: 12601
                          + "\n" + "\"opportunity_id\"" + ": " + recordOppId + "," // ex: 23063
                          + "\n" + "\"date_order\"" + ": " + "\"" + date_order + "\"" + ","
                          + "\n" + "\"booking_type\"" + ": " + "\"" + "examination" + "\"" + ","
                          + "\n" + @"                ""embryo_top_name"": false,"
                          + "\n" + @"                ""embryo_date"": false,"
                          + "\n" + @"                ""validity_date"": false,"
                          + "\n" + @"                ""product_ids"": [[6, false, []]],"
                          + "\n" + @"                ""booking_status"": false,"
                          + "\n" + @"                ""doctor_id"": false,"
                          + "\n" + @"                ""show_update_pricelist"": false,"
                          + "\n" + @"                ""pricelist_id"": 1,"
                          + "\n" + @"                ""company_id"": 1,"
                          + "\n" + @"                ""payment_term_id"": false,"
                          + "\n" + @"                ""sh_sale_ticket_ids"": [[6, false, []]],"
                          + "\n" + @"                ""description"": false,"
                          + "\n" + @"                ""order_line"": [],"
                          + "\n" + @"                ""medical_record_id"": false,"
                          + "\n" + @"                ""note"": ""<p>Điều khoản &amp; Điều kiện: <a href=\""https://qa-odoo.connext.biz/terms\"" target=\""_blank\"" rel=\""noreferrer noopener\"">https://qa-odoo.connext.biz/terms</a></p>"","
                          + "\n" + @"                ""sale_order_option_ids"": [],"
                          + "\n" + @"                ""client_order_ref"": false,"
                          + "\n" + @"                ""tag_ids"": [[6, false, []]],"
                          + "\n" + @"                ""show_update_fpos"": false,"
                          + "\n" + @"                ""fiscal_position_id"": false,"
                          + "\n" +                   "\"partner_invoice_id\"" + ": " + recordContactId + "," // ex: 12601
                          + "\n" +                   "\"warehouse_id\"" + ": " + 1 + ","
                          + "\n" + @"                ""picking_policy"": ""direct"","
                          + "\n" + @"                ""commitment_date"": false,"
                          + "\n" + @"                ""origin"": false,"
                          + "\n" + @"                ""campaign_id"": false,"
                          + "\n" + @"                ""medium_id"": false,"
                          + "\n" + @"                ""source_id"": false,"
                          + "\n" +                   "\"connext_source_type_id\"" + ": " + loaiNguonId + ","
                          + "\n" + @"                ""connext_source_type_detail_id"": false,"
                          + "\n" + @"                ""connext_source_type_detail_id_2"": false,"
                          + "\n" +                   "\"cbnv\"" + ": " + "false" + "," // = false if loaiNguonId = 16 (Offline / Tự đến)
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
                          + "\n" + @"                ""uid"": 51,"
                          + "\n" + @"                ""allowed_company_ids"": [1]"
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    } " 
                          + "\n" +@"}";
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
            Assert.That((int?)responseJs["result"], Is.GreaterThanOrEqualTo(7000));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST002_Get_WebSearchRead_Booking()
        {
            #region Variables declare
            const string inputSearch = "qa test api Contact"; // IVF
            const string fieldSearch = "wife_infor"; // IVF - wife_infor / wife_phone / husband_infor / husband_phone
            int id = 20;
            string order = "date_order DESC";
            string domain = "[[" + "\"" + fieldSearch + "\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"state\"" + ", " + "\"create_date\"" + ", " + "\"date_order\"" + ", " + "\"connext_source_type_id\"" + ", " + "\"wife_infor\"" + ", " + "\"wife_phone\"" + ", " + "\"husband_infor\"" + ", " + "\"husband_phone\"" + ", " + "\"description\"" + ", " + "\"booking_status\"" + ", " + "\"last_consulting_status\"" + ", " + "\"team_id\"" + ", "
                            + "\"create_team_id\"" + ", " + "\"telesale_employee\"" + ", " + "\"consulting_employee\"" + ", " + "\"doctor_id\"" + ", " + "\"source_booking\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + "]";
            string body = @"{
                          " + "\n" + "\"id\"" + ": " + id + ","
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
                          " + "\n" + @"                ""tz"": ""Asia/Bangkok"",
                          " + "\n" + @"                ""uid"": 51,
                          " + "\n" + @"                ""allowed_company_ids"": [1],
                          " + "\n" +                   "\"bin_size\"" + ": " + "true"
                            + "\n" + @"            },
                          " + "\n" + @"            ""count_limit"": 10001,
                          " + "\n" + "\"domain\"" + ": " + domain
                            + "\n" + "\"fields\"" + ": " + fields
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
            //Assert.That(recordsSort[0]["connext_source_type_id"].Any(jt => jt.Value<int>().Equals(62)), Is.True); // ticket 812 loai nguon = 55 (Chưa xác định); old: loaiNguonId (16)
            Assert.That(recordsSort[0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Chưa xác định")), Is.True); // ticket 812 loai nguon ='Chưa xác định'; old: Offline / Tự đến
            //Assert.That(recordsSort[0]["connext_source_type_id"].Any(jt => jt.Value<int>().Equals(loaiNguonId)), Is.True); // ticket 812 loai nguon = 55 (Chưa xác định); old: loaiNguonId (16)
            //Assert.That(recordsSort[0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tự đến")), Is.True); // ticket 812 loai nguon ='Chưa xác định'; old: Offline / Tự đến
            Assert.That((string?)recordsSort[0]["wife_infor"], Is.EqualTo(contactName + " - " + birthday.Replace("2004-10-29", "29/10/2004")));
            Assert.That((string?)recordsSort[0]["wife_phone"], Is.EqualTo(phone).Or.EqualTo(phone.Replace("19", "1 9").Replace("991", " 991")));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST003_Delete_WebSearchRead_Booking()
        {
            #region Get search Booking
            const string inputSearch = "qa test api Contact"; // IVF
            const string fieldSearch = "wife_infor"; // IVF - wife_infor / wife_phone / husband_infor / husband_phone
            response = GetWebSearchRead(urlSite, model, inputSearch, fieldSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject responseJs = JObject.Parse(response.Content);
            recordId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Variables declare (Booking)
            const int id = 243;
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" +       "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" +       "\"model\"" + ": " + "\"" + model + "\"" + ","
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
                          + "\n" + "\"id\"" + ": " + 44 + ","
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""args"": [{"
                          + "\n" + @"                ""__last_update"": false,"
                          + "\n" + @"                ""partner_gid"": 0,"
                          + "\n" + @"                ""additional_info"": false,"
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
                          + "\n" + @"                ""supplier_id"": false,"
                          + "\n" + @"                ""patient_id"": false,"
                          + "\n" + "\"birthday\"" + ": " + "\"" + birthday + "\"" + ","
                          + "\n" + "\"year_of_birth\"" + ": " + birthday.Replace("2004-10-29", "2004") + ","
                          + "\n" + "\"gender\"" + ": " + "\"" + gender + "\"" + ","
                          + "\n" + @"                ""sub_department_id"": false,"
                          + "\n" + @"                ""sub_department_ids"": [[6, false, []]],"
                          + "\n" + @"                ""id_card_no"": false,"
                          + "\n" + @"                ""practising_certificate_number"": false,"
                          + "\n" + @"                ""degree"": false,"
                          + "\n" + @"                ""service_unit_ids"": [[6, false, []]],"
                          + "\n" + @"                ""type"": ""contact"","
                          + "\n" + @"                ""street"": false,"
                          + "\n" + @"                ""street2"": false,"
                          + "\n" + @"                ""city"": false,"
                          + "\n" + @"                ""zip"": false,"
                          + "\n" + @"                ""state_id"": false,"
                          + "\n" + @"                ""district_id"": false,"
                          + "\n" + @"                ""ward_id"": false,"
                          + "\n" + @"                ""country_id"": 241,"
                          + "\n" + @"                ""vat"": false,"
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
                          + "\n" + @"                ""contract_id"": false,"
                          + "\n" + @"                ""account_name"": false,"
                          + "\n" + @"                ""account_number"": false,"
                          + "\n" + @"                ""bank_name"": false,"
                          + "\n" + @"                ""child_ids"": [],"
                          + "\n" + "\"user_id\"" + ": " + "false" + "," //  + uid +
                          + "\n" + "\"team_id\"" + ": " + "false" + "," // + team_id +
                          + "\n" + @"                ""property_payment_term_id"": false,"
                          + "\n" + @"                ""property_product_pricelist"": false,"
                          + "\n" + @"                ""property_supplier_payment_term_id"": false,"
                          + "\n" + @"                ""receipt_reminder_email"": false,"
                          + "\n" + @"                ""reminder_date_before_receipt"": 1,"
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
                          + "\n" + @"                ""connext_source_type_detail_id"": false,"
                          + "\n" + @"                ""connext_source_type_detail_id_2"": false,"
                          + "\n" + @"                ""cbnv"": false,"
                          + "\n" + @"                ""doi_tac"": false,"
                          + "\n" + @"                ""kh_gioi_thieu"": false,"
                          + "\n" + @"                ""zns_otp_ids"": [],"
                          + "\n" + @"                ""call_history_record_ids"": [],"
                          + "\n" + @"                ""partner_latitude"": 0,"
                          + "\n" + @"                ""partner_longitude"": 0,"
                          + "\n" + @"                ""personal_identification_document_ids"": []"
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
                          + "\n" + @"                ""show_phone"": false,"
                          + "\n" + @"                ""show_birthday"": false,"
                          + "\n" +                   "\"default_name\"" + ": " + "\"" + contactName + "\""
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

        private static IRestResponse PostCreateOpportunityName(string urlSite, string model, string sessionID)
        {
            #region Variables declare
            string body = @"{"
                          + "\n" +       "\"id\"" + ": " + 56 + ","
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" +           "\"args\"" + ": " + "[" + "\"" + opportunityName + "\"" + "]" + ","
                          + "\n" +           "\"model\"" + ": " + "\"" + model + "\"" + ","
                          + "\n" + @"        ""method"": ""name_create"","
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" +                   "\"uid\"" + ": " + uid + ","
                          + "\n" + @"                ""allowed_company_ids"": [1]"
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
            int id = 20;
            const string order = "date_order DESC";
            string domain = "[[" + "\"" + fieldSearch + "\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"state\"" + ", " + "\"create_date\"" + ", " + "\"date_order\"" + ", " + "\"connext_source_type_id\"" + ", " + "\"wife_infor\"" + ", " + "\"wife_phone\"" + ", " + "\"husband_infor\"" + ", " + "\"husband_phone\"" + ", " + "\"description\"" + ", " + "\"booking_status\"" + ", " + "\"last_consulting_status\"" + ", " + "\"team_id\"" + ", "
                            + "\"create_team_id\"" + ", " + "\"telesale_employee\"" + ", " + "\"consulting_employee\"" + ", " + "\"doctor_id\"" + ", " + "\"source_booking\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + "]";
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

        private static IRestResponse GetWebSearchReadLead(string urlSite, string inputSearch, string sessionID)
        {
            #region Variables declare
            const int id = 205;
            string order = "create_date DESC";
            const string contextLastKey = "default_type";
            const string contextLastKeyValue = "\"" + "lead" + "\"";
            string domain = "[" + "\"&\"" + ", " + "\"|\"" + ", " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "\"lead\"" + "], " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "false" + "], " + "\"|\", " + "\"|\", " + "\"|\", " + "\"|\", " + "[" + "\"partner_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email_from\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"contact_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone_mobile_search\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"create_date\"" + ", " + "\"date_open\"" + ", " + "\"contact_name\"" + ", " + "\"street2\"" + ", " + "\"city\"" + ", " + "\"phone\"" + ", " + "\"stage_id\"" + ", " + "\"name\"" + ", " + "\"last_telesale_note\"" + ", " + "\"status\"" + ", " + "\"tag_ids\"" + ", " + "\"team_id\"" + ", " + "\"user_id\"" + ", " + "\"connext_source_type_id\"" + ", " + "\"product_ids\"" + ", " + "\"connext_source_type_detail_id\"" + ", " + "\"last_trucchat_note\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\", " + "\"probability\", " + "\"is_sale_manager\", " + "\"is_chat_manager\", " + "\"is_team_leader\", " + "\"create_uid\", " + "\"mkt_chat\", " + "\"website\", " + "\"write_date\"" + "]";
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
                          " + "\n" + "\"" + contextLastKey + "\"" + ": " + contextLastKeyValue
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

        private static IRestResponse GetWebSearchReadOpp(string urlSite, string inputSearch, string sessionID)
        {
            #region Variables declare
            int id = 33;
            string order = "";
            const string unknown1 = "default_type";
            const string unknown1Value = "\"opportunity\"" + ",";
            const string unknown2 = "default_team_id";
            const string unknown2Value = "18";
            string domain = "[" + "\"&\"" + ", " + "\"&\"" + ", " + "\"&\"" + ", " + "[" + "\"type\"" + ", " + "\"=\"" + ", " + "\"opportunity\"" + "], " + "[" + "\"subtype\"" + ", " + "\"!=\"" + ", " + "\"contract\"" + "], " + "[" + "\"subtype\"" + ", " + "\"!=\"" + ", " + "\"examination\"" + "], " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", "
                            + "[" + "\"partner_id\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"partner_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email_from\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"contact_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone_mobile_search\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"create_date\"" + ", " + "\"date_open\"" + ", " + "\"stage_id\"" + ", " + "\"stage_color\"" + ", " + "\"name\"" + ", " + "\"last_telesale_note\"" + ", " + "\"tag_ids\"" + ", " + "\"patient_id\"" + ", " + "\"contact_name\"" + ", " + "\"phone\"" + ", " + "\"street2\"" + ", " + "\"city\"" + ", "
                            + "\"status\"" + ", " + "\"activity_ids\"" + ", " + "\"my_activity_date_deadline\"" + ", " + "\"is_sale_manager\"" + ", " + "\"is_chat_manager\"" + ", " + "\"is_team_leader\"" + ", " + "\"user_id\"" + ", " + "\"team_id\"" + ", " + "\"connext_source_type_id\"" + ", " + "\"website\"" + ", " + "\"product_ids\"" + ", " + "\"connext_source_type_detail_id\"" + ", " + "\"expected_revenue\"" + ", " + "\"write_date\"" + ", " + "\"activity_calendar_event_id\"" + "]";
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

            #region Run Tests
            response = ConnextApi.WebSearchRead(urlSite, body, modelLeadOpp, sessionID);
            return response;
            #endregion
        }

        private static IRestResponse GetWebSearchReadContact(string urlSite, string model, string inputSearch, string sessionID)
        {
            #region Variables declare
            int id = 17;
            string order = "";
            const string contextLastKey = "is_customer_individuals_page";
            const string contextLastKeyValue = "true";
            string domain = "[" + "\"&\"" + ", " + "\"&\"" + ", " + "[" + "\"is_customer\"" + ", " + "\"=\"" + ", " + "true], " + "[" + "\"is_company\"" + ", " + "\"=\"" + ", " + "false], " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", " + "\"|\"" + ", "
                            + "[" + "\"display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"ref\"" + ", " + "\"=\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"email\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"external_display_name\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "], " + "[" + "\"phone_mobile_search\"" + ", " + "\"ilike\"" + ", " + "\"" + inputSearch + "\"" + "]],";
            string fields = "[" + "\"activity_exception_decoration\"" + ", " + "\"activity_exception_icon\"" + ", " + "\"activity_state\"" + ", " + "\"activity_summary\"" + ", " + "\"activity_type_icon\"" + ", " + "\"activity_type_id\"" + ", " + "\"name\"" + ", " + "\"phone\"" + ", " + "\"birthday_display\"" + ", " + "\"display_name\"" + ", " + "\"unread_message_count\"" + ", " + "\"address\"" + ", " + "\"patient_id\"" + ", " + "\"recent_examination_date\"" + ", " + "\"recent_examination_service\"" + ", " + "\"booking_date\"" + ", " + "\"opportunity_latest_update\"" + ", " + "\"activity_ids\"" + ", " + "\"sale_customer_service\"" + "]";
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
            const int id = 259;
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

        private static IRestResponse DeleteWebSearchReadLead(string urlSite, string model, string recordIds, string sessionID)
        {
            #region Variables declare
            const int id = 205;
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

        private static IRestResponse DeleteWebSearchReadOpportunity(string urlSite, string model, string recordIds, string sessionID)
        {
            #region Variables declare
            const int id = 259;
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
                          " + "\n" + @"                ""default_type"": ""opportunity"",
                          " + "\n" + "\"default_team_id\"" + ": " + team_id
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
            const int id = 243;
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
            return response;
            #endregion
        }
        #endregion
    }
}
