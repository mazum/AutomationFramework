using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Extensions;

namespace Framework.WebElements
{
    public class WebTable
    {
        private IWebElement webTable;
        public IList<string> ColumnTitles { private set; get; }
        public IList<WebTableRow> Rows { private set; get; }
        public int RowCount { get { return Rows.Count; } }

        public WebTable(IWebElement table)
        {
            webTable = table;
            ColumnTitles = GetColumnTitles();
            Rows = GetRows();
        }

        public WebTable(IWebElement table, string rowXPathAttribute)
        {
            webTable = table;
            ColumnTitles = GetColumnTitles();
            Rows = GetRows(rowXPathAttribute);
        }

        private IList<string> GetColumnTitles()
        {
            var colTitles = webTable.FindElements(By.XPath("./thead/tr/th"));
            var titles = new List<string>();
            foreach (var title in colTitles)
            {
                titles.Add(title.Text.Trim());
            }
            return titles;
        }

        public IList<WebTableRow> GetRows()
        {
            var rows = webTable.FindElements(By.XPath("./tbody/tr"));
            var tableRows = new List<WebTableRow>();
            foreach (var row in rows)
            {
                tableRows.Add(new WebTableRow(row));
            }
            return tableRows;
        }

        public IList<WebTableRow> GetRows(string rowXPathAttribute)
        {
            var rows = webTable.FindElements(By.XPath("./tbody/tr" + rowXPathAttribute));
            var tableRows = new List<WebTableRow>();
            foreach (var row in rows)
            {
                tableRows.Add(new WebTableRow(row));
            }
            return tableRows;
        }

        public IWebElement GetTitleElement(string columnTitle)
        {
            var columnTitleCell = By.XPath("./thead/tr[1]/th[" + GetColumnIndex(columnTitle) + "]");
            return webTable.FindElement(columnTitleCell);
        }

        public string GetColumnTitle(int columnIndex)
        {
            var columnTitle = By.XPath("./thead/tr[1]/th[" + columnIndex + "]");
            return webTable.FindElement(columnTitle).Text;
        }

        public int GetColumnIndex(string columnTitle, bool isRegEx = false)
        {
            IList<IWebElement> titles = webTable.FindElements(By.XPath("./thead / tr[1] / th"));
            for (int i = 0; i < titles.Count; i++)
            {
                if (isRegEx)
                {
                    if (titles[i].Text.IsRegExMatch(columnTitle))
                        return i + 1;
                }
                else
                {
                    if (titles[i].Text.Trim().Equals(columnTitle))
                        return i + 1;
                }
            }
            // no match found
            throw new NoSuchElementException("Unable to find element with text \"" + columnTitle + "\" in table head");
        }

        public IWebElement GetCellElement(int rowIndex, int columnIndex)
        {
            var cell = By.XPath("./tbody/tr[" + rowIndex + "]/td[" + columnIndex + "]");
            return webTable.FindElement(cell);
        }

        public IWebElement GetCellElement(int rowIndex, string columnTitle, bool isRegEx = false)
        {
            return GetCellElement(rowIndex, GetColumnIndex(columnTitle, isRegEx));
        }

        public IWebElement GetCellInputElement(int rowIndex, int columnIndex)
        {
            By cell = By.XPath("./tbody/tr[" + rowIndex + "]/td[" + columnIndex + "]/input");
            return webTable.FindElement(cell);
        }

        public IWebElement GetCellInputElement(int rowIndex, string columnTitle, bool isRegEx = false)
        {
            return GetCellInputElement(rowIndex, GetColumnIndex(columnTitle, isRegEx));
        }

        public WebTableRow FindRowWithValueInColumn(int columnIndex, string value)
        {
            foreach (var row in Rows)
            {
                if (row.GetCellElement(columnIndex).Text.Equals(value))
                {
                    return row;
                }
            }
            // no match found
            throw new NoSuchElementException("Unable to find element with text \"" + value + "\" in table column " + columnIndex);
        }

        public WebTableRow FindRowWithValueInColumn(string columnTitle, string value)
        {
            return FindRowWithValueInColumn(GetColumnIndex(columnTitle), value);
        }

        public WebTableRow FindRowWithMatchInColumn(int columnIndex, string pattern)
        {
            foreach (var row in Rows)
            {
                if (row.GetCellElement(columnIndex).Text.IsRegExMatch
                    (pattern))
                {
                    return row;
                }
            }
            // no match found
            throw new NoSuchElementException("Unable to find element matching \"" + pattern + "\" in table column " + columnIndex);
        }

        public WebTableRow FindRowWithMatchInColumn(string columnTitle, string pattern)
        {
            return FindRowWithMatchInColumn(GetColumnIndex(columnTitle), pattern);
        }

        public WebTableRow FindRowWithMatch(string pattern)
        {
            foreach (var row in Rows)
            {
                if (row.Text.IsRegExMatch(pattern))
                {
                    return row;
                }
            }
            // no match found
            throw new NoSuchElementException("Unable to find row matching \"" + pattern + "\" in table");
        }

        public IList<IWebElement> GetCellsInColumn(int columnIndex)
        {
            var cells = new List<IWebElement>();
            foreach (var row in Rows)
            {
                cells.Add(row.GetCellElement(columnIndex));
            }
            return cells;
        }

        public IList<IWebElement> GetCellsInColumn(string columnTitle)
        {
            return GetCellsInColumn(GetColumnIndex(columnTitle));
        }
    }

    public class WebTableRow
    {
        private IWebElement row;

        public string Text { get { return row.Text; } }

        public WebTableRow(IWebElement row)
        {
            this.row = row;
        }

        public IWebElement GetCellElement(int columnIndex)
        {
            By cell = By.XPath("./td[" + columnIndex + "]");
            return row.FindElement(cell);
        }

        public IWebElement GetCellInputElement(int columnIndex)
        {
            By cell = By.XPath("./td[" + columnIndex + "]/input");
            return row.FindElement(cell);
        }
    }
}