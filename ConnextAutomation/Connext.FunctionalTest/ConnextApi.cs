using Connext.FunctionalTest.UIStepsDataManagement;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest
{
    internal class ConnextApi
    {
        // Initiate variables
        private static IRestResponse? response;

        #region Web Session Authenticate
        internal static IRestResponse PostSessionWeb(string url, string db, string username, string password)
        {
            string body = "{"
                           + "\n" + "\"jsonrpc\"" + ": " + "\"" + "2.0" + "\","
                           + "\n" + "\"params\"" + ": " + "{"
                           + "\n" + "\t" + "\"db\"" + ": " + "\"" + db + "\","
                           + "\n" + "\t" + "\"login\"" + ": " + "\"" + username + "\","
                           + "\n" + "\t" + "\"password\"" + ": " + "\"" + password + "\""
                           + "\n" + "}" + "\n" +
                           "}";

            response = Api.PostObject(url, "web/session/authenticate", body);
            return response;
        }
        internal static IRestResponse PostSessionWeb(string url, string body)
        {
            response = Api.PostObject(url, "web/session/authenticate", body);
            return response;
        }
        #endregion

        #region Web Search Read
        // POST - seach name/phone ...
        internal static IRestResponse WebSearchRead(string url, string body, string model, string sessionID)
        {
            //response = Api.PostObject(url, "web/dataset/call_kw/" + model + "/web_search_read?session_id=" + sessionID + "", body);
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/" + model + "/web_search_read", sessionID, body);
            return response;
        }
        #endregion

        #region Read
        // POST - Read (data on detail page Booking / Opportunity / ...)
        internal static IRestResponse Read(string url, string body, string model, string sessionID)
        {
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/" + model + "/read", sessionID, body);
            return response;
        }
        #endregion

        #region Create
        // POST - create lead
        internal static IRestResponse Create(string url, string body, string model, string sessionID)
        {
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/" + model + "/create", sessionID, body);
            return response;
        }
        #endregion

        #region Delete
        // POST - delete lead
        internal static IRestResponse Delete(string url, string model,string body, string sessionID)
        {
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/" + model + "/unlink", sessionID, body);
            return response;
        }
        #endregion

        #region Tich hop IMS
        // POST - IMS Gui Thông Tin Tai Kham (IMS to CRM)
        internal static IRestResponse IMSToCRM_SendInfo_TaiKham(string url, string body, string? apiKey = null)
        {
            response = Api.PostObject(url, "api/v1/ims/reexamination", body, apiKey);
            return response;
        }

        // POST - IMS Gui Thông Tin Lich Hen Thu Thuat (IMS to CRM)
        internal static IRestResponse IMSToCRM_SendInfo_LichHenThuThuat(string url, string body, string? apiKey = null)
        {
            response = Api.PostObject(url, "api/v1/ims/treatment_appointment", body, apiKey);
            return response;
        }

        // POST - IMS Gui Thông Tin Lich Hen Thu Thuat (IMS to CRM)
        internal static IRestResponse IMSToCRM_SendInfo_NhacGiahanMau(string url, string body, string? apiKey = null)
        {
            response = Api.PostObject(url, "api/v1/ims/renewal_reminder", body, apiKey);
            return response;
        }

        // POST - IMS Gui Thông Tin Beta HCG (IMS to CRM)
        internal static IRestResponse IMSToCRM_SendInfo_BetaHCG(string url, string body, string? apiKey = null)
        {
            response = Api.PostObject(url, "api/v1/ims/beta_hcg", body, apiKey);
            return response;
        }

        // POST - Update Embryo Booking (RUN MANUALLY)
        internal static IRestResponse UpdateEmbryoBookingRunManually(string url, string sessionID)
        {
            string body = @"{" 
                          + "\n" + @"    ""id"": 21," 
                          + "\n" + @"    ""jsonrpc"": ""2.0""," 
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""args"": [[129]],"
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""params"": {"
                          + "\n" + @"                    ""id"": 129,"
                          + "\n" + @"                    ""cids"": 1,"
                          + "\n" + @"                    ""menu_id"": 4,"
                          + "\n" + @"                    ""action"": 13,"
                          + "\n" + @"                    ""model"": ""ir.cron"","
                          + "\n" + @"                    ""view_type"": ""form"""
                          + "\n" + @"                },"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Saigon"","
                          + "\n" + @"                ""uid"": 24,"
                          + "\n" + @"                ""allowed_company_ids"": [1]"
                          + "\n" + @"            }"
                          + "\n" + @"        },"
                          + "\n" + @"        ""method"": ""method_direct_trigger"","
                          + "\n" + @"        ""model"": ""ir.cron"""
                          + "\n" + @"    }"
                          + "\n" + @"}";
            response = Api.PostObjectSessionID(url, "web/dataset/call_button", sessionID, body);
            return response;
        }

        // POST - Get Web Search Cryotop
        internal static IRestResponse GetWebSearchReadCryotop(string url, string inputSearch, string sessionID)
        {
            string body = @"{" 
                          + "\n" + @"    ""id"": 380,"
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""model"": ""sale.order.embryo.stat"","
                          + "\n" + @"        ""method"": ""web_search_read"","
                          + "\n" + @"        ""args"": [],"
                          + "\n" + @"        ""kwargs"": {"
                          + "\n" + @"            ""limit"": 80,"
                          + "\n" + @"            ""offset"": 0,"
                          + "\n" + @"            ""order"": ""booking_date ASC"","
                          + "\n" + @"            ""context"": {"
                          + "\n" + @"                ""lang"": ""vi_VN"","
                          + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                          + "\n" + @"                ""uid"": 51,"
                          + "\n" + @"                ""allowed_company_ids"": [1],"
                          + "\n" + @"                ""bin_size"": true"
                          + "\n" + @"            },"
                          + "\n" + @"            ""count_limit"": 10001,"
                          + "\n" +         "\"" + "domain" + "\"" + ":" + "[[" + "\"" + "partner_id" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch + "\"" + "]]" + ","
                          + "\n" + @"            ""fields"": [""booking_date"", ""wife_infor"", ""husband_infor"", ""patient_id"", ""address"", ""embryo_top_count"", ""expired_embryo_top_count"", ""status""]"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" + @"}";
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/sale.order.embryo.stat/web_search_read", sessionID, body);
            return response;
        }

        // POST - Get Web Search  ims.renewal.reminder (cau hinh - nhac nho gia han mau screen)
        internal static IRestResponse GetWebSearchReadIMSRenewalReminder(string url, string inputSearch, string sessionID)
        {
            string body = @"{" 
                          + "\n" +@"    ""id"": 16,"
                          + "\n" + @"    ""jsonrpc"": ""2.0"","
                          + "\n" + @"    ""method"": ""call"","
                          + "\n" + @"    ""params"": {"
                          + "\n" + @"        ""model"": ""ims.renewal.reminder"","
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
                          + "\n" + @"                ""params"": {"
                          + "\n" + @"                    ""action"": 933,"
                          + "\n" + @"                    ""model"": ""ims.renewal.reminder"","
                          + "\n" + @"                    ""view_type"": ""list"","
                          + "\n" + @"                    ""cids"": 1,"
                          + "\n" + @"                    ""menu_id"": 208"
                          + "\n" + @"                }"
                          + "\n" + @"            },"
                          + "\n" + @"            ""count_limit"": 10001,"
                          + "\n" +         "\"" + "domain" + "\"" + ": " + "[[" + "\"" + "patient" + "\"" + ", " + "\"" + "ilike" + "\"" + ", " + "\"" + inputSearch + "\"" + "]]" + ","
                          + "\n" + @"            ""fields"": [""patient_json_pretty"", ""code"", ""type"", ""straws_json_pretty"", ""status"", ""detail_status"", ""imported"", ""create_date""]"
                          + "\n" + @"        }"
                          + "\n" + @"    }"
                          + "\n" + @"}";
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/ims.renewal.reminder/web_search_read", sessionID, body);
            return response;
        }

        // POST - Delete Web Search  ims.renewal.reminder (cau hinh - nhac nho gia han mau screen)
        internal static IRestResponse DeleteWebSearchReadIMSRenewalReminder(string url, string id, string sessionID)
        {
            var body = @"{" 
                     + "\n" + @"    ""id"": 17,"
                     + "\n" + @"    ""jsonrpc"": ""2.0"","
                     + "\n" + @"    ""method"": ""call"","
                     + "\n" + @"    ""params"": {"
                     + "\n" + @"        ""model"": ""ims.renewal.reminder"","
                     + "\n" + @"        ""method"": ""unlink"","
                     + "\n" +         "\"" + "args" + "\"" + ": " + "[[" + id.ToString() + "]]" + ","
                     + "\n" + @"        ""kwargs"": {"
                     + "\n" + @"            ""context"": {"
                     + "\n" + @"                ""lang"": ""vi_VN"","
                     + "\n" + @"                ""tz"": ""Asia/Bangkok"","
                     + "\n" + @"                ""uid"": 51,"
                     + "\n" + @"                ""allowed_company_ids"": [1],"
                     + "\n" + @"                ""params"": {"
                     + "\n" + @"                    ""action"": 933,"
                     + "\n" + @"                    ""model"": ""ims.renewal.reminder"","
                     + "\n" + @"                    ""view_type"": ""list"","
                     + "\n" + @"                    ""cids"": 1,"
                     + "\n" + @"                    ""menu_id"": 208"
                     + "\n" + @"                }"
                     + "\n" + @"            }"
                     + "\n" + @"        }"
                     + "\n" + @"    }"
                     + "\n" + @"}";
            response = Api.PostObjectSessionID(url, "web/dataset/call_kw/ims.renewal.reminder/unlink", sessionID, body);
            return response;
        }

        // POST - IMS Gui Thông Tin Beta HCG (CRM to IMS)
        internal static IRestResponse CRMToIMS_SendInfo_BetaHCG(string url, string body, string? apiKey = null)
        {
            response = Api.PostObject(url, "api/v1/crm/beta_hcg", body, apiKey);
            return response;
        }
        #endregion

        #region Worldfone SouthTelecom
        // GET - Dialing / DialAnswer / HangUp / CDR
        internal static IRestResponse WorldfoneSouthTelecome(string url, string apiPath, string? apiKey = null)
        {
            response = Api.GetObject(url, apiPath, apiKey);
            return response;
        }
        #endregion
    }
}
