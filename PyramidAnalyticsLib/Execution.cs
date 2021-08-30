using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Execution
    {
        #region ENUMERATIONS
        public enum PrintingOutputType
        {
            pdf = 0, word = 1, powerpoint = 2, excel = 3, html = 4,
            image = 5, noprinting = 6, csv = 7, xml = 8, json = 9,
            png = 10, odata = 11
        }
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected string Id;
        protected long StartDate;
        protected long EndDate;
        protected PrintingOutputType OutputType;
        protected float SuccessPct;
        protected float PartialSuccessPct;
        protected float FailedPct;
        protected float CanceledPct;
        protected float RunningPct;
        protected float PendingPct;
        protected float TriggerStoppedPct;
        protected List<Task> Items;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Execution() { }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string id { set { this.Id = value; } get { return this.Id; } }
        public long startDate { set { this.StartDate = value; } get { return this.StartDate; } }
        public long endDate { set { this.EndDate = value; } get { return this.EndDate;} }
        public PrintingOutputType outputType { set { this.OutputType = value; } get { return this.OutputType; } }
        public float successPct { set { this.SuccessPct = value; } get { return this.SuccessPct; } }
        public float partialSuccessPct { set { this.PartialSuccessPct = value; } get { return this.PartialSuccessPct; } }
        public float failedPct { set { this.FailedPct = value; } get { return this.FailedPct; } }
        public float canceledPct { set { this.CanceledPct = value; } get { return this.CanceledPct; } }
        public float runningPct { set { this.RunningPct = value; } get { return this.RunningPct; } }
        public float pendingPct { set { this.PendingPct = value; } get { return this.PendingPct; } }
        public float triggerStoppedPct { set { this.TriggerStoppedPct = value; } get { return this.TriggerStoppedPct; } }
        public List<Task> items { set { this.Items = value; } get { return this.Items; } }
        #endregion PROPERTIES

        #region METHODES
        #endregion METHODES

        #region OVERRIDES
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion OVERRIDES

        #region STATIC METHODES
        public static List<Execution> parseExecutions(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));

            return JsonSerializer.Deserialize<List<Execution>>(dataListString);
        }
        #endregion STATIC METHODES
    }
}
