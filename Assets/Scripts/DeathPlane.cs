using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Death Plane Entered");
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.Kill();
        }
    }
}
