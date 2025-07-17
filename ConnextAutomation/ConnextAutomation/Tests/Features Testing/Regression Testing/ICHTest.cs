using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Generals;
using Connext.UITest.Pages;
using OpenQA.Selenium;
using SeleniumGendAdmin.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing
{
    [TestFixture, Order(6)]
    internal class ICHTest : BaseTestCase
    {
        #region Variables declare
        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;
        private static readonly string dateTimeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss").Replace("/", "").Replace(" ", "").Replace(":", "");
        /// table renderer (used for wait method)
        private string crmRender = "kanban_renderer",
        /// global data
        leadName = "QA auto lead dakhoa " + dateTimeNow,
        contactName = "QA auto lh dakhoa " + dateTimeNow;
        #endregion

        [Test, Category("DKHN - Regression Tests"), Ignore("")] // This feature can be changed according to customer requirements
        public void TC001_ICH_Opportunity()
        {
            #region Variables declare
            string urlInstance = LoginPage.url, instanceName = LoginPage.instanceName;
            string? dienThoaiData = null, opportunityName = "Cơ hội của " + contactName;
            const string inputSearch = "qa auto", searchboxRole = "searchbox", toChucLienHe = "Tổ chức/Liên hệ", coHoi = "Cơ hội", email = "Email",
            dienThoai = "Điện thoại", doanhThuDuKien = "Doanh thu dự kiến", vdGiaSanPhamPlaceHolder = "vd: Giá sản phẩm", them = "Thêm", sua = "Sửa",
            duLieuMoi = "Dữ liệu mới", daDatHen = "Đã đặt hẹn", nguoiNhan = "Người nhận?", guiTinNhanNoiBoPlaceHolder = "Gửi tin nhắn nội bộ...", guiClass = "Composer_buttonSend",
            protonMail = "qatestcnext01@proton.me", emailWebsite = "https://proton.me/mail", thetLapChung = "Thiết lập chung";
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - ICH Test 001 - Opportunity");
            try
            {
                if (urlInstance.Contains(instanceName))
                {
                    #region Go to Email (Proton) and check if there's any existing email received (from Connext) then delete it
                    // Go to Email (Proton) to delete all emails
                    LoginAction.Instance.NavigateSite(emailWebsite);

                    // Login to Proton
                    EmailAction.Instance.ClickProtonSignInLink(10)
                                        .WaitForElementVisible(60, EmailPage.protonSignInBtn)
                                        .InputProtonEmailTxt(10, EmailPage.email)
                                        .InputProtonPasswordTxt(10, EmailPage.emailPass)
                                        .ClickProtonSignInBtn(10)
                                        .WaitForElementVisible(60, EmailPage.protonInboxContainer);

                    // Click "Inbox" at the left pane to check if there any emails then delete it
                    EmailAction.Instance.ClickInboxProtonLink(10) //; Thread.Sleep(2000);
                                        .WaitForElementVisible(60, EmailPage.protonInboxContainer); Thread.Sleep(1000);

                    // Check if there's any existing email received (from Connext) then delete all
                    if (EmailAction.Instance.IsElementPresent(EmailPage.receiveProtonRowLink("1")))
                    {
                        EmailAction.Instance.ClickSelectAllInboxProtonCkb(10)
                                        .WaitForElementVisible(10, General.elementHtml("span", "Deselect all messages"))
                                        .ClickDeleteMailProtonBtn(10)
                                        .WaitForElementVisible(10, By.XPath(@"//span[contains(.,'Conversation moved to Trash') or contains(.,'conversation moved to Trash') or contains(.,'conversations moved to Trash') or contains(.,'Conversations moved to Trash')]")); Thread.Sleep(1000);
                    }
                    #endregion

                    #region Open a new tab to navigate to Connext site
                    // Open a new tab 
                    Driver.Browser.SwitchTo().NewWindow(WindowType.Tab);

                    // Switch to new tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last());

                    // Log into the application
                    LoginAction.Instance.LoginSite(60, urlInstance);

                    // Go to CRM page
                    GeneralAction.Instance.CLickMenuTitle(30, General.homeMenu)
                                          .WaitForElementVisible(30, General.dropDownShow)
                                          .CLickItemInDropdown(30, General.dataCuaToi)
                                          .WaitForElementVisible(30, General.listRenderer).Sleep(500)
                                          .CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10).Sleep(1000);

                    // search the existing 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.CLickMenuTitle(30, General.searchBoxRemoveDataIcon)
                                          .SearchViewInput(30, "Cơ hội", inputSearch);
                                          //.InputByAttributeValue(30, searchboxRole, inputSearch)
                                          //.WaitForElementVisible(30, General.searchBoxDropdown) // wait for sb dropdown displays
                                          //.CLickItemInDropdownSearchBox(30, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                          //.WaitForElementInvisible(30, General.searchBoxDropdown).Sleep(2000)
                                          //.CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);

                    /// Check if there's any existing 'Co hoi' (auto) then delete all
                    if (GeneralAction.Instance.IsElementPresent(General.tableCheckboxInRow("1")))
                    {
                        /// Delete all existing 'Co hoi' (auto)
                        GeneralAction.Instance.ClickCheckboxAlltable(30)
                                              .CLickButtonTitle(30, General.thucHien).WaitForElementVisible(10, General.dropDownShow)
                                              .CLickItemInDropdown(30, "Xoá").WaitForElementVisible(10, General.DialogShow)
                                              .CLickButtonTitle(30, "Đồng ý").WaitForElementInvisible(10, General.DialogShow).Sleep(1000);

                        /// Verify all existing 'Co hoi' (auto) are deleted 
                        verifyPoint = GeneralAction.Instance.IsCheckboxInRowTableDeleted("1");
                        verifyPoints.Add(summaryTC = "Verify all existing 'Co hoi' (auto) at 'Chu trinh' list are deleted: '" + inputSearch + "'", verifyPoint);
                        ExtReportResult(verifyPoint, summaryTC);
                    }

                    /// Click 'kanban' button at 'switch buttons' (views: List, Kanban, Calendar, Pivot, ...)
                    GeneralAction.Instance.CLickButtonTitle(30, General.kanban); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.kanbanRenderer); Thread.Sleep(1500);
                    #endregion

                    #region Create a new 'Co hoi'
                    // Create a new 'Co hoi'
                    /// Click "MOI" button
                    GeneralAction.Instance.CLickButtonTitle(30, General.moi)
                                          .WaitForElementVisible(30, General.kanbanQuickCreate);

                    // Input data to create 'Co hoi'
                    GeneralAction.Instance.InputFieldLabel(30, toChucLienHe, contactName).PressEnterKeyboard()
                                          //.InputFieldLabel(30, coHoi, opportunityName, vdGiaSanPhamPlaceHolder) // auto fill 'Cơ hội của ...'
                                          .InputFieldLabel(30, email, "qaauto@connext.com")
                                          .InputFieldLabel(30, dienThoai, dienThoaiData = "02219999001")
                                          .InputFieldLabel(30, doanhThuDuKien, "9.000.000")
                                          .CLickPriorityStar(30, doanhThuDuKien, "Rất cao")
                                          .WaitForElementVisible(10, General.formEditable).Sleep(500)
                                          .CLickButtonTitleInKabanDialog(30, them); Thread.Sleep(2500);
                    #endregion

                    #region Add Followers and verify the added Follower at chatter right pane
                    // Click on Record title of a column
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(30, duLieuMoi, opportunityName); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.formEditable); Thread.Sleep(1000);

                    // Click 'Add Followers' icon to add followers
                    GeneralAction.Instance.CLickButtonTitle(30, "Show Followers")
                                      .CLickMainMenuTitle(30, " Thêm Người theo dõi ")
                                      .WaitForElementVisible(30, General.DialogShow);
                    GeneralAction.Instance.InputFieldLabelDialog(30, nguoiNhan, protonMail).PressEnterKeyboard().Sleep(1000); // protonMail for 'Đa Khoa'
                    GeneralAction.Instance.CLickButtonTitleInDialog(30, "Thêm Người theo dõi").Sleep(500)
                                      .WaitForElementInvisible(30, General.DialogShow); Thread.Sleep(1000); // 5000

                    /// Check if the spinner Loading icon is shown then wait for it to load done
                    if (GeneralAction.Instance.IsElementPresent(General.spinnerLoading))
                    {
                        GeneralAction.Instance.WaitForElementInvisible(30, General.spinnerLoading); Thread.Sleep(100);
                    }

                    // Verify the added followers is shown at Chatter right pane
                    string data = protonMail;
                    verifyPoint = GeneralAction.Instance.FollowerListGetText(30, "2", data);
                    verifyPoints.Add(summaryTC = "Verify the added follower is shown at Chatter (Đến): '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Input to Gửi tin nhắn nội bộ...
                    const string msgInput = "Hi! ich xin chao nvnb";
                    GeneralAction.Instance.InputByAttributeValue(30, guiTinNhanNoiBoPlaceHolder, msgInput, "textarea");

                    // Click 'Gửi' button
                    GeneralAction.Instance.CLickButtonTitle(30, guiClass)
                                          .WaitForElementInvisible(30, General.spinnerLoading)
                                          .WaitForElementVisible(30, General.elementHtml("button", "Composer_buttonSend", "[@disabled]"))
                                          .WaitForElementVisible(30, General.chatterMessage(msgInput));
                    #endregion

                    #region Back to Email (1st tab) to verify the received email
                    // Switch to the 1st tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.First());

                    // Click "Inbox" at the left pane to check if there any emails then delete it
                    EmailAction.Instance.ClickInboxProtonLink(10); Thread.Sleep(2000);
                    Driver.Browser.Navigate().Refresh();

                    // Wait for Inbox page load done
                    EmailAction.Instance.WaitForElementVisible(30, EmailPage.receiveProtonRowLink("contains(.,'Re: " + opportunityName + "')"));

                    // Verify the received email (from Connext)
                    verifyPoint = EmailAction.Instance.ReceiveMailProtonRowLinkGetText(10, "1", data = "Re: " + opportunityName+ "")
                               && EmailAction.Instance.ReceiveMailProtonRowLinkGetText(10, "2", data = "Invitation to follow Lead/Cơ hội: " + opportunityName+ "");
                    verifyPoints.Add(summaryTC = "Verify Proton is received email from Connext: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Reply email to Connext
                    /// Click the received email to reply
                    EmailAction.Instance.ClickReceiveProtonRowLink(10, "1");

                    // Wait for mail box load done
                    By msgRecipient = By.XPath(@"//span[contains(.,'Connext Admin<connext@gend.vn>')]"); // old: <notification@connext.biz>
                    EmailAction.Instance.WaitForElementVisible(30, msgRecipient); Thread.Sleep(1000);

                    // Switch to Iframe Email Content (to verify the content)
                    var iFrameEmailContent = Driver.Browser.FindElement(By.XPath(@"//iframe[@title='Email content']"));
                    Driver.Browser.SwitchTo().Frame(iFrameEmailContent); Thread.Sleep(1000);

                    // Verify the email content received
                    verifyPoint = EmailAction.Instance.MessageRecipientContentProtonGetText(10, data = msgInput);
                    verifyPoints.Add(summaryTC = "Verify Proton is received email from Connext with Content: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);

                    // Switch to default Iframe (to click Reply button)
                    Driver.Browser.SwitchTo().DefaultContent(); Thread.Sleep(1000);

                    // Click Reply button
                    EmailAction.Instance.ClickReplyMailProtonBtn(10)
                                        .WaitForElementVisible(10, By.XPath(@"//section[contains(@class,'relative composer-content')]")); Thread.Sleep(1000);

                    // Switch to Iframe 'Email composer'
                    var iFrameEmailComposer = Driver.Browser.FindElement(By.XPath(@"//iframe[@title='Email composer']"));
                    Driver.Browser.SwitchTo().Frame(iFrameEmailComposer); Thread.Sleep(1000);

                    // Input message to reply
                    EmailAction.Instance.InputProtonEditorTxt(10, "Đã nhận được rồi! ich");

                    // Switch to default Iframe (to click Send button)
                    Driver.Browser.SwitchTo().DefaultContent(); Thread.Sleep(1000);

                    // Click Send button
                    EmailAction.Instance.ClickSendMailProtonBtn(10)
                                        .WaitForElementVisible(10, By.XPath(@"//span[.='Message sent.']"));

                    Thread.Sleep(1000);

                    /*
                    // Delete all the received email (from Connext)
                    /// CLick checkbox at the existing emails (to delete)
                    EmailAction.Instance.ClickReceiveProtonRowCheckbox(10, "1")
                                        .ClickReceiveProtonRowCheckbox(10, "2")
                                        .ClickDeleteMailProtonBtn(10)
                                        .WaitForElementInvisible(10, EmailPage.receiveProtonRowLink("contains(.,'Invitation to follow Lead/Cơ hội: Cơ hộicủa QA auto 01')"));
                    */
                    #endregion

                    #region Switch to 'Connext' tab and go to Settings-General to fetch email
                    // Switch to the 1st tab
                    Driver.Browser.SwitchTo().Window(Driver.Browser.WindowHandles.Last()); Thread.Sleep(2000);
                    Driver.Browser.Navigate().Refresh(); Thread.Sleep(5000);

                    //// Check if alert Popup browser shows then click OK button
                    //LoginAction.Instance.CheckIfAlertPopupBrowserShownThenClickOK(30); Thread.Sleep(1000);

                    // Wait for CRM - 'Co hoi' page load done 
                    GeneralAction.Instance.WaitForElementVisible(30, General.formEditable); Thread.Sleep(1000);

                    // Fetch email
                    /// Go to 'Thiet lap' >> 'Thiet lap chung' page
                    GeneralAction.Instance.CLickMenuTitle(30, General.homeMenu)
                                          .WaitForElementVisible(30, General.dropDownShow)
                                          .CLickItemInDropdown(30, General.thietLap)
                                          .WaitForElementVisible(30, General.formEditable)
                                          .CLickSettingsLeftMenuTitle(30, thetLapChung)
                                          .WaitForElementVisible(30, General.elementHtml("div", "app_settings_block", "[@data-key='general_settings']")); Thread.Sleep(1000);

                    /// Go to 'Custom Email Servers' >> Incomming Email Server (Máy chủ Nhận Email)
                    GeneralAction.Instance.CLickButtonTitle(30, "Máy chủ Nhận Email")
                                          .WaitForElementVisible(30, General.listRenderer);

                    /// Click "Tên" Máy chủ Nhận Email
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(30, "info")
                                          .WaitForElementVisible(30, General.formEditable); Thread.Sleep(1000);

                    /// Click 'fetch email' (Lấy về Ngay) button 
                    GeneralAction.Instance.CLickButtonTitle(30, "Lấy về Ngay")
                                          .WaitForElementVisible(10, General.elementHtml("button", "fetch_mail", "[@disabled]"))
                                          .WaitForElementInvisible(10, General.elementHtml("button", "fetch_mail", "[@disabled]")); Thread.Sleep(1000);

                    /// Click 'fetch email' (Lấy về Ngay) button 
                    GeneralAction.Instance.CLickButtonTitle(30, "Lấy về Ngay")
                                          .WaitForElementVisible(10, General.elementHtml("button", "fetch_mail", "[@disabled]"))
                                          .WaitForElementInvisible(10, General.elementHtml("button", "fetch_mail", "[@disabled]")); Thread.Sleep(1000);

                    /// Back to 'Incoming EMail Server' by clicking title 'breadcrumb'
                    GeneralAction.Instance.CLickItemBreadcrumb(30, "Máy chủ nhận Mail")
                                          .WaitForElementVisible(10, General.listRenderer);

                    /// Click "Tên" Máy chủ Nhận Email
                    GeneralAction.Instance.ClickDataRowTableWithValueColumn(30, "catchall")
                                          .WaitForElementVisible(30, General.formEditable); Thread.Sleep(1000);

                    /// Click 'fetch email' (Lấy về Ngay) button 
                    GeneralAction.Instance.CLickButtonTitle(30, "Lấy về Ngay")
                                          .WaitForElementVisible(30, General.elementHtml("button", "fetch_mail", "[@disabled]"))
                                          .WaitForElementInvisible(30, General.elementHtml("button", "fetch_mail", "[@disabled]")); Thread.Sleep(1000);

                    /// Click 'fetch email' (Lấy về Ngay) button 
                    GeneralAction.Instance.CLickButtonTitle(30, "Lấy về Ngay")
                                          .WaitForElementVisible(30, General.elementHtml("button", "fetch_mail", "[@disabled]"))
                                          .WaitForElementInvisible(30, General.elementHtml("button", "fetch_mail", "[@disabled]")); Thread.Sleep(1000);
                    #endregion

                    #region Go to 'Co hoi' detail to verify the reply email
                    // Go to 'Data cua toi' page
                    GeneralAction.Instance.CLickMenuTitle(30, General.homeMenu)
                                          .WaitForElementVisible(30, General.dropDownShow)
                                          .CLickItemInDropdown(30, General.dataCuaToi)
                                          .WaitForElementVisible(30, General.listRenderer); Thread.Sleep(1500);

                    // search the existing 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.CLickMenuTitle(30, General.searchBoxRemoveDataIcon)
                                          .SearchViewInput(30, "Cơ hội", inputSearch);
                                          //.InputByAttributeValue(30, searchboxRole, inputSearch)
                                          //.WaitForElementVisible(30, General.searchBoxDropdown) // wait for sb dropdown displays
                                          //.CLickItemInDropdownSearchBox(30, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                          //.WaitForElementInvisible(30, General.searchBoxDropdown).Sleep(2000)
                                          //.CheckIfSpinnerLoadingIconShownThenWaitLoadDone(10);

                    /// Click 'kanban' button at 'switch buttons' (views: List, Kanban, Calendar, Pivot, ...)
                    GeneralAction.Instance.CLickButtonTitle(30, General.kanban); Thread.Sleep(1000);
                    GeneralAction.Instance.WaitForElementVisible(30, General.kanbanRenderer); Thread.Sleep(1500);

                    // Click on Record title of a column
                    GeneralAction.Instance.CLickRecordTitleKanbanInCol(30, duLieuMoi, opportunityName)
                                          .WaitForElementVisible(30, General.formEditable); Thread.Sleep(1000);

                    // Verify the reply email
                    verifyPoint = GeneralAction.Instance.ChatterMessageRightPaneGetText(400, data = "Đã nhận được rồi! ich");
                    verifyPoints.Add(summaryTC = "Verify the reply email is shown at Chatter: 'Hôm nay - " + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion

                    #region Delete 'Co hoi'
                    // Click on 'Thuc Hien' --> 'Xoa'
                    GeneralAction.Instance.CLickButtonTitle(30, General.thucHien).WaitForElementVisible(30, General.dropDownShow)
                                          .CLickItemInDropdown(30, "Xoá").WaitForElementVisible(30, General.DialogShow)
                                          .CLickButtonTitle(30, "Đồng ý").Sleep(2000).CheckIfSpinnerLoadingIconShownThenWaitLoadDone(30).Sleep(100);

                    // Check if 'Lỗi Truy Cập' popup displays then click 'Đồng ý' button
                    if (GeneralAction.Instance.IsElementPresent(General.DialogShow))
                    {
                        GeneralAction.Instance.CLickButtonTitleInDialog(10, "Đồng ý")
                                              .WaitForElementInvisible(10, General.DialogShow); Thread.Sleep(1000);
                    }

                    // Back to 'Chu Trinh' page
                    GeneralAction.Instance.CLickItemBreadcrumb(30, "Chu trình")
                                          .WaitForElementVisible(30, General.rendererTable(crmRender));

                    // search the deleted 'Cơ hội'
                    /// Input a value in seachbox to Search
                    GeneralAction.Instance.SearchViewInput(30, "Cơ hội", inputSearch);
                                          //.InputByAttributeValue(30, searchboxRole, inputSearch)
                                          //.WaitForElementVisible(30, General.searchBoxDropdown) // wait for sb dropdown displays
                                          //.CLickItemInDropdownSearchBox(30, "Cơ hội", inputSearch) // search data with filter 'Cơ hội'
                                          //.WaitForElementInvisible(30, General.searchBoxDropdown); Thread.Sleep(1000);

                    // Verify the created 'Co hoi' is deleted
                    verifyPoint = GeneralAction.Instance.IsTitleRecordKanbanInColumnDeleted(duLieuMoi, data = opportunityName);
                    verifyPoints.Add(summaryTC = "Verify the created 'Cơ hội' is deleted: '" + data + "'", verifyPoint);
                    ExtReportResult(verifyPoint, summaryTC);
                    #endregion         
                }
                else
                {
                    Console.WriteLine(summaryTC = "Notes: This test case is only executed on 'qa-odoo' (qa-bvdkhn) site !!!");
                    test.Log(Status.Info, summaryTC);
                }
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
