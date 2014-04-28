using RTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTS.Web.Controllers.ApiControllers {
    public class TableAPIController : ApiController {
        public Table GetTable(int id) {
            return TableManager.Tables.Where(i => i.ID == id).Single();
        }
    }
}
