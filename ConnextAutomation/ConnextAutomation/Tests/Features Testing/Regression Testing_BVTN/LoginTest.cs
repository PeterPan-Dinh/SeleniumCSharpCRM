using AventStack.ExtentReports;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.UITest.Tests.Features_Testing.Regression_Testing_BVTN
{
    [TestFixture, Order(1)]
    internal class LoginTest : BaseTestCase
    {
        // Variables declare
        private static string? url = null;

        [Obsolete]
        readonly ExtentReports rep = ExtReportgetInstance();
        private string? summaryTC;

        [SetUp]
        public override void SetupTest()
        {
            //Data-driven for site testing
            verifyPoints.Clear();
            Driver.StartBrowser();
            LoginPage.configurationFile();
            url = LoginPage.url;
        }

        [Test, Category("BVTN - Regression Tests")]
        public void TC001_login_with_invalid_valid_account()
        {
            #region Variables declare
            string username = LoginPage.usernameBvtn;
            string password = LoginPage.passwordBvtn;
            string fullname = LoginPage.fullnameBvtn;
            string invalidUsername = LoginPage.usernameInvalid;
            string invalidPassword = LoginPage.passwordInvalid;
            #endregion

            #region Workflow scenario
            //Verify steps, if getting issue show warning message
            test = rep.CreateTest("Connext - Login Test 001");
            try
            {
                // Go to website
                LoginAction.Instance.NavigateSite(url);

                // Verify the login button is shown (Dang Nhap)
                verifyPoint = LoginAction.Instance.IsLoginButtonShown(10);
                verifyPoints.Add(summaryTC = "Verify Login button (Dang Nhap) is displayed after navigating to Website", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                #region login test - invalid account
                // Input User Name
                LoginAction.Instance.InputUserName(10, invalidUsername);

                // Input Password
                LoginAction.Instance.InputPassword(10, invalidPassword);

                // Click ('Dang Nhap') Login button
                LoginAction.Instance.ClickLoginButton(10);

                // Verify the warning message 'wrong Wrong login/password' ; / Sai tên đăng nhập/mật khẩu
                string msg = "Wrong login/password";
                verifyPoint = LoginAction.Instance.UserNamePasswordMessageGetText(10, msg);
                verifyPoints.Add(summaryTC = "Verify the message '" + msg + "' is shown", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Login test - valid account
                // Input User Name
                LoginAction.Instance.InputUserName(10, username);

                // Input Password
                LoginAction.Instance.InputPassword(10, password);

                // Click ('Dang Nhap') Login button
                LoginAction.Instance.ClickLoginButton(10);

                // Check if the alert popup 'WebRTC' is shown then click 'OK' button
                LoginAction.Instance.CheckIfAlertPopupBrowserShownThenClickOK(5);

                // Wait for Lead page load done
                LoginAction.Instance.WaitForElementVisible(30, LoginPage.allRenderers);
                //LoginAction.Instance.WaitForPageLoadDone(10);

                // Verify 'Lead' menu is shown after login successfully
                verifyPoint = LoginAction.Instance.IsLeadMenuShown(10);
                verifyPoints.Add(summaryTC = "Verify 'Lead' menu is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);

                // Verify User Name (full Name) menu is shown after login successfully
                verifyPoint = LoginAction.Instance.UserNameMenuGetText(10, fullname);
                verifyPoints.Add(summaryTC = "Verify User Name '" + fullname + "' menu is shown after login successfully", verifyPoint);
                ExtReportResult(verifyPoint, summaryTC);
                #endregion

                #region Logout test
                // Click logout
                LoginAction.Instance.ClicklogOut();

                // Verify the login button is shown (Login With Microsoft Account)
                verifyPoint = LoginAction.Instance.IsLoginButtonShown(1);
                verifyPoints.Add(summaryTC = "Verify Login button (Dang Nhap) is displayed after logout Website", verifyPoint);
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
