using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
//using OpenQA.Selenium.Support.UI;

namespace Framework.Extensions
{
    public static class WebElementExtensions
    {
        public static IList<string> GetTextFromElements(this IList<IWebElement> elementList)
        {
            var textList = new List<string>();
            foreach (var element in elementList)
                if (element.Displayed)
                    textList.Add(element.Text.Trim());
            return textList;
        }

        public static IList<string> GetMatchingTextFromElements(this IList<IWebElement> elementList, string regExPattern)
        {
            var textList = new List<string>();
            foreach (var element in elementList)
                textList.Add(element.Text.GetRegExMatch(regExPattern).Trim());
            return textList;
        }

        public static IList<string> GetCaptureGroupTextFromElements(this IList<IWebElement> elementList, string regExPattern)
        {
            var textList = new List<string>();
            foreach (var element in elementList)
                textList.Add(element.Text.GetRegExCaptureGroup(regExPattern).Trim());
            return textList;
        }

        public static IList<string> GetAttributeFromElements(this IList<IWebElement> elementList, string attributeName)
        {
            var attributeValues = new List<string>();
            foreach (var element in elementList)
                attributeValues.Add(element.GetAttribute(attributeName));
            return attributeValues;
        }
    }
}
