using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class CellLatice {

        private int width, height;
        public CellLatice(int width, int height) {
            this.width = width;
            this.height = height;
            this.background = new Dictionary<Pos, int>();
            this.Winner = null;
        }

        private List<List<Cell>> cells;

        private List<List<Cell>> getEmptyBoard() {
            List<List<Cell>> emptyBoard = new List<List<Cell>>();
            for (int i = 0; i < width; i++) {
                emptyBoard.Add(new List<Cell>());
            }
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    emptyBoard[i].Add(new Cell() {
                        Background = 0,
                        Foreground = "_"
                    });
                }
            }
            return emptyBoard;
        }

        private static string playerAGlyph = "A";
        private static string playerBGlyph = "B";


        private Dictionary<int, string> cellGlyph = new Dictionary<int, string>() {
             { 0, "_"},
             { 1, playerAGlyph },
             { 2, playerBGlyph },
             { 3, "C"}
        };

        private Dictionary<bool, string> playerGlyph = new Dictionary<bool, string>() {
            { true, playerAGlyph },
            { false, playerBGlyph }
        };

        private void setVal(Person p, int i) {
            p.Position.Normalize(this.width, this.height);
            this[p.Position].Foreground = this.cellGlyph[i];
        }

        private void incr(Pos p, int i) {
            this[p].Background = i;
        }

        public string Winner { get; set; }

        public List<List<Cell>> GetCells(Person p1, Person p2) {
            cells = this.getEmptyBoard();
            if (p1.Position.Equals(p2.Position)) {
                setVal(p1, 3);
            } else {
                setVal(p1, 1);
                setVal(p2, 2);
            }
            foreach (var inc in background) {
                incr(inc.Key, inc.Value);
            }
            var p1DeathVal = this.orintationToBackgroundVal[false];
            var p2DeathVal = this.orintationToBackgroundVal[true];

            if (this[p1.Position].Background == p1DeathVal) {
                this.Winner = this.playerGlyph[false];
            }

            if (this[p2.Position].Background == p2DeathVal) {
                this.Winner = this.playerGlyph[true];
            }
            return this.cells;
        }

        private Dictionary<Pos, int> background;

        internal void Reserve(bool orientation, Pos p) {
            var backgroundVal = this.orintationToBackgroundVal[orientation];
            this.background[p.Clone()] = backgroundVal;
        }

        private Dictionary<bool, int> orintationToBackgroundVal = new Dictionary<bool, int>() {
            { true, 10 },
            { false, 20 }
        };

        public Cell this[Pos i] {
            get {
                return this.cells[i.Y][i.X];
            }
            set {
                this.cells[i.Y][i.X] = value;
            }
        }

        public void PushBricks(Pos startPosition, Pos endPosition, Pos opponentPos, bool pusher) {
            var pushable = this.orintationToBackgroundVal[pusher];
            var dx = endPosition.X - startPosition.X;
            var dy = endPosition.Y - startPosition.Y;
            if (!this.background.ContainsKey(endPosition) || this.background[endPosition] != pushable) {
                return;
            }

            var current = endPosition.Clone();
            int maxIdx = 0;
            if (dx != 0) {
                maxIdx = width;
            }
            if (dy != 0) {
                maxIdx = height;
            }

            bool allowed = false;
            for (int i = 0; i < maxIdx; i++){
                var next = new Pos() {
                    X = current.X + dx,
                    Y = current.Y + dy
                };
                next.Normalize(this.width, this.height);
                current = next;

                if (current.Equals(opponentPos)) {
                    this.Winner = this.playerGlyph[pusher];
                }

                if (!this.background.ContainsKey(next)) {
                    allowed = true;
                    break;
                }
            }
            if (!allowed) {
                return;
            }

            var b = this.background[endPosition];
            this.background.Remove(endPosition);
            this.background.Add(current, b);
        }

        internal bool IsReserved(Pos newPos, bool orientation) {
            var immovable = this.orintationToBackgroundVal[!orientation];
            return this.background.ContainsKey(newPos) && this.background[newPos] == immovable;
        }
    }
}