using BattleForSpaceResources.Networking;
using Lidgren.Network;
using System.Collections.Generic;
using System.Threading.Tasks;
public class LoginHandler
{
    private ServerCore serverCore;
    public LoginHandler(ServerCore net)
    {
        this.serverCore = net;
    }
    public void Update()
    {
        //List<NetIncomingMessage> mss = serverCore.GetMessages();
        //foreach (NetIncomingMessage m in ServerCore.messages)
        //Parallel.ForEach(ServerCore.messages, (m) =>
        List<NetIncomingMessage> mss = serverCore.GetMessages();
        lock (mss)
        {
            for (int i = 0; i < mss.Count; i++)
            {
                NetIncomingMessage m = mss[i];
                switch (m.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        if (TryApproveConnection(m))
                            m.SenderConnection.Approve();
                        else
                            m.SenderConnection.Deny();
                        mss.Remove(m);
                        //ServerCore.instance.UpdatePackets(false, m, ServerCore.messages);
                        break;
                }
            }
        }
    }
    private bool TryApproveConnection(NetIncomingMessage m)
    {
        byte code = m.ReadByte();
        return code == 20;
    }
}