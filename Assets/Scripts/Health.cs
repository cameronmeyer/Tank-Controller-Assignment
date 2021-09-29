using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    private int _currentHealth = 3;
    [SerializeField] int _maxHealth = 3;
    private bool _isPlayer = false;
    private bool _isPoweredUp = false;
    private bool _isInvincible = false;
    [SerializeField] float _invincibilityDuration = 0.4f;
    private float _timeLastDamaged = 0f;

    [SerializeField] UIWriter _ui;

    public float CurrentHealth { get => _currentHealth; }
    public float MaxHealth { get => _maxHealth; }

    public event Action<int> HealthUpdate;
    public event Action GameEnd;

    void Start()
    {
        _isPlayer = this.GetComponent(typeof(Player)) != null;
        Debug.Log(gameObject.name + " is player: " + _isPlayer);
        _currentHealth = _maxHealth;

        HealthUpdate?.Invoke(_currentHealth);
    }

    public void Flash()
    {
        if (_isPlayer) { this.GetComponent<Player>().Flash(); }
        else { this.GetComponent<Boss>().Flash(); }
    }

    public void Kill()
    {
        if(_isPlayer) { this.GetComponent<Player>().Kill(); }
        else { this.GetComponent<Boss>().Kill(); }
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        Debug.Log("Player's health: " + _currentHealth);
    }

    public void Damage(int damage)
    {
        if(_isInvincible && Time.time >= (_timeLastDamaged + _invincibilityDuration))
        {
            _isInvincible = false;
        }

        if (!_isInvincible && (!_isPlayer || (_isPlayer &&!_isPoweredUp)))
        {
            _currentHealth -= damage;
            HealthUpdate?.Invoke(_currentHealth);
            _timeLastDamaged = Time.time;
            
            if (_currentHealth <= 0)
            {
                GameEnd?.Invoke();
                Kill();
            }
            else
            {
                _isInvincible = true;
                Flash();
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
