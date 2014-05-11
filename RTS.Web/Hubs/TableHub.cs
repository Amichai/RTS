using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RTS.Web.Models;
using RTS.Web.Models.BoardStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public BoardState2 NewMessage(string username, int tableID, string text) {
            log.InfoFormat("New message {0}", text);
            var t = TableManager.Get(tableID);
            t.State.Input(text, username, t.Users[0].Name == username);
            var state = TableManager.GetState(tableID);

            var otherPlayers = t.Users.Where(i => i.Name != username).Select(i => UserManager.Usernames[i.Name]).ToList();
            Clients.Clients(otherPlayers).State(state);
            return state;
        }

        public BoardState2 GetCurrentState(int tableID) {
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
            log.InfoFormat("Disconnected: {0}", Context.ConnectionId);
            return base.OnDisconnected();
        }
    }
}