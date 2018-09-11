using LitJson;
using Msg.ClientMessage;
using System;
using System.Net;
using ClientLogic;

public class LoginServer
{
    private int _connectCount = 0;
    private string _account = "";
    private string _password = "";

    public void DoLogin(string acc, string psd = "")
    {
        _account = acc;
        _password = psd;
        HttpFrom form = new HttpFrom(GameNetMgr.LoginServer);
        form.AddValue("account", acc);
        form.AddValue("password", psd);

        new HttpGetRequest(form.ToString(), OnLoginBack, DoLoginError).Send();
    }

    private void DoLoginError(WebExceptionStatus status)
    {
        _connectCount++;
        if (_connectCount > 3)
        {
            Logger.LogError("Login Error, Status:" + status);
            return;
        }
        Logger.Log("Reconnecting!!!, Count:" + _connectCount);
        DoLogin(_account, _password);
    }

    private void OnLoginBack(JsonData jsonData)
    {
        if (jsonData == null)
        {
            Logger.LogWarning("[LoginServer.OnLoginBack() => login failed~]");
            return;
        }
        int code = int.Parse(jsonData["Code"].ToString());
        if (code != 0)
        {
            Logger.LogWarning("[LoginServer.OnLoginBack() => login failed, error code:" + code + "]");
            return;
        }
        else
        {
            byte[] bs = Convert.FromBase64String(jsonData["MsgData"].ToString());
            S2CLoginResponse d = S2CLoginResponse.Parser.ParseFrom(bs);

            Action OnRunLoginSuccess = () =>
            {
                DoSelectServer(d.Acc, d.Token, d.Servers[0].Id);
            };

            LoomHelper.QueueOnMainThread(OnRunLoginSuccess);
        }
    }

    public void DoSelectServer(string acc, string token, int sid)
    {
        HttpFrom form = new HttpFrom(GameNetMgr.SelectServerUrl);
        form.AddValue("account", acc);
        form.AddValue("token", token);
        form.AddValue("server_id", sid);

        new HttpGetRequest(form.ToString(), OnSelectServerBack, OnSelectedServerBack).Send();
    }

    private void OnSelectedServerBack(WebExceptionStatus status)
    {
        Logger.LogError("selecte server failed!!");
    }


    private void OnSelectServerBack(JsonData jsonData)
    {
        if (jsonData == null)
        {
            Logger.LogError("[LoginServer.OnSelectServerBack() => select server failed~~]");
            return;
        }

        Action RunOnMainThread = () =>
        {
            int code = int.Parse(jsonData["Code"].ToString());
            if (code != 0)
            {
                Logger.LogWarning("[LoginServer.OnSelectServerBack() => select server failed, error code:" + code + "]");
                NetErrorHelper.DoErrorCode(code);
                return;
            }
            if (code == 0)
            {
                byte[] bd = Convert.FromBase64String(jsonData["MsgData"].ToString());
                Logger.Log("login in success");
                S2CSelectServerResponse pbData = S2CSelectServerResponse.Parser.ParseFrom(bd);
                GameNetMgr.Instance.mGameServer.ReqLoginGameServer(pbData.Acc, pbData.Token);
            }
            else
            {
                Logger.LogWarning("[GameNetMgr.DoSelectServer() => game logic failed!!!]");
            }
        };

        LoomHelper.QueueOnMainThread(RunOnMainThread);
    }
}