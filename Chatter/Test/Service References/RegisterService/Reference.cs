﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test.RegisterService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BaseRole", Namespace="http://schemas.datacontract.org/2004/07/Chatter.Contract.DataContract")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Test.RegisterService.Member))]
    public partial class BaseRole : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Member", Namespace="http://schemas.datacontract.org/2004/07/Chatter.Contract.DataContract")]
    [System.SerializableAttribute()]
    public partial class Member : Test.RegisterService.BaseRole {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime birthdayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string infomationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nickNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string passwordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string sexField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Test.RegisterService.MemberStatus statusField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime birthday {
            get {
                return this.birthdayField;
            }
            set {
                if ((this.birthdayField.Equals(value) != true)) {
                    this.birthdayField = value;
                    this.RaisePropertyChanged("birthday");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string id {
            get {
                return this.idField;
            }
            set {
                if ((object.ReferenceEquals(this.idField, value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string infomation {
            get {
                return this.infomationField;
            }
            set {
                if ((object.ReferenceEquals(this.infomationField, value) != true)) {
                    this.infomationField = value;
                    this.RaisePropertyChanged("infomation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nickName {
            get {
                return this.nickNameField;
            }
            set {
                if ((object.ReferenceEquals(this.nickNameField, value) != true)) {
                    this.nickNameField = value;
                    this.RaisePropertyChanged("nickName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                if ((object.ReferenceEquals(this.passwordField, value) != true)) {
                    this.passwordField = value;
                    this.RaisePropertyChanged("password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string sex {
            get {
                return this.sexField;
            }
            set {
                if ((object.ReferenceEquals(this.sexField, value) != true)) {
                    this.sexField = value;
                    this.RaisePropertyChanged("sex");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Test.RegisterService.MemberStatus status {
            get {
                return this.statusField;
            }
            set {
                if ((this.statusField.Equals(value) != true)) {
                    this.statusField = value;
                    this.RaisePropertyChanged("status");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MemberStatus", Namespace="http://schemas.datacontract.org/2004/07/Chatter.Contract.DataContract")]
    public enum MemberStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Online = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Offline = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Levave = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Busy = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="RegisterService.IRegister")]
    public interface IRegister {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IRegister/Register", ReplyAction="http://tempuri.org/IRegister/RegisterResponse")]
        Test.RegisterService.Member Register(Test.RegisterService.Member member);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IRegister/Register", ReplyAction="http://tempuri.org/IRegister/RegisterResponse")]
        System.IAsyncResult BeginRegister(Test.RegisterService.Member member, System.AsyncCallback callback, object asyncState);
        
        Test.RegisterService.Member EndRegister(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IRegisterChannel : Test.RegisterService.IRegister, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public RegisterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Test.RegisterService.Member Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Test.RegisterService.Member)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RegisterClient : System.ServiceModel.ClientBase<Test.RegisterService.IRegister>, Test.RegisterService.IRegister {
        
        private BeginOperationDelegate onBeginRegisterDelegate;
        
        private EndOperationDelegate onEndRegisterDelegate;
        
        private System.Threading.SendOrPostCallback onRegisterCompletedDelegate;
        
        public RegisterClient() {
        }
        
        public RegisterClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RegisterClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RegisterClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<RegisterCompletedEventArgs> RegisterCompleted;
        
        public Test.RegisterService.Member Register(Test.RegisterService.Member member) {
            return base.Channel.Register(member);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginRegister(Test.RegisterService.Member member, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginRegister(member, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public Test.RegisterService.Member EndRegister(System.IAsyncResult result) {
            return base.Channel.EndRegister(result);
        }
        
        private System.IAsyncResult OnBeginRegister(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Test.RegisterService.Member member = ((Test.RegisterService.Member)(inValues[0]));
            return this.BeginRegister(member, callback, asyncState);
        }
        
        private object[] OnEndRegister(System.IAsyncResult result) {
            Test.RegisterService.Member retVal = this.EndRegister(result);
            return new object[] {
                    retVal};
        }
        
        private void OnRegisterCompleted(object state) {
            if ((this.RegisterCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.RegisterCompleted(this, new RegisterCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void RegisterAsync(Test.RegisterService.Member member) {
            this.RegisterAsync(member, null);
        }
        
        public void RegisterAsync(Test.RegisterService.Member member, object userState) {
            if ((this.onBeginRegisterDelegate == null)) {
                this.onBeginRegisterDelegate = new BeginOperationDelegate(this.OnBeginRegister);
            }
            if ((this.onEndRegisterDelegate == null)) {
                this.onEndRegisterDelegate = new EndOperationDelegate(this.OnEndRegister);
            }
            if ((this.onRegisterCompletedDelegate == null)) {
                this.onRegisterCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnRegisterCompleted);
            }
            base.InvokeAsync(this.onBeginRegisterDelegate, new object[] {
                        member}, this.onEndRegisterDelegate, this.onRegisterCompletedDelegate, userState);
        }
    }
}
