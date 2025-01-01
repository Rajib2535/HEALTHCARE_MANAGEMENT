using Newtonsoft.Json;

namespace WEB_APP.Models
{
    public enum ToastType
    {
        ERROR,
        INFO,
        SUCCESS,
        WARNING
    }
    public class Toastr
    {

        [JsonProperty("newestOnTop")]
        public bool ShowNewestOnTop { get; set; }
        [JsonProperty("closeButton")]
        public bool ShowCloseButton { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }

        public Toastr()
        {
            ToastMessages = new List<ToastMessage>();
            ShowNewestOnTop = false;
            ShowCloseButton = true;
        }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType, bool isSticky)
        {
            var toast = new ToastMessage()
            {
                Title = title,
                Message = message,
                ToastType = toastType,
                IsSticky = isSticky
            };
            ToastMessages.Add(toast);
            return toast;
        }
    }
}
