using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Framework.ZTargetProject.Pages
{
    public class LogoutPage : BasePage
    {
        public LogoutPage(IWebDriver driver) : base(driver)
        {
            Driver = driver;
            WaitForTitle("Logged out");
        }

        private By loginButton = By.XPath("//a[contains(@class,'button primarycta')]");


        public LoginPage GoToLoginPage()
        {
            FindElement(loginButton).Click();
            return new LoginPage(Driver);
        }
    }
}