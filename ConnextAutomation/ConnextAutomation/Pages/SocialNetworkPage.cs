using AventStack.ExtentReports.Gherkin.Model;
using Connext.UITest.Core.BaseClass;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Generals;
using Microsoft.VisualBasic.ApplicationServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumGendAdmin.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Connext.UITest.Pages
{
    internal class SocialNetworkPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");
        internal static string facebookSite = xdoc.XPathSelectElement("config/account/facebook").Attribute("site").Value,
        facebookEmailOrPhone = xdoc.XPathSelectElement("config/account/facebook").Attribute("emailorphone").Value,
        facebookPassword = xdoc.XPathSelectElement("config/account/facebook").Attribute("password").Value,
        facebookUserName = xdoc.XPathSelectElement("config/account/facebook").Attribute("username").Value,
        zaloSite = xdoc.XPathSelectElement("config/account/zalo").Attribute("site").Value;


        // Initiate the By objects for elements
        /// <summary>
        /// Facebook page login (By objects for elements)
        /// </summary>
        internal static By emailOrPhoneFBTxt = By.Id("email");
        internal static By passwordFBTxt = By.Id("pass");
        internal static By userNameAccount(string username) => By.XPath(@"//span[.='" + username + "']");
        internal static By loginFBBtn = By.XPath(@"//button[@name='login']");
        internal static By searchFaceBookTxt = By.XPath(@"//input[@aria-label='Search Facebook' or @aria-label='Tìm kiếm trên Facebook']");
        internal static By seeAllFBBtn = By.XPath(@"//a[@aria-label='See all' or @aria-label='Xem tất cả']");
        internal static By searchResultsLinkFB(string subTitle, string title) => By.XPath(@"//div[" + subTitle + "]/preceding-sibling::div[.='" + title + "']");
        internal static By searchResultsLinkFB(string title) => By.XPath(@"//span[@dir='auto'][.='" + title + "']");
        internal static By findFriendsFBBtn = By.XPath(@"//div[@aria-label='Account controls and settings']//span[.='Find friends']");
        internal static By messageFBBtn = By.XPath(@"//div[@aria-label='Account controls and settings' or @aria-label='Account Controls and Settings']//div[contains(@aria-label,'Messenger') and not(@aria-hidden)]");
        internal static By messageFBDialog = By.XPath(@"//div[@aria-label='Messenger'and @role='dialog']");
        internal static By searchMessengerFBDialogTxt = By.XPath(messageFBDialog.ToString().Remove(0, 10) + "//input[@type='search']");
        internal static By chatMessageFBTab = By.XPath(@"//div[@role='none' and (@tabindex)]/div");
        internal static By chatMessageFBTxt = By.XPath(@"//div[@role='textbox' and (@aria-describedby)]/p");
        internal static By chatMessageFBReceiveReply(string msgReply) => By.XPath(@"//div[contains(@aria-label,'Messages in conversation with')]//span[contains(.,'" + msgReply + "')]");

        // Initiate the elements
        /// Highlight & Un-Highlight Element
        public IWebElement HighlightElement(IWebElement element, string? color = null, string? setOrRemoveAttr = null)
        {
            // Check if give color/setOrRemoveAttr with a specific color/setOrRemoveAttr, if no then will get red color (by default)
            if (color == null) color = "red"; if (setOrRemoveAttr == null) setOrRemoveAttr = "removeAttribute";

            IJavaScriptExecutor? js = Driver.Browser as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].setAttribute('style', arguments[1]);", element, " border: 3px solid " + color + "; "); Thread.Sleep(150);
            js.ExecuteScript("arguments[0]." + setOrRemoveAttr + "('style', arguments[1]);", element, " border: 3px solid " + color + ";"); // un-highlight
            return element;
        }

        /// <summary>
        /// Facebook page login (IWeb elements)
        /// </summary>
        public IWebElement txtEmailOrPhoneFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(emailOrPhoneFBTxt));
        }
        public IWebElement txtPasswordFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(passwordFBTxt));
        }
        public IWebElement btnLoginFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(loginFBBtn));
        }
        public IWebElement txtSearchFaceBook(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(searchFaceBookTxt));
        }
        public IWebElement btnSeeAllFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(seeAllFBBtn));
        }
        public IWebElement linkSearchResultsFB(int timeoutInSeconds, string subTitle, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(searchResultsLinkFB(subTitle, title)));
        }
        public IWebElement linkSearchResultsFB(int timeoutInSeconds, string title)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(searchResultsLinkFB(title)));
        }
        public IWebElement btnFindFriendsFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(findFriendsFBBtn));
        }
        public IWebElement btnMessageFBBtn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(messageFBBtn));
        }
        public IWebElement txtSearchMessengerFBDialog(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(searchMessengerFBDialogTxt));
        }
        public IWebElement txtChatMessageFB(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(chatMessageFBTxt));
        }
        public IWebElement receiveReplyChatMessageFB(int timeoutInSeconds, string msgReply)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(chatMessageFBReceiveReply(msgReply)));
        }
    }

    internal sealed class SocialNetworkAction : BasePage<SocialNetworkAction, SocialNetworkPage>
    {
        #region Constructor
        private SocialNetworkAction() { }
        #endregion

        #region Items Action
        // Wait for element visible
        public SocialNetworkAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public SocialNetworkAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
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

        // Scroll Into View IwebElement
        public SocialNetworkAction ScrollIntoView(IWebElement iwebE)
        {
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);
            return this;
        }

        // Facebook page login (Items Action)        
        public SocialNetworkAction InputEmailOrPhoneFBTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtEmailOrPhoneFB(timeoutInSeconds).Clear();
            this.Map.txtEmailOrPhoneFB(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public SocialNetworkAction InputPasswordFBTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtPasswordFB(timeoutInSeconds).Clear();
            this.Map.txtPasswordFB(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public SocialNetworkAction ClickLoginFBBtn(int timeoutInSeconds)
        {
            this.Map.btnLoginFB(timeoutInSeconds).Click();
            return this;
        }
        public SocialNetworkAction InputSearchFaceBookTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtSearchFaceBook(timeoutInSeconds).Clear();
            this.Map.txtSearchFaceBook(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public SocialNetworkAction ClickSeeAllFBBtn(int timeoutInSeconds)
        {
            ScrollIntoView(Map.btnSeeAllFB(timeoutInSeconds));
            this.Map.btnSeeAllFB(timeoutInSeconds).Click();
            return this;
        }
        public SocialNetworkAction ClickSearchResultsLink(int timeoutInSeconds, string subTitle, string title)
        {
            ScrollIntoView(Map.linkSearchResultsFB(timeoutInSeconds, subTitle, title));
            this.Map.linkSearchResultsFB(timeoutInSeconds, subTitle, title).Click();
            return this;
        }
        public SocialNetworkAction ClickSearchResultsLink(int timeoutInSeconds, string title)
        {
            ScrollIntoView(Map.linkSearchResultsFB(timeoutInSeconds, title));
            this.Map.linkSearchResultsFB(timeoutInSeconds, title).Click();
            return this;
        }
        public SocialNetworkAction ClickFindFriendsFBBtn(int timeoutInSeconds)
        {
            this.Map.btnFindFriendsFB(timeoutInSeconds).Click();
            return this;
        }
        public SocialNetworkAction ClickMessageFBBtn(int timeoutInSeconds)
        {
            this.Map.btnMessageFBBtn(timeoutInSeconds).Click();
            return this;
        }
        public SocialNetworkAction InputSearchMessengerFBDialogTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtSearchMessengerFBDialog(timeoutInSeconds).Clear();
            this.Map.txtSearchMessengerFBDialog(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public SocialNetworkAction InputChatMessageFBTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtChatMessageFB(timeoutInSeconds).Clear();
            this.Map.txtChatMessageFB(timeoutInSeconds).SendKeys(txt); Thread.Sleep(500);
            return this;
        }
        /// Verify Action <summary>
        public bool IsWebElementDisplayed(IWebElement element) // use for Priority Star, checkbox ....
        {
            bool result = false;
            try
            {
                var value = Map.HighlightElement(element, "green", "setAttribute").Displayed;//.GetAttribute(attribute);
                if (value == true)
                {
                    result = true;
                }
            }
            catch (Exception e) { }

            if (result == false)
            {
                Map.HighlightElement(element, "red", "setAttribute");
                Driver.TakeScreenShot("ss_isAttribtuePresent" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(element, "red", "removeAttribute");
            }

            return result;
        }
        public bool IsChatMessageFBShown(int timeoutInSeconds, string msgReply)
        {
            var iweb = Map.receiveReplyChatMessageFB(timeoutInSeconds, msgReply);
            return IsWebElementDisplayed(iweb);
        }
        #endregion

        #region Built-in Actions
        public SocialNetworkAction LoginFacebook(int timeoutInSeconds, string? url = null, string? emailOrPhone = null, string? password = null, string? userName = null)
        {
            // Check if user login with a specific value, if no then will get it from config.xml (by default)
            if (url == null) url = SocialNetworkPage.facebookSite;
            if (emailOrPhone == null) emailOrPhone = SocialNetworkPage.facebookEmailOrPhone;
            if (password == null) password = SocialNetworkPage.facebookPassword;
            if (userName == null) userName = SocialNetworkPage.facebookUserName;

            try
            {
                // Go to Facebook
                LoginAction.Instance.NavigateSite(url);

                // Login Facebook
                InputEmailOrPhoneFBTxt(timeoutInSeconds, emailOrPhone);
                InputPasswordFBTxt(timeoutInSeconds, password);
                ClickLoginFBBtn(timeoutInSeconds);
                WaitForElementVisible(timeoutInSeconds, SocialNetworkPage.userNameAccount(userName)); // 60s
                Thread.Sleep(1000);
            }
            catch (Exception exception)
            {
                // Print exception
                Console.WriteLine(exception);

                // Warning
                BaseTestCase.ExtReportResult(false, "Something wrong! Please check console log." + "<br/>" + exception);
                Assert.Inconclusive("Something wrong! Please check console log.");
            }
            return this;
        }
        #endregion
    }
}
