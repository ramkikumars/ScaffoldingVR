using System;
using Fusion;
using Fusion.Sockets;
using System.Threading.Tasks;
#if UNITY_EDITOR
#endif
public class DebugStart : NetworkDebugStart

{
    // Start is called before the first frame update
    protected override Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = DefaultRoomName,
            Initialized = initialized,
            ObjectPool = runner.GetComponent<INetworkObjectPool>()
        });
    }
}
