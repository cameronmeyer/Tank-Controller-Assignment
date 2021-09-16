using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    BossController _bossController;

    [SerializeField] AudioClip _bossDeathSound;

    private void Awake()
    {
        _bossController = GetComponent<BossController>();
    }

    public void Kill()
    {
        AudioHelper.PlayClip2D(_bossDeathSound, 1f);

        _bossController.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        MeshRenderer[] tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in tankArt)
        {
            mr.enabled = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Boss collide with player");
        IDamageable damageableObj = other.gameObject.GetComponent<IDamageable>();
        if (damageableObj != null)
        {
            damageableObj.Damage(1);
        }
    }
}
