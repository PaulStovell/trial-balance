﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.312
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaulStovell.TrialBalance.Website.UploadBuildTask.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.trialbalance.net.au/Services/UploadBuildService.asmx")]
        public string PaulStovell_TrialBalance_Website_UploadBuildTask_UploadBuildServiceProxy_UploadBuildService {
            get {
                return ((string)(this["PaulStovell_TrialBalance_Website_UploadBuildTask_UploadBuildServiceProxy_UploadBu" +
                    "ildService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://download.trialbalance.net.au/Services/FilePublishingService.asmx")]
        public string PaulStovell_TrialBalance_Website_UploadBuildTask_FilePublishingServiceProxy_FilePublishingService {
            get {
                return ((string)(this["PaulStovell_TrialBalance_Website_UploadBuildTask_FilePublishingServiceProxy_FileP" +
                    "ublishingService"]));
            }
        }
    }
}
