using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class Brick : IVisual {
        public Brick(int x, int y, int width, int height) {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Color = "gray";
            this.Styles = new Styles();
            this.Styles["position"] = "absolute";
        }

        public string ToHtml {
            get {
                this.Styles.Width = Width;
                this.Styles.Height = Height;
                this.Styles["margin-left"] = this.X.ToString() + "px";
                this.Styles["margin-top"] = this.Y.ToString() + "px";
                this.Styles.Add("background-color", this.Color);
                var html = string.Format("<div style=\"{0}\"></div>", this.Styles.ToString());
                return html;
            }
        }

        public List<IVisual> Children {
            get;
            set;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }

        public void Input(string i, string connectionID) {
            throw new NotImplementedException();
        }

        public Styles Styles {
            get;
            set;
        }

        internal IVisual Reflect(int height) {
            var v = this.Clone() as Brick;
            v.Y = height - v.Y;
            return v;
        }

        public IVisual Clone() {
            List<IVisual> children = null;
            if (this.Children != null) {
                children = this.Children.Select(i => i.Clone()).ToList();
            }
            var toReturn = new Brick(this.X, this.Y, this.Width, this.Height){
                Color = this.Color,
                Styles = this.Styles.Clone(),
            };
            toReturn.Children = children;
            return toReturn;
        }
    }
}