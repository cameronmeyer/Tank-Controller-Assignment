using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    //int _currentTreasure;

    TankController _tankController;
    MeshRenderer[] _tankArt;
    List<Color> colors = new List<Color>();

    [SerializeField] float _flashDuration = 0.5f;
    [SerializeField] Color _flashColor;

    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _playerDeathSound;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
        _tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < _tankArt.Length; i++)
        {
            colors.Add(_tankArt[i].material.color);
        }
    }

    public void Flash()
    {
        StartCoroutine(MaterialFlash());
    }

    private IEnumerator MaterialFlash()
    {
        float elapsedTime = 0;

        Color[] currentColors = new Color[_tankArt.Length];
        for (int i = 0; i < _tankArt.Length; i++)
        {
            currentColors[i] = _tankArt[i].material.color;
        }

        while (elapsedTime < _flashDuration / 2)
        {
            for (int i = 0; i < _tankArt.Length; i++)
            {
                _tankArt[i].material.color = Color.Lerp(currentColors[i], _flashColor, (elapsedTime / (_flashDuration / 2)));
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (elapsedTime < _flashDuration)
        {
            for (int i = 0; i < _tankArt.Length; i++)
            {
                _tankArt[i].material.color = Color.Lerp(_flashColor, colors[i], (elapsedTime / (_flashDuration / 2)));
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Kill()
    {
        // Spawn explosion particles
        ParticleSystem deathParticles = Instantiate<ParticleSystem>(_explosion, transform);
        deathParticles.Play();

        AudioHelper.PlayClip2D(_playerDeathSound, 1f);

        StartCoroutine(Reset());
        _tankController.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        foreach(MeshRenderer mr in _tankArt)
        {
            mr.enabled = false;
        }
    }

    /*public void IncreaseTreasure(int amount)
    {
        _currentTreasure += amount;
        refreshUI();
        Debug.Log("Player's treasure: " + _currentTreasure);
    }*/

    /*public void refreshUI()
    {
        _ui.SetTreasureUI(_currentTreasure);
    }*/

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Reset Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
