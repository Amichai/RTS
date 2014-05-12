using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RTS.Web.Models.BoardStateModels {
    [DebuggerDisplay("{Background} - {Foreground}")]
    public class Cell {
        public int Background { get; set; }
        public string Foreground { get; set; }
    }
}
