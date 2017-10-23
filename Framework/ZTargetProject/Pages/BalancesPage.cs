using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Framework.ZTargetProject.Data;
using Framework.Extensions;

namespace Framework.ZTargetProject.Pages
{
    public class BalancesPage : BaseMemberPage
    {
        public BalancesPage(IWebDriver driver)
            : base(driver)
        {
            Driver = driver;
            WaitForTitle("Balances");
        }

        private By balancesTable = By.CssSelector("div.account-summary + table.overview-table");
        private By estimatedBalance = By.CssSelector("div.amount");
        private By estimatedBalanceTable = By.CssSelector("div.amount + table.overview-table");
        private By benefitImprovementFormula = By.XPath("//span[contains(@class,'help') and text()='Benefit Improvement']/following - sibling::div[@class = 'formula-part'][1]");
        private By supplementaryImprovementFormula = By.XPath("//span[contains(@class,'help') and text()='Supplementary Benefit']/following-sibling::div[@class='formula - part'][1]");
        private By absFormula = By.CssSelector("div.formula-part-wrapper+div.formula-part-operator+div.formula-part");
        private By legendSectionValue = By.CssSelector("ul.operator-legend li.active span.value");
        private By legendReductionFactorSection = By.XPath("//p[contains(text(),'Reduction factor')]");
        private By clause34Operator = By.XPath("//div[starts-with(text(),'5BS')]/parent::div/parent::div/parent::div[@class = 'formula-part'] / following - sibling::div[@class = 'formula-part-operator'][1]");
        private By nonClause34Opertor = By.XPath("//div[starts-with(text(),'3BS')]/parent::div/parent::div/parent::div[@class = 'formula-part'] / following - sibling::div[@class = 'formula-part-operator'][1]");
        private By benefitImprovementOperator = By.XPath("//span[contains(@class,'help') and text()='Benefit Improvement']/following - sibling::div[@class = 'formula-part-operator'][1]");
        private By supplementaryImprovementOperator = By.CssSelector("div.formula-part-wrapper+div.formula-part-operator");
        private By allOperators = By.CssSelector("div.operator.seperator");
        private By accountBalanceEffectiveDate = By.XPath("//p[contains(text(),'The following balances are as at ')]");
        private By dbdFactSheetUrl = By.XPath("//div[@class='heading' and contains(text(),'Your defined benefit')]/followingsibling::div/p[2]/a");
        private By dbdFormula = By.ClassName("formula-compact");

        private string clause34Formula;
        private string nonClause34Formula;


        public MemberBalancesDetails GetBalanceData(bool isDbd)
        {
            var benefit = FindTable(balancesTable);
            var balanceData = new MemberBalancesDetails()
            {
                AccountBalanceAmount = FindElement(estimatedBalance).Text,
                BenefitPreservedAmount = benefit.GetCellElement(1, 2).Text,
                BenefitNonPreservedRestrictedAmount = benefit.GetCellElement(2, 2).Text,
                BenefitNonPreservedUnrestrictedAmount = benefit.GetCellElement(3, 2).Text,
            };

            if (isDbd)
            {
                var components = FindTable(estimatedBalanceTable);
                balanceData.DefinedBenefitAmount = components.GetCellElement(1, 2).Text;
                balanceData.AccumulationAmount = components.GetCellElement(2, 2).Text;
            }

            return balanceData;
        }

        public string GetFormulaText(string section, DateTime joinDate = default(DateTime),
            DateTime effectiveDate = default(DateTime), bool isClause34Active = false)
        {
            switch (section.ToLower())
            {
                case "clause34":
                    if (joinDate.Equals(default(DateTime)) || effectiveDate.Equals(default(DateTime)))
                        throw new NotImplementedException(section +
                            " without Unisuper Join date or Account Balance Effective Date is not implemented in GetFormulaText.");
                    clause34Formula = joinDate >= DateTime.Parse("2015-01-01")
                        ? "//span[contains(@class,'help') and contains(text(),'Service accrued from " +
                          joinDate.ToString("dd MMM yyyy", null) +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "')]/following-sibling::div[@class='formula-part'][1]"
                        : "//span[contains(@class,'help') and contains(text(),'Service accrued from " +
                          "01 Jan 2015" +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "')]/following-sibling::div[@class='formula-part'][1]";
                    return FindElement(By.XPath(clause34Formula)).Text.Replace("\r\n", " ");
                case "non clause34":
                    if (joinDate.Equals(default(DateTime)) || effectiveDate.Equals(default(DateTime)))
                        throw new NotImplementedException(section +
                                                          " without Unisuper Join date or Account Balance Effective Date is not implemented in GetFormulaText.");
                    nonClause34Formula = isClause34Active
                        ? "//span[contains(@class,'help') and text()='Service accrued before 01 Jan 2015']/followingsibling::div[@class='formula-part'][1]"
                        : "//span[contains(@class,'help') and text()='Service accrued from " +
                          joinDate.ToString("dd MMM yyyy", null) +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "']/following-sibling::div[@class='formula-part'][1]";
                    return FindElement(By.XPath(nonClause34Formula)).Text.Replace("\r\n", " ");
                case "benefit":
                    return FindElement(benefitImprovementFormula).Text.Replace("\r\n", " ");
                case "supplementary":
                    return FindElement(supplementaryImprovementFormula).Text.Replace("\r\n", " ");
                case "abs":
                    return FindElement(absFormula).Text;
                default:
                    throw new NotImplementedException(section + " not implemented in GetFormulaText.");
            }
        }

        public IDictionary<string, IList<string>> GetFormulaOperandList(string section,
            DateTime joinDate = default(DateTime),
            DateTime effectiveDate = default(DateTime), bool isClause34Active = false)
        {
            switch (section.ToLower())
            {
                case "clause34":
                    if (joinDate.Equals(default(DateTime)) || effectiveDate.Equals(default(DateTime)))
                        throw new NotImplementedException(section +
                                                          " without Unisuper Join date or Account Balance Effective Date is not implemented in GetFormulaText.");
                    clause34Formula = joinDate >= DateTime.Parse("2015-01-01")
                        ? "//span[contains(@class,'help') and contains(text(),'Service accrued from " +
                          joinDate.ToString("dd MMM yyyy", null) +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "')]/following-sibling::div[@class='formula-part'][1]"
                        : "//span[contains(@class,'help') and contains(text(),'Service accrued from " +
                          "01 Jan 2015" +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "')]/following-sibling::div[@class='formula-part'][1]";
                    return GetOperandList(By.XPath(clause34Formula));
                case "non clause34":
                    if (joinDate.Equals(default(DateTime)) || effectiveDate.Equals(default(DateTime)))
                        throw new NotImplementedException(section +
                                                          " without Unisuper Join date or Account Balance Effective Date is not implemented in GetFormulaText.");
                    nonClause34Formula = isClause34Active
                        ? "//span[contains(@class,'help') and text()='Service accrued before 01 Jan 2015']/followingsibling::div[@class='formula-part'][1]"
                        : "//span[contains(@class,'help') and text()='Service accrued from " +
                          joinDate.ToString("dd MMM yyyy", null) +
                          " to " +
                          effectiveDate.ToString("dd MMM yyyy", null) +
                          "']/following-sibling::div[@class='formula-part'][1]";
                    return GetOperandList(By.XPath(nonClause34Formula));
                case "benefit":
                    return GetOperandList(benefitImprovementFormula);
                case "supplementary":
                    return GetOperandList(supplementaryImprovementFormula);
                case "abs":
                    return GetOperandList(absFormula);
                default:
                    throw new NotImplementedException(section + " not implemented in GetFormulaText.");
            }
        }

        public string GetOperator(string section)
        {
            switch (section.ToLower())
            {
                case "clause34":
                    FindElement(clause34Operator).HighlightElement();
                    return FindElement(clause34Operator).Text;
                case "non clause34":
                    FindElement(nonClause34Opertor).HighlightElement();
                    return FindElement(nonClause34Opertor).Text;
                case "benefit":
                    FindElement(benefitImprovementOperator).HighlightElement();
                    return FindElement(benefitImprovementOperator).Text;
                case "supplementary":
                    FindElement(supplementaryImprovementOperator).HighlightElement();
                    return FindElement(supplementaryImprovementOperator).Text;
                default:
                    throw new NotImplementedException(section + " not implemented in GetFormulaText.");
            }
        }

        public int GetAllOperatorsCount()
        {
            return FindElements(allOperators).Count;
        }

        public string GetAccountBalanceEffectiveDate()
        {
            return FindElement(accountBalanceEffectiveDate).Text.GetRegExMatch(@"\d.*");
        }

        public string GetDbdFactSheetUrl()
        {
            return FindElement(dbdFactSheetUrl).GetAttribute("href");
        }

        public IList<string> GetReductionFactorFormulaList()
        {
            return
                FindElements(legendReductionFactorSection)
                    .Select(element => element.GetAttribute("textContent").ReplaceRegExMatch(@"\s+", " "))
                    .ToList();
        }

        public bool IsDbdFormulaDisplayed()
        {
            return IsElementVisible(dbdFormula);
        }

        private IDictionary<string, IList<string>> GetOperandList(By element)
        {
            IList<string> formulaOperandHeaderList = new List<string>(),
                formulaOperandTitleList = new List<string>(),
                legendValueList = new List<string>();
            foreach (var operand in FindElement(element).FindElements(By.CssSelector("div.operand")))
            {
                operand.HoverOver(Driver, IsMobile());
                formulaOperandHeaderList.Add(operand.Text);
                formulaOperandTitleList.Add(
                    operand.FindElement(By.CssSelector("div.formula-item.active")).GetAttribute("title"));
                legendValueList.Add(FindElement(legendSectionValue).Text);
            }
            return new Dictionary<string, IList<string>>()
            {
                {"Formula Operand Header", formulaOperandHeaderList},
                {"Formula Operand Title", formulaOperandTitleList},
                {"Legend Value", legendValueList}
            };
        }
    }
}