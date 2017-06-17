using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements.Compiler {
    public static class ExtensionMethods {

        public static StringBuilder TabbedLine(this StringBuilder instance, int tabCount, string format, params object[] args) {
            for (int i = 0; i < tabCount; i++)
                instance.Append("\t");
            instance.AppendFL(format, args);
            return instance;
        }

        public static StringBuilder TabbedLine(this StringBuilder instance, int tabCount, string value) {
            for (int i = 0; i < tabCount; i++)
                instance.Append("\t");
            instance.AppendLine(value);
            return instance;
        }

        public static StringBuilder Tabbed(this StringBuilder instance, int tabCount, string value) {
            for (int i = 0; i < tabCount; i++)
                instance.Append("\t");
            instance.Append(value);
            return instance;
        }

        public static StringBuilder Tabbed(this StringBuilder instance, int tabCount, string value, params object[] args) {
            for (int i = 0; i < tabCount; i++)
                instance.Append("\t");
            instance.AppendFormat(value, args);
            return instance;
        }

    }
}
