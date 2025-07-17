using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connext.UITest.Core.Selenium;

namespace Connext.UITest.Core.BaseClass
{
    internal class BasePageElementMap
    {
        protected IWebDriver? browser;
        protected WebDriverWait? browserWait;

        internal BasePageElementMap()
        {
            browser = Driver.Browser;
            browserWait = Driver.BrowserWait;
        }

        internal void SwitchToDefault()
        {
            browser.SwitchTo().DefaultContent();
        }
    }
}
