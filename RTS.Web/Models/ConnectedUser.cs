using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class ConnectedUser {
        public ConnectedUser(string connectionID) {
            this.Name = "Annonymous";
            this.ConnectionID = connectionID;
            this.WaitingAtTable = false;
        }
        public string Name { get; set; }
        public string ConnectionID { get; set; }
        public bool WaitingAtTable { get; set; }
    }
}