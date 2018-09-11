using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Collections_Generic_Dictionary_2_String_Delegate_Binding.Register(app);
            System_Object_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Xml_XmlNode_Binding.Register(app);
            System_Xml_XmlNodeList_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_Xml_XmlElement_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Byte_Binding.Register(app);
            System_Buffer_Binding.Register(app);
            System_String_Binding.Register(app);
            Logger_Binding.Register(app);
            Google_Protobuf_MessageExtensions_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            Google_Protobuf_ByteString_Binding.Register(app);
            System_Collections_Generic_Queue_1_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            Loom_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_Net_WebRequest_Binding.Register(app);
            System_IO_Stream_Binding.Register(app);
            System_Net_WebResponse_Binding.Register(app);
            System_IO_MemoryStream_Binding.Register(app);
            Google_Protobuf_MessageParser_1_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_IO_StreamReader_Binding.Register(app);
            System_IO_TextReader_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            LitJson_JsonData_Binding.Register(app);
            System_Convert_Binding.Register(app);
            LoomHelper_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Adapt_IMessage_Binding_Adaptor_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            System_Single_Binding.Register(app);
            Google_Protobuf_MessageParser_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Int32_Binding.Register(app);
            Google_Protobuf_CodedInputStream_Binding.Register(app);
            Google_Protobuf_FieldCodec_Binding.Register(app);
            Google_Protobuf_CodedOutputStream_Binding.Register(app);
            Google_Protobuf_ProtoPreconditions_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_String_Binding.Register(app);
            Google_Protobuf_Collections_RepeatedField_1_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Type_Binding.Register(app);
            GameResMgr_Binding.Register(app);
            System_Reflection_FieldInfo_Binding.Register(app);
            System_Diagnostics_Stopwatch_Binding.Register(app);
            UnityEngine_SceneManagement_SceneManager_Binding.Register(app);
            UnityEngine_SceneManagement_Scene_Binding.Register(app);
            UnityEngine_AsyncOperation_Binding.Register(app);
            System_Array_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            ObjectHelper_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Int32_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEventHelper_Binding.Register(app);
            UnityEngine_UI_InputField_Binding.Register(app);
            System_Net_ServicePointManager_Binding.Register(app);
            System_Net_WebException_Binding.Register(app);
            System_Net_HttpWebResponse_Binding.Register(app);
            System_Activator_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
