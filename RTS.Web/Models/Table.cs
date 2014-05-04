using RTS.Web.Hubs;
using RTS.Web.Models.BoardStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class Table {
        public Table() {
            this.ID = idCounter++;
            this.Users = new List<ConnectedUser>();
            //this.State = new CellLatice(5, 5);
            this.State = new Arena(300, 500);
        }

        public string AsString {
            get {
                return string.Format("ID: {0}, Connected: {1}", 
                    this.ID, 
                    string.Join(", ", this.Users.Select(i => i.Name)));
            }
        }

        public Table(ConnectedUser u) : this(){
            this.AddUser(u);
        }

        public int ID { get; set; }
        public List<ConnectedUser> Users { get; set; }

        public static int idCounter = 0;

        public static Table Create() {
            return new Table();
        }

        public IVisual State { get; set; }

        internal void AddUser(ConnectedUser u) {
            this.Users.Add(u);
            u.CurrentTable = this.ID;
        }

        internal void Remove(ConnectedUser match) {
            this.Users.Remove(match);
        }
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
            t.State.Input(msg, connectionID);
            return t;
        }

        public static IVisual GetState(int id) {
            var table = Tables.Where(i => i.ID == id).SingleOrDefault();
            if (table == null) {
                var newTable = new Table();
                newTable.ID = id;
                TableManager.Tables.Add(newTable);
                return newTable.State;
            } else {
                var state = table.State;
                return state;
            }
        }

        public static List<Table> Tables = new List<Table>();
    }
}