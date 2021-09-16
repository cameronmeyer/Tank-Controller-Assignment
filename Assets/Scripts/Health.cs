using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    private int _currentHealth = 3;
    [SerializeField] int _maxHealth = 3;
    private bool _isPlayer = false;
    private bool _isPoweredUp = false;

    [SerializeField] UIWriter _ui;

    void Start()
    {
        _isPlayer = this.GetComponent(typeof(Player)) != null;
        _currentHealth = _maxHealth;

        refreshUI();
    }

    public void refreshUI()
    {
        if (_isPlayer) { _ui.SetPlayerHealthUI(_currentHealth); }
        else { _ui.SetBossHealthUI(_currentHealth); }
        
    }

    public void Kill()
    {
        if(_isPlayer) { this.GetComponent<Player>().Kill(); }
        else { this.GetComponent<Boss>().Kill(); }
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        refreshUI();
        Debug.Log("Player's health: " + _currentHealth);
    }

    public void Damage(int damage)
    {
        if (!_isPoweredUp)
        {
            _currentHealth -= damage;
            refreshUI();
            Debug.Log("Player's health: " + _currentHealth);
            if (_currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void PowerUp()
    {
        _isPoweredUp = true;
    }

    public void PowerDown()
    {
        _isPoweredUp = false;
    }
}