using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    BossController _bossController;

    [SerializeField] GameObject _cover;

    [SerializeField] ParticleSystem _explosion;
    [SerializeField] AudioClip _bossDeathSound;

    private void Awake()
    {
        _bossController = GetComponent<BossController>();
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
        MeshRenderer[] tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in tankArt)
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
