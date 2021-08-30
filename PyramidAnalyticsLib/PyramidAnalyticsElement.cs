using System;
using System.Collections.Generic;
using System.Text;

namespace PyramidAnalytics
{
    public class PyramidAnalyticsElement
    {
        #region ATTRIBUTES
        protected string ID;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public PyramidAnalyticsElement()
        {
            this.ID = null;
        }

        public PyramidAnalyticsElement(string id)
        {
            this.ID = id;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string id { set { this.ID = value; } get { return this.ID; }}
        #endregion PROPERTIES
    }
}
