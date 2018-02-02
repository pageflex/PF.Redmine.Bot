using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;

namespace PF.Redmine.Bot.Controllers
{

    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly MicrosoftAppCredentials appCredentials;

        public MessagesController(IConfiguration configuration)
        {
            appCredentials = new MicrosoftAppCredentials(configuration);
        }
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        [HttpPost,Authorize(Roles = "Bot")]

        public async Task<OkResult> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;
                await ReplyMessage(activity, $"You sent {activity.Text} which was {length} characters");
            }
            else
            {
                await HandleSystemMessage(activity);
            }

            return Ok();
        }

        /// <summary>
        /// Handles the system message.
        /// </summary>
        /// <param name="activity">The activity.</param>
        private async Task<Activity> HandleSystemMessage(Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.DeleteUserData:
                    // Implement user deletion here
                    // If we handle user deletion, return a real message
                    break;

                case ActivityTypes.ConversationUpdate:
                    // Handle conversation state changes, like members being added and removed
                    // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                    // Not available in all channels
                    break;

                case ActivityTypes.ContactRelationUpdate:
                    // Handle add/remove from contact lists
                    // Activity.From + Activity.Action represent what happened
                    break;

                case ActivityTypes.Typing:
                    // Handle knowing that the user is typing
                    break;

                case ActivityTypes.Ping:
                    await ReplyMessage(activity, "Pong");
                    break;

                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Replies the message.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <param name="message">The message.</param>
        private async Task ReplyMessage(Activity activity, string message)
        {
            var serviceEndpointUri = new Uri(activity.ServiceUrl);
            var connector = new ConnectorClient(serviceEndpointUri, appCredentials);
            var reply = activity.CreateReply(message);

            await connector.Conversations.ReplyToActivityAsync(reply);
        }
    }
}