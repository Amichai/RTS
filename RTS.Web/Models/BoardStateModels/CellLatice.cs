using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class CellLatice : IVisual {
        public CellLatice(int width, int height) {
            this.CellHeight = height;
            this.CellWidth = width;

            this.Children = new List<IVisual>();
            for (int i = 0; i < width * height; i++) {
                var color = (i + 1) % 2 == 0 ? "red" : "gray";
                var cell = new Cell(color);
                cell.Styles.Add("display", "table-cell");
                this.Children.Add(cell);
            }
        }
        public Styles Styles { get; set; }


        private string getChildrenString() {
            string toReturn = "";
            int counter = 0;
            for (int i = 0; i < this.CellHeight; i++) {
                string row = @"<div style=""display:table-row"">";
                for (int j = 0; j < this.CellWidth; j++) {
                    row += this.Children[counter++].ToHtml;
                }
                row += "</div>";
                toReturn += row;
            }

            return toReturn;
        }

        public string ToHtml {
            get {
                var children = string.Concat(this.Children.Select(i => i.ToHtml));
                return string.Format(@"<div style=""width: 100%; display:table;"">
{0}
                </div>", getChildrenString());
            }
        }

        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        public List<IVisual> Children {
            get;
            set;
        }

        public void Input(string i, string connectionID) {
            int idx;
            if (!int.TryParse(i, out idx)) {
                return;
            }
            if (idx < 0 || idx >= this.Children.Count()) {
                return;
            }
            this.Children[idx].Styles["background-color"] = "blue";
        }
    }
}