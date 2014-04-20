using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RTS.Web.Hubs {
    [HubName("HomeHub")]
    public class HomeHub : Hub {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HomeHub() {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeHub>();
            //context.Clients.All.ConnectionEstablished();    
        }

        private static int hitCounter = 0;

        public int ButtonPress() {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeHub>();
            
            context.Clients.All.Test();
            return ++hitCounter;
        }

        public string MyConnectionID() {
            return Context.ConnectionId;
        }

        public bool JoinTable(string id1, string id2) {
            ///create a new table
            ///determine a unique table id
            //Clients.Client(id2).JoinTable(tableID)
            //Clients.Client(id1).JoinTable(tableID)

            return true;
        }

        public void CreateTable() {
            var u = UserHandler.Users.Where(i => i.ConnectionID == Context.ConnectionId).SingleOrDefault();
            if (u != null) {
                u.WaitingAtTable = true;
            }
            Clients.All.waitingTablesChanged(UserHandler.WaitingTables());
            //return UserHandler.TableIds;
        }

        public override Task OnConnected() {
            UserHandler.Add(Context.ConnectionId);
            Clients.All.connectedClientsChanged(UserHandler.ConnectedUsers());
            return base.OnConnected();
        }

        public override Task OnDisconnected() {
            var connectionID = Context.ConnectionId;
            var match = UserHandler.Users.Where(i => i.ConnectionID == connectionID).SingleOrDefault();
            if (match != null) {
                UserHandler.Remove(connectionID);
                Clients.All.connectedClientsChanged(UserHandler.ConnectedUsers());
                if (match.WaitingAtTable) {
                    Clients.All.waitingTablesChanged(UserHandler.WaitingTables());
                }
            }
            return base.OnDisconnected();
        }
    }

    public static class UserHandler {
        public static List<ConnectedUser> Users = new List<ConnectedUser>();
        public static List<string> WaitingTables() {
            return Users.Where(i => i.WaitingAtTable).Select(i => i.ConnectionID).ToList();
        }

        public static List<string> ConnectedUsers() {
            return Users.Select(i => i.ConnectionID).ToList();
        }

        public static void Remove(string connectionID) {
            var match = Users.Where(i => i.ConnectionID == connectionID).SingleOrDefault();
            if (match != null) {
                Users.Remove(match);
            }
        }

        public static void Add(string connectionID) {
            ///check that this user doesn't exist already
            if (Users.Where(i => i.ConnectionID == connectionID).Count() > 0) {
                return;
            }
            UserHandler.Users.Add(new ConnectedUser(connectionID));
        }

        //public static List<string> ConnectedIds = new List<string>();
        //public static List<string> TableIds = new List<string>();
    }
}