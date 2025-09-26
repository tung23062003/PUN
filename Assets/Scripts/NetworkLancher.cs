using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

/// <summary>
/// A comprehensive Launcher for Photon PUN2 targeted at MMO-like games.
/// - Attach to a persistent GameObject (DontDestroyOnLoad).
/// - Expose events for UI and other systems to subscribe.
/// - Uses MonoBehaviourPunCallbacks to receive Photon callbacks.
/// - PhotonNetwork.AutomaticallySyncScene can be enabled to let MasterClient change scene for all.
/// </summary>
public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    [Header("Photon Settings")]
    [Tooltip("Game version helps separate players on incompatible builds.")]
    public string gameVersion = "1.0";
    [Tooltip("If true, PhotonNetwork.AutomaticallySyncScene = true")]
    public bool autoSyncScene = true;
    [Tooltip("Default max players per room for quick creation")]
    public byte defaultMaxPlayers = 50;

    [Header("Room Defaults")]
    public bool defaultIsVisible = true;
    public bool defaultIsOpen = true;
    public bool cleanupCacheOnLeave = true;

    // Public events UI / systems can subscribe to:
    public event Action OnConnectedToPhoton;
    public event Action<DisconnectCause> OnDisconnectedFromPhoton;
    public event Action OnJoinedLobbyEvent;
    public event Action<List<RoomInfo>> OnRoomListUpdated;
    public event Action<Player> OnPlayerEntered;
    public event Action<Player> OnPlayerLeft;
    public event Action<Player> OnMasterClientSwitchedEvent;
    public event Action OnJoinedRoomEvent;
    public event Action OnLeftRoomEvent;
    public event Action<string> OnCreateRoomFailedEvent;
    public event Action<string> OnJoinRoomFailedEvent;
    public event Action<string> OnErrorEvent;

    // Cached room list (from OnRoomListUpdate)
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    // Matchmaking token/state
    private bool isMatchmaking = false;
    private string matchmakingKey = "mode"; // sample custom room property key

    // Singleton convenience
    public static NetworkLauncher Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.AutomaticallySyncScene = autoSyncScene;
        PhotonNetwork.GameVersion = gameVersion;
        // Optionally set send/serialize rates (tune for your game)
        // PhotonNetwork.SerializationRate = 10;
        // PhotonNetwork.SendRate = 20;
    }

    #region Public API

    /// <summary>
    /// Connect to Photon Cloud using settings configured in PhotonServerSettings.
    /// </summary>
    public void Connect(string optionalAuthToken = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("[Launcher] Already connected to Photon.");
            OnConnectedToPhoton?.Invoke();
            return;
        }

        // If you need custom auth, set it into AuthValues before connecting.
        if (!string.IsNullOrEmpty(optionalAuthToken))
        {
            PhotonNetwork.AuthValues = new AuthenticationValues(optionalAuthToken);
        }

        Debug.Log("[Launcher] Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Disconnect from Photon.
    /// </summary>
    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }

    /// <summary>
    /// Set player nickname (will be sent to server on connect if connected).
    /// </summary>
    public void SetNickname(string nick)
    {
        if (string.IsNullOrEmpty(nick)) return;
        PhotonNetwork.NickName = nick;
        if (PhotonNetwork.LocalPlayer != null)
        {
            PhotonNetwork.LocalPlayer.NickName = nick;
        }
    }

    /// <summary>
    /// Join default lobby to receive room lists.
    /// </summary>
    public void JoinDefaultLobby()
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnErrorEvent?.Invoke("Not connected to Photon. Call Connect() first.");
            return;
        }

        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// Create a room with options and custom properties (for matchmaking/filtering).
    /// customRoomProperties are visible in lobby if you provide customLobbyProps.
    /// </summary>
    public void CreateRoom(string roomName, byte maxPlayers, bool isVisible, bool isOpen,
        Hashtable customRoomProperties = null, string[] customLobbyProps = null)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnErrorEvent?.Invoke("Not connected to Photon.");
            return;
        }

        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsVisible = isVisible,
            IsOpen = isOpen,
            CleanupCacheOnLeave = cleanupCacheOnLeave
        };

        if (customRoomProperties != null)
        {
            ro.CustomRoomProperties = customRoomProperties;
            ro.CustomRoomPropertiesForLobby = customLobbyProps;
        }

        bool created = PhotonNetwork.CreateRoom(roomName, ro);
        if (!created)
        {
            OnCreateRoomFailedEvent?.Invoke("CreateRoom call failed (possibly invalid name).");
        }
    }

    /// <summary>
    /// Join a room by name.
    /// </summary>
    public void JoinRoomByName(string roomName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnErrorEvent?.Invoke("Not connected to Photon.");
            return;
        }

        PhotonNetwork.JoinRoom(roomName);
    }

    /// <summary>
    /// Join a random room optionally filtered by expected custom properties.
    /// Example: expectedProperties = new Hashtable { { "mode", "pvp" } }
    /// </summary>
    public void JoinRandomRoom(Hashtable expectedCustomRoomProperties = null, byte? maxPlayers = null)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnErrorEvent?.Invoke("Not connected to Photon.");
            return;
        }

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, maxPlayers ?? 0);
    }

    /// <summary>
    /// Leave current room (if in one).
    /// </summary>
    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            OnLeftRoomEvent?.Invoke();
        }
    }

    /// <summary>
    /// Start a simple matchmaking flow: try join random with expected props; if fails, create room with those props.
    /// </summary>
    public void StartMatchmaking(string mode, byte maxPlayers = 50)
    {
        if (!PhotonNetwork.IsConnected)
        {
            OnErrorEvent?.Invoke("Not connected to Photon.");
            return;
        }

        isMatchmaking = true;
        Hashtable expected = new Hashtable { { matchmakingKey, mode } };
        Debug.Log($"[Launcher] StartMatchmaking for mode={mode}");
        PhotonNetwork.JoinRandomRoom(expected, maxPlayers);
    }

    /// <summary>
    /// Stop matchmaking (if trying).
    /// </summary>
    public void StopMatchmaking()
    {
        isMatchmaking = false;
        // nothing special needed; when we get JoinRandomFailed we honor isMatchmaking flag
        Debug.Log("[Launcher] StopMatchmaking");
    }

    /// <summary>
    /// Update player's custom properties (dictionary), e.g. level/class/hp.
    /// </summary>
    public void UpdatePlayerCustomProperties(Hashtable props)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.LocalPlayer == null) return;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    /// <summary>
    /// Update room custom properties.
    /// </summary>
    public void UpdateRoomCustomProperties(Hashtable props)
    {
        if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null) return;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    /// <summary>
    /// Attempt reconnect & rejoin - useful when connection lost temporarily.
    /// </summary>
    public void ReconnectAndRejoin()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ReconnectAndRejoin();
        }
        else
        {
            Debug.Log("[Launcher] Already connected; no ReconnectAndRejoin needed.");
        }
    }

    #endregion

    #region Photon Callbacks

    public override void OnConnected()
    {
        Debug.Log("[Launcher] OnConnected to Photon low-level.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[Launcher] OnConnectedToMaster. Can join/create rooms now.");
        OnConnectedToPhoton?.Invoke();

        // Optionally auto-join lobby
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"[Launcher] OnDisconnected: {cause}");
        OnDisconnectedFromPhoton?.Invoke(cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[Launcher] Joined Lobby.");
        PhotonNetwork.JoinRandomOrCreateRoom();
        OnJoinedLobbyEvent?.Invoke();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("[Launcher] Left Lobby.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Update cached room list: Photon gives incremental info; rebuild cache
        // We'll clear and add non-removed rooms.
        Debug.Log($"[Launcher] OnRoomListUpdate: {roomList.Count} entries.");
        // Simple approach: replace cache with visible rooms
        cachedRoomList.Clear();
        foreach (var info in roomList)
        {
            // skip removed rooms
            if (!info.RemovedFromList)
                cachedRoomList.Add(info);
        }

        // Notify listeners
        OnRoomListUpdated?.Invoke(new List<RoomInfo>(cachedRoomList));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("[Launcher] Player entered: " + newPlayer.NickName);
        OnPlayerEntered?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("[Launcher] Player left: " + otherPlayer.NickName);
        OnPlayerLeft?.Invoke(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("[Launcher] Master client switched to: " + newMasterClient.NickName);
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("[Launcher] OnJoinedRoom. Room: " + PhotonNetwork.CurrentRoom.Name + " Players: " + PhotonNetwork.CurrentRoom.PlayerCount);
        OnJoinedRoomEvent?.Invoke();

        // Optionally cache room custom props or do other initialization
        // Example: spawn player prefab here OR use separate spawner system
        GameSpawner.Instance.SpawnPlayer();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[Launcher] OnLeftRoom");
        OnLeftRoomEvent?.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Launcher] OnCreateRoomFailed: {returnCode} - {message}");
        OnCreateRoomFailedEvent?.Invoke(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"[Launcher] OnJoinRoomFailed: {returnCode} - {message}");
        OnJoinRoomFailedEvent?.Invoke(message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"[Launcher] OnJoinRandomFailed: {returnCode} - {message}");
        // If we were in matchmaking mode, create a new room automatically with that matchmaking prop
        if (isMatchmaking)
        {
            isMatchmaking = false;
            // Create a new room with the expected property so future joiners can find it
            // We'll create a random name using timestamp
            string roomName = $"Room_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            Hashtable props = new Hashtable { { matchmakingKey, "unknown" } }; // you may pass the mode you tried
            Debug.Log("[Launcher] Creating fallback room: " + roomName);
            CreateRoom(roomName, defaultMaxPlayers, defaultIsVisible, defaultIsOpen, props, new string[] { matchmakingKey });
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // You can broadcast this event if needed - e.g. update UI
        Debug.Log($"[Launcher] Player properties updated for {targetPlayer.NickName}");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("[Launcher] Room properties updated.");
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        Debug.Log("[Launcher] FriendList updated. Count: " + friendList.Count);
    }

    #endregion

    #region Utilities / Debug

    /// <summary>
    /// Get a snapshot of current cached room list.
    /// </summary>
    public List<RoomInfo> GetCachedRoomList()
    {
        return new List<RoomInfo>(cachedRoomList);
    }

    /// <summary>
    /// Utility to print current players in room (for debug).
    /// </summary>
    public void DebugLogPlayersInRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            Debug.Log("[Launcher] Not in room.");
            return;
        }

        string s = $"Players in Room {PhotonNetwork.CurrentRoom.Name}: ";
        foreach (var kv in PhotonNetwork.CurrentRoom.Players)
        {
            s += $"{kv.Value.NickName}({kv.Value.ActorNumber}), ";
        }
        Debug.Log(s);
    }

    #endregion
}
