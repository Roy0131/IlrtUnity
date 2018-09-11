using LitJson;
using Msg.ClientMessage;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ClientLogic;

public abstract class HttpRequestBase<T>
{
    protected string _url;
    protected string _method;
    protected REventDelegate<T> _callBack;
    protected HttpWebRequest _request;
    protected REventDelegate<WebExceptionStatus> _errorCallBack;
    public HttpRequestBase(string url, REventDelegate<T> method, REventDelegate<WebExceptionStatus> errorCallBack = null)
    {
        _url = url;
        _callBack = method;
        _errorCallBack = errorCallBack;
        _method = "GET";
    }

    public void Send()
    {
        if (_url.Contains("https"))
            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        _request = WebRequest.Create(_url) as HttpWebRequest;
        _request.Method = _method;
        _request.Timeout = 5000;
        DoRequestData();
        try
        {
            _request.BeginGetResponse(new AsyncCallback(OnResponse), null);
        }
        catch (WebException ex)
        {
            Logger.LogError("[HttpRequestBase.Send() => https send error:" + ex.Status + "]");
        }
    }

    private void OnResponse(IAsyncResult result)
    {
        HttpWebResponse webResponse = null;
        T data = default(T);
        int code = -1;
        try
        {
            webResponse = _request.EndGetResponse(result) as HttpWebResponse;
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                data = ParseResponseData(webResponse);
                code = 0;
            }
            webResponse.Close();
            DoResponse(code, data);
            webResponse = null;
        }
        catch (WebException ex)
        {
            DoError(ex.Status);
        }

        OnResponseEnd();
    }

    protected virtual void OnResponseEnd()
    {
        
    }

    protected virtual void DoError(WebExceptionStatus status)
    {
        if (_errorCallBack != null)
            _errorCallBack(status);
        Dispose();
    }

    protected virtual void DoRequestData()
    {

    }

    protected virtual void DoResponse(int code, T data)
    {
        if (code != -1)
        {
            if (_callBack != null)
                _callBack(data);
        }
        Dispose();
    }

    public virtual void Dispose()
    {
        if (_request != null)
        {
            _request.Abort();
            _request = null;
        }
        _errorCallBack = null;
        _callBack = null;
    }

    public string RequestUrl
    {
        get { return _url; }
    }

    protected abstract T ParseResponseData(HttpWebResponse data);

    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}

public class HttpPostRequest : HttpRequestBase<S2C_MSG_DATA>
{
    private byte[] _postData;
    public HttpPostRequest(string url, REventDelegate<S2C_MSG_DATA> method, byte[] postData, REventDelegate<WebExceptionStatus> errorCallBack = null)
        : base(url, method, errorCallBack)
    {
        _method = "POST";
        _postData = postData;
    }

    protected override void DoRequestData()
    {
        _request.ContentType = "application/x-www-form-urlencoded";
        if (_postData != null)
        {
            using (Stream st = _request.GetRequestStream())
                st.Write(_postData, 0, _postData.Length);
            _postData = null;
        }
    }

    protected override S2C_MSG_DATA ParseResponseData(HttpWebResponse data)
    {
        S2C_MSG_DATA msgData = null;
        byte[] bt = null;
        using (Stream resStream = data.GetResponseStream())
        {
            MemoryStream ms = new MemoryStream();
            resStream.CopyTo(ms);
            bt = ms.ToArray();
            Logger.Log("[PostRequest Receive() <== byte length:" + bt.Length + "]");
            msgData = S2C_MSG_DATA.Parser.ParseFrom(bt);
            ms.Close();
        }
        return msgData;
    }

	protected override void OnResponseEnd()
	{
        base.OnResponseEnd();
        GameNetMgr.Instance.mGameServer.RemoveRequest(this);
	}
}

public class HttpGetRequest : HttpRequestBase<JsonData>
{
    public HttpGetRequest(string url, REventDelegate<JsonData> method, REventDelegate<WebExceptionStatus> errorCallBack = null)
        : base(url, method, errorCallBack)
    {
        _method = "GET";
    }

    protected override JsonData ParseResponseData(HttpWebResponse data)
    {
        JsonData json = null;
        using (Stream respStream = data.GetResponseStream())
        {
            using (StreamReader sr = new StreamReader(respStream, System.Text.Encoding.UTF8))
            {
                string jd = sr.ReadToEnd();
                Logger.Log("[GetRequest Receive() <==" + jd + "]");
                json = JsonMapper.ToObject(jd);
            }
        }
        return json;
    }
}