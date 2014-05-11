using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class Pos {
        public Pos Clone() {
            return new Pos() {
                X = this.X,
                Y = this.Y
            };
        }
        public Pos() {
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