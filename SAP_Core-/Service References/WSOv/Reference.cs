﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAP_Core.WSOv {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="clsBeRespuesta", Namespace="http://localhost:6340/WebServiceGrupoPana.asmx")]
    [System.SerializableAttribute()]
    public partial class clsBeRespuesta : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private int EstadoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MensajeField;
        
        private int DocEntryField;
        
        private int DocNumField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Estado {
            get {
                return this.EstadoField;
            }
            set {
                if ((this.EstadoField.Equals(value) != true)) {
                    this.EstadoField = value;
                    this.RaisePropertyChanged("Estado");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string Mensaje {
            get {
                return this.MensajeField;
            }
            set {
                if ((object.ReferenceEquals(this.MensajeField, value) != true)) {
                    this.MensajeField = value;
                    this.RaisePropertyChanged("Mensaje");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public int DocEntry {
            get {
                return this.DocEntryField;
            }
            set {
                if ((this.DocEntryField.Equals(value) != true)) {
                    this.DocEntryField = value;
                    this.RaisePropertyChanged("DocEntry");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=3)]
        public int DocNum {
            get {
                return this.DocNumField;
            }
            set {
                if ((this.DocNumField.Equals(value) != true)) {
                    this.DocNumField = value;
                    this.RaisePropertyChanged("DocNum");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Name="WebService - VISTSoap", Namespace="http://localhost:6340/WebServiceGrupoPana.asmx", ConfigurationName="WSOv.WebServiceVISTSoap")]
    public interface WebServiceVISTSoap {
        
        // CODEGEN: Generating message contract since element name JsonOrders from namespace http://localhost:6340/WebServiceGrupoPana.asmx is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:6340/WebServiceGrupoPana.asmx/RegistrarOrdenDeVenta", ReplyAction="*")]
        SAP_Core.WSOv.RegistrarOrdenDeVentaResponse RegistrarOrdenDeVenta(SAP_Core.WSOv.RegistrarOrdenDeVentaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:6340/WebServiceGrupoPana.asmx/RegistrarOrdenDeVenta", ReplyAction="*")]
        System.Threading.Tasks.Task<SAP_Core.WSOv.RegistrarOrdenDeVentaResponse> RegistrarOrdenDeVentaAsync(SAP_Core.WSOv.RegistrarOrdenDeVentaRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistrarOrdenDeVentaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistrarOrdenDeVenta", Namespace="http://localhost:6340/WebServiceGrupoPana.asmx", Order=0)]
        public SAP_Core.WSOv.RegistrarOrdenDeVentaRequestBody Body;
        
        public RegistrarOrdenDeVentaRequest() {
        }
        
        public RegistrarOrdenDeVentaRequest(SAP_Core.WSOv.RegistrarOrdenDeVentaRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://localhost:6340/WebServiceGrupoPana.asmx")]
    public partial class RegistrarOrdenDeVentaRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string JsonOrders;
        
        public RegistrarOrdenDeVentaRequestBody() {
        }
        
        public RegistrarOrdenDeVentaRequestBody(string JsonOrders) {
            this.JsonOrders = JsonOrders;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class RegistrarOrdenDeVentaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="RegistrarOrdenDeVentaResponse", Namespace="http://localhost:6340/WebServiceGrupoPana.asmx", Order=0)]
        public SAP_Core.WSOv.RegistrarOrdenDeVentaResponseBody Body;
        
        public RegistrarOrdenDeVentaResponse() {
        }
        
        public RegistrarOrdenDeVentaResponse(SAP_Core.WSOv.RegistrarOrdenDeVentaResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://localhost:6340/WebServiceGrupoPana.asmx")]
    public partial class RegistrarOrdenDeVentaResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public SAP_Core.WSOv.clsBeRespuesta RegistrarOrdenDeVentaResult;
        
        public RegistrarOrdenDeVentaResponseBody() {
        }
        
        public RegistrarOrdenDeVentaResponseBody(SAP_Core.WSOv.clsBeRespuesta RegistrarOrdenDeVentaResult) {
            this.RegistrarOrdenDeVentaResult = RegistrarOrdenDeVentaResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebServiceVISTSoapChannel : SAP_Core.WSOv.WebServiceVISTSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebServiceVISTSoapClient : System.ServiceModel.ClientBase<SAP_Core.WSOv.WebServiceVISTSoap>, SAP_Core.WSOv.WebServiceVISTSoap {
        
        public WebServiceVISTSoapClient() {
        }
        
        public WebServiceVISTSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebServiceVISTSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceVISTSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebServiceVISTSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SAP_Core.WSOv.RegistrarOrdenDeVentaResponse SAP_Core.WSOv.WebServiceVISTSoap.RegistrarOrdenDeVenta(SAP_Core.WSOv.RegistrarOrdenDeVentaRequest request) {
            return base.Channel.RegistrarOrdenDeVenta(request);
        }
        
        public SAP_Core.WSOv.clsBeRespuesta RegistrarOrdenDeVenta(string JsonOrders) {
            SAP_Core.WSOv.RegistrarOrdenDeVentaRequest inValue = new SAP_Core.WSOv.RegistrarOrdenDeVentaRequest();
            inValue.Body = new SAP_Core.WSOv.RegistrarOrdenDeVentaRequestBody();
            inValue.Body.JsonOrders = JsonOrders;
            SAP_Core.WSOv.RegistrarOrdenDeVentaResponse retVal = ((SAP_Core.WSOv.WebServiceVISTSoap)(this)).RegistrarOrdenDeVenta(inValue);
            return retVal.Body.RegistrarOrdenDeVentaResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SAP_Core.WSOv.RegistrarOrdenDeVentaResponse> SAP_Core.WSOv.WebServiceVISTSoap.RegistrarOrdenDeVentaAsync(SAP_Core.WSOv.RegistrarOrdenDeVentaRequest request) {
            return base.Channel.RegistrarOrdenDeVentaAsync(request);
        }
        
        public System.Threading.Tasks.Task<SAP_Core.WSOv.RegistrarOrdenDeVentaResponse> RegistrarOrdenDeVentaAsync(string JsonOrders) {
            SAP_Core.WSOv.RegistrarOrdenDeVentaRequest inValue = new SAP_Core.WSOv.RegistrarOrdenDeVentaRequest();
            inValue.Body = new SAP_Core.WSOv.RegistrarOrdenDeVentaRequestBody();
            inValue.Body.JsonOrders = JsonOrders;
            return ((SAP_Core.WSOv.WebServiceVISTSoap)(this)).RegistrarOrdenDeVentaAsync(inValue);
        }
    }
}
