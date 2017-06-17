using RandomCodeOrg.Pluto.CDI;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Sessions {
    public class SessionManager : ISessionContextFactory {


        private readonly IDictionary<long, Session> sessions = new Dictionary<long, Session>();

        private readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        private readonly object lockObject = new object();

        private readonly IDictionary<Thread, Session> newSessions = new Dictionary<Thread, Session>();

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(SessionManager));

        public bool HasNewSession {
            get {
                lock (lockObject) {
                    return newSessions.ContainsKey(Thread.CurrentThread);
                }
            }
        }

        private int timeout = 10;


        public int SessionTimeout {
            get {
                return timeout;
            }
        }

        public SessionManager() {

        }

        public void Clean() {
            lock (lockObject) {
                DateTime now = DateTime.Now;
                ISet<Session> toDiscard = new HashSet<Session>();
                foreach (Session session in sessions.Values) {
                    if (Math.Abs(now.Subtract(session.LastUsage).TotalSeconds) >= timeout) {
                        toDiscard.Add(session);
                    }
                }
                foreach(Session session in toDiscard) {
                    logger.Info("Session '{0}' expired.", session.Id);
                    session.CDIContext.Complete();
                    sessions.Remove(session.Id);
                }
            }
        }

        public ManagedContext CreateSessionContext() {
            long id;
            byte[] buffer = new byte[64 / 8];
            Session session;
            lock (lockObject) {
                do {
                    random.GetBytes(buffer);
                    id = BitConverter.ToInt64(buffer, 0);
                } while (sessions.ContainsKey(id));
                session = new Session(id);
                sessions[id] = session;
                newSessions[Thread.CurrentThread] = session;
                logger.Trace("New session created for id '{0}'.", id);
            }
            return session.CDIContext;
        }

        public Session PopNewSession() {
            lock (lockObject) {
                Thread th = Thread.CurrentThread;
                if (!newSessions.ContainsKey(th))
                    return null;
                Session result = newSessions[th];
                newSessions.Remove(th);
                return result;
            }
        }

        public Session GetSession(long sessionId) {
            lock (lockObject) {
                if (!sessions.ContainsKey(sessionId))
                    return null;
                return sessions[sessionId].Use();
            }
        }


    }
}
