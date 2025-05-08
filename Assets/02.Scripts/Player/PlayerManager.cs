using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> characters;

    private List<GameObject> _playerList;
    public List<GameObject> PlayerList => _playerList;

    private List<PlayerSO> _playerDatas; // 나중에 교체

    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
      
    }

}