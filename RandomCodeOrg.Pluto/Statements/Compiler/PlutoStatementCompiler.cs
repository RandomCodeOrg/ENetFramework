using Microsoft.CSharp;
using RandomCodeOrg.Pluto.CDI;
using slf4net;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements.Compiler {
    public class PlutoStatementCompiler {

        private readonly IDictionary<string, StatementBuilder> statementBuilders = new Dictionary<string, StatementBuilder>();

        private readonly ISet<string> referencedAssemblies = new HashSet<string>();

        private readonly ISet<string> namespaces = new HashSet<string>();

        private readonly string targetNamespace;

        private int key = 0;

        private readonly ILogger logger = LoggerFactory.GetLogger(typeof(PlutoStatementCompiler));

        private readonly IDictionary<string, Type> compilationResult = new Dictionary<string, Type>();

        private readonly CDIContainer cdi;

        public CDIContainer CDI {
            get {
                return cdi;
            }
        }

        public PlutoStatementCompiler(CDIContainer cdi, string targetNamespace) {
            this.targetNamespace = targetNamespace;
            this.cdi = cdi;
            Reference(Assembly.GetExecutingAssembly());
            Using("System", "System.Text", "System.IO", "System.Collections", "System.Collections.Generic");
        }


        public PlutoStatementCompiler Process(string statement) {
            if (statementBuilders.ContainsKey(statement))
                return this;
            statementBuilders[statement] = new StatementBuilder(cdi, key++, statement);
            return this;
        }

        
        

        public PlutoStatementCompiler Reference(params string[] assemblies) {
            foreach (string assembly in assemblies)
                referencedAssemblies.Add(assembly);
            return this;
        }


        public void Reference(params Assembly[] assemblies) {
            string location;
            foreach (Assembly a in assemblies) {
                location = GetLocation(a);
                if (referencedAssemblies.Contains(location))
                    continue;
                referencedAssemblies.Add(location);
                Reference(a.GetReferencedAssemblies().Select(dep => Assembly.Load(dep)).ToArray());
            }
        }
        
        private string GetLocation(Assembly a) {
            //if (0 == 0)
                return a.Location;
            /*string codeBase = a.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetFileName(path);*/
        }

        public PlutoStatementCompiler Using(params string[] namespaces) {
            foreach (string namesp in namespaces)
                this.namespaces.Add(namesp);
            return this;
        }


        public Type GetCompiledType(string fragment) {
            return compilationResult[fragment];
        }
        



        private void Compile(string[] namespacesToImport, IDictionary<string, Type> variables, CSharpCodeProvider csc, CompilerParameters parameters, StatementBuilder builder) {
            CompilerResults compilerResult;
            builder.Using(namespacesToImport);
            string source = builder.Build(true, variables, targetNamespace);
            compilerResult = csc.CompileAssemblyFromSource(parameters, source);
            if (!compilerResult.Errors.HasErrors) {
                var assem = compilerResult.CompiledAssembly;
                compilationResult[builder.Fragment] = RetrieveType(compilerResult, builder);
                return;
            }
            source = builder.Build(false, variables, targetNamespace);
            compilerResult = csc.CompileAssemblyFromSource(parameters, source);
            if (!compilerResult.Errors.HasErrors) {
                compilationResult[builder.Fragment] = RetrieveType(compilerResult, builder);
                return;
            }

            throw new StatementCompilationException(builder.Fragment, builder.StatementRow, builder.StatementColum, compilerResult.Errors);
        }

        private Type RetrieveType(CompilerResults compilerResult, StatementBuilder builder) {
            var assembly = compilerResult.CompiledAssembly;
            return compilerResult.CompiledAssembly.GetType(builder.GetAssemblyQulaifiedName(targetNamespace));
        }
        

        public Type Compile(IDictionary<string, Type> variables, string fragment) {
            if (compilationResult.ContainsKey(fragment))
                return compilationResult[fragment];
            Process(fragment);
            StatementBuilder sb = statementBuilders[fragment];

            var codeProvider = CreateCodeProvider();
            var parameters = CreateParameters();
            
            Compile(namespaces.ToArray(), variables, codeProvider, parameters, sb);

            return compilationResult[fragment];
        }

        public void Compile(IDictionary<string, Type> variables) {
            var csc = CreateCodeProvider();
            var parameters = CreateParameters();
           
            string[] namespacesToImport = namespaces.ToArray();

            var result = new Dictionary<string, Type>();

            foreach(StatementBuilder builder in statementBuilders.Values) {
                Compile(namespacesToImport, variables, csc, parameters, builder);
            }
            
        }
        

        protected CSharpCodeProvider CreateCodeProvider() {
            return new CSharpCodeProvider();
        }

        protected CompilerParameters CreateParameters() {
            var parameters = new CompilerParameters(referencedAssemblies.ToArray());
            parameters.GenerateInMemory = true;
            return parameters;
        }

    }
}
