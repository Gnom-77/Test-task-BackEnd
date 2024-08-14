namespace personal_meeting_management_app
{
    internal class Notifier
    {
        private bool _isExit;
        private MeetingManager _meetingManager;

        public Notifier(MeetingManager manager)
        {
            _meetingManager = manager;
            _isExit = false;
        }

        public void StartNotificationService()
        {
            while (!_isExit)
            {
                var meetings = _meetingManager.GetAllMeetings();
                foreach (var meeting in meetings)
                {
                    if (!meeting.IsNotified && DateTime.Now >= meeting.StartTime - meeting.ReminderTime)
                    {
                        Console.WriteLine($"\n---------Напоминание: {meeting.Title} начнется в {meeting.StartTime}---------");
                        meeting.IsNotified = true;
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public void IsExit()
        {
            _isExit = true;
        }

    }
}
