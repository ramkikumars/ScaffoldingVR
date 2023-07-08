using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;
using Fusion.Photon.Realtime;

public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;
    public String region;
    public String sessionName;
    public String sceneName;
    // Start is called before the first frame update
    // public NetworkCue networkCue;
    void Start()
    {

        if (networkRunner == null)
        {
            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network runner";

            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared,sessionName, region,NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
            }

            Debug.Log($"Server NetworkRunner started.");
            // networkCue.enabled=true;
        }

    }



    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName,string region,NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            //Handle networked objects that already exits in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;
        var appSettings = BuildCustomAppSetting(region);
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            Initialized = initialized,
            SceneManager = sceneManager,
            // CustomPhotonAppSettings=appSettings
        });
    }

    private AppSettings BuildCustomAppSetting(string region, string customAppID = null, string appVersion = "1.0.0")
    {

        var appSettings = PhotonAppSettings.Instance.AppSettings.GetCopy();

        appSettings.UseNameServer = true;
        appSettings.AppVersion = appVersion;

        if (string.IsNullOrEmpty(customAppID) == false)
        {
            appSettings.AppIdFusion = customAppID;
        }

        if (string.IsNullOrEmpty(region) == false)
        {
            appSettings.FixedRegion = region.ToLower();
        }

        // If the Region is set to China (CN),
        // the Name Server will be automatically changed to the right one
        // appSettings.Server = "ns.photonengine.cn";

        return appSettings;
    }

    public void CreateGame(string sessionName, string regionName)
    {
        Debug.Log($"Create session {sessionName} scene {sceneName} build Index {SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}")}");

        //Join existing game as a client
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Shared,sessionName,regionName,NetAddress.Any(), SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}"), null);

    }
}
