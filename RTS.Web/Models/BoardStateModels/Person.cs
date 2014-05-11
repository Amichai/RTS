using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class Person {
        public Person(int x, int y) {
            this.Position = new Pos() {
                X = x,
                Y = y
            };
        }

        public void UpdatePosition(Direction dir) {
            switch (dir) {
                case Direction.up:
                    Position.Y--;
                    break;
                case Direction.down:
                    Position.Y++;
                    break;
                case Direction.right:
                    Position.X--;
                    break;
                case Direction.left:
                    Position.X++;
                    break;
            }
        }

        public Pos Position { get; set; }
    }

    public enum Direction { up, down, right, left, none };
}