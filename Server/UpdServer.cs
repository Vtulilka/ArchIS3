using System.Net.Sockets;
using System.Text;
using NLog;
using Server;

namespace UpdServer
{
    public class Server
    {
        private const int Port = 8001;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static DataController DC = new DataController();
        static async Task Main()
        {
            using UdpClient udpServer = new UdpClient(Port);

            logger.Info("Сервер запущен и ждет подключения...");

            while (true)
            {
                var result = await udpServer.ReceiveAsync();
                string request = Encoding.UTF8.GetString(result.Buffer);

                logger.Info($"Запрос получен от {result.RemoteEndPoint}: {request}");

                string response = ProcessRequest(request);
                byte[] responseData = Encoding.UTF8.GetBytes(response);

                _ = udpServer.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                logger.Info($"Запрос отправлен: {result.RemoteEndPoint}: {response}");
            }
        }

        private static string ProcessRequest(string request)
        {
            string[] data = request.Split(",");
            string str = data[0];

            switch (str)
            {
                case "1":
                    return GetAllRecords();
                case "2":
                    try
                    {
                        return GetRecordByNumber(int.Parse(data[1]));
                    }
                    catch (Exception)
                    {
                        return $"{data[1]} не является номером";
                    }
                case "3":
                    try
                    {
                        DeleteRecord(int.Parse(data[1]));
                        return "Запись удалена";
                    }
                    catch (Exception)
                    {
                        return "Неверные данные";
                    }

                case "4":
                    try
                    {
                        AddRecord(data[1], data[2], int.Parse(data[3]), bool.Parse(data[4]));
                        return "Запись добавлена";
                    }
                    catch (Exception)
                    {
                        return "Данные неверные";
                    }
                case "5":
                    return DeletAll();

                default:
                    return "Недопустимая команда";
            }
        }
        private static string GetAllRecords()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < DC.GetStudents().Count; i++)
            {
                string sch = $"\nЗапись {i+1}: \nИмя: {DC.GetStudents()[i].Name}\nФамилия: {DC.GetStudents()[i].Surname}\nВозраст: {DC.GetStudents()[i].Age}\nЕсть ли двойки в четверти?: {DC.GetStudents()[i].IsBad}";
                
                sb.AppendLine(sch);                
            }
            return sb.ToString();
        }

        private static string GetRecordByNumber(int number)
        {
            School school = DC.GetStudents()[number-1];
            if (school != null)
            {
                return $"\nЗапись {number}\nID: {school.ID}\nИмя: {school.Name}\nФамилия: {school.Surname}\nВозраст: {school.Age}\nЕсть ли двойки в четверти?: {school.IsBad}";
            }
            return "Запись не найдена.";
        }

        private static void DeleteRecord(int number)
        {
            School school = DC.GetStudents()[number - 1];
            if (school != null)
            {
                DC.DeleteStudent(school.ID);
            }
        }

        private static string DeletAll()
        {
            DC.DeleteAll();
            return "Все записи удалены";
        }

        private static void AddRecord(string Name, string Surname, int Age, bool IsBad)
        {
            DC.AddStudent(new School { Name = Name, Surname = Surname, Age = Age, IsBad = IsBad });
        }
    }
}



