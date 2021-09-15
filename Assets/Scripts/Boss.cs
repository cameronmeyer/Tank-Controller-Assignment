using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    BossController _bossController;

    private void Awake()
    {
        _bossController = GetComponent<BossController>();
    }

    private void Update()
    {
        
    }

    public void Kill()
    {
        _bossController.enabled = false;
        MeshRenderer[] tankArt = gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in tankArt)
        {
            mr.enabled = false;
        }
    }
}
