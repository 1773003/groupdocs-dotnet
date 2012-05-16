using System;

namespace Groupdocs.Sdk
{
    using Groupdocs.Api.Contract;
    using Groupdocs.Common;

    public partial interface IGroupdocsService
    {
        /// <summary>
        /// Returns user profile
        /// </summary>
        /// <param name="userId">A global unique identifier of a user to return data for</param>
        /// <returns></returns>
        UserInfoResult GetUserProfile();
        UpdateUserResult UpdateUserProfile(string firstName, string lastName);
        ChangePasswordResult ChangeUserPassword(string newPassword, string oldPassword);

        UserInfoResult GetUserProfileByResetToken(string token);
        UserInfoResult GetUserProfileByVerifToken(string token);
        UserInfoResult GetUserProfileByClaimedToken(string token);

        UserInfoResult GetUserProfile(string userId);
        CreateUserResult CreateUser(string userId, string password, byte storageProvider);
        UserInfoResult CreateUserLogin(string userId, string password);
        ChangePasswordResult ChangeUserPassword(string userId, string newPassword, string oldPassword);
        ChangePasswordResult ResetUserPassword(string userId);

        AddStorageProviderResult AddStoragePreferences(StorageProvider provider, byte[] token);
        UpdateStorageProviderResult UpdateStoragePreferences(StorageProvider provider, byte[] token);
    }
}
