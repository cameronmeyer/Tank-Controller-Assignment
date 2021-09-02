using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Text _healthText;
    [SerializeField] Text _treasureText;

    public void SetHealthUI(int amount)
    {
        _healthText.text = "Health: " + amount + "HP";
    }

    public void SetTreasureUI(int amount)
    {
        _treasureText.text = "Treasure: $" + amount;
    }
}
