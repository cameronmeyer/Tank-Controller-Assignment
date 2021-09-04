using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _maxSpeed = 10f;

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Clamp(value, 1f, _maxSpeed);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);    
    }
}
