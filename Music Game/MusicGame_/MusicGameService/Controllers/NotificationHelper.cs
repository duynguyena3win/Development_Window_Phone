using System;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using MusicGameService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;

namespace MusicGameService.Controllers
{
    public class NotificationHelper
    {
        public static ApiServices Services { get; set; }

        static NotificationHelper()
        {
            ConfigOptions options = new ConfigOptions();
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));
            Services = new ApiServices(config);
        }
        public async static void CreateNotification(string textMessage)
        {
            string str_message = string.Empty;
            str_message = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                    "<wp:Notification xmlns:wp=\"WPNotification\">" +
                       "<wp:Toast>" +
                            "<wp:Text1>" + textMessage + "</wp:Text1>" +
                       "</wp:Toast> " +
                    "</wp:Notification>";
            
            MpnsPushMessage message = new MpnsPushMessage(new Toast());
            message.XmlPayload = str_message;

            try
            {
                var result = await Services.Push.SendAsync(message);
                Services.Log.Info(result.State.ToString());
            }
            catch (System.Exception ex)
            {
                Services.Log.Error(ex.Message, null, "Push.SendAsync Error");
            }
        }
    }
}