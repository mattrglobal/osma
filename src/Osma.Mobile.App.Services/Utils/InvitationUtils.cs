using System;
using AgentFramework.Core.Messages.Connections;
using Newtonsoft.Json;

namespace Osma.Mobile.App.Services.Utils
{
    /// <summary>
    /// Utilities for manipulating invitations
    /// </summary>
    public static class InvitationUtils
    {
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
    }
}
