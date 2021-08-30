using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Result
    {
        #region ATTRIBUTES
        private bool success;
        private string errorMessage;
        private List<PyramidAnalyticsElement> modifiedList;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Result(string resultString)
        {
            this.success = resultString.Contains("\"success\":true,");

            if (resultString.Contains("\"modifiedList\":"))
            {
                int errorMessageStart = resultString.IndexOf("\"errorMessage\":\"") + "\"errorMessage\":\"".Length;
                int errorMessageEnd = resultString.IndexOf("\",\"", errorMessageStart);
                this.errorMessage = resultString.Substring((errorMessageStart - 1), (errorMessageEnd - errorMessageStart + 2));
                this.errorMessage = this.errorMessage.Remove(0, 1);
                this.errorMessage = this.errorMessage.Remove(this.errorMessage.Length - 1);

                int modifiedListStart = resultString.IndexOf("\"modifiedList\":") + "\"modifiedList\":".Length;
                int modifiedListEnd = resultString.Length - 3;
                string modifiedListString = resultString.Substring(modifiedListStart, (modifiedListEnd - modifiedListStart + 1));
                this.modifiedList = JsonSerializer.Deserialize<List<PyramidAnalyticsElement>>(modifiedListString);
            }
            else
            {
                int errorMessageStart = resultString.IndexOf("\"errorMessage\":\"") + "\"errorMessage\":\"".Length;
                int errorMessageEnd = resultString.IndexOf("\"}}", errorMessageStart);
                this.errorMessage = resultString.Substring((errorMessageStart - 1), (errorMessageEnd - errorMessageStart + 2));
                this.errorMessage = this.errorMessage.Remove(0, 1);
                this.errorMessage = this.errorMessage.Remove(this.errorMessage.Length - 1);
                this.modifiedList = new List<PyramidAnalyticsElement>();
            }
        }

        public Result(bool success, string errorMessage, List<PyramidAnalyticsElement> modifiedList = null)
        {
            this.success = success;
            this.errorMessage = errorMessage;
            if(modifiedList == null)
            {
                this.modifiedList = new List<PyramidAnalyticsElement>();
            }
            else
            {
                this.modifiedList = modifiedList;
            }
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public bool Success { get { return this.success; } }
        public string ErrorMessage { get { return this.errorMessage; } }
        public List<PyramidAnalyticsElement> ModifiedList { get { return this.modifiedList; } }
        #endregion PROPERTIES

        #region OVERRIDES
        public override string ToString()
        {
            string modListString = "[";
            foreach (PyramidAnalyticsElement e in this.modifiedList)
                modListString += JsonSerializer.Serialize<PyramidAnalyticsElement>(e);
            modListString += "]";

            return "{\"success\":" + this.success.ToString().ToLower() + ",\"errorMessage\":\"" + this.errorMessage + "\",\"modifiedList\":" + modListString + "}";
        }
        #endregion OVERRIDES
    }
}
