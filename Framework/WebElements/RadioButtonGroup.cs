using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Framework.WebElements
{
    public class RadioButtonGroup
    {
        private IList<IWebElement> radioButtons;

        public RadioButtonGroup(IList<IWebElement> radioButtons)
        {
            this.radioButtons = radioButtons;
        }

        public void SelectRadioButton(string value)
        {
            foreach (var radioButton in radioButtons)
            {
                if (radioButton.GetAttribute("value").Equals(value))
                {
                    radioButton.Click();
                    break;
                }
            }
        }

        public string GetSelectedRadioButtonValue()
        {
            foreach (var radioButton in radioButtons)
            {
                if (radioButton.Selected)
                {
                    return radioButton.GetAttribute("value");
                }
            }
            return "";
        }

        public IWebElement GetRadioButtonElementWithLabel(string labelText)
        {
            IWebElement e = null;
            foreach (var radioButton in radioButtons)
            {
                if (radioButton.FindElement(By.XPath("./following::label[1]")).Text.Trim().Equals(labelText))
                {
                    e = radioButton;
                    break;
                }
            }
            return e;
        }

        public void SelectRadioButtonWithLabel(string labelText)
        {
            foreach (var radioButton in radioButtons)
            {
                if (radioButton.FindElement(By.XPath("./following::label[1]")).Text.Trim().Equals(labelText))
                {
                    radioButton.Click();
                    break;
                }
            }
        }

        public void SelectRadioButtonClickLabel(string labelText)
        {
            foreach (var radioButton in radioButtons)
            {
                var label = radioButton.FindElement(By.XPath("./following::label[1]"));
                if (label.Text.Trim().Equals(labelText))
                {
                    label.Click();
                    break;
                }
            }
        }
    }
}