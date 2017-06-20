using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Health {
    public class ValidationEntry {

        private readonly ValidationEntryType entryType;

        public ValidationEntryType EntryType {
            get {
                return entryType;
            }
        }


        private readonly string source;
        private readonly string message;

        public string Source => source;
        public string Message => message;
        
        public ValidationEntry(ValidationEntryType entryType, string source, string messageFormat, params object[] args) {
            this.entryType = entryType;
            this.source = source;
            message = string.Format(messageFormat, args);
        }

        public ValidationEntry(ValidationEntryType entryType, Type source, string messageFormat, params object[] args) : this(entryType, source.FullName, messageFormat, args) {

        }

        public ValidationEntry(ValidationEntryType entryType, MemberInfo member, string messageFormat, params object[] args) : this(entryType, string.Format("{0}.{1}", member.DeclaringType.FullName, member.Name), messageFormat, args) {

        }

    }
}
