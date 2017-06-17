using NHttp;
using RandomCodeOrg.ENetFramework.Statements;
using RandomCodeOrg.ENetFramework.UI;
using RandomCodeOrg.Pluto.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RandomCodeOrg.Pluto.UI {
    public class PlutoRenderContext : IRenderContext {

        private readonly IDictionary<string, ENetFramework.UI.IFrameworkRenderer> renderer = new Dictionary<string, ENetFramework.UI.IFrameworkRenderer>();

        private readonly IDictionary<string, CompiledToken> tokens = new Dictionary<string, CompiledToken>();

        private readonly PlutoStatementParser psp;

        private readonly IDictionary<string, object> performedResolutions = new Dictionary<string, object>();

        private readonly IDictionary<string, object> variables = new Dictionary<string, object>();
        private readonly ISet<string> requestedResolutions = new HashSet<string>();
        private readonly IList<IterationVariableRequest> requestedIterationVariables = new List<IterationVariableRequest>();

        private readonly IResolutionRegistry resolutionRegistry;

        private const string ID_ATTRIBUTE = "id";
        private const string NAME_ATTRIBUTE = "name";

        private readonly HttpRequest request;

        public PlutoRenderContext(PlutoStatementParser parser, HttpRequest request) {
            this.psp = parser;
            this.request = request;
            resolutionRegistry = new PlutoResolutionRegistry(psp, requestedResolutions, requestedIterationVariables);
            Register(new FragmentRenderer());
            Register(new LabelRenderer());
            Register(new TextRenderer());
            Register(new TextBoxRenderer());
            Register(new InputElementRenderer("SubmitButton", "submit"));
            Register(new FormRenderer());
            Register(new IterationRenderer());
        }


        public void Render(XmlDocument document) {
            Prepare(document, document.DocumentElement);
            PerformResolution();
            Refresh();
            Render(document, document.DocumentElement);
        }

        protected void Refresh() {
            foreach(object resolvedValue in performedResolutions.Values) {
                if(resolvedValue is IBinding) {
                    IBinding binding = resolvedValue.As<IBinding>();
                    string identifier = binding.Identifier;
                    if (request.Form.AllKeys.Contains(identifier)) {
                        binding.SetValue(request.Params[identifier]);
                    }else if (request.QueryString.AllKeys.Contains(identifier)) {
                        binding.SetValue(request.QueryString[identifier]);
                    }
                }
            }
        }
        

        protected void Prepare(XmlDocument document, XmlElement element) {
            if ("http://randomcodeorg.github.com/ENetFramework".Equals(element.NamespaceURI)) {
                if (renderer.ContainsKey(element.LocalName)) {
                    renderer[element.LocalName].Prepare(element, resolutionRegistry);
                }
            }
            foreach(XmlNode childNode in element.ChildNodes) {
                if (childNode is XmlElement)
                    Prepare(document, childNode.As<XmlElement>());
            }
        }

        
        


        public void Render(XmlDocument document, XmlElement element) {

            if ("http://randomcodeorg.github.com/ENetFramework".Equals(element.NamespaceURI)) {
                if (renderer.ContainsKey(element.LocalName)) {
                    renderer[element.LocalName].Render(this, document, element);
                }
            }

            List<XmlElement> children = new List<XmlElement>();
            foreach (XmlNode node in element.ChildNodes) {
                if (node is XmlElement)
                    children.Add((XmlElement)node);
            }

            foreach (XmlElement childElement in children) {

                /*if ("http://randomcodeorg.github.com/ENetFramework".Equals(childElement.NamespaceURI)) {
                    if (renderer.ContainsKey(childElement.LocalName)) {
                        renderer[childElement.LocalName].Render(this, document, childElement);
                    }
                }
                Render(document, childElement);*/
                Render(document, childElement);
            }
            List<XmlAttribute> toDelete = new List<XmlAttribute>();
            foreach(XmlAttribute attr in document.DocumentElement.Attributes) {
                if (attr.Name.StartsWith("xmlns:"))
                    toDelete.Add(attr);
            }
            foreach (XmlAttribute attr in toDelete)
                document.DocumentElement.RemoveAttribute(attr.Name);
        }


        protected bool Recurse(IRenderContext context, XmlDocument doc, XmlElement element) {
            bool result = false;
            XmlElement e;
            foreach (XmlNode child in element.ChildNodes) {
                if (child is XmlElement) {
                    e = (XmlElement)child;
                    if ("http://randomcodeorg.github.com/ENetFramework".Equals(e.NamespaceURI)) {
                        result = true;
                        context.Render(doc, e);
                    }
                }
            }
            if (result)
                return true;
            foreach (XmlNode child in element.ChildNodes) {
                if (child is XmlElement) {
                    result = result | Recurse(context, doc, (XmlElement)child);
                }
            }
            return result;
        }

        public void Register(ENetFramework.UI.IFrameworkRenderer renderer) {
            foreach (string supportedElement in renderer.SupportedElements) {
                this.renderer[supportedElement] = renderer;
            }
        }


        protected bool TryConvert<T>(string value, out T target) {
            Type targetType = typeof(T);
            if (targetType.IsAssignableFrom(typeof(bool))) {
                bool tmp;
                if (bool.TryParse(value, out tmp)) {
                    target = (T)(object)tmp;
                    return true;
                }
            }
            target = default(T);
            return false;
        }


        protected void PerformResolution() {
            object source;
            IDictionary<string, Type> locals = new Dictionary<string, Type>();

            foreach (IterationVariableRequest varRequest in requestedIterationVariables) {
                source = PerformResolution(locals, varRequest.SourceStatement);
                Type varType = GetIterationVariableType(source);
                locals[varRequest.VariableName] = varType;
            }
            foreach (string requested in requestedResolutions) {
                PerformResolution(locals, requested);
            }
        }

        protected Type GetIterationVariableType(object sourceValue) {
            if (sourceValue == null)
                return typeof(object);
            Type sourceType = sourceValue.GetType();
            if (sourceType.IsArray) {
                return sourceType.GetElementType();
            }
            if (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
                return sourceType.GetGenericArguments()[0];
            }
            throw new Exception("Not iterable."); // TODO: Throw specific exception.
        }

        protected object PerformResolution(IDictionary<string, Type> locals, string requested) {
            if (tokens.ContainsKey(requested))
                return performedResolutions[requested];
            
            CompiledToken compiled = psp.Compile(locals, requested);
            tokens[requested] = compiled;
            object result = compiled.Evaluate(variables);
            performedResolutions[requested] = result;
            return result;
        }

        public T Resolve<T>(string statement) {
            if (!requestedResolutions.Contains(statement) && psp.IsStatement(statement)) {
                throw new InvalidOperationException(string.Format("No resolution requested for '{0}'.", statement));
            }

            if (typeof(bool).IsAssignableFrom(typeof(T))) {
                bool result;
                if (bool.TryParse(statement, out result))
                    return (T)(object)result;
            }

            CompiledToken compiled = null;

            if (tokens.ContainsKey(statement)) {
                compiled = tokens[statement];
            } else {
                return (T) (object) statement;
            }
            
            if (compiled != null) {
                object result = compiled.Evaluate(variables);
                if(result is IBinding) {
                    result = result.As<IBinding>().GetValue();
                }else if (result != null && typeof(T).IsAssignableFrom(result.GetType())) {
                    return (T)result;
                } else if (typeof(T).IsAssignableFrom(typeof(string)) && result != null && !(result is string)) {
                    return (T)(object)string.Format("{0}", result);
                }
                return (T)result;
            }
            
            throw new Exception("Throw meaningful exception...");
        }

        public string BuildParmeterName(XmlElement element, string parameterStatement) {
            if (performedResolutions.ContainsKey(parameterStatement)) {
                object statementResult = performedResolutions[parameterStatement];
                if(statementResult is IBinding) {
                    IBinding binding = statementResult.As<IBinding>();
                    return binding.Identifier;
                }
            }

            if (element.HasAttribute(NAME_ATTRIBUTE)) {
                return element.GetAttribute(NAME_ATTRIBUTE);
            }
            return "";

            /*if (element.HasAttribute(ID_ATTRIBUTE)) {
                return element.GetAttribute(ID_ATTRIBUTE);
            }
            StringBuilder sb = new StringBuilder();
            XmlNode current = element;
            bool first = true;
            while (current != null) {
                if (first)
                    first = false;
                else
                    sb.Append(".");
                sb.Append(current.LocalName);
                current = current.ParentNode;
            }
            return string.Format("param{0}",sb.ToString().GetHashCode());*/
        }

        public void PushVariable(string name, object value) {
            variables[name] = value;
        }

        public void PopVariable(string name) {
            if (variables.ContainsKey(name))
                variables.Remove(name);
        }
    }
}
