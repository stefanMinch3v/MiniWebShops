namespace CharityAction.Web
{
    public class WebConstants
    {
        // admin
        public const string AdministratorRole = "Admin";

        // users
        public const int UserNameMaxLength = 20;
        public const int UserNameMinLength = 3;

        // events
        public const int EventMinLength = 3;
        public const int EventMaxContentLength = 5000;
        public const string SuccessfullyAddedEvent = "The event '{0}' was successfully added.";
        public const string SuccessfullyEditedEvent = "The event '{0}' was successfully changed.";
        public const string InvalidEventCreation = "Invalid attempt for creating event.";
        public const string SuccessfullyDeletedEvent = "The event '{0}' was successfully archived.";

        // temp data
        public const string TempDataErrorMessageKey = "ErrorMessage";
        public const string TempDataSuccessMessageKey = "SuccessMessage";

        // page
        public const int PageSize = 8;
    }
}
