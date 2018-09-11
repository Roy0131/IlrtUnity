using ILRuntime.Runtime.Enviorment;

public class ILHelper
{

    public static void InitILRunTime(AppDomain appDomain)
    {
        BindingAdaptor(appDomain);
        InitMethodConvertor(appDomain);
        
        appDomain.DelegateManager.RegisterFunctionDelegate<Adapt_IMessage.Adaptor>();
        appDomain.DelegateManager.RegisterMethodDelegate<Adapt_IMessage.Adaptor>();
        appDomain.DelegateManager.RegisterFunctionDelegate<System.Object, System.Security.Cryptography.X509Certificates.X509Certificate, System.Security.Cryptography.X509Certificates.X509Chain, System.Net.Security.SslPolicyErrors, System.Boolean>();
        appDomain.DelegateManager.RegisterMethodDelegate<System.IAsyncResult>();
        appDomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.IMessage>();

        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appDomain);
        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
    }

    private static void BindingAdaptor(AppDomain appDomain)
    {
        appDomain.RegisterCrossBindingAdaptor(new Adapt_IMessage());
    }

    private static void InitMethodConvertor(AppDomain appDomain)
    {
        appDomain.DelegateManager.RegisterDelegateConvertor<System.Net.Security.RemoteCertificateValidationCallback>((act) =>
        {
            return new System.Net.Security.RemoteCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) =>
            {
                return ((System.Func<System.Object, System.Security.Cryptography.X509Certificates.X509Certificate, System.Security.Cryptography.X509Certificates.X509Chain, System.Net.Security.SslPolicyErrors, System.Boolean>)act)(sender, certificate, chain, sslPolicyErrors);
            });
        });        appDomain.DelegateManager.RegisterDelegateConvertor<System.AsyncCallback>((act) =>
        {
            return new System.AsyncCallback((ar) =>
            {
                ((System.Action<System.IAsyncResult>)act)(ar);
            });
        });

    }
}
