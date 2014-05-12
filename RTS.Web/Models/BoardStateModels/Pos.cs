using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    [DebuggerDisplay("({X}, {Y})")]
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

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            var p = obj as Pos;
            return this.Equals(p);
        }

        public bool Equals(Pos obj) {
            if (obj == null) {
                return false;
            }
            return this.X == obj.X && this.Y == obj.Y;
        }

        public override int GetHashCode() {
            return this.X ^ this.Y;
        }
    }
}