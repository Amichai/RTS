using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace RTS.Web.Models.BoardStateModels {
    public class BoardState2 {
        ILog log = LogManager.GetLogger(typeof(BoardState2));

        public BoardState2(int width, int height) {
            this.width = width;
            this.height = height;
            this.Reset();
        }

        private CellLatice lattice;

        public bool? Winner {
            get {
                return this.lattice.Winner;
            }
        }

        public List<List<Cell>> State {
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

        private Pos calculateNewPosition(Direction dir, Pos p) {
            var newPosition = p.Clone();
            switch (dir) {
                case Direction.up:
                    newPosition.Y--;
                    break;
                case Direction.down:
                    newPosition.Y++;
                    break;
                case Direction.right:
                    newPosition.X--;
                    break;
                case Direction.left:
                    newPosition.X++;
                    break;
            }
            newPosition.Normalize(this.width, this.height);
            return newPosition;
        }

        internal void Input(string msg, string username, bool orientation) {
            if (msg == "32"){ 
                this.lattice.Reserve(orientation, orientation ? p1.Position : p2.Position);
                return;
            }
            var dir = getDirection(msg, orientation);
            
            move(dir, 
                orientation ? p1 : p2,
                orientation ? p2 : p1,  
                orientation);
        }
        
        private void move(Direction dir, Person p, Person other, bool orientation) {
            Pos newPos;
            newPos = this.calculateNewPosition(dir, p.Position);
            if (this.lattice.IsReserved(newPos, orientation)) {
                return;
            }
            this.lattice.PushBricks(p.Position, newPos, other.Position, orientation);
            p.SetPosition(newPos);
        }

        internal void Reset() {
            this.lattice = new CellLatice(width, height);
            this.p1 = new Person(width / 2, height / 2 + 2);
            this.p2 = new Person(width / 2, height / 2 - 2);
        }
    }
}