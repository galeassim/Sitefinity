﻿using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Security.Configuration;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Security.Claims.SWT;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Configuration;
using ServiceStack.Text;
using SecConfig = Telerik.Sitefinity.Security.Configuration;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Collections.Generic;
using Telerik.Sitefinity.Abstractions;
using System.Linq;

namespace JXTNext.Sitefinity.Widgets.Authentication.Mvc.Models.LoginForm
{
    public class LoginFormModel : ILoginFormModel
    {
        #region Properties

        /// <inheritDoc/>
        public string ServiceUrl
        {
            get
            {
                this.serviceUrl = this.serviceUrl ?? this.GetClaimsIssuer();
                return this.serviceUrl;
            }
            set
            {
                this.serviceUrl = value;
            }
        }

        /// <inheritDoc/>
        public bool AllowResetPassword { get; set; }

        /// <inheritDoc/>
        public bool ShowRememberMe { get; set; }

        /// <inheritDoc/>
        public string MembershipProvider
        {
            get
            {
                this.membershipProvider = this.membershipProvider ?? UserManager.GetDefaultProviderName();
                return this.membershipProvider;
            }
            set
            {
                this.membershipProvider = value;
            }
        }

        /// <inheritdoc />
        public string CssClass { get; set; }

        /// <inheritDoc/>
        public Guid? LoginRedirectPageId { get; set; }

        /// <inheritDoc/>
        public Guid? RegisterRedirectPageId { get; set; }

        /// <inheritDoc/>
        public string SerializedExternalProviders
        {
            get
            {
                return this.serializedExternalProviders;
            }
            set
            {
                if (this.serializedExternalProviders != value)
                {
                    this.serializedExternalProviders = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether password retrieval is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if password retrieval is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool EnablePasswordRetrieval
        {
            get
            {
                return UserManager.GetManager(this.MembershipProvider).EnablePasswordRetrieval;
            }
        }

        /// <summary>
        /// Gets a value indicating whether password reset is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if password reset is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool EnablePasswordReset
        {
            get
            {
                return UserManager.GetManager(this.MembershipProvider).EnablePasswordReset;
            }
        }

        #endregion

        #region Public Methods

        /// <inheritDoc/>
        public virtual LoginFormViewModel GetLoginFormViewModel()
        {
            var viewModel = new LoginFormViewModel();
            this.InitializeLoginViewModel(viewModel);
            return viewModel;
        }

        /// <inheritDoc/>
        public virtual void InitializeLoginViewModel(LoginFormViewModel viewModel)
        {
            if (viewModel != null)
            {
                viewModel.ServiceUrl = this.ServiceUrl;
                viewModel.MembershipProvider = this.MembershipProvider;
                viewModel.RedirectUrlAfterLogin = this.GetPageUrl(this.LoginRedirectPageId);
                viewModel.RegisterPageUrl = this.GetPageUrl(this.RegisterRedirectPageId);
                viewModel.ShowRegistrationLink = this.RegisterRedirectPageId.HasValue;
                viewModel.ShowForgotPasswordLink = this.AllowResetPassword && (this.EnablePasswordReset || this.EnablePasswordRetrieval);
                viewModel.Realm = ClaimsManager.CurrentAuthenticationModule.GetRealm();
                viewModel.CssClass = this.CssClass;
                viewModel.ShowRememberMe = this.ShowRememberMe;

                if (!string.IsNullOrEmpty(this.serializedExternalProviders))
                {
                    viewModel.ExternalProviders = JsonSerializer.DeserializeFromString<Dictionary<string, string>>(this.serializedExternalProviders);
                }
            }
        }

        /// <inheritDoc/>
        public virtual ResetPasswordViewModel GetResetPasswordViewModel(string securityToken, bool resetComplete = false, string error = null)
        {
            var securityParams = HttpUtility.ParseQueryString(securityToken);
            var userId = this.GetUserId(securityParams);
            var userManager = UserManager.GetManager(this.MembershipProvider);
            string question = null;

            if (userId != Guid.Empty)
            {
                var user = userManager.GetUser(userId);

                if (user != null)
                {
                    question = user.PasswordQuestion;
                }
            }

            return new ResetPasswordViewModel()
            {
                CssClass = this.CssClass,
                LoginPageUrl = this.GetPageUrl(null),
                RequiresQuestionAndAnswer = userManager.RequiresQuestionAndAnswer,
                Error = error,
                ResetPasswordQuestion = question,
                ResetComplete = resetComplete,
                SecurityToken = securityToken
            };
        }

        /// <inheritDoc/>
        public virtual ForgotPasswordViewModel GetForgotPasswordViewModel(string email = null, bool emailSent = false, string error = null)
        {
            return new ForgotPasswordViewModel()
            {
                CssClass = this.CssClass,
                LoginPageUrl = this.GetPageUrl(null),
                EmailSent = emailSent,
                Error = error,
                Email = email
            };
        }

        /// <inheritDoc/>
        public virtual void ResetUserPassword(string newPassword, string answer, NameValueCollection securityParams)
        {
            var userId = this.GetUserId(securityParams);

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException("User could not be retrieved.");
            }

            var manager = UserManager.GetManager(this.MembershipProvider);
            using (new ElevatedModeRegion(manager))
            {
                var resetPassword = manager.ResetPassword(userId, answer);
                manager.ChangePassword(userId, resetPassword, newPassword);
                manager.SaveChanges();
            }
        }

        /// <inheritDoc/>
        public virtual ForgotPasswordViewModel SendResetPasswordEmail(string email)
        {
            var viewModel = new ForgotPasswordViewModel();
            viewModel.Email = email;
            viewModel.EmailSent = false;


            var manager = UserManager.GetManager(this.MembershipProvider);
            var user = manager.GetUserByEmail(email);

            if (user != null)
            {
                if (UserManager.ShouldSendPasswordEmail(user, manager.Provider.GetType()))
                {
                    var currentNode = SiteMapBase.GetActualCurrentNode();
                    if (currentNode != null)
                    {
                        var resetPassUrl = Url.Combine(currentNode.Url, "resetpassword");

                        try
                        {
                            UserManager.SendRecoveryPasswordMail(UserManager.GetManager(user.ProviderName), email, resetPassUrl);
                        }
                        catch (Exception ex)
                        {
                            if (Exceptions.HandleException(ex, ExceptionPolicyName.IgnoreExceptions))
                                throw ex;

                            viewModel.Error = ex.Message;
                        }
                    }
                }
            }

            // Always send correct message (for security)
            viewModel.EmailSent = true;

            return viewModel;
        }

        /// <inheritDoc/>
        public virtual string GetErrorFromViewModel(System.Web.Mvc.ModelStateDictionary modelStateDict)
        {
            var firstErrorValue = modelStateDict.Values.FirstOrDefault(v => v.Errors.Any());
            if (firstErrorValue != null)
            {
                var firstError = firstErrorValue.Errors.FirstOrDefault();
                if (firstError != null)
                {
                    return firstError.ErrorMessage;
                }
            }

            return null;
        }

        /// <inheritDoc/>
        public virtual string GetPageUrl(Guid? pageId)
        {
            if (pageId.HasValue)
            {
                return HyperLinkHelpers.GetFullPageUrl(pageId.Value);
            }
            else
            {
                var currentNode = SiteMapBase.GetActualCurrentNode();
                return currentNode != null ? HyperLinkHelpers.GetFullPageUrl(currentNode.Id) : null;
            }
        }

        public virtual LoginFormViewModel Authenticate(LoginFormViewModel input, HttpContextBase context)
        {
            input.LoginError = false;

            if (Config.Get<SecurityConfig>().AuthenticationMode == SecConfig.AuthenticationMode.Claims)
            {
                var owinContext = context.Request.GetOwinContext();
                var challengeProperties = ChallengeProperties.ForLocalUser(input.UserName, input.Password, this.MembershipProvider, input.RememberMe, context.Request.Url.ToString());
                challengeProperties.RedirectUri = this.GetReturnURL(context);
                owinContext.Authentication.Challenge(challengeProperties, ClaimsManager.CurrentAuthenticationModule.STSAuthenticationType);
            }
            else
            {
                User user;
                UserLoggingReason result = SecurityManager.AuthenticateUser(
                    this.MembershipProvider,
                    input.UserName,
                    input.Password,
                    input.RememberMe,
                    out user);

                if (result != UserLoggingReason.Success)
                {
                    input.LoginError = true;
                }
                else
                {
                    input.RedirectUrlAfterLogin = this.GetReturnURL(context);
                }
            }

            return input;
        }

        /// <summary>
        /// Authenticates external provider and make IdentityServer challenge
        /// </summary>
        /// <param name="input">Provider name.</param>
        /// <param name="context">Current http context from controller</param>
        public void AuthenticateExternal(string input, HttpContextBase context)
        {
            var widgetUrl = context.Request.Url.ToString();
            var owinContext = context.Request.GetOwinContext();
            var challengeProperties = ChallengeProperties.ForExternalUser(input, widgetUrl);
            challengeProperties.RedirectUri = this.GetReturnURL(context);

            owinContext.Authentication.Challenge(challengeProperties, ClaimsManager.CurrentAuthenticationModule.STSAuthenticationType);
        }
        #endregion

        #region Private Fields and methods

        /// <summary>
        /// Tries the resolve URL from URL referrer.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns></returns>
        private bool TryResolveUrlFromUrlReferrer(HttpContextBase context, out string redirectUrl)
        {
            redirectUrl = string.Empty;
            try
            {
                ////There is few ways to redirect to another page
                ////First method is to combine realm param with redirect_uri param to get the full redirect url
                ////Example: ?realm=http://localhost:8086/&redirect_uri=/Sitefinity/Dashboard
                ////Second method is to use only realm or redirect_uri param to get the full redirect url
                ////Examples: ?realm=http://localhost:8086/Sitefinity/Dashboard
                ////          ?redirect_uri=http://localhost:8086/Sitefinity/Dashboard
                ////Third method is to get ReturnUrl param
                ////Example: ?ReturnUrl=http://localhost:8086/Sitefinity/Dashboard
                Uri urlReferrer = context.Request.UrlReferrer;
                if (urlReferrer != null)
                {
                    var querySegment = HttpUtility.UrlDecode(urlReferrer.Query);
                    if (querySegment.StartsWith("?"))
                    {
                        querySegment = querySegment.Substring(1);
                    }

                    //check query string for all search params
                    var queryStrings = querySegment.Split('&');
                    string realm = string.Empty;
                    string redirect_uri = string.Empty;
                    string returnUrl = string.Empty;
                    bool foundReturnUrl = false;
                    foreach (var queryString in queryStrings)
                    {
                        var queryStringPair = queryString.Split('=');
                        switch (queryStringPair[0])
                        {
                            case "realm":
                                realm = queryStringPair[1];
                                break;
                            case "redirect_uri":
                                redirect_uri = queryStringPair[1];
                                break;
                            case "ReturnUrl":
                                returnUrl = querySegment.Remove(0, "ReturnUrl=".Length);
                                foundReturnUrl = true;
                                break;
                        }

                        if (foundReturnUrl)
                        {
                            break;
                        }
                    }

                    //based on the found params get the correct redirectUrl
                    if (!string.IsNullOrWhiteSpace(realm))
                    {
                        redirectUrl = realm;

                        if (!string.IsNullOrWhiteSpace(redirect_uri))
                        {
                            //removing accumulation of more then one '/'
                            if (redirect_uri[0] == '/')
                            {
                                redirectUrl = redirectUrl.TrimEnd('/') + redirect_uri;
                            }
                            else
                            {
                                redirectUrl += redirect_uri;
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(redirect_uri))
                    {
                        redirectUrl = redirect_uri;
                    }
                    else if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        redirectUrl = returnUrl;
                    }

                    return true;
                }
            }
            catch (UriFormatException)
            {
                // According to the documentation (http://msdn.microsoft.com/en-us/library/system.web.httprequest.urlreferrer.aspx),
                // UrlReferrer could throw UriFormatException in case The HTTP Referer request header is malformed and cannot be converted to a Uri object. 
                return false;
            }
            return false;
        }

        /// <summary>
        /// Gets the claims issuer.
        /// </summary>
        /// <returns></returns>
        private string GetClaimsIssuer()
        {
            var claimsModule = ClaimsManager.CurrentAuthenticationModule;

            if (claimsModule != null)
            {
                return claimsModule.GetIssuer();
            }
            else
            {
                return LoginFormModel.DefaultRealmConfig;
            }
        }

        /// <summary>
        /// Inners the get user identifier.
        /// </summary>
        /// <returns>
        /// The user id or null.
        /// </returns>
        private Guid GetUserId(NameValueCollection securityParams)
        {
            var cip = SecurityManager.GetManager().GetPasswordRecoveryUser(securityParams);
            if (cip != null)
            {
                return cip.UserId;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Gets ReturnURL set by administrator or taken from query string
        /// </summary>
        /// <returns>
        /// ReturnURL to redirect or empty string
        /// </returns>
        protected string GetReturnURL(HttpContextBase context)
        {
            string redirectUrl = context.Request.Url.AbsoluteUri;

            if (!string.IsNullOrEmpty(context.Request.Url.Query))
            {
                // remove err flag in redirect data
                redirectUrl = redirectUrl.Replace("&err=true", string.Empty).Replace("err=true", string.Empty);
            }

            if (this.LoginRedirectPageId.HasValue)
            {
                redirectUrl = this.GetPageUrl(this.LoginRedirectPageId);
            }
            else
            {
                //Get redirectUrl from query string parameter
                string redirectUrlFromQS;
                this.TryResolveUrlFromUrlReferrer(context, out redirectUrlFromQS);

                if (!string.IsNullOrWhiteSpace(redirectUrlFromQS))
                {
                    redirectUrl = redirectUrlFromQS;
                }
            }

            return redirectUrl;
        }

        private string serviceUrl;
        private const string DefaultRealmConfig = "http://localhost";
        private string membershipProvider;
        private string serializedExternalProviders;

        #endregion
    }
}
