using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public NetworkObject networkObject => GetComponent<NetworkObject>();
    // Start is called before the first frame update
    public string playerName;
    // public TextMeshPro playerNameTmPro;

    // [System.Serializable]
    [Networked]
    public NetworkString <_16> nickName{ get; set; }
    void Start()
    {

    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("Spawned local player");
        }
        else Debug.Log("Spawned remote player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        changed.Behaviour.OnNickNameChanged();
    }
    private void OnNickNameChanged(){
        // playerNameTmPro.text=nickName.ToString();
    }
}
