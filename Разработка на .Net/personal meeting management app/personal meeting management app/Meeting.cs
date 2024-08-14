namespace personal_meeting_management_app
{
    internal class Meeting
    {
        private string _title;
        private DateTime _startTime;
        private DateTime _endTime;
        private TimeSpan _reminderTime;
        private bool _isNotified;

        public Meeting(string title, DateTime startTime, DateTime endTime, TimeSpan reminderTime) 
        {
            _title = title;
            _startTime = startTime;
            _endTime = endTime;
            _reminderTime = reminderTime;
            _isNotified = false;
        }

        public override string ToString()
        {
            return $"{_title} | {_startTime} - {_endTime} | Напомнить за {_reminderTime}";
        }

        public void ChangeStartTime(DateTime newStartTime)
        {
            _startTime = newStartTime;
        }

        public void ChangeEndTime(DateTime newEndTime)
        {
            _endTime = newEndTime;
        }

        public void ChangeReminderTime(TimeSpan newReminderTime)
        {
            _reminderTime = newReminderTime;
        }

        public string Title
        {
            get { return _title; }
        }
        public DateTime StartTime
        {
            get { return _startTime; }
        }
        public DateTime EndTime
        {
            get { return _endTime; }
        }
        public TimeSpan ReminderTime
        {
            get { return _reminderTime; }
        }
        public bool IsNotified
        {
            get { return _isNotified; }
            set { _isNotified = value; }
        }

    }
}
