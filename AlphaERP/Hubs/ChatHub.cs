using Microsoft.AspNet.SignalR;
using System;

namespace AlphaERP.Hubs
{
    public class ChatHub : Hub
    {
       
        public void Send(string sender, string receiver, string message)
        {
            string x = DateTime.Now.ToString("yy-MM-dd hh:mm");
            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            hubContext.Clients.All.notify(sender, receiver, message, x);
        }

        public void notify(int Id, string UserName, string UserNameEn)
        {
            Clients.All.notify(Id, UserName, UserNameEn);
        }

    }
}