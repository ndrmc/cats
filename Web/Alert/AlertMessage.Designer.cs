﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cats.Alert {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AlertMessage {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AlertMessage() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Cats.Alert.AlertMessage", typeof(AlertMessage).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The document has been Deleted.&quot;.
        /// </summary>
        public static string Workflow_DefaultDelete {
            get {
                return ResourceManager.GetString("Workflow_DefaultDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The document has been Edited.&quot;.
        /// </summary>
        public static string Workflow_DefaultEdit {
            get {
                return ResourceManager.GetString("Workflow_DefaultEdit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The document has been Printed.&quot;.
        /// </summary>
        public static string Workflow_DefaultPrint {
            get {
                return ResourceManager.GetString("Workflow_DefaultPrint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Loss Entry has been made for this Document.&quot;.
        /// </summary>
        public static string Workflow_LossEntry {
            get {
                return ResourceManager.GetString("Workflow_LossEntry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;A Letter has been Printed for this Document.&quot;.
        /// </summary>
        public static string Workflow_PrintLetter {
            get {
                return ResourceManager.GetString("Workflow_PrintLetter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{1} has been Printed.&quot;.
        /// </summary>
        public static string Workflow_PrintwithParam1 {
            get {
                return ResourceManager.GetString("Workflow_PrintwithParam1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;This Document Has been Rejected.&quot;.
        /// </summary>
        public static string Workflow_RejectedDocument {
            get {
                return ResourceManager.GetString("Workflow_RejectedDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{1} Has been Rejected.&quot;.
        /// </summary>
        public static string Workflow_RejectedDocumentwithParam1 {
            get {
                return ResourceManager.GetString("Workflow_RejectedDocumentwithParam1", resourceCulture);
            }
        }
    }
}
