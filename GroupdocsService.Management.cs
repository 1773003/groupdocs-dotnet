using System;
using System.Collections.Specialized;
using Microsoft.Http;

namespace Groupdocs.Sdk
{
    using Groupdocs.Api.Contract;
    using Groupdocs.Api.Contract.UriTemplates;
    using Groupdocs.Common;

    public partial class GroupdocsService
    {
        /// <summary>
        /// Returns user profile
        /// </summary>
        /// <param name="userId">A global unique identifier of a user to return data for</param>
        /// <returns></returns>
        public UserInfoResult GetUserProfile()
        {
            var parameters = new NameValueCollection() { { "userId", this.UserId } };
            var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.GetProfile);
            var response = SubmitRequest<UserInfoResponse>(template, parameters);

            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public UserInfoResult GetUserProfile(string userId)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "userId", userId } };
            var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.GetUserProfile);
            var response = SubmitRequest<UserInfoResponse>(template, parameters);

            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public UserInfoResult GetUserProfileByResetToken(string token)
        {
            return GetUserProfileByToken(token, UserManagementUriTemplates.GetProfileByResetToken);
        }

        public UserInfoResult GetUserProfileByVerifToken(string token)
        {
            return GetUserProfileByToken(token, UserManagementUriTemplates.GetProfileByVerifToken);
        }

        public UserInfoResult GetUserProfileByClaimedToken(string token)
        {
            return GetUserProfileByToken(token, UserManagementUriTemplates.GetProfileByClaimedToken);
        }

        public UpdateUserResult UpdateUserProfile(string firstName, string lastName)
        {
            var userInfo = new UserInfo()
            {
                FirstName = firstName,
                LastName = lastName
            };

            return UpdateUserProfile(userInfo);
        }

        public UpdateUserResult UpdateUserProfile(UserInfo profile)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "userId", profile.Guid.ToString() } };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<UserInfo>(profile))
            {
                var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.UpdateUserProfile);
                var response = SubmitRequest<UpdateUserResponse>(template, parameters, "PUT", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public ChangePasswordResult ChangeUserPassword(string newPassword, string oldPassword)
        {
            var parameters = new NameValueCollection() { { "userId", this.UserId } };
            var pswdInfo = new UserPasswordInfo()
            {
                NewPasswordSalt = newPassword,
                OldPasswordSalt = oldPassword
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<UserPasswordInfo>(pswdInfo))
            {
                var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.ChagePassword);
                var response = SubmitRequest<ChangePasswordResponse>(template, parameters, "PUT", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        /// <summary>
        /// Creates user account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="storageProvider"></param>
        /// <returns></returns>
        public CreateUserResult CreateUser(string userId, string password, byte storageProvider)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId } };
            var payload = new UserInfo
            {
                PrimaryEmail = userId,
                PasswordSalt = password,
                StorageProvider = storageProvider
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<UserInfo>(payload))
            {
                var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.CreateUser);
                var response = SubmitRequest<CreateUserResponse>(template, parameters, "POST", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public UserInfoResult CreateUserLogin(string userId, string password)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "userId", userId } };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<string>(password))
            {
                var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.CreateLogin);
                var response = SubmitRequest<UserInfoResponse>(template, parameters, "POST", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public ChangePasswordResult ChangeUserPassword(string userId, string newPassword, string oldPassword)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "userId", userId } };
            var pswdInfo = new UserPasswordInfo()
            {
                NewPasswordSalt = newPassword,
                OldPasswordSalt = oldPassword
            };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<UserPasswordInfo>(pswdInfo))
            {
                var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.ChageUserPassword);
                var response = SubmitRequest<ChangePasswordResponse>(template, parameters, "PUT", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public ChangePasswordResult ResetUserPassword(string userId)
        {
            var parameters = new NameValueCollection() { { "callerId", this.UserId }, { "userId", userId } };
            var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.ResetPassword);
            var response = SubmitRequest<ChangePasswordResponse>(template, parameters, "DELETE", HttpContent.CreateEmpty());

            if (response.Code != ResponseCode.Ok)
            {
                throw new GroupdocsServiceException(response.ErrorMessage);
            }

            return response.Result;
        }

        public AddStorageProviderResult AddStoragePreferences(StorageProvider provider, byte[] token)
        {
            var parameters = new NameValueCollection() { { "userId", this.UserId }, { "provider", provider.ToString() } };
            var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.AddStorageProvider);
            var providerInfo = new StorageProviderInfo { Provider = provider, Token = token };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<StorageProviderInfo>(providerInfo))
            {
                var response = SubmitRequest<AddStorageProviderResponse>(template, parameters, "POST", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }

        public UpdateStorageProviderResult UpdateStoragePreferences(StorageProvider provider, byte[] token)
        {
            var parameters = new NameValueCollection() { { "userId", this.UserId }, { "provider", provider.ToString() } };
            var template = UserManagementUriTemplates.BuildUriTemplate(UserManagementUriTemplates.UpdateStorageProvider);
            var providerInfo = new StorageProviderInfo { Provider = provider, Token = token };

            using (var content = HttpContentNetExtensions.CreateJsonNetDataContract<StorageProviderInfo>(providerInfo))
            {
                var response = SubmitRequest<UpdateStorageProviderResponse>(template, parameters, "PUT", content);

                if (response.Code != ResponseCode.Ok)
                {
                    throw new GroupdocsServiceException(response.ErrorMessage);
                }

                return response.Result;
            }
        }
    }
}
