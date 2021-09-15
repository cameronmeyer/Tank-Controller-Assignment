using UnityEngine;

public class HealthIncrease : CollectibleBase
{
    [SerializeField] int _healthAdded = 1;

    protected override void Collect(Player player)
    {
        player.GetComponent<Health>().IncreaseHealth(_healthAdded);
    }
}
