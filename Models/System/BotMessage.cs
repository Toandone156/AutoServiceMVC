namespace AutoServiceMVC.Models.System
{
    public class BotMessage
    {
        public string role { get; set; }
        public string? content { get; set; }
    }

    public class ResponseFunctionMessage : BotMessage
    {
        public object function_call { get; set; }
    }

    public class RequestFunctionMessage : BotMessage
    {
        public string name { get; set; }
    }
}
