using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Health {
    public class ValidationReport {

        private readonly IList<ValidationEntry> insertionOrder = new List<ValidationEntry>();
        private readonly IDictionary<ValidationEntryType, ISet<ValidationEntry>> entries = new Dictionary<ValidationEntryType, ISet<ValidationEntry>>();

        private readonly IReadOnlyCollection<ValidationEntry> entriesReadOnly;

        public IReadOnlyCollection<ValidationEntry> Entries {
            get {
                return entriesReadOnly;
            }
        }

        public bool HasErrors {
            get {
                return entries.ContainsKey(ValidationEntryType.Error);
            }
        }

        public bool HasWarnings {
            get {
                return entries.ContainsKey(ValidationEntryType.Warning);
            }
        }

        public ValidationReport() {
            entriesReadOnly = new ReadOnlyCollection<ValidationEntry>(insertionOrder);
        }


        public void Register(ValidationEntry entry) {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));
            if (!entries.ContainsKey(entry.EntryType)) {
                entries[entry.EntryType] = new HashSet<ValidationEntry>();
            }
            ISet<ValidationEntry> entrySet = entries[entry.EntryType];
            if (entrySet.Contains(entry))
                return;
            entrySet.Add(entry);
            insertionOrder.Add(entry);
        }


        public string GetPrintableString() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine().AppendLine("===============================================");
            sb.AppendLine("\t\tValidation report");
            sb.AppendLine("===============================================");
            sb.AppendLine();
            if (Entries.Count == 0)
                sb.AppendLine("There are no validation entries. Nothing to see here...");
            foreach (ValidationEntry entry in Entries) {
                sb.AppendFL("- {0} for {1}: {2}", entry.EntryType, entry.Source, entry.Message);
            }
            sb.AppendLine();
            return sb.ToString();
        }


    }
}
