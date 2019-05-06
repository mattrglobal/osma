using System;
using System.Web;
using AgentFramework.Core.Messages.Connections;
using Newtonsoft.Json;

namespace Osma.Mobile.App.Services.Utils
{
    /// <summary>
    /// Utilities for manipulating invitations
    /// </summary>
    public static class InvitationUtils
    {
        static readonly char[] padding = { '=' };
        /// <summary>
        /// Attempts to decode an invite from a json string
        /// </summary>
        /// <param name="inviteContents">A json string representation of an invite</param>
        /// <exception cref="ArgumentNullException">If the <param name="inviteContents"/> was empty or null</exception>
        /// <returns>A Connection message</returns>
        public static ConnectionInvitationMessage DecodeInvite(string inviteContents)
        {
            if (string.IsNullOrEmpty(inviteContents))
                throw new ArgumentNullException(nameof(inviteContents));

            return JsonConvert.DeserializeObject<ConnectionInvitationMessage>(inviteContents);
        }

        public static string FromBase64String(string inviteContents)
        {
            string invite = inviteContents.Replace('_', '/').Replace('-', '+');
            switch (inviteContents.Length % 4)
            {
                case 2: invite += "=="; break;
                case 3: invite += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(invite);
            string inviteJson = System.Text.Encoding.ASCII.GetString(bytes);
            return inviteJson;
        }

        public static string FromUri(string text)
        {
            Uri inviteUri;
            bool check = Uri.TryCreate(text, UriKind.Absolute, out inviteUri) && (inviteUri.Scheme == Uri.UriSchemeHttp || inviteUri.Scheme == Uri.UriSchemeHttps);
            if (check) // if uri is provided
            {
                return System.Web.HttpUtility.ParseQueryString(inviteUri.Query).Get("c_i"); // get c_i from invite....
            }
            else
            {
                return text;
            }
        }
    }
}
