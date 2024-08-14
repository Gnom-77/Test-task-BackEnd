namespace personal_meeting_management_app
{
    internal class MeetingManager
    {
        private List<Meeting> meetings = new List<Meeting>();

        public void AddMeeting(Meeting meeting)
        {
            if (meeting.StartTime <= DateTime.Now)
            {
                throw new InvalidOperationException("Встреча должна быть запланирована на будущее время.");
            }

            if (IsOverlapping(meeting))
            {
                throw new InvalidOperationException("Встречи не должны пересекаться.");
            }

            if((meeting.EndTime - meeting.StartTime) <= TimeSpan.Zero)
            {
                throw new InvalidOperationException("Дата конца встречи должна быть позже начала встречи.");
            }
            if (meeting.ReminderTime < TimeSpan.Zero)
            {
                throw new InvalidOperationException("Время напоминания не может быть отрицательным.");
            }

            meetings.Add(meeting);
            Console.WriteLine("Встреча успешно добавлена.");
        }

        public void RemoveMeeting(string title, DateTime startTime)
        {
            var meeting = meetings.FirstOrDefault(m => m.Title == title && m.StartTime == startTime);
            if (meeting != null)
            {
                meetings.Remove(meeting);
                Console.WriteLine("Встреча успешно удалена.");
            }
            else
            {
                Console.WriteLine("Встреча не найдена.");
            }
        }

        public void UpdateMeeting(string title, DateTime startTime, DateTime? newStartTime = null, DateTime? newEndTime = null, TimeSpan? newReminderTime = null)
        {
            var meeting = meetings.FirstOrDefault(m => m.Title == title && m.StartTime == startTime);
            if (meeting != null)
            {
                DateTime updatedStartTime = newStartTime ?? meeting.StartTime;
                DateTime updatedEndTime = newEndTime ?? meeting.EndTime;
                TimeSpan updatedReminderTime = newReminderTime ?? meeting.ReminderTime;

                if (updatedStartTime <= DateTime.Now)
                {
                    throw new InvalidOperationException("Встреча должна быть запланирована на будущее время.");
                }

                if ((updatedEndTime - updatedStartTime) <= TimeSpan.Zero)
                {
                    throw new InvalidOperationException("Дата конца встречи должна быть позже начала встречи.");
                }

                if (updatedReminderTime < TimeSpan.Zero)
                {
                    throw new InvalidOperationException("Время напоминания не может быть отрицательным.");
                }

                // Временное создание обновленной встречи для проверки на пересечение
                var tempMeeting = new Meeting(title, updatedStartTime, updatedEndTime, updatedReminderTime);
                if (IsOverlapping(tempMeeting, meeting))
                {
                    throw new InvalidOperationException("Встречи не должны пересекаться.");
                }

                meeting.ChangeStartTime(updatedStartTime);
                meeting.ChangeEndTime(updatedEndTime);
                meeting.ChangeReminderTime(updatedReminderTime);

                Console.WriteLine("Встреча успешно обновлена.");
            }
            else
            {
                Console.WriteLine("Встреча не найдена.");
            }
        }

        public IEnumerable<Meeting> GetMeetingsForDay(DateTime day)
        {
            return meetings.Where(m => m.StartTime.Date == day.Date).OrderBy(m => m.StartTime);
        }

        public void ExportMeetingsToFile(DateTime day, string filePath)
        {
            var meetingsForDay = GetMeetingsForDay(day);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var meeting in meetingsForDay)
                {
                    sw.WriteLine(meeting);
                }
            }
            Console.WriteLine($"Расписание встреч за {day.ToShortDateString()} экспортировано в {filePath}");
        }

        private bool IsOverlapping(Meeting newMeeting, Meeting? excludeMeeting = null)
        {
            return meetings.Any(m => m != excludeMeeting && m.StartTime < newMeeting.EndTime && newMeeting.StartTime < m.EndTime);
        }

        public List<Meeting> GetAllMeetings()
        {
            return meetings;
        }
    }

}
