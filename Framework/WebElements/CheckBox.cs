using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.WebElements
{
    public class Checkbox
    {
        private IWebElement checkbox;

        public Checkbox(IWebElement checkbox)
        {
            this.checkbox = checkbox;
        }

        public void Select(bool value)
        {
            if (checkbox.Selected != value)
                checkbox.Click();
        }

        public bool IsSelected()
        {
            return checkbox.Selected;
        }
    }
}