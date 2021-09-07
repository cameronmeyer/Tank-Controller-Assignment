using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    ParticleSystemRenderer psr;

    private void Awake()
    {
        psr = GetComponent<ParticleSystemRenderer>();
        psr.pivot = new Vector3(psr.pivot.x, 0.5f, psr.pivot.z);
    }
}
