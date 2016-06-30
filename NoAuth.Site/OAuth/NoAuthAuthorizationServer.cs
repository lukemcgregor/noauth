using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;

namespace NoAuth.Site.OAuth
{
    internal class NoAuthAuthorizationServer : IAuthorizationServerHost
    {
        public NoAuthAuthorizationServer(ICryptoKeyStore cryptoStore, INonceStore nonceStore)
        {
            CryptoKeyStore = cryptoStore;
            NonceStore = nonceStore;
        }

        #region Implementation of IAuthorizationServerHost

        public ICryptoKeyStore CryptoKeyStore { get; set; }
        public INonceStore NonceStore { get; set; }

        public AccessTokenResult CreateAccessToken(IAccessTokenRequest accessTokenRequestMessage)
        {
            var accessToken = new AuthorizationServerAccessToken();

            // Just for the sake of the sample, we use a short-lived token.  This can be useful to mitigate the security risks
            // of access tokens that are used over standard HTTP.
            // But this is just the lifetime of the access token.  The client can still renew it using their refresh token until
            // the authorization itself expires.
            accessToken.Lifetime = TimeSpan.FromMinutes(60);

            // Also take into account the remaining life of the authorization and artificially shorten the access token's lifetime
            // to account for that if necessary.
            //// TODO: code here

            // For this sample, we assume just one resource server.
            // If this authorization server needs to mint access tokens for more than one resource server,
            // we'd look at the request message passed to us and decide which public key to return.
            accessToken.ResourceServerEncryptionKey = Keys.GetResourceServerEncryptionPublicKey();

            accessToken.AccessTokenSigningKey = Keys.GetAuthorizationServerSigningPrivateKey();

            var result = new AccessTokenResult(accessToken);
            return result;
        }

        public IClientDescription GetClient(string clientIdentifier)
        {
            return new Client();
        }

        public bool IsAuthorizationValid(IAuthorizationDescription authorization)
        {
            return this.IsAuthorizationValid(authorization.Scope, authorization.ClientIdentifier, authorization.UtcIssued, authorization.User);
        }

        public AutomatedUserAuthorizationCheckResponse CheckAuthorizeResourceOwnerCredentialGrant(string userName, string password, IAccessTokenRequest accessRequest)
        {
            // This web site delegates user authentication to OpenID Providers, and as such no users have local passwords with this server.
            throw new NotSupportedException();
        }

        public AutomatedAuthorizationCheckResponse CheckAuthorizeClientCredentialsGrant(IAccessTokenRequest accessRequest)
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool CanBeAutoApproved(EndUserAuthorizationRequest authorizationRequest)
        {
            if (authorizationRequest == null)
            {
                throw new ArgumentNullException("authorizationRequest");
            }
            return true;
        }

        private bool IsAuthorizationValid(HashSet<string> requestedScopes, string clientIdentifier, DateTime issuedUtc, string username)
        {
            return true;
        }

    }
}