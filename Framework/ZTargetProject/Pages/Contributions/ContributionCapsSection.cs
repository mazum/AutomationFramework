using OpenQA.Selenium;
using Framework.Extensions;
using Framework.WebElements;

namespace Framework.ZTargetProject.Pages
{
    public class ContributionCapsSection : BasePage
    {
        public ContributionCapsSection(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
        }

        private By concContributionsContainer = By.XPath("(//div[contains(@class, 'capbar ng-binding')])[1]");
        private By nonConcContributionsContainer = By.XPath("(//div[contains(@class, 'capbar ng-binding')])[2]");
        private By contributionsBar = By.XPath(".//div[@class='capbarwrapper']"); //relative
        private By contributionsBarSegment = By.XPath(".//div[@class='capbarsegment_text']"); //relative
        private By limitLabel = By.XPath(".//div[@class='capbar-message ngscope']");
        private By contributionTextAndValueLabel = By.CssSelector("div.capbarlegend_item.ng-scope");
        
        /// <summary>
        /// index = 1 for the left part of the bar
        /// index = 2 for the right part of the bar
        /// </summary>
        public string GetConcBarSegmentValue(int index)
        {
            return FindElement(concContributionsContainer).FindElements(contributionsBarSegment)[index].Text;
        }

        public string GetNonConcBarSegmentValue(int index)
        {
            return FindElement(nonConcContributionsContainer).FindElements(contributionsBarSegment)[index].Text;
        }

        public string GetConcBarValue()
        {
            return FindElement(concContributionsContainer).FindElement(contributionsBar).Text;
        }

        public string GetNonConcBarValue()
        {
            return FindElement(nonConcContributionsContainer).FindElement(contributionsBar).Text;
        }

        public string GetConcLimit()
        {
            return FindElement(concContributionsContainer).FindElement(limitLabel).Text;
        }

        public string GetNonConcLimit()
        {
            return FindElement(nonConcContributionsContainer).FindElement(limitLabel).Text;
        }

        public string GetConcCombinedValue()
        {
            return FindElement(concContributionsContainer).FindElement(contributionTextAndValueLabel).Text;
        }

        public string GetNonConcCombinedValue()
        {
            return FindElement(nonConcContributionsContainer).FindElement(contributionTextAndValueLabel).Text;
        }

    }
}