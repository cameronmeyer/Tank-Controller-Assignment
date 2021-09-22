using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] float _speedAmount = 5;
    protected override void PlayerImpact(Player player)
    {
        TankController controller = player.GetComponent<TankController>();
        if (controller != null)
        {
            controller.MoveSpeed *= _speedAmount;
        }
    }
}
