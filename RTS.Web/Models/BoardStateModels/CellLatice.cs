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
                        Foreground = 0
                    });
                }
            }
            return emptyBoard;
        }

        private void setVal(Person p, int i) {
            p.Position.Normalize(this.width, this.height);
            cells[p.Position.Y][p.Position.X].Foreground = i;
        }

        private void incr(Pos p, int i) {
            cells[p.Y][p.X].Background = i;
        }

        public List<List<Cell>> GetCells(Person p1, Person p2) {
            cells = this.getEmptyBoard();
            setVal(p1, 1);
            setVal(p2, 2);
            foreach (var inc in background) {
                incr(inc.Key, inc.Value);
            }
            return this.cells;
        }

        private Dictionary<Pos, int> background;

        internal void Reserve(bool orientation, Pos p) {
            if (orientation) {
                this.background[p.Clone()] = 10;
            } else {
                this.background[p.Clone()] = 20;
            }
        }
    }
}