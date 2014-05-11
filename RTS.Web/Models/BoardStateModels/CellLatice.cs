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
        }

        private List<List<int>> cells;

        private List<List<int>> getEmptyBoard() {
            List<List<int>> emptyBoard = new List<List<int>>();
            for (int i = 0; i < width; i++) {
                emptyBoard.Add(new List<int>());
            }
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    emptyBoard[i].Add(0);
                }
            }
            return emptyBoard;
        }

        private void setVal(Person p, int i) {
            p.Position.Normalize(this.width, this.height);
            cells[p.Position.Y][p.Position.X] = i;
        }

        public List<List<int>> GetCells(Person p1, Person p2) {
            cells = this.getEmptyBoard();
            setVal(p1, 1);
            setVal(p2, 2);
            return this.cells;
        }
    }
}