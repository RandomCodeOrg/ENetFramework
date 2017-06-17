using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RandomCodeOrg.ENetFramework.Container;

namespace RandomCodeOrg.Pluto.CDI {
    public class ContextStateManager {


        private readonly object lockObject = new object();

        private readonly IDictionary<Thread, ManagedContext> activeRequestContexts = new Dictionary<Thread, ManagedContext>();

        private readonly IDictionary<Thread, ManagedContext> activeSessionContexts = new Dictionary<Thread, ManagedContext>();

        private readonly ISessionContextFactory sessionContextFactory;

        public ContextStateManager(ISessionContextFactory sessionContextFactory) {
            this.sessionContextFactory = sessionContextFactory;
        }


        public ManagedContext BeginSession() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                ManagedContext context = new ManagedContext();
                activeSessionContexts[current] = context;
                return context;
            }
        }

        public ManagedContext SuspendSession() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                ManagedContext context = activeSessionContexts[current];
                activeSessionContexts.Remove(current);
                return context;
            }
        }

        public void ContinueSession(ManagedContext context) {
            lock(lockObject) {
                Thread current = Thread.CurrentThread;
                activeSessionContexts[current] = context;
            }
        }

        public void CompleteSession() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                ManagedContext context = activeSessionContexts[current];
                context.Complete();
                activeSessionContexts.Remove(current);
            }
        }

        public void BeginRequest() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                activeRequestContexts[current] = new ManagedContext();        
            }
        }

        public void CompleteRequest() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                ManagedContext reqContext = activeRequestContexts[current];
                reqContext.Complete();
                activeRequestContexts.Remove(current);
            }
        }
        
        public ManagedContext GetRequestContext() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                return activeRequestContexts[current];
            }
        }

        public ManagedContext GetSessionContext() {
            lock (lockObject) {
                Thread current = Thread.CurrentThread;
                if (!activeSessionContexts.ContainsKey(current)) {
                    if(sessionContextFactory != null) {
                        activeSessionContexts[current] = sessionContextFactory.CreateSessionContext();
                    } else {
                        return null;
                    }
                }
                return activeSessionContexts[current];
            }
        }

    }
}
