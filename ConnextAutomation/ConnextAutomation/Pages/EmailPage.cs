using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Connext.UITest.Core.BaseClass;
using Connext.UITest.Core.BaseTestCase;
using Connext.UITest.Core.Selenium;
using Connext.UITest.Pages;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;

namespace SeleniumGendAdmin.Pages
{
    internal class EmailPage : BasePageElementMap
    {
        // Initiate variables
        internal static WebDriverWait? wait;
        internal static readonly XDocument xdoc = XDocument.Load(@"Config\Config.xml");
        internal static string email = xdoc.XPathSelectElement("config/account/email").Attribute("mail").Value,
        emailPass = xdoc.XPathSelectElement("config/account/email").Attribute("password").Value;

        // Initiate the By objects for elements
        /// <summary>
        /// email page login (Gmail) (By objects for elements)
        /// </summary>
        internal static By gmailEmailTxt = By.Id("identifierId"),
        gmailPasswordTxt = By.XPath(@"//input[@type='password']"),
        gmailNextBtn = By.XPath(@"//div[contains(@id,'Next')]//button"),
        inboxGmailLink = By.XPath(@"//a[.='Inbox']"),
        receiveLastGmailLink = By.XPath(@"//div[@role='tabpanel'][1]/div[3]//tbody/tr"),
        receiveGmailLink = By.XPath(@"//span[.='Invitation to join GenD Admin']/ancestor::td/parent::tr"),
        acceptYourInvitationLink = By.XPath(@"//a[.='ACCEPT YOUR INVITATION']"),
        deleteMailBtn = By.XPath(@"//div[@aria-label='Delete']"),
        leftpaneSnoozedBtn = By.XPath(@"//a[.='Snoozed']"),
        leftpaneMoreBtn = By.XPath(@"//span[.='More' and @class='CJ']"),
        leftpaneTrashBinIcon = By.XPath(@"//a[@aria-label='Bin']"),
        deleteForeverBtn = By.XPath(@"//div[.='Delete forever' and @class='Bn']"),
        selectAllInboxCkb = By.XPath(@"//div[@class='D E G-atb' and @gh='tm']//span[@role='checkbox']"),
        selectAllTrashBinCkb = By.XPath(@"//div[@data-query='in:trash']/parent::div[@aria-label='search refinement']/following-sibling::div//span[@role='checkbox']"),
        verificationPane = By.XPath(@"//div[contains(@data-app-config,'identifier')]"),
        thirdStepVerificationLink = By.XPath(@"//ul[@class='OVnw0d']/li[3]/div"), // Confirm your recovery email 
        verifyAccountLink = By.XPath(@"//strong[.='https://g.co/verifyaccount']"),
        recoveryEmailAddressTxt = By.Id("knowledge-preregistered-email-response"),
        fourthStepVerificationLink = By.XPath(@"//ul[@class='OVnw0d']/li[4]/div"), // steps phone number
        phoneNumberTxt = By.Id("phoneNumberId"),
        verifyAccountNextBtn = By.XPath(@"//div[@jsname='Njthtb']//button"),

        /// <summary>
        /// email page login (Yahoo) (By objects for elements)
        /// </summary>
        yahooEmailTxt = By.Id("login-username"),
        yahooNextBtn = By.Id("login-signin"),
        yahooPasswordTxt = By.Id("login-passwd"),
        yahooMailIcon = By.Id("ybarMailLink"),
        leftpaneYahooInboxLink = By.XPath(@"//div[@data-test-folder-container='Inbox']"),
        receiveYahooLink = By.XPath(@"//a[contains(@aria-label,'Invitation to join GenD Admin')]"),
        yahooInboxSelectAllCkb = By.XPath(@"//button[@data-test-id='checkbox']"),
        yahooDeleteBtn = By.XPath(@"//button[contains(@data-test-id,'delete')]"),
        leftpaneYahooTrashLink = By.XPath(@"//div[@data-test-folder-container='Trash']"),
        yahooOkPopupBtn = By.XPath(@"//button[.='OK']"),

        /// <summary>
        /// email page login (Proton) (By objects for elements)
        /// </summary>
        protonSignInLink = By.XPath(@"//a[.='Sign in']"),
        protonEmailTxt = By.Id("username"),
        protonPasswordTxt = By.Id("password"),
        protonSignInBtn = By.XPath(@"//button[@type='submit']"),
        protonInboxContainer = By.XPath(@"//div[contains(@class,'items-column-list-container')]"),
        inboxProtonLink = By.XPath(@"//a[contains(@class,'navigation-link active')]"),
        deleteMailProtonBtn = By.XPath(@"//button[contains(@data-testid,'movetotrash')]");
        internal static By receiveProtonRowLink(string index) => By.XPath(@"//div[contains(@class,'list-container')]/div/div[" + index + "]");
        internal static By receiveProtonRowCheckbox(string index) => By.XPath(receiveProtonRowLink(index).ToString().Remove(0, 10) + "//label");
        internal static By messageRecipientContentProton = By.XPath(@"//div[contains(@summary,'mail_notification')]/parent::div");
        internal static By replyMailProtonBtn = By.XPath(@"//button[.='Reply']");
        internal static By editorProtonTxt = By.XPath(@"//div[@id='rooster-editor']");
        internal static By sendMailProtonBtn = By.XPath(@"//button[.='Send']");
        internal static By selectAllInboxProtonCkb = By.XPath("//span[contains(@class,'select-all')]");

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
        /// email page login (Gmail) (IWeb elements)
        /// </summary>
        public IWebElement txtGmailEmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(gmailEmailTxt));
        }
        public IWebElement txtGmailPassword(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(gmailPasswordTxt));
        }
        public IWebElement btnGmailNext(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(gmailNextBtn));
        }
        public IWebElement linkInboxGmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(inboxGmailLink));
        }
        public IWebElement linkReceiveLastGmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(receiveLastGmailLink));
        }
        public IWebElement linkReceiveGmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(receiveGmailLink));
        }
        public IWebElement linkAcceptYourInvitation(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(acceptYourInvitationLink));
        }
        public IWebElement buttonDeleteMail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(deleteMailBtn));
        }
        public IWebElement btnLeftpaneSnoozed(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leftpaneSnoozedBtn));
        }
        public IWebElement btnLeftpaneMore(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leftpaneMoreBtn));
        }
        public IWebElement iconLeftpaneTrashBin(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leftpaneTrashBinIcon));
        }
        public IWebElement btnDeleteForever(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(deleteForeverBtn));
        }
        public IWebElement ckbSelectAllInbox(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(selectAllInboxCkb));
        }
        public IWebElement ckbSelectAllTrashBin(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(selectAllTrashBinCkb));
        }
        public IWebElement linkThirdStepVerification(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(thirdStepVerificationLink));
        }
        public IWebElement linkFourthStepVerification(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(fourthStepVerificationLink));
        }
        public IWebElement txtPhoneNumber(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(phoneNumberTxt));
        }
        public IWebElement txtRecoveryEmailAddress(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(recoveryEmailAddressTxt));
        }
        public IWebElement btnVerifyAccountNext(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(verifyAccountNextBtn));
        }

        /// <summary>
        /// email page login (Yahoo) (IWeb elements)
        /// </summary>
        public IWebElement txtYahooEmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(yahooEmailTxt));
        }
        public IWebElement btnYahooNext(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(yahooNextBtn));
        }
        public IWebElement txtYahooPassword(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(yahooPasswordTxt));
        }
        public IWebElement iconYahooMail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(yahooMailIcon));
        }
        public IWebElement linkReceiveYahoo(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(receiveYahooLink));
        }
        public IWebElement ckbYahooInboxSelectAll(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(yahooInboxSelectAllCkb));
        }
        public IWebElement btnYahooDelete(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(yahooDeleteBtn));
        }
        public IWebElement linkLeftpaneYahooTrash(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leftpaneYahooTrashLink));
        }
        public IWebElement linkLeftpaneYahooInbox(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(leftpaneYahooInboxLink));
        }
        public IWebElement btnYahooOkPopup(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(yahooOkPopupBtn));
        }

        /// <summary>
        /// email page login (Proton) (IWeb elements)
        /// </summary>
        public IWebElement linkProtonSignIn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(protonSignInLink));
        }
        public IWebElement txtProtonEmail(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(protonEmailTxt));
        }
        public IWebElement txtProtonPassword(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(protonPasswordTxt));
        }
        public IWebElement btnProtonSignIn(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(protonSignInBtn));
        }
        public IWebElement linkRowReceiveProton(int timeoutInSeconds, string row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(receiveProtonRowLink(row)));
        }
        public IWebElement checkboxRowReceiveProton(int timeoutInSeconds, string row)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(receiveProtonRowCheckbox(row)));
        }
        public IWebElement linkInboxProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(inboxProtonLink));
        }
        public IWebElement buttonDeleteMailProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(deleteMailProtonBtn));
        }
        public IWebElement messageReceivedContentProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(messageRecipientContentProton));
        }
        public IWebElement buttonReplyMailProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(replyMailProtonBtn));
        }
        public IWebElement txtProtonEditor(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => d.FindElement(editorProtonTxt));
        }
        public IWebElement buttonSendMailProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(sendMailProtonBtn));
        }
        public IWebElement ckbSelectAllInboxProton(int timeoutInSeconds)
        {
            wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(selectAllInboxProtonCkb));
        }
    }

    internal sealed class EmailAction : BasePage<EmailAction, EmailPage>
    {
        #region Constructor
        private EmailAction() { }
        #endregion

        #region Items Action
        // Wait for a css attribute to change
        public EmailAction WaitForACssAttributeChange(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
            }
            return this;
        }

        // Wait for element visible
        public EmailAction WaitForElementVisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
            }
            return this;
        }

        // Wait for element Invisible (can use for dropdown on-overlay Invisible)
        public EmailAction WaitForElementInvisible(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
            }
            return this;
        }

        // Wait for loading Spinner icon to disappear
        public EmailAction WaitForLoadingIconToDisappear(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
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

        // Switch to iframe (with wait method) to interact with window/tab 
        public EmailAction SwitchToFrameWithWaitMethod(int timeoutInSeconds, By element)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(element));
            }
            return this;
        }

        // email page login (Gmail) (Items Action)
        public EmailAction InputGmailEmailTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtGmailEmail(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction InputGmailPasswordTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtGmailPassword(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction ClickGmailNextBtn(int timeoutInSeconds)
        {
            this.Map.btnGmailNext(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickInboxGmailLink(int timeoutInSeconds)
        {
            this.Map.linkInboxGmail(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickReceiveLastGmailLink(int timeoutInSeconds)
        {
            this.Map.linkReceiveLastGmail(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickReceiveGmailLink(int timeoutInSeconds)
        {
            this.Map.linkReceiveGmail(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickAcceptYourInvitationLink(int timeoutInSeconds)
        {
            this.Map.linkAcceptYourInvitation(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickDeleteMailBtn(int timeoutInSeconds)
        {
            this.Map.buttonDeleteMail(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickLeftpaneSnoozedBtn(int timeoutInSeconds)
        {
            MouseHover(timeoutInSeconds, EmailPage.leftpaneSnoozedBtn);
            ScrollToView(this.Map.btnLeftpaneSnoozed(timeoutInSeconds));
            this.Map.btnLeftpaneSnoozed(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickLeftpaneMoreBtn(int timeoutInSeconds)
        {
            ScrollToView(this.Map.btnLeftpaneMore(timeoutInSeconds));
            this.Map.btnLeftpaneMore(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickLeftpaneTrashBinIcon(int timeoutInSeconds)
        {
            ScrollToView(this.Map.iconLeftpaneTrashBin(timeoutInSeconds));
            this.Map.iconLeftpaneTrashBin(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickDeleteForeverBtn(int timeoutInSeconds)
        {
            this.Map.btnDeleteForever(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickSelectAllInboxCkb(int timeoutInSeconds)
        {
            this.Map.ckbSelectAllInbox(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickSelectAllTrashBinCkb(int timeoutInSeconds)
        {
            this.Map.ckbSelectAllTrashBin(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction CLickThirdStepVerificationLink(int timeoutInSeconds)
        {
            this.Map.linkThirdStepVerification(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction CLickFourthStepVerificationLink(int timeoutInSeconds)
        {
            this.Map.linkFourthStepVerification(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction InputPhoneNumberTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtPhoneNumber(timeoutInSeconds).Clear();
            this.Map.txtPhoneNumber(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction InputRecoveryEmailAddressTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtRecoveryEmailAddress(timeoutInSeconds).Clear();
            this.Map.txtRecoveryEmailAddress(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction CLickVerifyAccountNextBtn(int timeoutInSeconds)
        {
            MouseHover(timeoutInSeconds, EmailPage.verifyAccountNextBtn);
            this.Map.btnVerifyAccountNext(timeoutInSeconds).Click();
            return this;
        }

        // email page login (Yahoo) (Items Action)
        public EmailAction InputYahooEmailTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtYahooEmail(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction ClickYahooNextBtn(int timeoutInSeconds)
        {
            this.Map.btnYahooNext(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction InputYahooPasswordTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtYahooPassword(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction ClickYahooMailIcon(int timeoutInSeconds)
        {
            this.Map.iconYahooMail(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickReceiveYahooLink(int timeoutInSeconds)
        {
            this.Map.linkReceiveYahoo(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickYahooInboxSelectAllCkb(int timeoutInSeconds)
        {
            this.Map.ckbYahooInboxSelectAll(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickYahooDeleteBtn(int timeoutInSeconds)
        {
            this.Map.btnYahooDelete(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickLeftpaneYahooTrashLink(int timeoutInSeconds)
        {
            ScrollToView(this.Map.linkLeftpaneYahooTrash(timeoutInSeconds));
            //this.Map.linkLeftpaneYahooTrash(timeoutInSeconds).Click(); --> issue: slow performance
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(this.Map.linkLeftpaneYahooTrash(timeoutInSeconds)).Click().MoveToElement(this.Map.linkLeftpaneYahooTrash(timeoutInSeconds)).Click().Build().Perform();
            return this;
        }
        public EmailAction ClickLeftpaneYahooInboxLink(int timeoutInSeconds)
        {
            ScrollToView(this.Map.linkLeftpaneYahooInbox(timeoutInSeconds));
            //this.Map.linkLeftpaneYahooInbox(timeoutInSeconds).Click(); --> issue: slow performance
            Actions action = new Actions(Driver.Browser);
            action.MoveToElement(this.Map.linkLeftpaneYahooInbox(timeoutInSeconds)).Perform();
            return this;
        }
        public EmailAction ClickYahooOkPopupBtn(int timeoutInSeconds)
        {
            this.Map.btnYahooOkPopup(timeoutInSeconds).Click();
            return this;
        }

        // email page login (Proton) (Items Action)
        public EmailAction ClickProtonSignInLink(int timeoutInSeconds)
        {
            this.Map.linkProtonSignIn(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction InputProtonEmailTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtProtonEmail(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction InputProtonPasswordTxt(int timeoutInSeconds, string txt)
        {
            this.Map.txtProtonPassword(timeoutInSeconds).SendKeys(txt);
            return this;
        }
        public EmailAction ClickProtonSignInBtn(int timeoutInSeconds)
        {
            this.Map.btnProtonSignIn(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickInboxProtonLink(int timeoutInSeconds)
        {
            this.Map.linkInboxProton(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickReceiveProtonRowLink(int timeoutInSeconds, string row)
        {
            this.Map.linkRowReceiveProton(timeoutInSeconds, row).Click(); Thread.Sleep(500);
            return this;
        }
        public EmailAction ClickReceiveProtonRowCheckbox(int timeoutInSeconds, string row)
        {
            this.Map.checkboxRowReceiveProton(timeoutInSeconds, row).Click(); Thread.Sleep(500);
            return this;
        }
        public EmailAction ClickDeleteMailProtonBtn(int timeoutInSeconds)
        {
            this.Map.buttonDeleteMailProton(timeoutInSeconds).Click(); Thread.Sleep(200);
            return this;
        }
        public EmailAction ClickReplyMailProtonBtn(int timeoutInSeconds)
        {
            this.Map.buttonReplyMailProton(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction InputProtonEditorTxt(int timeoutInSeconds, string text)
        {
            this.Map.txtProtonEditor(timeoutInSeconds).SendKeys(OpenQA.Selenium.Keys.Control + "a"); Thread.Sleep(250);
            this.Map.txtProtonEditor(timeoutInSeconds).SendKeys(text); Thread.Sleep(250);
            return this;
        }
        public EmailAction ClickSendMailProtonBtn(int timeoutInSeconds)
        {
            this.Map.buttonSendMailProton(timeoutInSeconds).Click();
            return this;
        }
        public EmailAction ClickSelectAllInboxProtonCkb(int timeoutInSeconds)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.Click(Map.ckbSelectAllInboxProton(timeoutInSeconds)).Build().Perform();

            //this.Map.ckbSelectAllInboxProton(timeoutInSeconds).Click();
            return this;
        }
        /// verify elements
        public bool ReceiveMailProtonRowLinkGetText(int timeoutInSeconds, string row, string textParam)
        {
            var iweb = Map.linkRowReceiveProton(timeoutInSeconds, row);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_SelectedDropdown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }
        public bool MessageRecipientContentProtonGetText(int timeoutInSeconds, string textParam)
        {
            var iweb = Map.messageReceivedContentProton(timeoutInSeconds);
            bool element = Map.HighlightElement(iweb, "green").Text.Contains(textParam);
            if (element == false)
            {
                Map.HighlightElement(iweb, "red", "setAttribute");
                Driver.TakeScreenShot("ss_SelectedDropdown" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss.ffftt"));
                Map.HighlightElement(iweb, "red", "removeAttribute"); // un-highlight
                return element;
            }
            return element;
        }

        // Methods - Mouse hover, Scroll, Page down ....
        public EmailAction ScrollIntoView(IWebElement iwebE)
        {
            // using JavaScriptExecutor
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript("arguments[0].scrollIntoView(false);", iwebE);
            return this;
        }
        public EmailAction ScrollIntoElement(IWebElement iwebE)
        {
            Actions actions = new Actions(Driver.Browser);
            actions.MoveToElement(iwebE);
            actions.Perform();
            return this;
        }
        public EmailAction MouseHover(int timeoutInSeconds, By by)
        {
            var element = new WebDriverWait(Driver.Browser, TimeSpan.FromSeconds(timeoutInSeconds)).Until(ExpectedConditions.ElementIsVisible(by));
            new Actions(Driver.Browser).MoveToElement(element).Perform();
            return this;
        }
        public EmailAction PageDownToScrollDownPage()
        {
            Actions actions = new Actions(Driver.Browser);
            actions.SendKeys(OpenQA.Selenium.Keys.PageDown).Build().Perform();
            return this;
        }
        public void ScrollTo(int xPosition = 0, int yPosition = 0)
        {
            var js = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            IJavaScriptExecutor je = (IJavaScriptExecutor)Driver.Browser;
            je.ExecuteScript(js);
        }
        public IWebElement ScrollToView(By by)
        {
            var element = Driver.Browser.FindElement(by);
            ScrollToView(element);
            return element;
        }
        public void ScrollToView(IWebElement element)
        {
            if (element.Location.Y > 200)
            {
                ScrollTo(0, element.Location.Y - 100); // Make sure element is in the view but below the top navigation pane
            }

        }
        #endregion

        #region Built-in Actions
        public EmailAction LoginGmail()
        {
            // Variables declare
            const string url = "https://mail.google.com";
            string email = LoginPage.email;
            string emailPass = LoginPage.emailPass;
            string recoveryEmailAddr = LoginPage.revoceryEmailAddress;
            try 
            {
                // Go to Gmail
                LoginAction.Instance.NavigateSite(url);

                // Check if Gmail Login is shown then input account to login into Gmail
                System.Threading.Thread.Sleep(2000);
                if (IsElementPresent(EmailPage.gmailEmailTxt))
                {
                    InputGmailEmailTxt(10, email)
                    .ClickGmailNextBtn(10)
                    .WaitForElementVisible(10, EmailPage.gmailPasswordTxt)
                    .InputGmailPasswordTxt(10, emailPass)
                    .ClickGmailNextBtn(10);

                    // check if Google Verify Account then verify this account
                    System.Threading.Thread.Sleep(2000);
                    if (IsElementPresent(EmailPage.verificationPane))
                    {
                        // Click on "Confirm your recovery email address" link
                        CLickThirdStepVerificationLink(10)
                        .WaitForElementVisible(10, EmailPage.recoveryEmailAddressTxt)
                        .InputRecoveryEmailAddressTxt(10, recoveryEmailAddr)
                        .CLickVerifyAccountNextBtn(10);
                    }
                }
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
        public EmailAction CheckIfGmailInBoxExistingThenDelete()
        {
            try
            {
                // Click "Inbox" at the left pane to check if there any emails then delete it
                ClickInboxGmailLink(10);
                System.Threading.Thread.Sleep(2000);
                if (IsElementPresent(EmailPage.receiveGmailLink))
                {
                    ClickSelectAllInboxCkb(10)
                    .ClickDeleteMailBtn(10)
                    .WaitForElementInvisible(10, EmailPage.receiveGmailLink);

                    // Click on "More" at left pane to go to "Bin" (To delete forever all emails)
                    ClickLeftpaneSnoozedBtn(10) // Mouse hover on "Snoozed" button to see "More" button
                    .ClickLeftpaneMoreBtn(10)
                    .ClickLeftpaneTrashBinIcon(10)
                    .ClickSelectAllTrashBinCkb(10)
                    .ClickDeleteForeverBtn(10)
                    .WaitForElementInvisible(10, EmailPage.receiveGmailLink);

                    // Restart Browser
                    Driver.StopBrowser();
                    Driver.StartBrowser();
                }
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
        public EmailAction LoginYahoo()
        {
            // Variables declare
            const string url = "https://login.yahoo.com";
            string email = LoginPage.email;
            string emailPass = LoginPage.emailPass;

            try
            {
                // Go to Yahoo
                LoginAction.Instance.NavigateSite(url);

                // Check if Yahoo Login is shown then input account to login into Yahoo
                System.Threading.Thread.Sleep(2000);
                if (IsElementPresent(EmailPage.yahooEmailTxt))
                {
                    // Login Yahoo
                    InputYahooEmailTxt(10, email)
                    .ClickYahooNextBtn(10)
                    .WaitForElementVisible(10, EmailPage.yahooPasswordTxt)
                    .InputYahooPasswordTxt(10, emailPass)
                    .ClickYahooNextBtn(10)
                    .ClickYahooMailIcon(10)
                    .WaitForElementVisible(10, EmailPage.leftpaneYahooInboxLink);
                }
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
        public EmailAction CheckIfYahooInBoxExistingThenDelete()
        {
            try
            {
                // Click "Inbox" at the left pane to check if there any emails then delete it
                ClickLeftpaneYahooInboxLink(10);
                System.Threading.Thread.Sleep(2000);
                if (IsElementPresent(EmailPage.receiveYahooLink))
                {
                    ClickYahooInboxSelectAllCkb(10)
                    .ClickYahooDeleteBtn(10)
                    .WaitForElementInvisible(10, EmailPage.receiveYahooLink)
                    .ClickLeftpaneYahooTrashLink(10) // --> Go to "Trash" (To delete all emails)
                    .WaitForElementVisible(120, EmailPage.receiveYahooLink)
                    .ClickYahooInboxSelectAllCkb(10)
                    .ClickYahooDeleteBtn(10)
                    .ClickYahooOkPopupBtn(10)
                    .WaitForElementInvisible(10, EmailPage.receiveYahooLink);

                    // Restart Browser
                    Driver.StopBrowser();
                    Driver.StartBrowser();
                }
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
