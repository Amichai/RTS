using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class Cell : IVisual {
        public Cell(string color) {
            this.Styles = new Styles();
            this.Children = new List<IVisual>();
            this.Styles.Width = 100;
            this.Styles.Height = 100;
            this.Styles.Add("background-color", color);
        }

        public string ToHtml {
            get {
                string children = "";
                children = string.Concat(this.Children.Select(i => i.ToHtml));
                return string.Format("<div style=\"{0}\">{1}</div>", this.Styles.ToString(), children);
            }
        }

        public void Input(string i, string connectionID) {
            this.Styles.Width++;
        }

        public IVisual Clone() {
            throw new NotImplementedException();
        }

        public Styles Styles { get; set; }

        public List<IVisual> Children {
            get;
            set;
        }
    }
}