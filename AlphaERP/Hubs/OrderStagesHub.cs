using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlphaERP.Hubs
{

    public class OrderStagesHub : Hub
    {
        public void UpdateOrderStages(int orderYear, int orderNo)
        {
            Clients.All.updateOrderStages(orderYear, orderNo);
        }
    }
}