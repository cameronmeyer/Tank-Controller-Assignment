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

    [SerializeField] GameObject _base;
    [SerializeField] GameObject _turret;
    [SerializeField] GameObject _turretPivot;

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
        TurnTurret();
    }

    public void MoveTank()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.position += move * _moveSpeed;

            _base.transform.rotation = Quaternion.LookRotation(move);
        }
        
        /*
        // calculate the move amount
        float moveAmountThisFrame = Input.GetAxis("Vertical") * _moveSpeed;
        // create a vector from amount and direction
        Vector3 moveOffset = transform.forward * moveAmountThisFrame;
        // apply vector to the rigidbody
        _rb.MovePosition(_rb.position + moveOffset);
        // technically adjusting vector is more accurate! (but more complex)
        */
    }

    private Vector3 temp;

    public void TurnTurret()
    {
        float distanceToPlayer = Vector3.Distance(Camera.main.transform.position, _turretPivot.transform.position) - .9f; // - 1.6f;
        Vector3 cameraPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToPlayer);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(cameraPoint);
        
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        
        _turretPivot.transform.rotation = Quaternion.LookRotation(localPoint, Vector3.up);
        //_turretPivot.transform.Rotate(-_turretPivot.transform.rotation.eulerAngles.x, 0, -_turretPivot.transform.rotation.eulerAngles.z, Space.World);
        //_turretPivot.transform.rotation.eulerAngles = new Vector3(0, _turretPivot.transform.rotation.eulerAngles.y, 0);


        //Debug gizmo--remove later
        temp = transform.TransformPoint(localPoint);
        Debug.Log(temp);

        /*
        // calculate the turn amount
        float turnAmountThisFrame = Input.GetAxis("Horizontal") * _turnSpeed;
        // create a Quaternion from amount and direction (x,y,z)
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountThisFrame, 0);
        // apply quaternion to the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
        */
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(temp, .1f);
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
