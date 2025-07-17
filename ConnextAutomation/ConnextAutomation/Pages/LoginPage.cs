using Connext.UITest.Core.BaseClass;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Connext.UITest.Pages
{
    internal class LoginPage : BasePageElementMap
    {
        // load config.xml file
        internal static readonly string xmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Config\" + @"Config.xml");
        internal static XmlDocument xmlDocLoad(string xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFilePath);
                return doc;
            }
            catch (Exception e) { return null; }
        }
        internal static string? projectName = null, siteName = null, url = null, instanceName = null, bearerToken = null;
        internal static XmlDocument configurationFile()
        {
            XmlDocument xdoc = xmlDocLoad(xmlFilePath);
            // Parse an XML file (Get Project & Site name to run)
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "project_name")
                {
                    projectName = node.InnerText;
                    if (projectName == null || projectName == "" || (projectName != "dkhn" && projectName != "bvtn" && projectName != "horus"))
                    {
                        Console.WriteLine("Project '" + projectName + "' does not exist!");
                        break;
                    }
                }
                if (node.Name == "site_name")
                {
                    siteName = node.InnerText;
                }
            }

            // Determine Project & Site name to run
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == projectName)
                {
                    foreach (XmlNode siteNode in node.ChildNodes)
                    {
                        if (siteNode.Name == siteName)
                        {
                            url = siteNode.Attributes[0].Value;
                            instanceName = siteNode["instanceName"].InnerText;
                            bearerToken = siteNode["tokens"].Attributes["bearerToken"].InnerText;
                        }
                    }
                }
            }
            return xdoc;
        }

        // Initiate variables
        internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");
        internal static string username = xdoc.XPathSelectElement("config/account/valid").Attribute("username").Value,
        password = xdoc.XPathSelectElement("config/account/valid").Attribute("password").Value,
        fullname = xdoc.XPathSelectElement("config/account/valid").Attribute("fullname").Value,
        usernameInvalid = xdoc.XPathSelectElement("config/account/invalid").Attribute("username").Value,
        passwordInvalid = xdoc.XPathSelectElement("config/account/invalid").Attribute("password").Value,
        email = xdoc.XPathSelectElement("config/account/email").Attribute("mail").Value,
        emailPass = xdoc.XPathSelectElement("config/account/email").Attribute("password").Value,
        revoceryEmailAddress = xdoc.XPathSelectElement("config/account/email").Attribute("recoveryemailaddr").Value,

        /// dkhn - ivf
        /// Admin (ivf)
        adminIVFUsername = xdoc.XPathSelectElement("config/account/ivf/admin").Attribute("username").Value,
        adminIVFPass = xdoc.XPathSelectElement("config/account/ivf/admin").Attribute("password").Value,
        adminIVFFullname = xdoc.XPathSelectElement("config/account/ivf/admin").Attribute("fullname").Value,
        /// Truc chat
        trucChatLeadIVFUsername = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("usernamelead").Value,
        trucChatLeadIVFEmail = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("emaillead").Value,
        trucChatLeadIVFPass = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("passwordlead").Value,
        trucChatIVFUsername = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("username").Value,
        trucChatIVFEmail = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("email").Value,
        trucChatIVFPass = xdoc.XPathSelectElement("config/account/ivf/trucchat").Attribute("password").Value,

        /// Telesale
        telesaleLeadIVFUsername = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("usernamelead").Value,
        telesaleLeadIVFEmail = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("emaillead").Value,
        telesaleLeadIVFPass = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("passwordlead").Value,
        telesaleIVFUsername = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("username").Value,
        telesaleIVFEmail = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("email").Value,
        telesaleIVFPass = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("password").Value,
        telesaleIVFTeam = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("team").Value,
        telesaleIVFnv1 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv1").Value,
        telesaleIVFnv2 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv2").Value,
        telesaleIVFnv3 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv3").Value,
        telesaleIVFnv4 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv4").Value,
        telesaleIVFnv5 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv5").Value,
        telesaleIVFnv6 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv6").Value,
        telesaleIVFnv7 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv7").Value,
        telesaleIVFnv8 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv8").Value,
        telesaleIVFnv9 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv9").Value,
        telesaleIVFnv10 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv10").Value,
        telesaleIVFnv11 = xdoc.XPathSelectElement("config/account/ivf/telesale").Attribute("nv11").Value,

        /// Tu van
        tuVanLeadIVFUsername = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("usernamelead").Value,
        tuVanLeadIVFEmail = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("emaillead").Value,
        tuVanLeadIVFPass = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("passwordlead").Value,
        tuVanIVFUsername = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("username").Value,
        tuVanIVFEmail = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("email").Value,
        tuVanIVFPass = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("password").Value,
        tuVanIVFTeam = xdoc.XPathSelectElement("config/account/ivf/tuvan").Attribute("team").Value,

        /// CSKH
        cskhLeadIVFUsername = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("usernamelead").Value,
        cskhLeadIVFEmail = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("emaillead").Value,
        cskhLeadIVFPass = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("passwordlead").Value,
        cskhIVFUsername = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("username").Value,
        cskhIVFEmail = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("email").Value,
        cskhIVFPass = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("password").Value,
        cskhIVFTeam = xdoc.XPathSelectElement("config/account/ivf/cskh").Attribute("team").Value,

        /// Khach doan
        khachDoanLeadUsername = xdoc.XPathSelectElement("config/account/khachdoan").Attribute("usernamelead").Value,
        khachDoanLeadEmail = xdoc.XPathSelectElement("config/account/khachdoan").Attribute("emaillead").Value,
        khachDoanLeadPass = xdoc.XPathSelectElement("config/account/khachdoan").Attribute("passwordlead").Value,

        /// bvtn
        usernameBvtn = xdoc.XPathSelectElement("config/account/bvtn_valid").Attribute("username").Value,
        passwordBvtn = xdoc.XPathSelectElement("config/account/bvtn_valid").Attribute("password").Value,
        fullnameBvtn = xdoc.XPathSelectElement("config/account/bvtn_valid").Attribute("fullname").Value;

        internal static WebDriverWait? wait;

        // Initiate the By objects for elements
        internal static By userNameTxt = By.Id("login");
        internal static By passWordTxt = By.Id("password");
        internal static By loginBtn = By.XPath(@"//button[@type='submit']");
        internal static By logOutDropdown = By.XPath(@"//div[contains(@class,'user_menu')]/button");
        internal static By logOutBtn = By.XPath(@"//a[.='Đăng xuất']");  //a[@data-menu='logout']
        internal static By kanbanRendererTable = By.XPath(@"//div[contains(@class,'kanban_renderer')]");
        internal static By listRenderer = By.XPath(@"//div[contains(@class,'list_renderer')]");
        internal static By kPIDashboardRenderer = By.XPath(@"//div[contains(@class,'ks_dashboard_ninja')]");
        internal static By allRenderers = By.XPath(@"//div[contains(@class,'kanban_renderer') or contains(@class,'kanban_quick_create') or contains(@class,'list_renderer') or contains(@class,'list_view') or contains(@class,'form_editable') or contains(@class,'form_readonly') or contains(@class,'ks_dashboard_ninja')]");
        internal static By crmMenu = By.XPath(@"//a[.='CRM']");
        internal static By leadMenu = By.XPath(@"//a[.='Lead']");
        internal static By kPIDashboardMenu = By.XPath(@"//a[.='KPI Trực chat']");
        internal static By wrongUserNPassMsg = By.XPath(@"//p[@role='alert']");
        internal static By userNameMenu = By.XPath(@"//div[contains(@class,'user_menu')]//span[.]");

        // Initiate the elements
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr=null) // Highlight & Un-Highlight Element
        {
            // Check if give color/setOrRemoveAttr with a specific color/setOrRemoveAttr, if no then will get blue color (by default)
            color ??= "blue"; setOrRemoveAttr ??= "removeAttribute";

            IJavaScriptExecutor? js = Driver.Browser as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, " border: 3px solid " + color + ";"); Thread.Sleep(150);
            js.ExecuteScript("arguments[0]." + setOrRemoveAttr + "('style', arguments[1]);", element, " border: 3px solid " + color + ";"); // un-highlight
            return element;
        }
        public IWebElement txtUserName(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(userNameTxt));
        }
        public IWebElement txtPassword(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(passWordTxt));
        }
        public IWebElement btnLogin(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(loginBtn));
        }
        public IWebElement dropdownLogOut(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(logOutDropdown));
        }
        public IWebElement btnLogOut(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(logOutBtn));
        }
        public IWebElement menuCRM(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(crmMenu));
        }
        public IWebElement menuLead(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leadMenu));
        }
        public IWebElement menuKPIDashboard(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(kPIDashboardMenu));
        }
        public IWebElement msgWrongUserNPass(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(wrongUserNPassMsg));
        }
        public IWebElement menuUserName(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(userNameMenu));
        }
    }

    internal sealed class LoginAction : BasePage<LoginAction, LoginPage>
    {
        #region Constructor
        private LoginAction() { }
        #endregion

        #region Items Action
        // Wait for element visible
        public LoginAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        public LoginAction WaitForNewWindowOpen(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                int previousWinCount = Driver.Browser.WindowHandles.Count;
                wait.Until(d => d.WindowHandles.Count == previousWinCount + 1);
            }
            return this;
        }

        // Wait for the popup Window closed
        public LoginAction WaitForPopupWindowClosed(int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                int previousWinCount = Driver.Browser.WindowHandles.Count;
                wait.Until(d => d.WindowHandles.Count < 2);
            }
            return this;
        }

        // Checking element exists or not
        public bool IsElementPresent(By by)
        {
            try
            {
                Driver.Browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        // Wait for element Invisible
        public LoginAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }


        // verify elements
        public bool IsLoginButtonShown(int timeoutInSeconds)
        {
            var iweb = Map.btnLogin(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute"); // keep highlight in red border if fail
                Driver.TakeScreenShot("ss_IsLoginBtnShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsCRMMenuShown(int timeoutInSeconds)
        {
            var iweb = Map.menuCRM(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsCRMmenuShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsLeadMenuShown(int timeoutInSeconds)
        {
            var iweb = Map.menuLead(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsLeadmenuShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool IsKPIDashboardMenuShown(int timeoutInSeconds)
        {
            var iweb = Map.menuKPIDashboard(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Displayed;
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_IsKPIDashboardShown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool UserNamePasswordMessageGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.msgWrongUserNPass(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_msgWrongUserNPass" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool UserNameMenuGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.menuUserName(timeoutInSeconds);
            bool element =  Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(Map.menuUserName(timeoutInSeconds), "red", "setAttribute");
                Driver.TakeScreenShot("ss_menuUserName" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }

        // Actions
        public LoginAction NavigateSite(string url)
        {
            Driver.Browser.Navigate().GoToUrl(string.Concat(url));
            return this;
        }
        public LoginAction InputUserName(int timeoutInSeconds, string text)
        {
            Map.HighlightElement(Map.txtUserName(timeoutInSeconds)).Clear();
            Map.HighlightElement(Map.txtUserName(timeoutInSeconds)).SendKeys(text);
            return this;
        }
        public LoginAction InputPassword(int timeoutInSeconds, string text)
        {
            Map.HighlightElement(Map.txtPassword(timeoutInSeconds)).Clear();
            Map.HighlightElement(Map.txtPassword(timeoutInSeconds)).SendKeys(text);
            return this;
        }
        public LoginAction ClickLoginButton(int timeoutInSeconds)
        {
            Map.HighlightElement(Map.btnLogin(timeoutInSeconds)).Click();
            return this;
        }

        public bool isAlertPresent()
        {
            bool presentFlag = false;
            try
            {
                // Check the presence of alert
                var alert = Driver.Browser.SwitchTo().Alert();
                // Alert present; set the flag
                presentFlag = true;
                // if present consume the alert
                alert.Accept();
            }
            catch (NoAlertPresentException ex)
            {
                // Alert not present
                return false; //ex.printStackTrace();
            }

            return presentFlag;

        }
        public LoginAction CheckIfAlertPopupBrowserShownThenClickOK(int timeoutInSeconds)
        {
            int time = 0;
            while (isAlertPresent() == false && time < timeoutInSeconds)
            {
                if (isAlertPresent() == true)
                {
                    Driver.Browser.SwitchTo().Alert().Accept(); //alert.Accept();
                    break;
                }
                if (time == timeoutInSeconds)
                {
                    Console.WriteLine("No alert browser displays!");
                }
                time++;
                Thread.Sleep(1000);
            }
            return this;
        }
        public LoginAction WaitForPageLoadDone(int timeoutInSeconds)
        {
            int timne = 0;
            while (timne <= timeoutInSeconds)
            {
                if (IsElementPresent(LoginPage.kanbanRendererTable) ||
                    IsElementPresent(LoginPage.listRenderer))
                { break; }

                if (timne == timeoutInSeconds)
                {
                    Console.WriteLine("Timeout on web page load in " + timeoutInSeconds + "s");
                    BaseTestCase.ExtReportResult(false, "Timeout on web page load in "+ timeoutInSeconds + "s");
                }

                timne++;
                Thread.Sleep(1000);
            }
            return this;
        }
        #endregion

        #region Built-in Actions
        public LoginAction LoginSite(int timeoutInSeconds, string? url = null, string? username = null, string? password = null)
        {
            // Check if user login with a specific url, if no then will get url from config.xml (by default)
            url ??= LoginPage.url;
            username ??= LoginPage.username;
            password ??= LoginPage.password;

            // Steps
            try
            {
                NavigateSite(url);
                WaitForElementVisible(timeoutInSeconds, LoginPage.loginBtn); // 60
                InputUserName(10, username);
                InputPassword(10, password);
                ClickLoginButton(10);
                CheckIfAlertPopupBrowserShownThenClickOK(5);
                WaitForElementVisible(timeoutInSeconds, LoginPage.kPIDashboardRenderer); //listRenderer
                //WaitForPageLoadDone(timeoutInSeconds);
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                BaseTestCase.ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong with Microsoft Login! Please check console log.");
            }

            return this;
        }

        public LoginAction LoginSiteXRender(int timeoutInSeconds, string? url = null, string? username = null, string? password = null, By? render=null)
        {
            // Check if user login with a specific url, if no then will get url from config.xml (by default)
            url ??= LoginPage.url;
            username ??= LoginPage.username;
            password ??= LoginPage.password;
            render ??= LoginPage.kPIDashboardRenderer;

            // Steps
            try
            {
                NavigateSite(url);
                WaitForElementVisible(timeoutInSeconds, LoginPage.loginBtn);
                InputUserName(10, username);
                InputPassword(10, password);
                ClickLoginButton(10);
                CheckIfAlertPopupBrowserShownThenClickOK(5);
                WaitForElementVisible(timeoutInSeconds, render);
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                BaseTestCase.ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong with Microsoft Login! Please check console log.");
            }

            return this;
        }

        public LoginAction LoginSiteBVTN(int timeoutInSeconds, string? url = null, string? username = null, string? password = null)
        {
            // Check if user login with a specific url, if no then will get url from config.xml (by default)
            if (url == null) url = LoginPage.url;
            if (username == null) username = LoginPage.usernameBvtn;
            if (password == null) password = LoginPage.passwordBvtn;

            // Steps
            try
            {
                NavigateSite(url);
                WaitForElementVisible(timeoutInSeconds, LoginPage.loginBtn); // 60
                InputUserName(10, username);
                InputPassword(10, password);
                ClickLoginButton(10);
                CheckIfAlertPopupBrowserShownThenClickOK(5);
                WaitForElementVisible(timeoutInSeconds, LoginPage.listRenderer);
                //WaitForPageLoadDone(timeoutInSeconds);
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                BaseTestCase.ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong with Microsoft Login! Please check console log.");
            }

            return this;
        }

        public LoginAction ClicklogOut()
        {
            Map.HighlightElement(Map.dropdownLogOut(10)).Click();
            Map.HighlightElement(Map.btnLogOut(10)).Click();
            WaitForElementVisible(10, LoginPage.loginBtn);
            return this;
        }
        #endregion
    }
}
