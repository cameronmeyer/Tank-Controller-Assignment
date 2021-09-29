using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    BossController _bossController;
    MeshRenderer[] _tankArt;
    List<Color> colors = new List<Color>();

    [SerializeField] GameObject _cover;

    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _bossDeathSound;

    [SerializeField] float _flashDuration = 0.5f;
    [SerializeField] Color _flashColor;

    private void Awake()
    {
        _bossController = GetComponent<BossController>();
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
        ParticleSystem deathParticles = Instantiate<ParticleSystem>(_explosion, _cover.transform);
        deathParticles.Play();

        AudioHelper.PlayClip2D(_bossDeathSound, 1f);

        _bossController.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        foreach (MeshRenderer mr in _tankArt)
        {
            mr.enabled = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Boss collide with " + other.gameObject.name);

        IDamageable damageableObj = other.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null)
        {
            damageableObj.Damage(1);
        }
    }
}
