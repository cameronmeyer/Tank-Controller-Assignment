using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Health _playerHealth;
    [SerializeField] Health _bossHealth;
    [SerializeField] Slider _playerHealthBar;
    [SerializeField] Slider _bossHealthBar;
    [SerializeField] float _lerpDuration;
    //[SerializeField] Text _treasureText;

    private void Awake()
    {
        _playerHealthBar.maxValue = _playerHealth.MaxHealth;
        _bossHealthBar.maxValue = _bossHealth.MaxHealth;
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
        StartCoroutine(SetPlayerHealthUI(amount));
    }

    private void OnBossHealthUpdate(int amount)
    {
        StartCoroutine(SetBossHealthUI(amount));
    }

    private IEnumerator SetPlayerHealthUI(int amount)
    {
        float elapsedTime = 0;
        float initValue = _playerHealthBar.value;

        while(elapsedTime < _lerpDuration)
        {
            _playerHealthBar.value = Mathf.Lerp(initValue, amount, (elapsedTime/_lerpDuration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SetBossHealthUI(int amount)
    {
        float elapsedTime = 0;
        float initValue = _bossHealthBar.value;

        while (elapsedTime < _lerpDuration)
        {
            _bossHealthBar.value = Mathf.Lerp(initValue, amount, (elapsedTime / _lerpDuration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    /*public void SetTreasureUI(int amount)
    {
        _treasureText.text = "Treasure: $" + amount;
    }*/
}
