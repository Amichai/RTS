using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class BoardState2 {
        public BoardState2() {
            this.State = Enumerable.Range(0, 10).ToList();
        }
        public List<int> State { get; set; }

        private static Random rand = new Random();
        internal void Input(string msg, string connectionID) {
            this.State[rand.Next(10)]++;
        }
    }
}