using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [SerializeField] Player _player;

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

    [SerializeField] GameObject _patrolPositionsObj;
    private List<Vector3> _patrolPositions;
    private float _timeToNextMovement;
    private Vector3 _nextLocation;
    private bool _isPatroling = false;
    private bool _isRushing = false;

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

    private void Start()
    {
        _patrolPositions = new List<Vector3>();
        foreach(Transform child in _patrolPositionsObj.transform)
        {
            _patrolPositions.Add(child.position);
        }
        
    }

    private void Update()
    {
        Fire();
    }

    private void FixedUpdate()
    {
        Movement();
        TurnTurret();
    }

    private void Movement()
    {
        // make sure it's time to do a new movement
        if(Time.time >= _timeToNextMovement)
        {
            _isPatroling = false;
            _isRushing = false;

            // determine whether we will be idle (0), patroling (1), or rushing (2)
            int movementAction = Random.Range(0, 2);

            switch (movementAction)
            {
                case 0:
                    _nextLocation = transform.position;
                    Debug.Log("Idle Now");
                    break;
                case 1:
                    _nextLocation = _patrolPositions[Random.Range(0, _patrolPositions.Count)];
                    _isPatroling = true;
                    Debug.Log("Patroling Now");
                    break;
                case 2:
                    _nextLocation = _player.transform.position;
                    _isRushing = true;
                    Debug.Log("Rushing Now");
                    break;
            }

            // randomly pick how long we will perform the movement action for
            _timeToNextMovement = Time.time + Random.Range(1.2f, 3f);
        }

        MoveTank();
    }

    private void MoveTank()
    {
        Vector3 move = _nextLocation - transform.position;

        //Debug.Log(move);

        if(move.magnitude <= 0.1f) { return; } // don't move if we're already basically in position

        move.Normalize();

        transform.position += move * _moveSpeed;

        _base.transform.rotation = Quaternion.LookRotation(move);
    }

    private void TurnTurret()
    {
        _turretPivot.transform.rotation = Quaternion.LookRotation((_player.transform.position - transform.position), Vector3.up);
    }

    private void Fire()
    {/*
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
        }*/
    }
}
