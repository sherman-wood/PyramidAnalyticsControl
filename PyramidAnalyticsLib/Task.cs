using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Task
    {
        #region ENUMERATIONS
        public enum TaskStatus
        {
            pending = 0, finished = 1, finished_rendering = 2, error = 3, running = 4,
            finished_with_errors = 5, stopped_without_running = 6, during_lock = 7, canceled = 8, canceling = 9,
            pending_for_evaluation = 10, during_lock_evaluation = 11, running_evaluation = 12, stopped_after_evaluation = 13, evaluation_canceled = 14, deleted =15
        }

        public enum ScheduleResultType
        {
            none = 0, containerid = 1, path = 2, rendereddata = 3, flowitemid = 4, alertcontent = 5, filedeleted = 6
        }
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected string Id;
        protected string OutputId;
        protected string ExecutorId;
        protected string Description;
        protected TaskStatus Task_status;
        protected ScheduleResultType Schedule_result_type;
        protected string Result;
        protected string Summary;
        protected long StartDate;
        protected long EndDate;
        protected bool IsSpool;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Task() { }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string id { set { this.Id = value; } get { return this.Id; } }
        public string outputId { set { this.OutputId = value; } get { return this.OutputId; } }
        public string executorId { set { this.ExecutorId = value; } get { return this.ExecutorId; } }
        public string description { set { this.Description = value; } get { return this.Description; } }
        public TaskStatus taskStatus { set { this.Task_status = value; } get { return this.Task_status; } }
        public ScheduleResultType scheduleResultType { set { this.Schedule_result_type = value; } get { return this.Schedule_result_type; } }
        public string result { set { this.Result = value; } get { return this.Result; } }
        public string summary { set { this.Summary = value; } get { return this.Summary; } }
        public long startDate { set { this.StartDate = value; } get { return this.StartDate; } }
        public long endDate { set { this.EndDate = value; } get { return this.EndDate; } }
        public bool isSpool { set { this.IsSpool = value; } get { return this.IsSpool; } }
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
        public static List<Task> parseTasks(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));

            return JsonSerializer.Deserialize<List<Task>>(dataListString);
        }
        #endregion STATIC METHODES
    }
}
