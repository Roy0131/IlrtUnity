using ClientLogic;
using Google.Protobuf;
using Msg.ClientMessage;
using Msg.ClientMessageId;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameServer
{
    private Dictionary<int, NetMsgEventHandle> _dictNetHandle = new Dictionary<int, NetMsgEventHandle>();
    public int mPlayerId { get; set; }
    public string mToken { set; private get; }
    private ByteStream _recvStream;
    private List<NetMsgRecvData> _lstRecvDatas = new List<NetMsgRecvData>();

    private List<HttpPostRequest> _lstPostRequest = new List<HttpPostRequest>();

    public GameServer()
    {
        mPlayerId = 0;
        mToken = "";
        _recvStream = new ByteStream();
        InitNetHandle();
    }

    private void InitNetHandle()
    {
        _dictNetHandle.Clear();

        RegisterNetMsgType<S2CEnterGameResponse>((int)MSGID.S2CEnterGameResponse, S2CEnterGameResponse.Parser, DoEnterGame);
    }

    public void DoEnterGame(S2CEnterGameResponse value)
    {
        Logger.Log("[GameServer.DoEnterGame() => enter game success, player id:" + value.PlayerId + "]");
        _blEnterGame = true;
    }

    private void RegisterNetMsgType<T>(int msgId, MessageParser msgParser, REventDelegate<T> method) where T : IMessage
    {
        if (_dictNetHandle.ContainsKey(msgId))
        {
            Logger.LogError("[HttpMgr.RegisterNetMsgType() => msgId:" + msgId + "重复注册]");
            return;
        }
        _dictNetHandle.Add(msgId, new NetMsgEventHandle(msgId, msgParser, typeof(T), method));
    }

    public void ReqLoginGameServer(string acc, string token)
    {
        mToken = token;
        C2SEnterGameRequest enterGameReq = new C2SEnterGameRequest();
        enterGameReq.Acc = acc;
        enterGameReq.Token = token;

        Send(MSGID.C2SEnterGameRequest, enterGameReq.ToByteString());
    }

    private void DoSend(C2S_MSG_DATA postData)
    {
        HttpPostRequest request = new HttpPostRequest(GameNetMgr.GAME_LOGIC_URL, OnReceiveData, postData.ToByteArray());
        _lstPostRequest.Add(request);
        request.Send();
        Logger.LogWarning("[HttpRequest.Send() ==> send url:" + request.RequestUrl + ", msgId:" + postData.MsgCode + ", msg len:" + postData.Data.Length + "]");
    }

    public void RemoveRequest(HttpPostRequest request)
    {
        if (_lstPostRequest.IndexOf(request) == -1)
        {
            Logger.LogWarning("GameServer.RemoveRequest() => request can't found!!");
            return;
        }
        _lstPostRequest.Remove(request);
        if (_queueSendDatas.Count > 1)
        {
            C2S_MSG_DATA postData = _queueSendDatas.Dequeue();
            DoSend(postData);
        }
    }


    private Queue<C2S_MSG_DATA> _queueSendDatas = new Queue<C2S_MSG_DATA>();
    private void Send(MSGID msgId, ByteString value)
    {
        C2S_MSG_DATA postData = new C2S_MSG_DATA();
        postData.PlayerId = mPlayerId;
        postData.Token = mToken;
        postData.MsgCode = (int)msgId;
        postData.Data = value;
        if (_queueSendDatas.Count > 0)
            _queueSendDatas.Enqueue(postData);
        else
            DoSend(postData);
    }

    private void OnReceiveData(S2C_MSG_DATA value)
    {
        if (value.ErrorCode < 0)
        {

            Action RunOnMainThread = () =>
            {
                NetErrorHelper.DoErrorCode(value.ErrorCode);
            };

            Loom.QueueOnMainThread(RunOnMainThread);
            _recvStream.Clear();
            return;
        }

        byte[] bytes = value.Data.ToByteArray();
        _recvStream.AddBytes(bytes);
        while (_recvStream.BytesAvailable >= 4)
        {
            int msgId = _recvStream.ReadInt(); //消息id
            int msgLen = _recvStream.ReadInt(); //消息长度
            byte[] msgData = _recvStream.ReadBytes(msgLen);
            if (_dictNetHandle.ContainsKey(msgId))
            {
                NetMsgEventHandle handle = _dictNetHandle[msgId];
                NetMsgRecvData data = NetPacketHelper.Read(msgId, msgData, handle);
                if (data != null)
                    _lstRecvDatas.Add(data);
            }
            else
            {
                Logger.LogWarning("[GameServer.OnReceiveData() => 消息号:" + msgId + "未注册]");
            }
        }
        _recvStream.Clear();
    }

    #region HeartBeat
    private static bool _blEnterGame;
    private static float _flHeartBeatTime = 5f;

    private void SendHeartBeat()
    {
        C2SHeartbeat req = new C2SHeartbeat();
        Send(MSGID.C2SHeartbeat, req.ToByteString());
    }

    #endregion
    public void Update()
    {
        NetMsgRecvData recvData = null;
        while ((recvData = GetRecvDate()) != null)
            recvData.Exec();
        if (_blEnterGame)
        {
            _flHeartBeatTime -= Time.deltaTime;
            if (_flHeartBeatTime <= 0.01f)
            {
                _flHeartBeatTime = 30f;
                SendHeartBeat();
            }
        }
    }

    private NetMsgRecvData GetRecvDate()
    {
        if (_lstRecvDatas == null || _lstRecvDatas.Count == 0)
            return null;
        NetMsgRecvData data = _lstRecvDatas[0];
        _lstRecvDatas.RemoveAt(0);
        return data;
    }

    public void Dispose()
    {
        if (_dictNetHandle != null)
        {
            _dictNetHandle.Clear();
            _dictNetHandle = null;
        }
        if (_lstRecvDatas != null)
        {
            _lstRecvDatas.Clear();
            _lstRecvDatas = null;
        }
        if (_lstPostRequest != null)
        {
            for (int i = 0; i < _lstPostRequest.Count; i++)
                _lstPostRequest[i].Dispose();
            _lstPostRequest.Clear();
            _lstPostRequest = null;
        }
    }
}