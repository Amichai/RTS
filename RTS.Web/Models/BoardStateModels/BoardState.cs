using RTS.Web.Models.BoardStateModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models {
    public class BoardState : IVisual {
        public BoardState() {
            this.vals = new List<string>();
        }
        private List<string> vals;

        public List<IVisual> Children { get; set; }

        public Styles Styles { get; set; }

        public string ToHtml {
            get {
                return string.Join("<br/>", vals);
            }
        }

        public void Input(string i, string connectionID) {
            this.vals.Add(i + " " + connectionID);
        }

        public IVisual Clone() {
            throw new NotImplementedException();
        }
    }
}