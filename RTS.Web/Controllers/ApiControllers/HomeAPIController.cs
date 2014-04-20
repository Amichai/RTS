using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RTS.Web.Controllers.ApiControllers {
    public class HomeAPIController : ApiController {
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id) {
            return "value";
        }

        public List<string> GetConnectedClients() {
            return Hubs.UserHandler.ConnectedUsers();
        }

        public List<string> GetWaitingTables() {
            return Hubs.UserHandler.WaitingTables();
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
