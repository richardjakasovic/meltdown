using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetworkManager : MonoBehaviour
{
    public static GameNetworkManager Instance { get; private set; }

    private Lobby currentLobby;
    private string joinCode;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        // Give the clone a different player ID so Unity treats it as a separate player
        if (ParrelSync.ClonesManager.IsClone())
        {
            string customArgument = ParrelSync.ClonesManager.GetArgument();
            AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}");
        }

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
    }

    // HOST
    public async Task StartHost(int maxPlayers = 4)
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log("Join code: " + joinCode);

        var utpTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utpTransport.SetRelayServerData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );

        // Create lobby so friends can find the session
        var lobbyOptions = new CreateLobbyOptions
        {
            IsPrivate = false,
            Data = new Dictionary<string, DataObject>
            {
                { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
            }
        };

        currentLobby = await LobbyService.Instance.CreateLobbyAsync("Meltdown", maxPlayers, lobbyOptions);
        Debug.Log("Lobby created: " + currentLobby.LobbyCode);

        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    // JOIN
    public async Task StartClient(string lobbyCode)
    {
        currentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
        joinCode = currentLobby.Data["joinCode"].Value;

        var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        var utpTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utpTransport.SetRelayServerData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();
    }

    public string GetLobbyCode() => currentLobby?.LobbyCode;
}