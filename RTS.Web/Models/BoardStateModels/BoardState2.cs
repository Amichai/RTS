using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class BoardState2 {
        public BoardState2(int width, int height) {
            this.width = width;
            this.height = height;
            this.State = new List<List<int>>();
            for (int i = 0; i < width; i++) {
                this.State.Add(new List<int>());
            }
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    this.State[i].Add(0);
                }
            }
            this.p = new pos() {
                X = width / 2,
                Y = height / 2,
            };
        }
        public List<List<int>> State { get; set; }

        private int width, height;

        private pos p;
        

        internal void Input(string msg, string connectionID) {
            var newP = p.Clone();
            switch (msg) {
                case "119": ///w
                    newP.Y = (p.Y - 1);
                    break;
                case "115": // s
                    newP.Y = (p.Y + 1);
                    break;
                case "97": //a
                    newP.X = (p.X - 1);
                    break;
                case "100": // d
                    newP.X = (p.X + 1);
                    break;
            }
            newP.Normalize(width, height);
            this.State[p.Y][p.X] = 0;
            this.State[newP.Y][newP.X] = 1;

            p = newP.Clone();
        }
    }

    class pos {
        public pos Clone() {
            return new pos() {
                X = this.X,
                Y = this.Y
            };
        }
        public pos() {
            this.X = 0;
            this.Y = 0;
        }
        public int X { get; set; }
        public int Y { get; set; }

        internal void Normalize(int width, int height) {
            if (X < 0) {
                X += width;
            }
            if (Y < 0) {
                Y += height;
            }
            if (X >= width) {
                X -= width;
            }
            if (Y >= height) {
                Y -= height;
            }
        }
    }
}