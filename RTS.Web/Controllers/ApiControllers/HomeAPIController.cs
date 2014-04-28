using RTS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTS.Web.Controllers.ApiControllers {
    public class HomeAPIController : ApiController {
        public List<ConnectedUser> GetConnectedClients() {
            return Hubs.UserManager.ConnectedUsers();
        }

        public List<Table> GetWaitingTables() {
            return Hubs.UserManager.WaitingTables();
        }

        public void Post([FromBody]string value) {
        }

        // PUT api/homeapi/put?id=5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/homeapi/delete?id=5
        public void Delete(int id) {
        }
    }
}
