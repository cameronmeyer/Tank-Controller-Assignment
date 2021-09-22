using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PowerUpBase : MonoBehaviour
{
    [SerializeField] protected float _powerupDuration = 5f;
    [SerializeField] ParticleSystem _powerupParticles;
    [SerializeField] AudioClip _powerupSound;

    protected abstract void PowerUp(Player player);
    protected abstract void PowerDown(Player player);

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            MeshRenderer powerupRenderer = gameObject.GetComponent<MeshRenderer>();
            powerupRenderer.enabled = false;

            PowerUp(player);
            Feedback();
            StartCoroutine(PoweredUp(player));
        }
    }

    private void Feedback()
    {
        // particles
        if (_powerupParticles != null)
        {
            _powerupParticles = Instantiate(_powerupParticles, transform.position, Quaternion.identity);
            _powerupParticles.Play();
        }

        // audio
        // TODO: consider object pooling for performance
        if (_powerupParticles != null)
        {
            AudioHelper.PlayClip2D(_powerupSound, 1f);
        }
    }

    private IEnumerator PoweredUp(Player player)
    {
        yield return new WaitForSeconds(_powerupDuration);
        PowerDown(player);
        gameObject.SetActive(false);
    }
}
