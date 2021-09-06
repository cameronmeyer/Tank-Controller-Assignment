using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _maxSpeed = 10f;

    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Clamp(value, 1f, _maxSpeed);
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
    }
}
