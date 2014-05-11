using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class BoardState2 {
        public BoardState2(int width, int height) {
            this.width = width;
            this.height = height;
            this.lattice = new CellLatice(width, height);

            this.p1 = new Person(width / 2, height / 2 + 2);

            this.p2 = new Person(width / 2, height / 2 - 2);
        }

        private CellLatice lattice;

        public List<List<int>> State {
            get {
                return lattice.GetCells(p1, p2);
            }
        }

        private int width, height;

        private Person p1, p2;

        private Direction getDirection(string msg, bool orientation) {
            switch (msg) {
                case "119": ///w
                    return orientation ? Direction.up: Direction.down;
                case "115": // s
                    return orientation ? Direction.down: Direction.up;
                case "97": //a
                    return Direction.right;
                case "100": // d
                    return Direction.left;
            }
            return Direction.none;
        }

        internal void Input(string msg, string username, bool orientation) {
            var dir = getDirection(msg, orientation);
            if (orientation) {
                this.p1.UpdatePosition(dir);
            } else {
                this.p2.UpdatePosition(dir);
            }
        }
    }
}