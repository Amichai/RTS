using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class ConnectedUser {
        public ConnectedUser(string connectionID) {
            this.Name = "Annonymous";
            this.ConnectionID = connectionID;
            this.CurrentTable = null;
        }

        public int? CurrentTable { get; set; }

        public string Name { get; set; }
        public string ConnectionID { get; set; }
        //public UserState State { get; set; }
    }
    //public enum UserState { WaitingAtTable, Playing, Idle };
}