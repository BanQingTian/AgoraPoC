using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zrime;

public static class MsgId
{
    #region special id

    public const string __READY_PLAY_MSG_ = "ready_play_msg";
    public const string __JOIN_NEW_PLAYER_MSG_ = "join_new_player_msg";

    public const string __BEGIN_MOVE_MSG = "begin_move_msg";

    public const string __COMMON_MSG = "common_msg";

    #endregion
}

public class ZMessageManager
{

    public static ZMessageManager m_Instance;
    public static ZMessageManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new ZMessageManager();
            }
            return m_Instance;
        }
    }

    public ZClient client;

    private bool m_Initialized = false;

    public void Init()
    {
        if (m_Initialized) return;

        client = ZClient.Instance;
        client.Persist();
        client.AddListener(MsgId.__READY_PLAY_MSG_, _Response_ReadyPlay);
        client.AddListener(MsgId.__JOIN_NEW_PLAYER_MSG_, _Response_JoinNewPlayer);

        client.AddListener(MsgId.__COMMON_MSG, __Response_CommonMSG);

        m_Initialized = true;
    }

    #region C2SFunc

    public void SendConnectAndJoinRoom(string serverIp, string port)
    {
        client.Connect(serverIp, port);
    }

    public void SendMsg(string msdId, string msgContent)
    {
        client.SendMsg(msdId, msgContent);
    }

    #endregion


    #region ResponseFunc


    public void _Response_JoinNewPlayer(object msg)
    {
        Player player = msg as Player;

        Debug.Log("createAAAAAA");
        PlayerEntity pe = GameObject.Instantiate<PlayerEntity>(ZNetworkingManager.Instance.GetPrefab());
        pe.gameObject.SetActive(false);
        pe.Init(player);
        pe.UpdatePoseData();

        ZPlayerMe.Instance.AddPlayer(player.PlayerId, pe);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateCallerPanel(player.PlayerId, true);
        // update ui === todo

        if (ZMain.Instance.isMaster)
        {
            Debug.Log("==============="+MainController.Instance.Mode);
            switch (MainController.Instance.Mode)
            {
                case CurMode.Video:
                    ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "video_mode", "shelter"));
                    break;
                case CurMode.Audio:
                    ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "audio_mode", "shelter"));
                    break;
                case CurMode.None:
                    ZMessageManager.Instance.SendMsg(MsgId.__COMMON_MSG, string.Format("{0},{1}", "none_mode", "shelter"));
                    break;
                default:
                    break;
            }
        }
    }

    public void __Response_CommonMSG(object msg)
    {

        Message m = msg as Message;

        MainController.Instance.MsgHandler(m);
    }


    public void _Response_ReadyPlay(object msg)
    {
        // msg.content = player type,player id,
        Message m = msg as Message;
        Debug.Log(m.Content);
        var arrs = m.Content.Split(',');
        // GameManager.Instance.__Func_Ready(arrs[0], arrs[1]);
    }

    #endregion
}
