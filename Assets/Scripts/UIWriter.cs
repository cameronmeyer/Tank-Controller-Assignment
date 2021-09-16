using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Text _playerHealthText;
    [SerializeField] Text _bossHealthText;
    [SerializeField] Text _treasureText;

    public void SetPlayerHealthUI(int amount)
    {
        _playerHealthText.text = "Player HP: " + amount + "HP";
    }

    public void SetBossHealthUI(int amount)
    {
        _bossHealthText.text = "Boss HP: " + amount + "HP";
    }

    public void SetTreasureUI(int amount)
    {
        _treasureText.text = "Treasure: $" + amount;
    }
}
