using System.Collections.Generic;

namespace personal_meeting_management_app
{
    internal class Scheduler
    {
        private MeetingManager _meetingManager = new MeetingManager();
        private Notifier _notifier;

        public Scheduler()
        {
            _notifier = new Notifier(_meetingManager);
        }

        public void Start()
        {
            // Запуск службы уведомлений в отдельном потоке
            Thread notificationThread = new Thread(new ThreadStart(_notifier.StartNotificationService));
            notificationThread.Start();

            while (true)
            {
                Console.WriteLine("\n1. Добавить встречу");
                Console.WriteLine("2. Удалить встречу");
                Console.WriteLine("3. Обновить встречу");
                Console.WriteLine("4. Просмотреть встречи на день");
                Console.WriteLine("5. Экспортировать встречи на день");
                Console.WriteLine("6. Выйти");
                Console.Write("Выберите опцию: ");
                var option = Console.ReadLine();
                Console.WriteLine("");

                switch (option)
                {
                    case "1":
                        AddMeeting();
                        break;
                    case "2":
                        RemoveMeeting();
                        break;
                    case "3":
                        UpdateMeeting();
                        break;
                    case "4":
                        ViewMeetingsForDay();
                        break;
                    case "5":
                        ExportMeetingsForDay();
                        break;
                    case "6":
                        _notifier.IsExit();
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        private void AddMeeting()
        {
            Console.Write("Введите название встречи: ");
            string title = Console.ReadLine() ?? "Unknown message";

            DateTime startTime = CheckDateInput("Введите дату и время начала встречи (dd.mm.yyyy HH:mm): ");

            DateTime endTime = CheckDateInput("Введите дату и время окончания встречи (dd.mm.yyyy HH:mm): ");

            TimeSpan reminderTime = CheckReminderTime("Введите время напоминания перед встречей в минутах: ");

            try
            {
                Meeting meeting = new Meeting(title, startTime, endTime, reminderTime);
                _meetingManager.AddMeeting(meeting);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RemoveMeeting()
        {
            Console.Write("Введите название встречи, которую хотите удалить: ");
            string title = Console.ReadLine() ?? "Default";

            DateTime startTime = CheckDateInput("Введите дату и время начала встречи, которую хотите удалить (dd.mm.yyyy HH:mm): ");

            _meetingManager.RemoveMeeting(title, startTime);
        }

        private void UpdateMeeting()
        {
            Console.Write("Введите название встречи, которую хотите обновить: ");
            string title = Console.ReadLine() ?? "Default";

            DateTime startTime = CheckDateInput("Введите дату и время начала встречи, которую хотите обновить (dd.mm.yyyy HH:mm): ");

            Console.Write("Введите новую дату и время начала встречи (оставьте пустым, если не хотите изменять): ");
            string newStartTimeInput = Console.ReadLine() ?? "";
            DateTime? newStartTime = string.IsNullOrWhiteSpace(newStartTimeInput) ? null : DateTime.Parse(newStartTimeInput);

            Console.Write("Введите новую дату и время окончания встречи (оставьте пустым, если не хотите изменять): ");
            string newEndTimeInput = Console.ReadLine() ?? "";
            DateTime? newEndTime = string.IsNullOrWhiteSpace(newEndTimeInput) ? null : DateTime.Parse(newEndTimeInput);

            Console.Write("Введите новое время напоминания перед встречей в минутах (оставьте пустым, если не хотите изменять): ");
            string newReminderTimeInput = Console.ReadLine() ?? "";
            TimeSpan? newReminderTime = string.IsNullOrWhiteSpace(newReminderTimeInput) ? null : TimeSpan.FromMinutes(int.Parse(newReminderTimeInput));

            try
            {
                _meetingManager.UpdateMeeting(title, startTime, newStartTime, newEndTime, newReminderTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ViewMeetingsForDay()
        {
            DateTime day = CheckDateInput("Введите дату для просмотра встреч (dd.mm.yyyy): ");

            var meetings = _meetingManager.GetMeetingsForDay(day);
            if (meetings.Any())
            {
                foreach (var meeting in meetings)
                {
                    Console.WriteLine(meeting);
                }
            }
            else
            {
                Console.WriteLine("На этот день нет запланированных встреч.");
            }
        }

        private void ExportMeetingsForDay()
        {
            DateTime day = CheckDateInput("Введите дату для экспорта встреч (dd.mm.yyyy): ");

            Console.Write("Введите путь для сохранения файла: ");
            string filePath = Console.ReadLine() ?? "default.txt";

            _meetingManager.ExportMeetingsToFile(day, filePath);
        }

        private DateTime CheckDateInput(string text)
        {
            DateTime dateTime;
            Console.Write(text);
            string input = Console.ReadLine() ?? "00.00.0000";
            while (!DateTime.TryParse(input, out dateTime))
            {
                Console.Write("Некорректный формат ввода. Попробуйте еще раз: ");
                input = Console.ReadLine() ?? "00.00.0000";
            }


            return dateTime;
        }
        private TimeSpan CheckReminderTime(string text)
        {
            TimeSpan reminderTime;
            Console.Write(text);
            string input = Console.ReadLine() ?? "0";
            while (!TimeSpan.TryParse(input, out reminderTime))
            {
                Console.Write("Некорректный формат ввода. Попробуйте еще раз: ");
                input = Console.ReadLine() ?? "0";
            }


            return TimeSpan.FromMinutes(Convert.ToInt32(input));
        }
    }
}
