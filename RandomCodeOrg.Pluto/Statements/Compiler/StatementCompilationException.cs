using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCodeOrg.Pluto.Statements.Compiler {
    public class StatementCompilationException : Exception {


        private readonly IReadOnlyCollection<CompilerError> compilerErrors;

        public IReadOnlyCollection<CompilerError> CompilerErrors {
            get {
                return compilerErrors;
            }
        }
        

        public StatementCompilationException(string source, int row, int column, CompilerErrorCollection errors) : base(BuildMessage(source, row, column, errors)) {
            IList<CompilerError> compErrors = new List<CompilerError>();
            foreach(CompilerError e in errors) {
                compErrors.Add(e);
            }
            this.compilerErrors = new ReadOnlyCollection<CompilerError>(compErrors);
        }



        private static string BuildMessage(string source, int row, int column, CompilerErrorCollection errors) {
            CompilerError error = null;
            string[] sourceRows = source.Split('\n');
            int maxRows = sourceRows.Length;
            int maxColumn = sourceRows[sourceRows.Length - 1].Length;

            foreach(CompilerError current in errors) {
                if (current.IsWarning)
                    continue;
                if (current.Line < row || current.Line >= row + maxRows)
                    continue;
                error = current;
                break;
            }

            if(error != null) {
                string message = error.ErrorText;
                int translatedColumn = error.Column - column;
                int translatedRow = error.Line - row + 1;
                return string.Format("Could not compile the given statement.\nCompiler Error {0} at Line {1} Column {2}: {3}\nSource: {4}", error.ErrorNumber, translatedRow, translatedColumn, error.ErrorText, source);
            }


            return string.Format("Can not compile the given statement ('{0}').", source);
        }


    }
}
