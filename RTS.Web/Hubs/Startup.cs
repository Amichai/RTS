using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTS.Web.Hubs {
    public class Startup {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app) {
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule()); 
            app.MapSignalR();
        }

        public class ErrorHandlingPipelineModule : HubPipelineModule {
            protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext) {
                log.Info("=> Exception " + exceptionContext.Error.Message);
                if (exceptionContext.Error.InnerException != null) {
                    log.Info("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
                }
                base.OnIncomingError(exceptionContext, invokerContext);

            }
        }
    }
}