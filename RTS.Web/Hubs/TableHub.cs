﻿using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RTS.Web.Models;
using RTS.Web.Models.BoardStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RTS.Web.Hubs {
    [HubName("TableHub")]
    public class TableHub : Hub {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TableHub() {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeHub>();
            //context.Clients.All.ConnectionEstablished();    
        }

        public IVisual NewMessage(string username, int tableID, string text) {
            var t = TableManager.Update(tableID, text, username);
            var state = TableManager.GetState(tableID);
            var otherUser = t.Users.Where(i => i.Name != username).Single();
            //var otherPlayers = t.Users.Where(i => i.Name != username).Select(i => UserManager.Usernames[i.Name]).ToList();
            (state as Arena).Perspective = otherUser.Perspective;
            var otherPlayers = new List<string>() { UserManager.Usernames[otherUser.Name] };
            Clients.Clients(otherPlayers).State(state.Clone());
            (state as Arena).Perspective = !otherUser.Perspective;
            return state;
        }

        public IVisual GetCurrentState(int tableID, string username) {
            var state = TableManager.GetState(tableID);
            return state;
        }

        public string MyConnectionID(string username) {
            var id = Context.ConnectionId;
            UserManager.Usernames[username] = id;
            return id;
        }

        public override Task OnConnected() {
            return base.OnConnected();
        }

        public override Task OnDisconnected() {
            return base.OnDisconnected();
        }
    }
}