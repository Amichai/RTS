using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class BoardState {
        public BoardState() {
            this.vals = new List<string>();
        }
        private List<string> vals;

        public string AsString {
            get {
                return string.Join("<br/>", vals);
            }
        }

        public void Add(string i, string connectionID) {
            this.vals.Add(i + " " + connectionID);
        }
    }
}