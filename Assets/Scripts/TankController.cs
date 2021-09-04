using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 0.25f;
    [SerializeField] float _turnSpeed = 2f;

    [SerializeField] GameObject _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _projectileMinSpeed = 3f;
    [SerializeField] float _projectileFullChargeTime = 2f;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Exit();
        Reset();
        Fire();
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTank();
    }

    public void MoveTank()
    {
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
    }

    public void TurnTank()
    {
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    public void Fire()
    {
        // TODO: convert this into a charge shot mechanic. get time button first held, then if held for _projectileFullChargeTime,
        // launch projectile at max speed
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            Instantiate(_projectile, _projectileSpawn.transform.position, _projectileSpawn.transform.rotation);
        }
    }

    public void Exit()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
    }

    public void Reset()
    {
        if(Input.GetKeyDown("backspace"))
        {
            Debug.Log("Reset Scene");
            SceneManager.LoadScene("Sandbox");
        }
    }
}
