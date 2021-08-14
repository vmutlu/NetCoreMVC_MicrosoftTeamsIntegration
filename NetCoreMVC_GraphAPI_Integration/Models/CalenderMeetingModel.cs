using System;

namespace NetCoreMVC_GraphAPI_Integration.Models
{
    public class CalenderMeetingModel
    {
        public CalenderMeetingModel()
        {
            Body = new Body() { ContentType = "html" };
            Start = new EventDateTime() { TimeZone = "Etc/GMT-3" };
            End = new EventDateTime() { TimeZone = "Etc/GMT-3" };
            IsOnlineMeeting = true;
            OnlineMeetingProvider = "TeamsForBusiness";
        }

        public string Id { get; set; }
        public string Subject { get; set; }
        public Body Body { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
        public bool IsOnlineMeeting { get; set; }
        public string OnlineMeetingProvider { get; set; }
        public OnlineMeeting OnlineMeeting { get; set; }
    }

    public class Body
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }

    public class EventDateTime
    {
        public DateTime DateTime { get; set; }
        public string TimeZone { get; set; }
    }

    public class OnlineMeeting
    {
        public string JoinUrl { get; set; }
    }
}
