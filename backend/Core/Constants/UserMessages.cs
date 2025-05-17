namespace Core.Constants
{
    public static class UserMessages
    {
        public const string LoginSuccess = "User authenticated successfully.";
        public const string LoginFailure = "Invalid username or password.";
        public const string AuthError = "Authentication error: {0}";
        public const string LoginUserNameError = "Username is required.";

        public const string CreationSuccess = "User created successfully.";
        public const string CreationFailure = "Failed to create user: {0}";

        public const string RetrievalAllFailure = "Failed to retrieve users: {0}";
        public const string RetrievalByIdFailure = "Failed to retrieve user: {0}";
        public const string IdMismatch = "User ID does not match.";

        public const string UpdateSuccess = "User updated successfully.";
        public const string UpdateFailure = "Failed to update user: {0}";

        public const string DeleteSuccess = "User deleted successfully.";
        public const string DeleteFailure = "Failed to delete user: {0}";
    }
}

