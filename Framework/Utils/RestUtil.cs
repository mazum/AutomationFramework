using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Framework.Utils
{
    class RestUtil
    {
        public static string GeneratePostData(object requestObj)
        {
            string empty = string.Empty;
            if (requestObj == null)
                return empty;
            XmlDocument xmlDoc = new XmlDocument();
            string str1 = "http://www.unisuper.com.au/services/acurity/1.0.0/";
            string str2 = "ns0";
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDeclaration.Encoding = Encoding.UTF8.WebName;
            XmlNode element = xmlDoc.CreateElement(str2 + ":" + requestObj.GetType().Name, str1);
            XmlAttribute attribute = xmlDoc.CreateAttribute("xmlns", str2, "http://www.w3.org/2000/xmlns/");
            attribute.Value = str1;
            element.Attributes.Append(attribute);
            xmlDoc.AppendChild(element);
            xmlDoc.InsertBefore(xmlDeclaration, element);
            CreateXMLChildNodes(requestObj, requestObj.GetType().GetProperties(), xmlDoc, element, str2, str1);
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter w = XmlWriter.Create(stringWriter))
                {
                    xmlDoc.WriteTo(w);
                    w.Flush();
                    empty = stringWriter.GetStringBuilder().ToString();
                }
            }
            return empty;
        }

        private static void CreateXMLChildNodes(object obj, PropertyInfo[] properties, XmlDocument xmlDoc, XmlNode parentNode, string xmlPrefix, string xmlNS)
        {
            foreach (PropertyInfo property in properties)
            {
                XmlNode element = xmlDoc.CreateElement(property.Name, xmlNS);
                object obj1 = property.GetValue(obj, null);
                if (property.PropertyType.IsClass && property.PropertyType.Assembly.FullName == obj.GetType().Assembly.FullName)
                {
                    element.Prefix = xmlPrefix;
                    parentNode.AppendChild(element);
                    CreateXMLChildNodes(obj1, property.PropertyType.GetProperties(), xmlDoc, element, xmlPrefix, xmlNS);
                }
                else if (obj1 != null)
                {
                    element.Prefix = xmlPrefix;
                    DateTime dateTime;
                    if (obj1 is DateTime && element.Name.Equals("ns0:MessageTimeStamp"))
                    {
                        XmlNode xmlNode = element;
                        dateTime = (DateTime)obj1;
                        string str = dateTime.ToString("yyyy-MMddTHH:mm:ss.fff");
                        xmlNode.InnerText = str;
                    }
                    else if (obj1 is DateTime)
                    {
                        XmlNode xmlNode = element;
                        dateTime = (DateTime)obj1;
                        string str = dateTime.ToString("yyyy-MM-dd");
                        xmlNode.InnerText = str;
                    }
                    else
                        element.InnerText = obj1.ToString();
                    parentNode.AppendChild(element);
                }
            }
        }

        public static string PostWebRequest(string WebRequestURL, string postData, string contentType = "application/x-www-form-urlencoded")
        {
            WebRequest webRequest = WebRequest.Create(WebRequestURL);
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            string empty = string.Empty;
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            using (Stream requestStream = webRequest.GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            WebResponse response;
            try
            {
                response = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }
            string statusDescription = ((HttpWebResponse)response).StatusDescription;
            response.Close();
            return statusDescription;
        }
    }
}