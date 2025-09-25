using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawner : MonoBehaviour
{
    public static GameSpawner Instance;

    [SerializeField] private Transform[] playerSpawnPos;
    [SerializeField] private Transform[] monsterSpawnPos;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.Log($"Singleton {gameObject.name} is not unique");
        }
    }

    public void SpawnPlayer()
    {
        Transform spawn = playerSpawnPos[Random.Range(0, playerSpawnPos.Length)];
        GameObject player = PhotonNetwork.Instantiate(GameConstants.PLAYER, spawn.position, spawn.rotation);
    }
}
