using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.PredefinedScenarios
{
    internal class Tich_Hop_IMS : BaseFunctionTest
    {
        #region Initiate variables
        internal static IRestResponse? response;
        internal static string? apiPath;
        internal const string modelLeadOpp = "crm.lead";
        internal const string modelBooking = "sale.order";
        internal const string modelContact = "res.partner";
        internal static string pid1 = "TEST10000001",
        pid2 = pid1.Replace("01", "02"),
        pid3 = pid1.Replace("01", "03"),
        pid4 = pid1.Replace("01", "04"),
        pid5 = pid1.Replace("01", "05"),
        pid6 = pid1.Replace("01", "06"),
        pid7 = pid1.Replace("01", "07"),
        contactName1 = "QA Test API IMS To CRM 01",
        contactName2 = contactName1.Replace("01", "02"),
        contactName3 = contactName1.Replace("01", "03"),
        contactName4 = contactName1.Replace("01", "04"),
        contactName5 = contactName1.Replace("01", "05"),
        contactName6 = contactName1.Replace("01", "06"),
        contactName7 = contactName1.Replace("01", "07"),
        mobileData1 = "02119999991",
        mobileData2 = mobileData1.Replace("91", "92"),
        mobileData3 = mobileData1.Replace("91", "93"),
        mobileData4 = mobileData1.Replace("91", "94"),
        mobileData5 = mobileData1.Replace("91", "95"),
        mobileData6 = mobileData1.Replace("91", "96"),
        mobileData7 = mobileData1.Replace("91", "97");
        #endregion

        #region Head and Clean
        [SetUp]
        public void SetupData()
        {
        }

        [TearDown]
        public void Cleanup()
        {
            // Delete ...
        }
        #endregion

        #region TestMethod (IVF)
        [Test, Category("DKHN - API Smoke Tests")]
        public void ST001_IMSToCRM_GuiInfo_TaiKham()
        {
            #region Variables declare
            JObject responseJs;
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string patient = "{"
                             + "\n" + "\"pid\"" + ": " + "\"" + pid1 + "\","
                             + "\n" + "\"name\"" + ": " + "\"" + contactName1 + "\","
                             + "\n" + "\"birthday\"" + ": " + "\"" + "1997-11-17" + "\","
                             + "\n" + "\"gender\"" + ": " + "\"" + "Nam" + "\","
                             + "\n" + "\"mobile\"" + ": " + "\"" + mobileData1 + "\","
                             + "\n" + "\"role\"" + ": " + "\"" + "Chồng" + "\"" + "\n" +
                             "}";
            string index = "[{"
                           + "\n" + "\"beta_hcg\"" + ": " + "\"" + "10" + "\","
                           + "\n" + "\"note\"" + ": " + "\"" + "abc test" + "\"" +
                           "}]";
            string data = "[{"
                          + "\n" + "\"patient\"" + ": " + patient + ","
                          + "\n" + "\"date\"" + ": " + "\"" + dateNow + "T09:33:09Z" + "\","
                          + "\n" + "\"type\"" + ": " + "\"" + "FET" + "\","
                          + "\n" + "\"index\"" + ": " + index + ","
                          + "\n" + "\"code\"" + ": " + "\"" + "TEST_IMS_CONNEXT_01" + "\","
                          + "\n" + "\"note\"" + ": " + "null" + "\n" +
                          "}]";
            var body = "{" + "\n" + "\"data\"" + ": " + data +
                       "}";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // (Run Tests) Send Info Tai Kham (IMS to CRM)
                response = ConnextApi.IMSToCRM_SendInfo_TaiKham(urlSite, body, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs.Count, Is.EqualTo(2));
                Assert.That(responseJs["message"].ToString, Is.EqualTo("OK")); // old: CRM Received Data Successfully!
            }
            //if (urlSite.Contains("qa-horus") || urlSite.Contains("qa-bvtn")) {}
            else Console.WriteLine("No run test case on this site!!!");
            #endregion

            #region Get search Booking (Husband infor)
            Thread.Sleep(5000);
            // variables
            const string fieldSearch = "husband_infor"; // wife_infor / husband_infor
            const string inputSearch = "qa test api IMS To CRM 01";
            // Get search Booking
            response = GetWebSearchReadBooking(urlSite, fieldSearch, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            var recordBookingId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());

            // Read (detail) Booking
            response = ReadBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Opportunity
            // Get search Opportunity
            response = GetWebSearchReadOpp(urlSite, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            int recordOppId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());   
            int getTeamId = responseJs["result"]["records"][0]["team_id"].Value<int>();
            string? teamId = null;
            if (getTeamId == 0) { teamId = "false"; }
            else { teamId = getTeamId.ToString(); }
            // Read (detail) Opp
            response = ReadOpp(urlSite, recordOppId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            //Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            //Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(static jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Contact
            // Get search Contact
            response = GetWebSearchReadContact(urlSite, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            var recordContactId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Delete create Booking
            response = DeleteCreateBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Opportunity
            response = DeleteCreateOpp(urlSite, recordOppId, teamId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Contact
            response = DeleteCreateContact(urlSite, recordContactId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST002_IMSToCRM_GuiInfo_LichHenThuThuat()
        {
            #region Variables declare
            JObject responseJs;
            DateTime timestampDaysBefore = DateTime.UtcNow.AddDays(-2);
            string twoDaysBefore = timestampDaysBefore.ToString("yyyy-MM-dd");
            DateTime timestampDaysAfter = DateTime.UtcNow.AddDays(+2);
            string twoDaysAfter = timestampDaysAfter.ToString("yyyy-MM-dd");
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string patient = "{"
                             + "\n" + "\"pid\"" + ": " + "\"" + pid2 + "\","
                             + "\n" + "\"name\"" + ": " + "\"" + contactName2 + "\","
                             + "\n" + "\"birthday\"" + ": " + "\"" + "1997-11-17" + "\","
                             + "\n" + "\"gender\"" + ": " + "\"" + "Nữ" + "\","
                             + "\n" + "\"mobile\"" + ": " + "\"" + mobileData2 + "\","
                             + "\n" + "\"role\"" + ": " + "\"" + "Vợ" + "\"" + "\n" +
                             "}";
            string doctor = "{"
                            + "\n" + "\"code\"" + ": " + "\"" + "BSD1001" + "\","
                            + "\n" + "\"name\"" + ": " + "\"" + "Bác sĩ Điệp" + "\"" + "\n" + 
                            "}";
            string oocyte_donor = "{"
                              + "\n" + "\"pid\"" + ": " + "\"" + pid3 + "\"," 
                              + "\n" + "\"name\"" + ": " + "\"" + contactName3 + "\","
                              + "\n" + "\"birthday\"" + ": " + "\"" + "1998-11-18" + "\","
                              + "\n" + "\"gender\"" + ": " + "\"" + "Nam" + "\","
                              + "\n" + "\"mobile\"" + ": " + "\"" + mobileData3 + "\","
                              + "\n" + "\"role\"" + ": " + "\"" + "Chồng" + "\"" + "\n" +
                              "}";
            string data = "[{"
                          + "\n" + "\"patient\"" + ": " + patient + ","
                          + "\n" + "\"doctor\"" + ": " + doctor + ","
                          + "\n" + "\"oocyte_donor\"" + ": " + oocyte_donor + ","
                          + "\n" + "\"date\"" + ": " + "\"" + twoDaysAfter + "T09:33:09Z" + "\","
                          + "\n" + "\"date_hcg\"" + ": " + "\"" + dateNow + "T09:33:09Z" + "\","
                          + "\n" + "\"type\"" + ": " + "\"" + "Chuyển phôi trữ" + "\","
                          + "\n" + "\"code\"" + ": " + "\"" + "TEST_IMS_CONNEXT_02" + "\","
                          + "\n" + "\"state\"" + ": " + "\"" + "Chờ ngày thủ thuật" + "\"" + "\n" +
                          "}]";
            var body = "{" + "\n" + "\"data\"" + ": " + data +
                       "}";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // Send Info Tai Kham (IMS to CRM)
                response = ConnextApi.IMSToCRM_SendInfo_LichHenThuThuat(urlSite, body, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs.Count, Is.EqualTo(2));
                Assert.That(responseJs["message"].ToString, Is.EqualTo("OK"));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion

            #region Get search Booking (Husband infor)
            Thread.Sleep(6000);
            // variables
            const string fieldSearch = "husband_infor"; // wife_infor / husband_infor
            const string inputSearch = "qa test api";
            // Get search Booking
            response = GetWebSearchReadBooking(urlSite, fieldSearch, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            var recordBookingId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            // Read (detail) Booking
            response = ReadBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Opportunity
            // Get search Opportunity
            response = GetWebSearchReadOpp(urlSite, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            int recordOppId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            int getTeamId = responseJs["result"]["records"][0]["team_id"].Value<int>();
            string? teamId = null;
            if (getTeamId == 0) { teamId = "false"; }
            else { teamId = getTeamId.ToString(); }
            // Read (detail) Opp
            response = ReadOpp(urlSite, recordOppId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            //Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            //Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Contact
            // Get search Contact
            response = GetWebSearchReadContactWithOR(urlSite, "api ims to crm 02", "api ims to crm 03", sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            string recordContactId1 = responseJs["result"]["records"][0]["id"].ToString();
            string recordContactId2 = responseJs["result"]["records"][1]["id"].ToString();
            #endregion

            #region Delete create Booking
            response = DeleteCreateBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Opportunity
            response = DeleteCreateOpp(urlSite, recordOppId, teamId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Contact
            string recordContactId = recordContactId1 + ", " + recordContactId2;
            response = DeleteCreateContact(urlSite, recordContactId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST003_IMSToCRM_GuiInfo_LichHenThuThuat_TiemKichTrung()
        {
            #region Variables declare
            JObject responseJs;
            DateTime timestampDaysBefore = DateTime.UtcNow.AddDays(-2);
            string twoDaysBefore = timestampDaysBefore.ToString("yyyy-MM-dd");
            DateTime timestampDaysAfter = DateTime.UtcNow.AddDays(+2);
            string twoDaysAfter = timestampDaysAfter.ToString("yyyy-MM-dd");
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string patient = "{"
                             + "\n" + "\"pid\"" + ": " + "\"" + pid4 + "\","
                             + "\n" + "\"name\"" + ": " + "\"" + contactName4 + "\","
                             + "\n" + "\"birthday\"" + ": " + "\"" + "1997-11-17" + "\","
                             + "\n" + "\"gender\"" + ": " + "\"" + "Nữ" + "\","
                             + "\n" + "\"mobile\"" + ": " + "\"" + mobileData4 + "\","
                             + "\n" + "\"role\"" + ": " + "\"" + "Vợ" + "\"" + "\n" +
                             "}";
            string doctor = "{"
                            + "\n" + "\"code\"" + ": " + "\"" + "BSD1001" + "\","
                            + "\n" + "\"name\"" + ": " + "\"" + "Bác sĩ Điệp" + "\"" + "\n" +
                            "}";
            string oocyte_donor = "{"
                              + "\n" + "\"pid\"" + ": " + "\"" + pid5 + "\","
                              + "\n" + "\"name\"" + ": " + "\"" + contactName5 + "\","
                              + "\n" + "\"birthday\"" + ": " + "\"" + "1998-11-17" + "\","
                              + "\n" + "\"gender\"" + ": " + "\"" + "Nữ" + "\","
                              + "\n" + "\"mobile\"" + ": " + "\"" + mobileData5 + "\","
                              + "\n" + "\"role\"" + ": " + "\"" + "Đơn thân" + "\"" + "\n" +
                              "}";
            string data = "[{"
                          + "\n" + "\"patient\"" + ": " + patient + ","
                          + "\n" + "\"doctor\"" + ": " + doctor + ","
                          + "\n" + "\"oocyte_donor\"" + ": " + oocyte_donor + ","
                          + "\n" + "\"date\"" + ": " + "\"" + twoDaysAfter + "T09:33:09Z" + "\","
                          + "\n" + "\"date_hcg\"" + ": " + "\"" + dateNow + "T09:33:09Z" + "\","
                          + "\n" + "\"type\"" + ": " + "\"" + "Chọc hút" + "\","
                          + "\n" + "\"code\"" + ": " + "\"" + "TEST_IMS_CONNEXT_03" + "\","
                          + "\n" + "\"state\"" + ": " + "\"" + "Chờ ngày thủ thuật" + "\"" + "\n" +
                          "}]";
            var body = "{" + "\n" + "\"data\"" + ": " + data +
                       "}";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // Send Info Tai Kham (IMS to CRM)
                response = ConnextApi.IMSToCRM_SendInfo_LichHenThuThuat(urlSite, body, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs.Count, Is.EqualTo(2));
                Assert.That(responseJs["message"].ToString, Is.EqualTo("OK"));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion

            #region Get search Booking (Wife infor)
            Thread.Sleep(5000);
            // variables
            const string fieldSearch = "wife_infor"; // wife_infor / husband_infor
            const string inputSearch = "qa test api";
            // Get search Booking
            response = GetWebSearchReadBooking(urlSite, fieldSearch, inputSearch + " ims to crm 05", sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            var recordBookingId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            // Read (detail) Booking
            response = ReadBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Opportunity
            // Get search Opportunity
            response = GetWebSearchReadOpp(urlSite, inputSearch + " ims to crm 04", sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            int recordOppId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            int getTeamId = responseJs["result"]["records"][0]["team_id"].Value<int>();
            string? teamId = null;
            if (getTeamId == 0) { teamId = "false"; }
            else { teamId = getTeamId.ToString(); }
            // Read (detail) Opp
            response = ReadOpp(urlSite, recordOppId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            //Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            //Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Contact
            // Get search Contact
            response = GetWebSearchReadContactWithOR(urlSite, "api ims to crm 04", "api ims to crm 05", sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            string recordContactId1 = responseJs["result"]["records"][0]["id"].ToString();
            string recordContactId2 = responseJs["result"]["records"][1]["id"].ToString();
            #endregion

            #region Delete create Booking
            response = DeleteCreateBooking(urlSite, recordBookingId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Opportunity
            response = DeleteCreateOpp(urlSite, recordOppId, teamId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Contact
            string recordContactId = recordContactId1 + ", " + recordContactId2;
            response = DeleteCreateContact(urlSite, recordContactId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests"), Ignore("")]
        public void ST004_IMSToCRM_GuiInfo_NhacGiahanMau() // (Cryotop)
        {
            #region Variables declare
            /// Variables
            string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            int yearNow = (int)DateTime.Now.Year,   
            monthNowInt = (int)DateTime.Now.Month,
            dayNow = (int)DateTime.Now.Day;
            DateTime today = DateTime.Now; // .Now or .Today
            DateTime GetMonthNow = today.AddMonths(+0);
            DateTime oneMonthEarlier = today.AddMonths(+1);
            DateTime oneDaysLater = today.AddDays(+1);
            DateTime twoDaysLater = today.AddDays(+2);
            DateTime threeDaysLater = today.AddDays(+3);
            DateTime threeDaysEarlier = today.AddDays(-3);
            DateTime sixDaysEarlier = today.AddDays(-6);
            string plus1Month = oneMonthEarlier.ToString("MM");
            string plus1Days = oneDaysLater.ToString("dd"); // ticket 936 Báo cáo gửi tin nhắn trữ phôi cách sau 1 ngày
            string plus2Days = twoDaysLater.ToString("dd");
            string plus3Days = threeDaysLater.ToString("dd"); // ticket 936 Báo cáo gửi tin nhắn trữ phôi cách sau 3 ngày
            string minus3Days = threeDaysEarlier.ToString("dd");
            string minus6Days = sixDaysEarlier.ToString("dd");
            string? monthNow = null;
            if (monthNowInt < 10)
            {
                if (monthNowInt == 2)
                {
                    if (Convert.ToInt32(plus2Days) > 28) plus2Days = "28";
                    if (Convert.ToInt32(minus3Days) > 28) minus3Days = "28";
                    if (Convert.ToInt32(minus6Days) > 28) minus6Days = "28";
                }
                monthNow = GetMonthNow.ToString("MM");
            }
            if (monthNowInt > 9) { monthNow = GetMonthNow.ToString("MM"); }
            if (plus1Month == "2" || plus1Month == "02")
            {
                if (Convert.ToInt32(plus2Days) > 28) plus2Days = "28";
            }

            /// Body
            string patient = "{"
                             + "\n" + "\"pid\"" + ": " + "\"" + pid6 + dateTimeNow + "\","
                             + "\n" + "\"name\"" + ": " + "\"" + contactName6 + "_" + dateTimeNow + "\","
                             + "\n" + "\"birthday\"" + ": " + "\"" + "1997-11-17" + "\","
                             + "\n" + "\"gender\"" + ": " + "\"" + "Nữ" + "\","
                             + "\n" + "\"mobile\"" + ": " + "\"" + mobileData6 + "\","
                             + "\n" + "\"role\"" + ": " + "\"" + "Vợ" + "\"" + "\n" +
                             "}";
            string straws = "[{"
                            + "\n" + "\"name\"" + ": " + "\"" + "Cryotop Trắng - 1" + "\","
                            + "\n" + "\"type\"" + ": " + "\"" + "Phôi" + "\","
                            + "\n" + "\"date_or\"" + ": " + "\"" + (yearNow - 1) + "-" + monthNow + "-" + minus6Days + "\","
                            + "\n" + "\"date_expire\"" + ": " + "\"" + yearNow + "-" + plus1Month + "-" + plus3Days + "\"" + "\n" // org: plus2Days
                            + "}," + "\n"
                            + "{"
                            + "\n" + "\"name\"" + ": " + "\"" + "Cryotop Trắng - 1" + "\","
                            + "\n" + "\"type\"" + ": " + "\"" + "Phôi" + "\","
                            + "\n" + "\"date_or\"" + ": " + "\"" + (yearNow - 1) + "-" + monthNow + "-" + minus3Days + "\","
                            + "\n" + "\"date_expire\"" + ": " + "\"" + yearNow + "-" + plus1Month + "-" + plus3Days + "\"" + "\n" // org: plus2Days
                            + "}," + "\n"
                             + "{"
                            + "\n" + "\"name\"" + ": " + "\"" + "Cryotop Vàng - 2" + "\","
                            + "\n" + "\"type\"" + ": " + "\"" + "Phôi" + "\","
                            + "\n" + "\"date_or\"" + ": " + "\"" + (yearNow - 1) + "-" + monthNow + "-" + minus3Days + "\","
                            + "\n" + "\"date_expire\"" + ": " + "\"" + yearNow + "-" + plus1Month + "-" + plus3Days + "\"" + "\n" // org: plus2Days
                            + "}," + "\n"
                            + "{"
                            + "\n" + "\"name\"" + ": " + "\"" + "Cryotop Xanh - 3" + "\","
                            + "\n" + "\"type\"" + ": " + "\"" + "Phôi" + "\","
                            + "\n" + "\"date_or\"" + ": " + "\"" + yearNow + "-" + monthNow + "-" + minus3Days + "\","
                            + "\n" + "\"date_expire\"" + ": " + "\"" + yearNow + "-" + monthNow + "-" + plus3Days + "\"" + "\n" + // org: (yearNow + 1) + ... + plus2Days
                            "}]";
            string data = "[{"
                          + "\n" + "\"patient\"" + ": " + patient + ","
                          + "\n" + "\"code\"" + ": " + "\"" + "TEST_IMS_CONNEXT_" + dateTimeNow + "\","
                          + "\n" + "\"type\"" + ": " + "\"" + "Phôi" + "\","
                          + "\n" + "\"straws\"" + ": " + straws +
                          "}]";
            var body = "{" + "\n" + "\"data\"" + ": " + data +
                       "}";
            #endregion

            #region Run Tests
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // Send Info Tai Kham (IMS to CRM)
                response = ConnextApi.IMSToCRM_SendInfo_NhacGiahanMau(urlSite, body, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                JObject responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs.Count, Is.EqualTo(2));
                Assert.That(responseJs["message"].ToString, Is.EqualTo("OK"));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion

            #region Update Embryo Booking (RUN MANUALLY --> Thiet lap >> Ky thuat >> Hoat dong dinh ky)
            response = ConnextApi.UpdateEmbryoBookingRunManually(urlSite, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            JObject resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((int?)resJs["id"], Is.GreaterThanOrEqualTo(21)); Thread.Sleep(1000);

            // re-run to make sure this feature works correctly
            response = ConnextApi.UpdateEmbryoBookingRunManually(urlSite, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((int?)resJs["id"], Is.GreaterThanOrEqualTo(21));
            #endregion

            #region Get web search Opportunity (Lead screen)
            Thread.Sleep(8000);
            // variables
            const string inputSearch = "api ims to crm 06_";
            // Get search Opportunity (Lead)
            response = GetWebSearchReadLead(urlSite, inputSearch + dateTimeNow, sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            int recordLeadId = int.Parse(resJs["result"]["records"][0]["id"].ToString());
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((string?)resJs["result"]["records"][0]["contact_name"], Is.EqualTo(contactName6 + "_" + dateTimeNow));
            // Read (detail) Opp
            response = ReadOpp(urlSite, recordLeadId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            //Assert.That(resJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            //Assert.That(resJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Booking (Wife infor)
            // variables
            const string fieldSearch = "wife_infor"; // wife_infor / husband_infor
            // Get search Booking
            response = GetWebSearchReadBooking(urlSite, fieldSearch, contactName6 + "_" + dateTimeNow, sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            string recordBookingId1 = resJs["result"]["records"][0]["id"].ToString();
            string recordBookingId2 = resJs["result"]["records"][1]["id"].ToString();
            string recordBookingId3 = resJs["result"]["records"][2]["id"].ToString();
            string recordBookingId4 = resJs["result"]["records"][3]["id"].ToString();
            // Read (detail) Booking
            response = ReadBooking(urlSite, recordBookingId1.ToString(), sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            Assert.That(resJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get web search Contact
            // Get search Contact
            response = GetWebSearchReadContact(urlSite, inputSearch + dateTimeNow, sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((string?)resJs["result"]["records"][0]["name"], Is.EqualTo(contactName6 + "_" + dateTimeNow));
            var recordContactId = int.Parse(resJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Get web search read Cryotop (Tru Phoi screen)
            Thread.Sleep(2000);
            // Get search read Cryotop
            response = ConnextApi.GetWebSearchReadCryotop(urlSite, inputSearch + dateTimeNow, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((string?)resJs["result"]["records"][0]["wife_infor"], Is.EqualTo(contactName6 + "_" + dateTimeNow + " - 1997"));
            #endregion

            #region Get web search read ims.renewal.reminder (cau hinh - nhac nho gia han mau screen)
            // Get search read ims.renewal.reminder
            response = ConnextApi.GetWebSearchReadIMSRenewalReminder(urlSite, inputSearch, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            int recordCryotopId = int.Parse(resJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Delete web search read ims.renewal.reminder (cau hinh - nhac nho gia han mau screen)
            // Delete search read ims.renewal.reminder
            response = ConnextApi.DeleteWebSearchReadIMSRenewalReminder(urlSite, recordCryotopId.ToString(), sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That(resJs.Count, Is.EqualTo(3));
            Assert.That((bool?)resJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Booking (4 Bookings)
            Thread.Sleep(2000);
            string recordBookingId = recordBookingId1 + ", " + recordBookingId2 + ", " + recordBookingId3 + ", " + recordBookingId4;
            response = DeleteCreateBooking(urlSite, recordBookingId, sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That((bool?)resJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Lead
            response = DeleteCreateLead(urlSite, recordLeadId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That((bool?)resJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Contact
            response = DeleteCreateContact(urlSite, recordContactId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            resJs = JObject.Parse(response.Content);
            Assert.That((bool?)resJs["result"], Is.EqualTo(true));
            #endregion
        }

        [Test, Category("DKHN - API Smoke Tests")]
        public void ST005_IMSToCRM_GuiInfo_BetaHCG()
        {
            #region Variables declare
            JObject responseJs;
            string patient = "{"
                             + "\n" + "\"pid\"" + ": " + "\"" + pid7 + "\","
                             + "\n" + "\"name\"" + ": " + "\"" + contactName7 + "\"," 
                             + "\n" + "\"birthday\"" + ": " + "\"" + "1997-11-17" + "\","
                             + "\n" + "\"gender\"" + ": " + "\"" + "Nữ" + "\","
                             + "\n" + "\"mobile\"" + ": " + "\"" + mobileData7 + "\","
                             + "\n" + "\"role\"" + ": " + "\"" + "Vợ" + "\"" + "\n" + 
                             "}";
            string index = "[]";
            string data = "[{"
                          + "\n" + "\"patient\"" + ": " + patient + ","
                          + "\n" + "\"date\"" + ": " + "\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\","
                          + "\n" + "\"type\"" + ": " + "\"" + "FET" + "\","
                          + "\n" + "\"index\"" + ": " + index + ","
                          + "\n" + "\"code\"" + ": " + "\"" + "TEST_IMS_CONNEXT_05" + "\"" + "\n" +
                          "}]";
            var body = "{" + "\n" + "\"data\"" + ": " + data +
                       "}";
            #endregion

            #region Run Tests 
            // Check if url site is not QA-DKHN then no run
            if (urlSite.Contains(instanceName) || urlSite.Contains("staging-bvdkhn"))
            {
                // Send Info Tai Kham (IMS to CRM)
                response = ConnextApi.IMSToCRM_SendInfo_BetaHCG(urlSite, body, apiKey);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                // Parse IRestResponse to JObject
                responseJs = JObject.Parse(response.Content);
                Assert.That(responseJs.Count, Is.EqualTo(2));
                Assert.That(responseJs["message"].ToString, Is.EqualTo("OK"));
            }
            else Console.WriteLine("No run test case on this site!!!");
            #endregion

            #region Get search Opportunity
            Thread.Sleep(9000);
            // variables
            const string inputSearch = "api ims to crm 07";
            // Get search Opportunity
            response = GetWebSearchReadOpp(urlSite, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            int recordOppId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            int getTeamId = responseJs["result"]["records"][0]["team_id"].Value<int>();
            string? teamId = null;
            if (getTeamId == 0) { teamId = "false"; }
            else { teamId = getTeamId.ToString(); }
            // Read (detail) Opp
            response = ReadOpp(urlSite, recordOppId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            //Assert.That(responseJs["result"][0]["service_unit_id"].Any(jt => jt.Value<string>().Contains("IVF")), Is.True);
            //Assert.That(responseJs["result"][0]["connext_source_type_id"].Any(jt => jt.Value<string>().Contains("Offline / Tái khám")), Is.True);
            #endregion

            #region Get search Contact
            // Get search Contact
            response = GetWebSearchReadContact(urlSite, inputSearch, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That(responseJs.Count, Is.EqualTo(3));
            var recordContactId = int.Parse(responseJs["result"]["records"][0]["id"].ToString());
            #endregion

            #region Delete create Opportunity
            response = DeleteCreateOpp(urlSite, recordOppId, teamId, sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion

            #region Delete create Contact
            response = DeleteCreateContact(urlSite, recordContactId.ToString(), sessionID);
            // Parse IRestResponse to JObject
            responseJs = JObject.Parse(response.Content);
            Assert.That((bool?)responseJs["result"], Is.EqualTo(true));
            #endregion
        }
        #endregion

        #region Methods
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

        private static IRestResponse ReadOpp(string urlSite, string id, string sessionID)
        {
            #region Variables declare
            string body = @"{"
                          + "\n" + @"    ""id"": 224,"
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {" 
                          + "\n" +       "\"" + "args" + "\"" + ": " + "[[" + id + "]" + ", "
                          + "\n" + @"                                    [""show_enrich_button"", ""type_domain"", ""subtype"", ""is_telesale"", ""show_stage_id"", ""stage_id"", ""active"", ""company_id"", ""active_opportunity_count"", ""total_opportunity_count"", ""calendar_event_count"", ""total_booking_count"", ""quotation_count"", ""sale_amount_total"", ""sale_order_count"", ""duplicate_lead_count"", ""ticket_count"", ""name"", ""contract_code"", ""company_currency"", ""expected_revenue"", ""automated_probability"", ""is_automated_probability"", ""probability"", ""is_partner_visible"", ""partner_id"", ""partner_name"", ""contact_name"", ""birthday"", ""year_of_birth"", ""phone"", ""sdt_2"", ""sdt_3"", ""len_duplicated_lead"", ""required_phone"", ""duplicated_lead_ids"", ""email_from"", ""website"", ""street"", ""state_id"", ""district_id"", ""ward_id"", ""street2"", ""city"", ""zip"", ""country_id"", ""hide_patient_type"", ""patient_type"", ""lang_active_count"", ""lang_code"", ""lang_id"", ""show_husband_id"", ""husband_id"", ""is_blacklisted"", ""partner_is_blacklisted"", ""phone_blacklisted"", ""mobile_blacklisted"", ""email_state"", ""phone_state"", ""partner_email_update"", ""partner_phone_update"", ""medical_examination_day"", ""status"", ""lost_reason_id"", ""date_conversion"", ""user_company_ids"", ""show_husband"", ""husband_name"", ""husband_phone"", ""husband_birthday"", ""husband_year"", ""service_unit_id"", ""tag_ids"", ""last_telesale_note"", ""is_group"", ""type"", ""is_dakhoa"", ""product_ids"", ""net_revenue"", ""date_deadline"", ""priority"", ""is_sale_manager"", ""is_chat_manager"", ""is_team_leader"", ""user_id"", ""team_id"", ""contract_id"", ""medical_examination_date_closed"", ""contract_examination_due_date"", ""medical_examination_due_date"", ""start_date_get_sample"", ""end_date_get_sample"", ""place_of_registration"", ""package_examination"", ""lead_properties"", ""current_user_role"", ""description"", ""message_bounce"", ""is_trucchat"", ""campaign_id"", ""medium_id"", ""source_id"", ""refer_by"", ""referred"", ""mkt_ads"", ""connext_source_type_id"", ""connext_source_type_detail_id"", ""connext_source_type_detail_id_2"", ""click_id"", ""cbnv"", ""kh_gioi_thieu"", ""show_source_detail"", ""mkt_chat"", ""sale_tele"", ""user_sale_consultation"", ""pro_doctor"", ""pro_nursing"", ""sale_customer_service"", ""title"", ""function"", ""mobile"", ""day_open"", ""day_close"", ""ech_user_endpoint"", ""is_empty_beta_hcg_id"", ""index_beta_hcg_ids"", ""medical_examination_ids"", ""accompanying_contacts"", ""display_name""]],"
                          + "\n" + @"        ""model"": ""crm.lead"","
                          + "\n" + @"        ""method"": ""read"","
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" + @"                ""uid"": 51,"
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""bin_size"": true,"
                          + "\n" + @"                ""default_type"": ""opportunity"","
                          + "\n" + @"                ""default_team_id"": 18"
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    }" + "\n" +@"}";
            #endregion

            #region Run Tests
            response = ConnextApi.Read(urlSite, body, modelLeadOpp, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse GetWebSearchReadBooking(string urlSite, string fieldSearch, string inputSearch, string sessionID)
        {
            #region Variables declare
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
                          " + "\n" + "\"model\"" + ": " + "\"" + modelBooking + "\"" + ","
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
            response = ConnextApi.WebSearchRead(urlSite, body, modelBooking, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse ReadBooking(string urlSite, string id, string sessionID)
        {
            #region Variables declare
            string body = @"{" 
                          + "\n" + @"    ""id"": 209,"
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {" 
                          + "\n" +       "\"" + "args" + "\"" + ": " + "[[" + id + "], " + "[" + "\"" + "__last_update" + "\"" + ", " + "\"" + "authorized_transaction_ids" + "\"" + ", " + "\"" + "state" + "\"" + ", " + "\"" + "partner_credit_warning" + "\"" + ", " + "\"" + "delivery_count" + "\"" + ", " + "\"" + "invoice_count" + "\"" + ", " + "\"" + "purchase_order_count" + "\"" + ", " + "\"" + "sale_ticket_count" + "\"" + ", " + "\"" + "name" + "\"" + ", " + "\"" + "partner_id" + "\"" + ", " + "\"" + "show_husband" + "\"" + ", " + "\"" + "husband_id" + "\"" + ", " + "\"" + "accompanying_contact" + "\"" + ", " + "\"" + "is_show_button_update_customer" + "\"" + ", " + "\"" + "opportunity_id" + "\"" + ", " + "\"" + "date_order" + "\"" + ", " + "\"" + "booking_type" + "\"" + ", " + "\"" + "embryo_top_name" + "\"" + ", " + "\"" + "embryo_date" + "\"" + ", " + "\"" + "validity_date" + "\"" + ", " + "\"" + "service_unit_id" + "\"" + ", " + "\"" + "product_ids" + "\"" + ", " + "\"" + "booking_status" + "\"" + ", " + "\"" + "doctor_id" + "\"" + ", " + "\"" + "show_update_pricelist" + "\"" + ", " + "\"" + "pricelist_id" + "\"" + ", " + "\"" + "company_id" + "\"" + ", " + "\"" + "currency_id" + "\"" + ", " + "\"" + "tax_country_id" + "\"" + ", " + "\"" + "payment_term_id" + "\"" + ", " + "\"" + "sh_sale_ticket_ids" + "\"" + ", " + "\"" + "is_dakhoa" + "\"" + ", " + "\"" + "description" + "\"" + ", " + "\"" + "order_line" + "\"" + ", " + "\"" + "medical_record_id" + "\"" + ", " + "\"" + "ims_patient_id" + "\"" + ", " + "\"" + "note" + "\"" + ", " + "\"" + "tax_totals" + "\"" + ", " + "\"" + "sale_order_option_ids" + "\"" + ", " + "\"" + "user_id" + "\"" + ", " + "\"" + "team_id" + "\"" + ", " + "\"" + "require_signature" + "\"" + ", " + "\"" + "require_payment" + "\"" + ", " + "\"" + "reference" + "\"" + ", " + "\"" + "client_order_ref" + "\"" + ", " + "\"" + "tag_ids" + "\"" + ", " + "\"" + "show_update_fpos" + "\"" + ", " + "\"" + "fiscal_position_id" + "\"" + ", " + "\"" + "partner_invoice_id" + "\"" + ", " + "\"" + "invoice_status" + "\"" + ", " + "\"" + "warehouse_id" + "\"" + ", " + "\"" + "picking_policy" + "\"" + ", " + "\"" + "commitment_date" + "\"" + ", " + "\"" + "expected_date" + "\"" + ", " + "\"" + "show_json_popover" + "\"" + ", " + "\"" + "json_popover" + "\"" + ", " + "\"" + "effective_date" + "\"" + ", " + "\"" + "delivery_status" + "\"" + ", " + "\"" + "origin" + "\"" + ", " + "\"" + "campaign_id" + "\"" + ", " + "\"" + "medium_id" + "\"" + ", " + "\"" + "source_id" + "\"" + ", " + "\"" + "connext_source_type_id" + "\"" + ", " + "\"" + "connext_source_type_detail_id" + "\"" + ", " + "\"" + "connext_source_type_detail_id_2" + "\"" + ", " + "\"" + "cbnv" + "\"" + ", " + "\"" + "refer_by" + "\"" + ", " + "\"" + "kh_gioi_thieu" + "\"" + ", " + "\"" + "show_source_detail" + "\"" + ", " + "\"" + "signed_by" + "\"" + ", " + "\"" + "signed_on" + "\"" + ", " + "\"" + "signature" + "\"" + ", " + "\"" + "is_addnew_treament_card" + "\"" + ", " + "\"" + "treatment_card_id" + "\"" + ", " + "\"" + "treatment_card_code" + "\"" + ", " + "\"" + "treatment_card_date" + "\"" + ", " + "\"" + "treatment_id" + "\"" + ", " + "\"" + "treatment_card_qty" + "\"" + ", " + "\"" + "treatment_qty" + "\"" + ", " + "\"" + "treatment_booked_qty" + "\"" + ", " + "\"" + "medical_examination_ids" + "\"" + ", " + "\"" + "display_name" + "\"" + "]]," 
                          + "\n" + @"        ""model"": ""sale.order"","
                          + "\n" + @"        ""method"": ""read""," 
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" + @"                ""uid"": 51,"
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""bin_size"": true"
                          + "\n" + @"            }"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" +
                          @"}";
            #endregion

            #region Run Tests
            response = ConnextApi.Read(urlSite, body, modelBooking, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse GetWebSearchReadContact(string urlSite, string inputSearch, string sessionID)
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
                          " + "\n" + "\"model\"" + ": " + "\"" + modelContact + "\"" + ","
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
            response = ConnextApi.WebSearchRead(urlSite, body, modelContact, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse GetWebSearchReadContactWithOR(string urlSite, string inputSearch01, string inputSearch02, string sessionID)
        {
            #region Variables declare
            string body = @"{" 
                        + "\n" + @"    ""id"": 511,"
                        + "\n" + @"    ""jsonrpc"": ""2.0"","
                        + "\n" + @"    ""method"": ""call"","
                        + "\n" + @"    ""params"": {"
                        + "\n" + @"        ""model"": ""res.partner"","
                        + "\n" + @"        ""method"": ""web_search_read"","
                        + "\n" + @"        ""args"": [],"
                        + "\n" + @"        ""kwargs"": {"
                        + "\n" + @"            ""limit"": 80,"
                        + "\n" + @"            ""offset"": 0,"
                        + "\n" + @"            ""order"": """","
                        + "\n" + @"            ""context"": {"
                        + "\n" + @"                ""lang"": ""vi_VN"","
                        + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                        + "\n" + @"                ""uid"": 51,"
                        + "\n" + @"                ""allowed_company_ids"": [1],"
                        + "\n" + @"                ""bin_size"": true,"
                        + "\n" + @"                ""is_customer_individuals_page"": true"
                        + "\n" + @"            },"
                        + "\n" + @"            ""count_limit"": 10001,"
                        + "\n" +         "\"" + "domain" + "\"" + ": " + "[" + "\"" + "&" + "\"" + ", " + "\"" + "&" + "\"" + ", " + "[" + "\"" + "is_customer" + "\"" + ", " + "\"" + "=" + "\"" + ", " + "true" + "], " + "[" + "\"" + "is_company" + "\"" + ", " + "\"" + "=" + "\"" + ", " + "false" + "], " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" +"\"" + ", " + 
                                                    "[" + "\"" + "display_name" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], [" + "\"" + "ref" + "\"" + ", " + "\"" + "=" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], [" + "\"" + "email" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], [" + "\"" + "phone" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], [" + "\"" + "external_display_name" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], [" + "\"" + "phone_mobile_search" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch01 + "\"" + "], " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", " + "\"" + "|" + "\"" + ", [" + "\"" + "display_name" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "], [" + "\"" + "ref" + "\"" + ", " + "\"" + "=" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "], [" + "\"" + "email" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "], [" + "\"" + "phone" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "], [" + "\"" + "external_display_name" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "], [" + "\"" + "phone_mobile_search" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch02 + "\"" + "]],"
                        + "\n" + @"            ""fields"": [""activity_exception_decoration"", ""activity_exception_icon"", ""activity_state"", ""activity_summary"", ""activity_type_icon"", ""activity_type_id"", ""name"", ""phone"", ""birthday_display"", ""display_name"", ""unread_message_count"", ""address"", ""patient_id"", ""recent_examination_date"", ""recent_examination_service"", ""booking_date"", ""opportunity_latest_update"", ""activity_ids"", ""sale_customer_service""]"
                        + "\n" + @"        }"
                        + "\n" + @"    }"
                        + "\n" + @"}";
            #endregion

            #region Run Tests
            response = ConnextApi.WebSearchRead(urlSite, body, modelContact, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse DeleteCreateBooking(string urlSite, string recordId, string sessionID)
        {
            #region Variables declare
            const int id = 243;
            const int uid = 51;
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + modelBooking + "\"" + ","
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
            response = ConnextApi.Delete(urlSite, modelBooking, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse DeleteCreateLead(string urlSite, string recordId, string sessionID)
        {
            #region Variables declare
            const int id = 205;
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + modelLeadOpp + "\"" + ","
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
            response = ConnextApi.Delete(urlSite, modelLeadOpp, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse DeleteCreateOpp(string urlSite, int recordId, string team_id, string sessionID)
        {
            #region Variables declare
            const int id = 259;
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + modelLeadOpp + "\"" + ","
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
            response = ConnextApi.Delete(urlSite, modelLeadOpp, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }

        private static IRestResponse DeleteCreateContact(string urlSite, string recordId, string sessionID)
        {
            #region Variables declare
            const int id = 72;
            string args = "[[" + recordId + "]]";
            string body = @"{
                          " + "\n" + "\"id\": " + id + ","
                            + "\n" + @"    ""jsonrpc"": ""2.0"",
                          " + "\n" + @"    ""method"": ""call"",
                          " + "\n" + @"    ""params"": {
                          " + "\n" + "\"model\"" + ": " + "\"" + modelContact + "\"" + ","
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
            response = ConnextApi.Delete(urlSite, modelContact, body, sessionID);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            return response;
            #endregion
        }
        #endregion
    }
}
