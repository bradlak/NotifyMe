using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using NotifyMe.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace NotifyMe.Backend.Controllers
{
    [MobileAppController]
    public class NotificationsController : ApiController
    {
        string connectionString = "";
        string notificationHubName = "";

        private NotificationHubClient hub;

        public NotificationsController()
        {
            hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubName);
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok("Hello world");
        }

        [Route("api/notifications/register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] DeviceRegistration device)
        {
            RegistrationDescription registration = new GcmRegistrationDescription(device.Handle);

            var registrationId = await CreateRegistrationId(device.Handle);

            registration.RegistrationId = registrationId;
            registration.Tags = new HashSet<string>() { device.Tag };

            try
            {
                await hub.CreateOrUpdateRegistrationAsync(registration);
                return Ok();
            }
            catch (MessagingException ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/notifications/send")]
        [HttpPost]
        public async Task<IHttpActionResult> Send([FromBody] Message message)
        {
            try
            {
                var registrations = await hub.GetRegistrationsByTagAsync(message.RecipientId, 100);

                NotificationOutcome outcome;

                if (registrations.Any(r => r is GcmRegistrationDescription))
                {
                    var notif = "{ \"data\" : {\"subject\":\"Message from " + message.From + "\", \"message\":\"" + message.Body + "\"}}";
                    outcome = await hub.SendGcmNativeNotificationAsync(notif, message.RecipientId);
                    return Ok(outcome);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private async Task<string> CreateRegistrationId(string handle = null)
        {
            string newRegistrationId = null;

            if (handle != null)
            {
                var registrations = await hub.GetRegistrationsByChannelAsync(handle, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await hub.CreateRegistrationIdAsync();

            return newRegistrationId;
        }
    }
}