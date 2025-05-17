namespace Core.Constants
{
    public static class ContactMessages
    {
        public const string CreationSuccess = "Contact created successfully.";
        public const string CreationFailure = "Failed to create contact: {0}";

        public const string RetrievalAllFailure = "Failed to retrieve contacts: {0}";
        public const string RetrievalByIdFailure = "Failed to retrieve contact: {0}";
        public const string IdMismatch = "Contact ID does not match.";

        public const string UpdateSuccess = "Contact updated successfully.";
        public const string UpdateFailure = "Failed to update contact: {0}";

        public const string DeleteSuccess = "Contact deleted successfully.";
        public const string DeleteFailure = "Failed to delete contact: {0}";
    }

}
