using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    /// <summary>
    /// Positions bricks in space
    /// </summary>
    public class Arena : IVisual {
        public int Width { get; set; }
        public int Height { get; set; }

        private int brickHeight = 30;
        private int brickWidth = 50;

        public Arena(int width, int height, List<IVisual> children) :this(width, height) {
            this.Children = children;
        }

        public Arena(int width, int height) {
            this.Width = width;
            this.Height = height;
            this.Styles = new Styles();
            this.Styles["position"] = "absolute";
            this.Color = "blue";
            this.Children = new List<IVisual>();
            this.Children.Add(new Brick(10, this.brickHeight, brickWidth, brickHeight));
            this.Children.Add(new Brick(40, this.Height - this.brickHeight * 2, brickWidth, brickHeight));
        }

        public bool Perspective { get; set; }

        public string Color { get; set; }

        private List<IVisual> Reflect() {
            return this.Children.Select(i => (i as Brick).Reflect(this.Height)).ToList();
        }

        private static Random rand = new Random();

        ///TODO: switch to html canvas!

        public string ToHtml {
            get {
                if (Perspective) {
                    this.Children = this.Reflect();
                    this.Children.Reverse();
                }
                this.Styles.Width = this.Width;
                this.Styles.Height = this.Height;
                this.Styles.Add("background-color", this.Color);
                var html = string.Format("<div style=\"{0}\">", this.Styles.ToString());
                foreach (var c in this.Children) {
                    html += c.ToHtml;
                }
                html += "</div>";
                return html;
            }
        }

        public List<IVisual> Children {
            get;
            set;
        }

        public void Input(string i, string connectionID) {
            switch (i) {
                case "55": ///left
                    (this.Children.Last() as Brick).X-= 2;
                    break;
                case "122": //right
                    (this.Children.Last() as Brick).X += 2;
                    break;
                case "54": // up
                    (this.Children.Last() as Brick).Y -= 2;

                    break;
                case "56": ///down
                    (this.Children.Last() as Brick).Y += 2;
                    break;
            }
        }

        public Styles Styles {
            get;
            set;
        }

        public IVisual Clone() {
            var c2 = this.Children.Select(i => i.Clone()).ToList();
            var toReturn = new Arena(this.Width, this.Height) {
                brickHeight = this.brickHeight,
                brickWidth = this.brickWidth,
                Color = this.Color,
                Perspective = this.Perspective,
                Styles = this.Styles.Clone()
            };
            toReturn.Children = c2;
            return toReturn;
        }
    }
}