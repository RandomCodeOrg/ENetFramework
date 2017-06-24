using RandomCodeOrg.ENetFramework.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity;
using slf4net;

namespace RandomCodeOrg.Pluto.CDI {
    public class PersistenceProxyImplementation : ProxyImplementation {

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(PersistenceProxyImplementation));

        protected override object DoCallMethod(object instance, MethodInfo method, object[] args) {
            if (instance is DbContext) {
                return HandleEFCall(instance.As<DbContext>(), method, args);
            }
            return base.DoCallMethod(instance, method, args);
        }


        protected object HandleEFCall(DbContext context, MethodInfo method, object[] args) {
            logger.Trace("Scheduling call to entity framework persistence.");
            lock (context) {
                using (var dbContextTransaction = context.Database.BeginTransaction()) {
                    try {
                        logger.Trace("Created new databse transaction.");
                        object result = base.DoCallMethod(context, method, args);
                        if (context.ChangeTracker.HasChanges()) {
                            logger.Debug("Data changed. Saving db context to data store...");
                            context.SaveChanges();
                        } else {
                            logger.Debug("Call did not cause any data changes.");
                        }
                        dbContextTransaction.Commit();
                        return result;
                    } catch {
                        logger.Warn("An exception occured during the execution. Rolling back transaction...");
                        dbContextTransaction.Rollback();
                        logger.Warn("Rollback completed.");
                        throw;
                    }
                }
            }

        }

    }
}
