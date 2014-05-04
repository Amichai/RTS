using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Web.Models.BoardStateModels {
    public interface IVisual {
        string ToHtml { get; }
        List<IVisual> Children { get; set; }
        void Input(string i, string connectionID);
        Styles Styles { get; set; }
        IVisual Clone();
    }
}
