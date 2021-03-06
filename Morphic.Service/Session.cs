﻿namespace Morphic.Service
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Microsoft.Extensions.Logging;
    using Settings.SolutionsRegistry;

    public class SessionOptions
    {
        public string Endpoint { get; set; } = "";

        public string FrontEnd { get; set; } = "";

        public Uri FontEndUri => new Uri(this.FrontEnd);
    }

    public abstract class Session : IHttpServiceCredentialsProvider
    {
        /// <summary>
        /// Create a new session with the given URL
        /// </summary>
        protected Session(SessionOptions options, Storage storage, Keychain keychain, IUserSettings userSettings,
            ILogger logger, ILogger<HttpService> httpLogger, Solutions solutions)
        {
            this.Service = new HttpService(new Uri(options.Endpoint), this, httpLogger);
            this.Storage = storage;
            this.keychain = keychain;
            this.logger = logger;
            this.Solutions = solutions;
            this.userSettings = userSettings;
        }

        /// <summary>
        /// The underlying Morphic service this session talks to
        /// </summary>
        public HttpService Service { get; }

        protected readonly Keychain keychain;

        public readonly Storage Storage;

        protected readonly IUserSettings userSettings;

        protected readonly ILogger logger;

        public Solutions Solutions { get; }

        /// <summary>
        /// Open the session by trying to login with the saved user information, if any
        /// </summary>
        /// <returns>A task that completes when the user information has been fetched</returns>
        public abstract Task Open();

        #region Authentication

        /// <summary>
        /// The current user's saved credentials, if any
        /// </summary>
        protected ICredentials? CurrentCredentials
        {
            get
            {
                if (this.CurrentUserId != null)
                {
                    string? username = this.userSettings.GetUsernameForId(this.CurrentUserId);
                    if (username != null)
                    {
                        if (this.keychain.LoadUsername(this.Service.Endpoint, username) is ICredentials credentials)
                        {
                            return credentials;
                        }
                    }
                    return this.keychain.LoadKey(this.Service.Endpoint, this.CurrentUserId);
                }
                return null;
            }
        }

        public ICredentials? CredentialsForHttpService(HttpService service)
        {
            return this.CurrentCredentials;
        }

        public void HttpServiceAuthenticatedUser(User user)
        {
            this.User = user;
        }

        public abstract Task Signin(User user);

        public async Task<bool> Authenticate(UsernameCredentials credentials)
        {
            AuthResponse? auth = await this.Service.Authenticate(credentials);
            bool success = auth != null;
            if (success)
            {
                this.keychain.Save(credentials, this.Service.Endpoint);
                this.Service.AuthToken = auth!.Token;
                this.userSettings.SetUsernameForId(credentials.Username, auth.User.Id);
                await this.Signin(auth.User);
            }
            return success;
        }

        #endregion

        #region User Info

        /// <summary>
        /// The current user's id
        /// </summary>
        public string? CurrentUserId
        {
            get => this.userSettings.UserId;
            set => this.userSettings.UserId = value;
        }

        private User? userValue;

        /// <summary>
        /// The current user's information
        /// </summary>
        public User? User
        {
            get => this.userValue;
            set
            {
                this.userValue = value;
                this.CurrentUserId = value?.Id;
            }
        }

        #endregion

        /// <summary>Gets a setting value.</summary>
        public Task<T> GetSetting<T>(SettingId settingId, T defaultValue = default)
        {
            return this.Solutions.GetSetting(settingId).GetValue<T>(defaultValue);
        }

        /// <summary>Gets a setting value.</summary>
        public Task<object?> GetSetting(SettingId settingId)
        {
            return this.Solutions.GetSetting(settingId).GetValue();
        }

        /// <summary>Sets the value of a setting.</summary>
        public Task<bool> SetSetting(SettingId settingId, object? newValue)
        {
            return this.Solutions.GetSetting(settingId).SetValue(newValue);
        }

    }
}
