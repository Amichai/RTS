using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    public class Styles {
        class cssVal {
            public cssVal(string val, string suffix = "") {
                this.Val = val;
                this.Suffix = suffix;
            }
            public override string ToString() {
                return this.Val + this.Suffix;
            }
            public string Val { get; set; }
            public string Suffix { get; set; }
        }

        public Styles() {
            this.styles = new Dictionary<string, cssVal>();
        }

        public string this[string i] {
            get {
                return this.styles[i].Val;
            }
            set {
                this.styles[i].Val = i;
            }
        }

        private Dictionary<string, cssVal> styles;

        public override string ToString() {
            string toReturn = "";
            foreach (var s in this.styles) {
                toReturn += string.Format("{0}:{1};", s.Key, s.Value);
            }
            return toReturn;
        }

        public void Add(string key, string val) {
            this.styles[key] = new cssVal(val);
        }

        public int Width {
            get {
                return int.Parse(this.styles["width"].Val);
            }
            set {
                if (!this.styles.ContainsKey("width")) {
                    this.styles["width"] = new cssVal("", "px");
                }
                this.styles["width"].Val = value.ToString();
                
            }
        }

        public int Height {
            get {
                return int.Parse(this.styles["height"].Val);
            }
            set {
                if (!this.styles.ContainsKey("height")) {
                    this.styles["height"] = new cssVal("", "px");
                }
                this.styles["height"].Val = value.ToString();
            }
        }
    }
}