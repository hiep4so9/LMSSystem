using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;

namespace LMSSystem.Repositories
{
    public class MeetingRepository
    {
        public async Task<string> GetMeetingDetails(int scheduleId)
        {
            // Đường dẫn đến file JSON chứa thông tin xác thực
            string credentialsPath = "path/to/credentials.json";

            // Xây dựng phân đoạn quyền truy cập cho Google Calendar API
            string[] scopes = { CalendarService.Scope.CalendarReadonly };

            // Đọc file JSON chứa thông tin xác thực
            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(scopes);
            }

            // Tạo phiên bản CalendarService từ thông tin xác thực
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Your Application Name"
            });

            // Lấy thông tin lịch trình từ Google Calendar API
            string meetingDetails = "";
            try
            {
                // Thực hiện truy vấn lấy sự kiện từ Google Calendar API
                EventsResource.ListRequest request = service.Events.List("primary");
                request.Q = "meeting"; // Lọc các sự kiện có liên quan đến cuộc họp
                Events events = await request.ExecuteAsync();

                // Tìm sự kiện tương ứng với ScheduleID
                var scheduleEvent = events.Items.FirstOrDefault(e => e.Id == scheduleId.ToString());

                if (scheduleEvent != null)
                {
                    // Lấy thông tin cuộc họp từ sự kiện
                    meetingDetails = scheduleEvent.HtmlLink; // Hoặc các thông tin khác của cuộc họp
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ (nếu có)
                Console.WriteLine("Error retrieving meeting details: " + ex.Message);
            }

            return meetingDetails;
        }

    }
}
