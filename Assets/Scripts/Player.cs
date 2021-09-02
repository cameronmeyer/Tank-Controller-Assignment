using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    int _currentTreasure;
    bool _isPoweredUp = false;
    [SerializeField] UIWriter _ui;

    TankController _tankController;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        refreshUI();
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        refreshUI();
        Debug.Log("Player's health: " + _currentHealth);
    }
    public void DecreaseHealth(int amount)
    {
        if(!_isPoweredUp)
        {
            _currentHealth -= amount;
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

    public void Kill()
    {
        StartCoroutine(Reset());
        _tankController.enabled = false;
        MeshRenderer[] tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer mr in tankArt)
        {
            mr.enabled = false;
        }
    }

    public void IncreaseTreasure(int amount)
    {
        _currentTreasure += amount;
        refreshUI();
        Debug.Log("Player's treasure: " + _currentTreasure);
    }

    public void refreshUI()
    {
        _ui.SetHealthUI(_currentHealth);
        _ui.SetTreasureUI(_currentTreasure);
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Sandbox");
    }
}
