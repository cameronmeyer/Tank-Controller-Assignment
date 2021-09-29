using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIWriter : MonoBehaviour
{
    [SerializeField] Health _playerHealth;
    [SerializeField] Health _bossHealth;
    [SerializeField] Slider _playerHealthBar;
    [SerializeField] Slider _bossHealthBar;
    [SerializeField] Text _gameEndText;
    [SerializeField] Text _gameEndTextShadow;
    [SerializeField] float _barLerpDuration = 1.0f;
    [SerializeField] float _vignetteLerpDuration = 1.0f;
    [SerializeField] float _maxVignetteIntensity = 0.3f;

    [SerializeField] Volume _vol;
    Vignette _vignette;

    private CinemachineShake _cs;
    [SerializeField] float _shakeIntensity = 3f;
    [SerializeField] float _shakeTimer = 0.1f;

    [SerializeField] AudioClip _playerDamageSound;

    private void Awake()
    {
        _playerHealthBar.maxValue = _playerHealth.MaxHealth;
        _bossHealthBar.maxValue = _bossHealth.MaxHealth;
        _vol.profile.TryGet<Vignette>(out _vignette);
        _cs = Camera.main.GetComponent<CinemachineShake>();
    }

    private void OnEnable()
    {
        _playerHealth.HealthUpdate += OnPlayerHealthUpdate;
        _bossHealth.HealthUpdate += OnBossHealthUpdate;

        _playerHealth.GameEnd += OnGameLose;
        _bossHealth.GameEnd += OnGameWin;
    }

    private void OnDisable()
    {
        _playerHealth.HealthUpdate -= OnPlayerHealthUpdate;
        _bossHealth.HealthUpdate -= OnBossHealthUpdate;
    }

    private void OnPlayerHealthUpdate(int amount)
    {
        StartCoroutine(SetPlayerHealthUI(amount));

        if (_playerHealthBar.value > amount)
        {
            StartCoroutine(DamageFlash());
            _cs.Shake(_shakeIntensity, _shakeTimer);
            AudioHelper.PlayClip2D(_playerDamageSound, 1f);
        }
    }

    private void OnBossHealthUpdate(int amount)
    {
        StartCoroutine(SetBossHealthUI(amount));

        if (_bossHealthBar.value > amount)
        {
            _cs.Shake(_shakeIntensity, _shakeTimer);
        }
    }

    private IEnumerator DamageFlash()
    {
        float elapsedTime = 0;

        while (elapsedTime < _vignetteLerpDuration / 6)
        {
            _vignette.intensity.value = Mathf.Lerp(0f, _maxVignetteIntensity, (elapsedTime / (_vignetteLerpDuration / 6)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(_vignetteLerpDuration / 2);

        while (elapsedTime < _vignetteLerpDuration)
        {
            _vignette.intensity.value = Mathf.Lerp(_maxVignetteIntensity, 0f, (elapsedTime / _vignetteLerpDuration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SetPlayerHealthUI(int amount)
    {
        float elapsedTime = 0;
        float initValue = _playerHealthBar.value;

        while(elapsedTime < _barLerpDuration)
        {
            _playerHealthBar.value = Mathf.Lerp(initValue, amount, (elapsedTime/_barLerpDuration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SetBossHealthUI(int amount)
    {
        float elapsedTime = 0;
        float initValue = _bossHealthBar.value;

        while (elapsedTime < _barLerpDuration)
        {
            _bossHealthBar.value = Mathf.Lerp(initValue, amount, (elapsedTime / _barLerpDuration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnGameWin()
    {
        _gameEndText.text = "You Win!";
        _gameEndTextShadow.text = "You Win!";
    }

    private void OnGameLose()
    {
        _gameEndText.text = "You Lose!";
        _gameEndTextShadow.text = "You Lose!";
    }
}
