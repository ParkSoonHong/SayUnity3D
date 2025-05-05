using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<GameObject> characters;

    private List<GameObject> _playerList;
    public List<GameObject> PlayerList => _playerList;

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
        _playerList = new List<GameObject>(characters.Count);
        foreach (GameObject character in characters)
        {
            GameObject player = Instantiate(character);
            player.SetActive(false);
            _playerList.Add(player);
        }
    }
}
