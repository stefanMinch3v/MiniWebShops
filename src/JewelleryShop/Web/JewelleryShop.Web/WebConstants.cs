namespace JewelleryShop.Web
{
    public class WebConstants
    {
        // admin
        public const string AdministratorRole = "Admin";

        // users
        public const int UserNameMaxLength = 20;
        public const int UserNameMinLength = 3;

        // product success data
        public const string SuccessfullyAddedProduct = "The product was successfully added!";
        public const string SuccessfullyEditedProduct = "The product was successfully edited!";
        public const string SuccessfullyDeletedProduct = "The product was successfully deleted!";

        // home controller name
        public const string HomeControllerName = "Home";

        // product error data
        public const string TitleError = "The title is required!";
        public const string DescriptionError = "The description is required!";
        public const string PurchaseNumberError = "The product number is required!";
        public const string PriceError = "The price must be minimum 0!";
        public const string UnexistingProduct = "The product does not exist!";

        // imgur registered profile data
        public const string ClientIdImgur = "52e2522ba76e7d2";

        // imgur token error data
        public const string InvalidImgurToken = "Failed login to Imgur account!";

        // temp data messages
        public const string TempDataErrorMessageKey = "ErrorMessage";
        public const string TempDataSuccessMessageKey = "SuccessMessage";
    }
}
