namespace STS.Web.ViewModels
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string errorMassage)
        {
            ErrorMessage = errorMassage;
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorMessage { get; set; }
    }
}
