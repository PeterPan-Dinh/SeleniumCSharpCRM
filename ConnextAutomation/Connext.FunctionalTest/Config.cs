using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;
using System.ComponentModel;

namespace Connext.FunctionalTest
{
    public class Config
    {
        // get value from App config
        public static readonly string? projectName = ConfigurationManager.AppSettings.Get("ProjectName");
        public static readonly string? siteName = ConfigurationManager.AppSettings.Get("SiteName");
        // load config.xml file
        internal static readonly string xmlFilePath = Path.GetFullPath(@"../../../../Connext.FunctionalTest/" + @"App.config");
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
        internal static string? url = null, db = null, username = null, password = null, sessionID = null, apikey = null, instanceName = null;
        internal static XmlDocument configurationFile()
        {
            XmlDocument xdoc = xmlDocLoad(xmlFilePath);
            // Parse an XML file (Get Project & Site name to run)
            foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
            {
                if (node.Name == "siteSettings")
                {
                    foreach (XmlNode siteSettingsNode in node.ChildNodes)
                    {
                        if (siteSettingsNode.Name == projectName)
                        {
                            foreach (XmlNode settingsNode in siteSettingsNode.ChildNodes)
                            {
                                if (settingsNode["instanceName"].Attributes["value"].Value == siteName)
                                {
                                    XmlNode? currentNode = settingsNode.SelectSingleNode("instanceName[@value='" + siteName + "']");
                                    XmlNode? parentNode = currentNode.ParentNode;
                                    url = parentNode["Url"].Attributes["value"].Value;
                                    db = parentNode["Database"].Attributes["value"].Value;
                                    apikey = parentNode["apikey"].Attributes["value"].Value;
                                    username = parentNode["username"].Attributes["value"].Value;
                                    password = parentNode["password"].Attributes["value"].Value;
                                    sessionID = parentNode["sessionid"].Attributes["value"].Value;
                                    instanceName = parentNode["instanceName"].Attributes["value"].Value;
                                }
                            }
                        }
                    }
                }
            }
            return xdoc;
        }
    }
}
