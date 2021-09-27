using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Health _playerHealth;
    [SerializeField] Health _bossHealth;
    [SerializeField] Text _playerHealthText;
    [SerializeField] Slider _playerHealthBar;
    [SerializeField] Text _bossHealthText;
    //[SerializeField] Text _treasureText;

    private void Awake()
    {
        _playerHealthBar.maxValue = _playerHealth.MaxHealth;
    }

    private void OnEnable()
    {
        _playerHealth.HealthUpdate += OnPlayerHealthUpdate;
        _bossHealth.HealthUpdate += OnBossHealthUpdate;
    }

    private void OnDisable()
    {
        _playerHealth.HealthUpdate -= OnPlayerHealthUpdate;
        _bossHealth.HealthUpdate -= OnBossHealthUpdate;
    }

    private void OnPlayerHealthUpdate(int amount)
    {
        SetPlayerHealthUI(amount);
    }

    private void OnBossHealthUpdate(int amount)
    {
        SetBossHealthUI(amount);
    }

    private void SetPlayerHealthUI(int amount)
    {
        //_playerHealthText.text = "Player HP: " + amount + "HP";
        _playerHealthBar.value = amount;
    }

    private void SetBossHealthUI(int amount)
    {
        _bossHealthText.text = "Boss HP: " + amount + "HP";
    }

    /*public void SetTreasureUI(int amount)
    {
        _treasureText.text = "Treasure: $" + amount;
    }*/
}
