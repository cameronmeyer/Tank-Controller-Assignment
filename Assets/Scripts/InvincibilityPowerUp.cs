using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityPowerUp : PowerUpBase
{
    private MeshRenderer[] _playerRenderers;
    private Material[] _playerMats;
    [SerializeField] Material _powerUpMaterial;

    protected override void PowerUp(Player player)
    {
        _playerRenderers = player.GetComponentsInChildren<MeshRenderer>();
        _playerMats = new Material[_playerRenderers.Length];

        // Swap to powered up material
        for(int i = 0; i < _playerRenderers.Length; i++)
        {
            _playerMats[i] = _playerRenderers[i].material;
            _playerRenderers[i].material = _powerUpMaterial;
        }

        player.GetComponent<Health>().PowerUp();
    }

    protected override void PowerDown(Player player)
    {
        // Revert materials
        for (int i = 0; i < _playerRenderers.Length; i++)
        {
            _playerRenderers[i].material = _playerMats[i];
        }

        player.GetComponent<Health>().PowerDown();
    }
}
