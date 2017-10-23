using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Framework.ZTargetProject.Data;
using Framework.Extensions;

namespace Framework.ZTargetProject.Pages
{
    public class CurrentInvestmentAllocationSection : BasePage
    {
        public CurrentInvestmentAllocationSection(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
        }

        private By investmentsContainer = By.XPath("//div[contains(@class,'investments-container')]");
        private By investmentSingleOptionText = By.XPath("//div[contains(@class,'investments-container')]/.//div[contains(@class, 'capbar-legend_item')]");
        private By investmentTable = By.CssSelector("table.investments-table");
        private By allocationSingleOptionChart = By.XPath("//div[contains(@class,'investments-container')]/.//div[contains(@class, 'capbar-segment')]");
        private By allocationOptionsChart = By.ClassName("highcharts-series-group");
        private By allocationHighchartsTooltip = By.CssSelector("div.highcharts-tooltip");
        private By allocationDonutGraph = By.CssSelector("div.investments-chart-container > div > div.ng-scope");


        public IList<InvestmentOption> GetInvestments(bool showMultiOptionsStyle, int chartPosition = 1)
        {
            bool getOptionNameFromHiddenColumn = false;
            if (chartPosition > 1)
            {
                investmentTable = By.XPath("(//table[@class='investments-table'])[" + chartPosition + "]");
                if (!IsMobile())
                    getOptionNameFromHiddenColumn = true;
            }
            ScrollIntoView(investmentsContainer);
            var investmentDetails = new List<InvestmentOption>();
            if (showMultiOptionsStyle)
            {
                WaitUntilElementIsVisible(investmentTable);
                var table = FindTable(investmentTable);
                foreach (var row in table.Rows)
                    investmentDetails.Add(new InvestmentOption
                    {
                        OptionName = getOptionNameFromHiddenColumn ? row.GetCellElement(1).FindElement(By.XPath("./span")).GetAttribute("title"):row.GetCellElement(1).Text.Trim(),
                        AllocationAmount = row.GetCellElement(2).Text.Trim(),
                        AllocationPercentage = row.GetCellElement(3).Text.Trim()
                    });
            }
            else
            {
                var text = WaitUntilElementIsVisible(investmentSingleOptionText).Text.Split(':');
                investmentDetails.Add(new InvestmentOption
                {
                    OptionName = text[0].Trim(),
                    AllocationAmount = text[1].Trim(),
                    AllocationPercentage = FindElement(allocationSingleOptionChart).Text
                });
            }
            return investmentDetails;
        }

        public bool IsAllocationChartDisplayed(bool showMultiOptionsStyle, int chartPosition = 1)
        {
            if (showMultiOptionsStyle)
                return FindElement(investmentsContainer).FindElements(allocationOptionsChart).Count >= chartPosition;

            return IsElementVisible(allocationSingleOptionChart);
        }

        public IList<string> GetInvestmentsFromChart(IList<decimal> sectionPercentages, int chartPosition = 1)
        {
            var sections = new List<string>();
            var e = FindElements(allocationDonutGraph)[chartPosition - 1];
            var offsetXToCenter = e.Size.Width / 2;
            var offsetYToCenter = e.Size.Height / 2 - 3; // the graph is not quite in the middle of the element
            var r = offsetYToCenter - 40;
            double angleDegrees = -89;
            foreach (var percentage in sectionPercentages)
            {
                var segmentAngleDegrees = Convert.ToDouble(360 * percentage / 100);
                double angle = (angleDegrees + segmentAngleDegrees / 2) * Math.PI / 180.0; //radians angle
                int x = offsetXToCenter + Convert.ToInt32(r * Math.Cos(angle));
                int y = offsetYToCenter + Convert.ToInt32(r * Math.Sin(angle));
                new Actions(Driver).MoveToElement(e, x, y).Click().Build().Perform();
                Thread.Sleep(300);
                if (percentage < 15) // for smaller segments the graph needs sometimes 2 clicks to update the tooltip content
                {
                    new Actions(Driver).MoveToElement(e, x, y).Click().Build().Perform();
                    Thread.Sleep(300);
                }
                sections.Add(FindElements(allocationHighchartsTooltip)[chartPosition - 1].Text.Replace(":", ""));
                angleDegrees += segmentAngleDegrees;
            }
            return sections.Distinct().ToList();
        }

    }
}