using RandomCodeOrg.ENetFramework.Container;
using RandomCodeOrg.Pluto.CDI;
using slf4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements.Compiler {
    class StatementBuilder {

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(StatementBuilder));

        private readonly ISet<string> namespaces = new HashSet<string>();

        private readonly string fragment;

        private readonly int key;

        private int statementRow;

        private readonly string injectAttributeName;

        private readonly CDIContainer cdi;

        public int StatementRow {
            get {
                return statementRow;
            }
        }

        private int statementColumn;

        public int StatementColum {
            get {
                return statementColumn;
            }
        }

        internal int Key {
            get {
                return key;
            }
        }

        internal string Fragment {
            get {
                return fragment;
            }
        }



        public StatementBuilder(CDIContainer cdi, int key, string fragment) {
            this.fragment = fragment;
            this.key = key;
            this.cdi = cdi;
            this.injectAttributeName = string.Format("{0}.{1}", typeof(InjectAttribute).Namespace, "Inject");
        }


        public StatementBuilder Using(params string[] namespaces) {
            foreach (string namesp in namespaces)
                this.namespaces.Add(namesp);
            return this;
        }
        
        internal string GetAssemblyQulaifiedName(string targetNamespace) {
            return string.Format("{0}.GeneratedStatement{1}", targetNamespace, key);
        }

        public string Build(bool insertReturn, string targetNamespace) {
            StringBuilder sb = new StringBuilder();
            foreach (string nsp in namespaces) {
                sb.AppendFL("using {0};", nsp);
            }
            sb.AppendLine().AppendFL("namespace {0} {{", targetNamespace);
            sb.TabbedLine(1, "public class GeneratedStatement{0} : RandomCodeOrg.Pluto.Statements.CompiledStatement {{", key);
            sb.AppendLine();
            foreach(Type t in cdi.GetInjectableTypes()) {
                sb.TabbedLine(2, "[{0}]", injectAttributeName);
                sb.TabbedLine(2, "private {0} {1};", t.FullName, cdi.GetIdentifier(t));
            }
            sb.AppendLine();
            sb.TabbedLine(2, "protected override object DoEvaluate() {");
            FinalizeFragment(sb, 3, insertReturn);
            sb.TabbedLine(2, "}");
            sb.AppendLine();
            sb.TabbedLine(1, "}");
            sb.AppendLine("}");

            return sb.ToString();
        }


        protected void FinalizeFragment(StringBuilder sb, int indentLevel, bool insertReturn) {
            string fragment = this.fragment;
            if (!fragment.EndsWith(";"))
                fragment = fragment + ";";
            if (insertReturn) {
                if (!fragment.StartsWith("return ")) {
                    sb.Tabbed(indentLevel, "return ");
                    RecordStatementPosition(sb);
                    sb.Append(fragment);
                } else {
                    sb.Tabbed(indentLevel, "");
                    RecordStatementPosition(sb);
                    sb.Append(fragment);
                }
            } else {
                sb.Tabbed(indentLevel, "");
                RecordStatementPosition(sb);
                sb.Append(fragment);
            }
            if (!fragment.EndsWith(";"))
                sb.AppendLine(";");
            else
                sb.AppendLine();
            
            if (!insertReturn) {
                sb.TabbedLine(indentLevel, "throw new RandomCodeOrg.Pluto.Statements.NoResultException();");
            }
        }

        protected void RecordStatementPosition(StringBuilder sb) {
            string src = sb.ToString();
            string[] rows = src.Split('\n');
            int row = rows.Length;
            string lastRow = rows[row - 1];
            statementColumn = lastRow.Length;
            statementRow = row;
        }


    }
}
