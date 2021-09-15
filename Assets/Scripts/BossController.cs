using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 0.25f;

    [SerializeField] GameObject _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _projectileMinSpeed = 3f;
    [SerializeField] float _projectileMaxSpeed = 10f;
    [SerializeField] float _projectileFullChargeTime = 2f;
    [SerializeField] float _projectileScaleFactor = 1f;
    private bool _chargingFire = false;
    private float _chargeStartTime = 0f;
    private float _chargeTime = 0f;
    [SerializeField] ParticleSystem _muzzleFlash;

    [SerializeField] GameObject _base;
    [SerializeField] GameObject _turret;
    [SerializeField] GameObject _turretPivot;

    [SerializeField] GameObject _ground;

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
    }

    private Vector3 temp;

    public void TurnTurret()
    {
        float distanceToTank = Vector3.Distance(Camera.main.transform.position, _turret.transform.position); 
        Vector3 cameraPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToTank);   
        Vector3 localPoint = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(cameraPoint));
        localPoint.y = _turret.transform.localPosition.y;

        _turretPivot.transform.rotation = Quaternion.LookRotation(localPoint, Vector3.up);
    }

    public void Fire()
    {
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
        {
            _chargingFire = true;
            _chargeStartTime = Time.time;
            _chargeTime = Time.time;
        }
        else if (_chargingFire && (Input.GetKeyUp("space") || Input.GetMouseButtonUp(0)))
        {
            _chargingFire = false;
            _chargeTime = Time.time;

            // calculate the % of full charge on the projectile
            float chargeAmount = (_chargeTime - _chargeStartTime) / ((_chargeStartTime + _projectileFullChargeTime) - _chargeStartTime);
            chargeAmount = Mathf.Clamp(chargeAmount, 0, 1);

            // calculate projectile speed based on chargeAmount
            // (remapping from {0,1} range to {minSpeed, maxSpeed} range)
            float projectileSpeed = _projectileMinSpeed + chargeAmount * (_projectileMaxSpeed - _projectileMinSpeed);

            GameObject projectile = Instantiate(_projectile, _projectileSpawn.transform.position, _projectileSpawn.transform.rotation);
            PlayerProjectile pProjectile = projectile.GetComponent<PlayerProjectile>();
            projectile.transform.localScale = projectile.transform.localScale * (1 + chargeAmount * _projectileScaleFactor);
            pProjectile.Speed = projectileSpeed;

            _muzzleFlash.Play();

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), _ground.GetComponent<Collider>());
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
