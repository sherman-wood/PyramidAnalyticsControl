using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Schedule
    {
        #region ENUMERATION
        public enum ScheduleType
        {
            printing = 0, etl = 1, machine_learning = 2, user_groups = 3, logs_clean = 4, 
            provision = 5, recommendation = 6, image_snapshot = 7, alert = 8, clean_db = 9,
            upgrade_hierarchy_security = 10, image_snapshot_now = 11, print_etl = 12, subscription = 13
        }

        public enum ScheduleDataType { once = 0, recurring = 1, run_once = 2 }
        public enum ScheduleStatus { idle = 0, stopped = 1, finished = 2, deleted = 3, hold = 4, canceled = 5 }
        #endregion ENUMERATION

        #region ATTRIBUTES
        protected string ScheduleId;
        protected string Name;
        protected string Description;
        protected ScheduleType Type;
        protected long CreatedDate;
        protected string CreatedBy;
        protected long StartDate;
        protected long EndDate;
        protected ScheduleDataType schedule_data_type = ScheduleDataType.once;
        protected ScheduleStatus Status;
        protected string ScheduleItemId;
        protected string ScheduleItemName;
        #endregion

        #region CONSTRUCTORS
        public Schedule() { }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string scheduleId { set { this.ScheduleId = value; } get { return this.ScheduleId; } }
        public string name { set { this.Name = value; } get { return this.Name; } }
        public string description { set { this.Description = value; } get { return this.Description; } }
        public ScheduleType type { set { this.Type = value; } get { return this.Type; } }
        public long createdDate { set { this.CreatedDate = value; } get { return this.CreatedDate; } }
        public string createdBy { set { this.CreatedBy = value; } get { return this.CreatedBy; } }
        public long startDate { set { this.StartDate = value; } get { return this.StartDate; } }
        public long endDate { set { this.EndDate = value; } get { return this.EndDate; } }
        public ScheduleDataType scheduleDataType { set { this.schedule_data_type = value; } get { return this.schedule_data_type; } }
        public ScheduleStatus status { set { this.Status = value; } get { return this.Status; } }
        public string scheduleItemId { set { this.ScheduleItemId = value; } get { return this.ScheduleItemId; } }
        public string scheduleItemName { set { this.ScheduleItemName = value; } get { return this.ScheduleItemName; } }
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
        public static List<Schedule> parseSchedules(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));

            return JsonSerializer.Deserialize<List<Schedule>>(dataListString);
        }
        #endregion STATIC METHODES
    }
}
