﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace RandomCodeOrg.Pluto.Config {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor", IsNullable=false)]
    public partial class ApplicationDescriptor {
        
        private ApplicationInformation informationField;
        
        private Configuration[] configurationField;
        
        /// <remarks/>
        public ApplicationInformation Information {
            get {
                return this.informationField;
            }
            set {
                this.informationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Configuration")]
        public Configuration[] Configuration {
            get {
                return this.configurationField;
            }
            set {
                this.configurationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class ApplicationInformation {
        
        private string titleField;
        
        private string descriptionField;
        
        /// <remarks/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class Time {
        
        private float valueField;
        
        private TimeUnit unitField;
        
        public Time() {
            this.unitField = TimeUnit.Minutes;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public float Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(TimeUnit.Minutes)]
        public TimeUnit Unit {
            get {
                return this.unitField;
            }
            set {
                this.unitField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public enum TimeUnit {
        
        /// <remarks/>
        Hours,
        
        /// <remarks/>
        Minutes,
        
        /// <remarks/>
        Seconds,
        
        /// <remarks/>
        Milliseconds,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class SessionConfiguration {
        
        private Time timeoutField;
        
        private string cookieNameField;
        
        private int maxSessionsField;
        
        /// <remarks/>
        public Time Timeout {
            get {
                return this.timeoutField;
            }
            set {
                this.timeoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token")]
        public string CookieName {
            get {
                return this.cookieNameField;
            }
            set {
                this.cookieNameField = value;
            }
        }
        
        /// <remarks/>
        public int MaxSessions {
            get {
                return this.maxSessionsField;
            }
            set {
                this.maxSessionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class NavigationRule {
        
        private object targetField;
        
        private string[] itemsField;
        
        private ItemsChoiceType[] itemsElementNameField;
        
        private string nameField;
        
        /// <remarks/>
        public object Target {
            get {
                return this.targetField;
            }
            set {
                this.targetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FromOutcome", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("FromPath", typeof(string), DataType="anyURI")]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName {
            get {
                return this.itemsElementNameField;
            }
            set {
                this.itemsElementNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor", IncludeInSchema=false)]
    public enum ItemsChoiceType {
        
        /// <remarks/>
        FromOutcome,
        
        /// <remarks/>
        FromPath,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class NavigationConfiguration {
        
        private string[] welcomeField;
        
        private NavigationRule[] ruleField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Alternative", IsNullable=false)]
        public string[] Welcome {
            get {
                return this.welcomeField;
            }
            set {
                this.welcomeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Rule")]
        public NavigationRule[] Rule {
            get {
                return this.ruleField;
            }
            set {
                this.ruleField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TypeImport))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class TypeReference {
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="token")]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class TypeImport : TypeReference {
        
        private bool includePrivateField;
        
        private bool includeProtectedField;
        
        private bool includePublicField;
        
        public TypeImport() {
            this.includePrivateField = false;
            this.includeProtectedField = false;
            this.includePublicField = true;
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IncludePrivate {
            get {
                return this.includePrivateField;
            }
            set {
                this.includePrivateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool IncludeProtected {
            get {
                return this.includeProtectedField;
            }
            set {
                this.includeProtectedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool IncludePublic {
            get {
                return this.includePublicField;
            }
            set {
                this.includePublicField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class AssemblyReference {
        
        private string nameField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class CompilationConfiguration {
        
        private AssemblyReference[] referencesField;
        
        private string[] namespacesField;
        
        private TypeImport[] extensionsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Assembly", IsNullable=false)]
        public AssemblyReference[] References {
            get {
                return this.referencesField;
            }
            set {
                this.referencesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Import", DataType="token", IsNullable=false)]
        public string[] Namespaces {
            get {
                return this.namespacesField;
            }
            set {
                this.namespacesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable=false)]
        public TypeImport[] Extensions {
            get {
                return this.extensionsField;
            }
            set {
                this.extensionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://randomcodeorg.github.com/ENetFramework/ApplicationDescriptor")]
    public partial class Configuration {
        
        private CompilationConfiguration compilationField;
        
        private NavigationConfiguration navigationField;
        
        private SessionConfiguration sessionField;
        
        private string nameField;
        
        public Configuration() {
            this.nameField = "Default";
        }
        
        /// <remarks/>
        public CompilationConfiguration Compilation {
            get {
                return this.compilationField;
            }
            set {
                this.compilationField = value;
            }
        }
        
        /// <remarks/>
        public NavigationConfiguration Navigation {
            get {
                return this.navigationField;
            }
            set {
                this.navigationField = value;
            }
        }
        
        /// <remarks/>
        public SessionConfiguration Session {
            get {
                return this.sessionField;
            }
            set {
                this.sessionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("Default")]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
}
