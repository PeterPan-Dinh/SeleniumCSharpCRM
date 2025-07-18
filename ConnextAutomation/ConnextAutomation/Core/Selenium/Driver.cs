﻿using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Connext.UITest.Core.Selenium
{
    internal static class Driver
    {
        private static IWebDriver? browser;
        private static WebDriverWait? browserWait;

        public static IWebDriver? Browser
        {
            get
            {
                if (browser == null)
                { throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method class"); }
                return browser;
            }
            private set { browser = value; }
        }

        public static WebDriverWait? BrowserWait
        {
            get
            {
                if (browserWait == null || browser == null)
                { throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method class"); }
                return browserWait;
            }
            private set { browserWait = value; }
        }

        internal static void ClearBrowserCache()
        {
            Browser.Manage().Cookies.DeleteAllCookies();
            //System.Threading.Thread.Sleep(7000); // wait 7 seconds to clear cookies
        }

        internal static void StartBrowser(string? browserType = null, int defaultTimeOut = 30)
        {
            XDocument xdoc = XDocument.Load(@"Config\Config.xml");
            var browser = xdoc.XPathSelectElement("config/browser").Value;

            // Using Chrome browser by default, if no then get default browser at Config.xml
            if (browserType == null) browserType = browser; //"chrome";

            // Open browser type (chrome; firefox; ie ...)
            string webDriverPath = @"Core\WebDrivers\";
            switch (browserType)
            {
                case "CHROME":
                case "Chrome":
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddUserProfilePreference("download.prompt_for_download", false);
                    options.AddUserProfilePreference("disable-popup-blocking", "true");
                    options.AddUserProfilePreference("safebrowsing.enabled", "true");
                    options.AddArguments("--disable-notifications");
                    Browser = new ChromeDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath), options);
                    Browser.Manage().Window.Maximize();
                    break;
                case "FIREFOX":
                case "FireFox":
                case "Firefox":
                case "firefox":
                    string firefoxBinaryPath = xdoc.XPathSelectElement("config/gecko").Attribute("path").Value;
                    Browser = new FirefoxDriver(new string(firefoxBinaryPath), new FirefoxOptions());
                    ClearBrowserCache();
                    break;

                case "IE":
                case "ie":
                    Browser = new InternetExplorerDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath));
                    Browser.Manage().Window.Maximize();
                    //ClearBrowserCache();
                    break;

                case "Edge":
                case "edge":
                    Browser = new EdgeDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath));
                    Browser.Manage().Window.Maximize();
                    //ClearBrowserCache();
                    break;

                default:
                    break;
            }
            BrowserWait = new WebDriverWait(Browser, TimeSpan.FromSeconds(defaultTimeOut));
        }

        internal static void StartBrowserIncognito(string? browserType = null, int defaultTimeOut = 30)
        {
            XDocument xdoc = XDocument.Load(@"Config\Config.xml");
            var browser = xdoc.XPathSelectElement("config/browser").Value;

            // Using Chrome browser by default, if no then get default browser at Config.xml
            if (browserType == null) browserType = browser; //"chrome";

            // Open browser type (chrome; firefox; ie ...)
            string webDriverPath = @"Core\WebDrivers\";
            switch (browserType)
            {
                case "CHROME":
                case "Chrome":
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArguments("--incognito");
                    options.AddUserProfilePreference("download.prompt_for_download", false);
                    options.AddUserProfilePreference("disable-popup-blocking", "true");
                    options.AddUserProfilePreference("safebrowsing.enabled", "true");
                    Browser = new ChromeDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath), options);
                    Browser.Manage().Window.Maximize();
                    break;
                case "FIREFOX":
                case "FireFox":
                case "Firefox":
                case "firefox":
                    string firefoxBinaryPath = xdoc.XPathSelectElement("config/gecko").Attribute("path").Value;
                    Browser = new FirefoxDriver(new string(firefoxBinaryPath), new FirefoxOptions());
                    ClearBrowserCache();
                    break;

                case "IE":
                case "ie":
                    Browser = new InternetExplorerDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath));
                    Browser.Manage().Window.Maximize();
                    //ClearBrowserCache();
                    break;

                case "Edge":
                case "edge":
                    Browser = new EdgeDriver(Path.Combine(Environment.CurrentDirectory, webDriverPath));
                    Browser.Manage().Window.Maximize();
                    //ClearBrowserCache();
                    break;

                default:
                    break;
            }
            BrowserWait = new WebDriverWait(Browser, TimeSpan.FromSeconds(defaultTimeOut));
        }

        internal static void StopBrowser()
        {
            Browser.Quit();
            //proc.CloseMainWindow(); // --> apply for Godaddy Your browser is a bit unusual ...
            Browser = null;
            BrowserWait = null;
        }

        internal static void TakeScreenShot(string screenShotName)
        {
            XDocument xdoc = XDocument.Load(@"Config\Config.xml");
            string folderName = xdoc.XPathSelectElement("config/screenshot/screenshotPath").Value;
            string screenshotPath = Path.GetFullPath(@"../../../../" + folderName + "/"); ;
            string screenshotFormat = xdoc.XPathSelectElement("config/screenshot/screenshotFormat").Value;

            Screenshot ss = ((ITakesScreenshot)Driver.browser).GetScreenshot();
            ss.SaveAsFile(screenshotPath + screenShotName + screenshotFormat); //ScreenshotImageFormat.Jpeg
        }
    }
}
