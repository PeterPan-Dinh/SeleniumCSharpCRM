using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(7)]
    internal class ECHTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        /// table renderer (used for wait method)
        private const string listRenderer = "list_renderer";
        #endregion

        [Test, Category("DKHN - Regression Tests")]
        public void TC001_ECH_Facebook_Create_Lead()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, socialNetworkSite = SocialNetworkPage.facebookSite, userName = SocialNetworkPage.facebookUserName,
            adminIVFUsername = LoginPage.adminIVFUsername, adminIVFPass = LoginPage.adminIVFPass,
            dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
            const string searchboxRole = "searchbox", inputSearch = "qa auto", ech = "ECH", guiTinNhanChoKHPlaceHolder = "Gửi tin nhắn cho khách hàng...",
            guiClass = "Composer_buttonSend";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - ECH Test 001 - Facebook create Lead");
            try
            {
                #region Login Facebook
                // Log into Facebook site
                SocialNetworkAction.Instance.LoginFacebook(60);

                // Click 'Message' button
                SocialNetworkAction.Instance.ClickMessageFBBtn(10)
                                            .WaitForElementVisible(10, SocialNetworkPage.messageFBDialog); Thread.Sleep(1000);

                //// Input to search Messenger and then click on it --> FB UI was changed so that cannot wait link 'Information technology company...'
                //SocialNetworkAction.Instance.InputSearchMessengerFBDialogTxt(10, "Connext");
                //SocialNetworkAction.Instance.WaitForElementVisible(10, SocialNetworkPage.searchResultsLinkFB(".='Information technology company' or .='Công ty công nghệ thông tin'", "Connext"))
                //                            .ClickSearchResultsLink(10, ".='Information technology company' or .='Công ty công nghệ thông tin'", "Connext")
                //                            .WaitForElementVisible(10, SocialNetworkPage.chatMessageFBTab);

                // Click 'Connext' icon chat to chat
                SocialNetworkAction.Instance.ClickSearchResultsLink(10, "Connext")
                                            .WaitForElementVisible(10, SocialNetworkPage.chatMessageFBTab);

                // Chat message
                string inputChat = "KV xin chào fb connext dkhn! " + dateTimeNow;
                SocialNetworkAction.Instance.InputChatMessageFBTxt(10, inputChat);
                GeneralAction.Instance.PressEnterKeyboard(); Thread.Sleep(1000);
                #endregion

                #region Open a new tab to navigate to Connext site
                // Open a new tab 
                Driver.Browser.SwitchTo().NewWindow(WindowType.Tab);

                // Switch to new tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                // Log into the application
                LoginAction.Instance.LoginSite(60, urlInstance, adminIVFUsername, adminIVFPass);

                // Go to Home menu >> Lead page
                GeneralAction.Instance.GoToLeftMenu(10, General.lead).Sleep(1000).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(30);
                #endregion

                #region  Verify a new lead is created after client's FB send a chat
                // Search Lead
                // Click on 'Gỡ' icon to remove the default filter at 'search box' ('')
                GeneralAction.Instance.CLickMenuTitle(10, General.searchBoxRemoveDataIcon); Thread.Sleep(1000);

                /// Input a value in seachbox to Search
                GeneralAction.Instance.SearchViewInput(60, General.lead, "Facebook User - " + inputChat).Sleep(2000);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(30, General.spinnerLoading); Thread.Sleep(100);
                }

                // Verify a new lead is created after client's FB send a chat
                string data = "Facebook User"; // userName
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(30, "1", "contact_name", data);
                verifyPoints.Add(summaryTC = "Verify a new lead is created after client's FB send a chat: Lead với Tên liên hệ là '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Click checkbox at a specific row(field value)
                GeneralAction.Instance.ClickDataRowTableWithValueColumn(30, "Facebook User - " + inputChat) // userName + " - " + inputChat
                                      .WaitForElementVisible(30, General.formEditable);

                // Verify lead's chatter shows message from the new client with content
                verifyPoint = GeneralAction.Instance.ChatterMessageRightPaneGetText(360, data = inputChat);
                verifyPoints.Add(summaryTC = "Verify lead's chatter shows message from the new client with content: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Get 'user endpoint id' value
                /// Click 'Kết nối' (notebook header) tab to get 'user endpoint id'
                GeneralAction.Instance.CLickNotebookHeader(30, "Kết nối")
                                      .WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1000);

                /// Get 'user endpoint id' value
                string user_endpoint_id = GeneralAction.Instance.DataRecordRowListTableNotebookGetText(30, "Facebook User", "@name='user_endpoint_id'"); // userName, "@name='user_endpoint_id'") (Định danh người dùng cuối)
                string user_endpoint_external_id = GeneralAction.Instance.DataRecordRowListTableNotebookGetText(30, "Facebook User", "@name='user_endpoint_external_id'"); // userName, "@name='user_endpoint_id'") (Định danh bên ngoài đầy đủ)
                #endregion

                #region Reply for Facebook client
                //// Click to select communication-port
                //GeneralAction.Instance.CLickCommunicationPortChatterDropdown(10)
                //                      .CLickCommunicationPortChatterItemInDropdown(10, "Connext Test Page (Facebook Page Messenger)  - Facebook User");

                // Input to Gửi tin nhắn nội bộ...
                string msgInput = "Hi! ech lead kính chào KH! " + dateTimeNow;
                GeneralAction.Instance.InputByAttributeValue(30, guiTinNhanChoKHPlaceHolder, msgInput, "textarea");

                // Click 'Gửi' button
                GeneralAction.Instance.CLickButtonTitle(30, guiClass)
                                      .WaitForElementInvisible(30, General.spinnerLoading)
                                      .WaitForElementVisible(30, General.elementHtml("button", "Composer_buttonSend", "[@disabled]"))
                                      .WaitForElementVisible(30, General.chatterMessage(msgInput));

                // Verify the sent message is shown at Chattter with content
                verifyPoint = GeneralAction.Instance.ChatterMessageRightPaneGetText(360, data = msgInput);
                verifyPoints.Add(summaryTC = "Verify the sent message is shown at Chattter with content: 'Hôm nay - " + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Delete the created lead and 'user endpoint id' (at Contact)
                // Delete the created lead
                /// Click on 'Thuc Hien' --> 'Xoa'
                GeneralAction.Instance.ThucHienXoaDelete(60); Thread.Sleep(1500);

                /// Check if the spinner Loading icon is shown then wait for it to load done
                if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                {
                    GeneralAction.Instance.WaitForElementInvisible(10, General.spinnerLoading); Thread.Sleep(100);
                }

                /// Verify the created Lead is deleted 
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created Lead is deleted: '" + (data=inputChat) + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                /// Go to 'Lien he' >> 'User endpoint' menu to delete 'user endpoint id'
                GeneralAction.Instance.GoToLeftMenu(10, General.lienHe, "Khách hàng cá nhân", listRenderer)
                                      .CLickMenuTitle(30, General.nguoiDungCuoi)
                                      .WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1000);

                /// Search (filter) 'user endpoint id' to delete
                GeneralAction.Instance.CLickButtonTitle(30, General.timKiem)
                                      .WaitForElementVisible(30, General.dropDownShow)
                                      .CLickButtonTitle(30, "Nâng cao")
                                      .WaitForElementVisible(30, General.DialogShow)
                                      .CLickButtonTitle(30, " Thêm bộ lọc ")
                                      .WaitForElementVisible(30, General.elementHtml("div", "node_children_container"))
                                      .ClickHtmlElement(30, "div", "field_selector o_edit_mode")
                                      .InputHtmlElement(30, "div", "field_selector_search", "Định danh người dùng cuối", "//input[@class='o_input']");
                /// Press Enter
                GeneralAction.Instance.PressEnterKeyboard(); Thread.Sleep(1000);
                /// Input 'user endpoint id' to filter
                GeneralAction.Instance.InputHtmlElement(30, "input", "leaf_value_input", user_endpoint_id)
                                      .CLickButtonTitleInDialog(30, "Lưu");

                /// Verify the 'user endpoint id' is shown after searching
                string nameAttrVal = "user_endpoint_id";
                verifyPoint = GeneralAction.Instance.DataColByTxtOrAttrValInRowTableGetText(30, "1", nameAttrVal, data = user_endpoint_id);
                verifyPoints.Add(summaryTC = "Verify the 'user endpoint id' is shown after searching: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Delete 'user endpoint id'
                /// Click to check the checkbox at the 'x' row and then click 'Thực hiện' >> Xóa
                GeneralAction.Instance.ClickToCheckboxInRowTable(30, "1").ThucHienXoaDelete(60);

                /// Verify the created 'user endpoint id' is deleted 
                verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                verifyPoints.Add(summaryTC = "Verify the Created 'user endpoint id' is deleted: '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Back to Facebook (client) to verify the received chat message
                // Switch to the 1st tab
                Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());

                // Verify the received chat message (from Connext)
                verifyPoint = SocialNetworkAction.Instance.IsChatMessageFBShown(10, data = msgInput);
                verifyPoints.Add(summaryTC = "Verify the received chat message (from Connext): '" + data + "'", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            #endregion
        }
    }
}
