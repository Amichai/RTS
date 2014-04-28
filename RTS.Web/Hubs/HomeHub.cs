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
        }

        public string MyConnectionID() {
            return Context.ConnectionId;
        }

        public bool UpdateName(string name) {
            var id = Context.ConnectionId;
            UserManager.Get(id).Name = name;
            Clients.All.connectedClientsChanged(UserManager.ConnectedUsers());
            return true;
        }

        ///Todo: we need to work fully interms of persistent and unique usernames!

        public bool JoinTable(string id1, string id2) {
            //var table = Table.Create();
            var table = UserManager.Tables.Where(i => i.Users.Select(j => j.ConnectionID).Contains(id1)).Single();
            var tableID = table.ID;
            log.InfoFormat("{0} and {1} joining table: {2}", id1, id2, tableID);
            TableManager.Add(table);
            var u1 = UserManager.Get(id1);
            var u2 = UserManager.Get(id2);
            Clients.Client(id1).JoinTable(tableID);
            Clients.Client(id2).JoinTable(tableID);
            table.Users.Add(u1);
            table.Users.Add(u2);
            return true;
        }

        public void CreateTable() {
            var u = UserManager.Users.Where(i => i.ConnectionID == Context.ConnectionId).SingleOrDefault();
            var newTable = new Table(u);
            UserManager.Tables.Add(newTable);
            Clients.All.waitingTablesChanged(UserManager.WaitingTables());
        }
        private static int userCounter = 1;

        public override Task OnConnected() {
            UserManager.Add(Context.ConnectionId);
            var username = "anonymous" + userCounter++;
            Clients.Caller.SetUsername(username);
            var id = Context.ConnectionId;
            UserManager.Get(id).Name = username;
            Clients.All.connectedClientsChanged(UserManager.ConnectedUsers());
            return base.OnConnected();
        }

        public override Task OnDisconnected() {
            var connectionID = Context.ConnectionId;
            var match = UserManager.Users.Where(i => i.ConnectionID == connectionID).SingleOrDefault();
            if (match != null) {
                UserManager.Remove(connectionID);
                Clients.All.connectedClientsChanged(UserManager.ConnectedUsers());

                if (match.CurrentTable != null) {
                    var tableID = match.CurrentTable;
                    var table = UserManager.Tables.Where(i => i.ID == tableID).Single();
                    table.Remove(match);
                    Clients.All.waitingTablesChanged(UserManager.WaitingTables());
                }
            }
            return base.OnDisconnected();
        }
    }

    public static class UserManager {

        public static Dictionary<string, string> Usernames = new Dictionary<string,string>();
        public static List<ConnectedUser> Users = new List<ConnectedUser>();
        public static List<Table> Tables = new List<Table>();
        public static List<Table> WaitingTables() {
            return Tables.Where(i => i.Users.Count() == 1).ToList();
            //return Users.Where(i => i.State == UserState.WaitingAtTable).ToList();
        }

        public static List<ConnectedUser> ConnectedUsers() {
            return Users.ToList();
        }

        public static void Remove(string connectionID) {
            lock (Users) {
                var match = Users.Where(i => i.ConnectionID == connectionID).SingleOrDefault();
                if (match != null) {
                    Users.Remove(match);
                }
            }
        }

        public static ConnectedUser Get(string connectionID) {
            return Users.Where(i => i.ConnectionID == connectionID).SingleOrDefault();
        }

        public static void Add(string connectionID) {
            ///check that this user doesn't exist already
            lock (Users) {
                if (Users.Where(i => i.ConnectionID == connectionID).Count() > 0) {
                    return;
                }
                Users.Add(new ConnectedUser(connectionID));
            }
        }
    }
}