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
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Xml_XmlNode_Binding.Register(app);
            System_Xml_XmlNodeList_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            System_Xml_XmlElement_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Type_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_Type_Binding.Register(app);
            System_Object_Binding.Register(app);
            GameResMgr_Binding.Register(app);
            System_String_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            System_Reflection_FieldInfo_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEventHelper_Binding.Register(app);
            System_Diagnostics_Stopwatch_Binding.Register(app);

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
