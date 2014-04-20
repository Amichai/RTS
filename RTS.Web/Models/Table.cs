using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class Table {
        public Table() {
            this.ID = idCounter++;
            this.Users = new List<ConnectedUser>();
            this.State = new List<string>();
        }
        public int ID { get; set; }
        public List<ConnectedUser> Users { get; set; }

        public static int idCounter = 0;

        public static Table Create() {
            return new Table();
        }

        public List<string> State { get; set; }
    }

    public static class TableManager {

        public static void Add(Table t) {
            Tables.Add(t);
        }

        public static Table Get(int tableID) {
            return Tables.Where(i => i.ID == tableID).Single();
        }

        public static Table Update(int tableID, string msg, string connectionID) {
            var t = Get(tableID);
            t.State.Add(msg);
            return t;
        }

        public static List<string> GetState(int id) {
            return Tables.Where(i => i.ID == id).Single().State;
        }

        public static List<Table> Tables = new List<Table>();
    }
}