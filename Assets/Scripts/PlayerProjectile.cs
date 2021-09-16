using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private float _speed;
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;
    [SerializeField] AudioClip _projectileFire;

    private CinemachineShake _cs;
    [SerializeField] float _shakeIntensity = 3f;
    [SerializeField] float _shakeTimer = 0.1f;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private void Awake()
    {
        AudioHelper.PlayClip2D(_projectileFire, 1f);
        _cs = Camera.main.GetComponent<CinemachineShake>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        PlayerProjectile otherProjectile = other.gameObject.GetComponent<PlayerProjectile>();

        if(otherProjectile != null)
        {
            Physics.IgnoreCollision(otherProjectile.GetComponent<Collider>(), GetComponent<Collider>());
            return;
        }

        IDamageable damageableObj = other.gameObject.GetComponent<IDamageable>();
        if(damageableObj != null)
        {
            damageableObj.Damage(1);
        }

        ImpactFeedback(Quaternion.LookRotation(other.contacts[0].normal));

        Destroy(gameObject);
    }

    void ImpactFeedback(Quaternion impactRotation)
    {
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles, transform.position, impactRotation);
            _impactParticles.Play();
        }

        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }

        _cs.Shake(_shakeIntensity, _shakeTimer);
    }
}
