using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.ENetFramework {
    public class ApplicationStartupConfig {

        private readonly IDictionary<string, List<string>> argumentDictionary = new Dictionary<string, List<string>>();
        private readonly List<string> argumentList = new List<string>();

        private const string FLAG_INDICATOR = "-";

        public IReadOnlyList<string> RawArguments {
            get {
                return argumentList;
            }
        }
        
        public ApplicationStartupConfig(string[] arguments) {
            string argumentName = null;

            foreach(string arg in arguments) {
                if (arg.StartsWith(FLAG_INDICATOR)) {
                    if(argumentName != null) {
                        AppendOption(argumentName);
                    }
                    argumentName = arg;
                } else if(argumentName != null) {
                    AppendOption(argumentName, arg);
                    argumentName = null;
                } else {
                    argumentList.Add(arg);
                }
            }
            if (argumentName != null)
                AppendOption(argumentName);
        }

        private void AppendOption(string key) {
            string rawKey = key.Substring(1);
            if (!argumentDictionary.ContainsKey(rawKey)) {
                argumentDictionary[rawKey] = new List<string>();
            }
        }

        private void AppendOption(string key, string value) {
            string rawKey = key.Substring(1);
            if (!argumentDictionary.ContainsKey(rawKey)) {
                argumentDictionary[rawKey] = new List<string>();
            }
            argumentDictionary[rawKey].Add(value);
        }

        public bool HasFlag(string key) {
            return argumentDictionary.ContainsKey(key);
        }

        public bool HasOption(string key) {
            return argumentDictionary.ContainsKey(key) && argumentDictionary[key].Count > 0;
        }
       
        public string GetOption(string key) {
            if (!argumentDictionary.ContainsKey(key)) {
                return null;
            }
            return argumentDictionary[key].FirstOrDefault();
        }

        public IReadOnlyList<string> GetOptions(string key) {
            if (!argumentDictionary.ContainsKey(key))
                return null;
            return argumentDictionary[key];
        }

        public bool TryOption(string key, out string target) {
            target = null;
            IReadOnlyList<string> options;
            if (!TryOptions(key, out options)) return false;
            if (options.Count <= 0)
                return false;
            target = options[0];
            return true;
        }

        public bool TryOptions(string key, out IReadOnlyList<string> target) {
            target = null;
            if (!argumentDictionary.ContainsKey(key))
                return false;
            target = argumentDictionary[key];
            return true;
        }

    }
}
