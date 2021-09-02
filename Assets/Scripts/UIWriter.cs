using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Text _treasureText;

    public void SetTreasureUI(int amount)
    {
        _treasureText.text = "Treasure: $" + amount;
    }
}
