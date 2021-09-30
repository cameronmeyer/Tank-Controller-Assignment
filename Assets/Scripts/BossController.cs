using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [SerializeField] Player _player;

    [SerializeField] float _moveSpeed = 0.25f;
    [SerializeField] float _rushSpeed = 0.5f;

    [SerializeField] GameObject _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] AudioClip _projectileFire;
    [SerializeField] AudioClip _laserFire;
    private bool _canFire = true;
    [SerializeField] float _fireDelay = 1f;
    private float _timeLastFired = 0f;
    [SerializeField] float _multishotSpread = 45f;
    [SerializeField] int _multishotCount = 5;
    [SerializeField] float _multishotDistance = 2f;

    [SerializeField] GameObject _base;
    [SerializeField] GameObject _turret;
    [SerializeField] GameObject _turretPivot;

    [SerializeField] GameObject _ground;

    [SerializeField] Transform LowerLeftBound;
    [SerializeField] Transform UpperRightBound;
    [SerializeField] float _telegraphDuration = 1f;
    private float _timeToNextMovement;
    private Vector3 _nextLocation;
    private bool _isPatroling = false;
    private bool _isRushing = false;
    private bool _isTelegraphing = false;
    private bool _isMultishot = false;
    private bool _isLaser = false;

    [SerializeField] ParticleSystem _rushParticles;
    [SerializeField] ParticleSystem _rushTelegraphParticles;
    [SerializeField] ParticleSystem _multishotTelegraphParticles;
    [SerializeField] AudioClip _multishotTelegraphSound;
    [SerializeField] ParticleSystem _laserParticles;
    [SerializeField] ParticleSystem _laserTelegraphParticles;
    [SerializeField] AudioClip _laserTelegraphSound;

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

    private void FixedUpdate()
    {
        // ensure tank is flat on the ground
        _base.transform.rotation = Quaternion.Euler(0,
                                                    _base.transform.eulerAngles.y,
                                                    _base.transform.eulerAngles.z);
        Movement();
        TurnTurret();
        Fire();
        Feedback();
    }

    private void Movement()
    {
        // make sure it's time to do a new movement
        if(Time.time >= _timeToNextMovement)
        {
            _isPatroling = false;
            _isRushing = false;

            // determine whether we will be idle (0), patroling (1), or rushing (2)
            int movementAction = 0;// = Random.Range(0, 3);
            float actionProbability = Random.value;
            if(actionProbability <= 0.5f) { movementAction = 0; }
            else if(actionProbability <= 0.9f) { movementAction = 1; }
            else { movementAction = 2; }

            switch (movementAction)
            {
                case 0: // IDLE
                    _nextLocation = transform.position;
                    _timeToNextMovement = Time.time + Random.Range(1.2f, 3f);
                    Debug.Log("Idle Now");
                    break;
                case 1: // PATROL
                    _nextLocation = new Vector3(UnityEngine.Random.Range(LowerLeftBound.position.x, UpperRightBound.position.x),
                                                UnityEngine.Random.Range(LowerLeftBound.position.y, UpperRightBound.position.y),
                                                UnityEngine.Random.Range(LowerLeftBound.position.z, UpperRightBound.position.z));
                    _timeToNextMovement = Time.time + Random.Range(1.5f, 2.2f);
                    _isPatroling = true;
                    StartCoroutine(rotateBase((_nextLocation - transform.position), _telegraphDuration));
                    Debug.Log("Patroling Now");
                    break;
                case 2: // RUSH
                    _nextLocation = _player.transform.position;
                    _timeToNextMovement = Time.time + 3.2f;
                    _isRushing = true;
                    StartCoroutine(rotateBase((_nextLocation - transform.position), _telegraphDuration));
                    Debug.Log("Rushing Now");
                    break;
            }
        }

        MoveTank();
    }

    private void MoveTank()
    {
        Vector3 move = _nextLocation - transform.position;
        Vector3 normMove = move;
        normMove.Normalize();

        float currentMoveSpeed;
        if(_isRushing) { currentMoveSpeed = _rushSpeed; }
        else { currentMoveSpeed = _moveSpeed; }

        // don't move the object if it's already within the position threshold
        if (move.magnitude <= (normMove * currentMoveSpeed).magnitude) // if the distance vector is less than the amount we can move in 1 frame
        {
            if (_isPatroling && !_isTelegraphing) // calculate a new point to patrol to
            {
                _nextLocation = new Vector3(UnityEngine.Random.Range(LowerLeftBound.position.x, UpperRightBound.position.x),
                                            UnityEngine.Random.Range(LowerLeftBound.position.y, UpperRightBound.position.y),
                                            UnityEngine.Random.Range(LowerLeftBound.position.z, UpperRightBound.position.z));
                StartCoroutine(rotateBase((_nextLocation - transform.position), _telegraphDuration));
            }
            else if(_isRushing && !_isTelegraphing)
            {
                _isRushing = false; // the rush towards the player has been completed within the time limit
                return;
            }
            else
            {
                return; // don't move if we're already basically in position
            }
        }
        else if (!_isTelegraphing && (_isPatroling || _isRushing)) // actually translate and rotate the object if it's supposed to move
        {
            transform.position += normMove * currentMoveSpeed;

            StartCoroutine(rotateBase(move, 0));
        }
    }

    private IEnumerator rotateBase(Vector3 direction, float duration)
    {
        if (duration == 0f)
        {
            _base.transform.rotation = Quaternion.LookRotation(direction);
            yield return null;
        }

        _timeToNextMovement += duration;

        float elapsedTime = 0;
        Quaternion initValue = _base.transform.rotation;
        Quaternion target = Quaternion.LookRotation(direction);

        Vector3 tempNextLocation = _nextLocation;
        _nextLocation = transform.position;

        while (elapsedTime < duration)
        {
            _isTelegraphing = true;
            _base.transform.rotation = Quaternion.Slerp(
                                            initValue,
                                            target,
                                            (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _isTelegraphing = false;
        _nextLocation = tempNextLocation;
    }

    private void TurnTurret()
    {
        _turretPivot.transform.rotation = Quaternion.LookRotation((_player.transform.position - transform.position), Vector3.up);
    }

    private void Feedback()
    {
        if(_isRushing && _isTelegraphing && !_rushTelegraphParticles.isPlaying)
        {
            _rushTelegraphParticles.Play();
        }
        else if(_isRushing && !_isTelegraphing)
        {
            if(_rushTelegraphParticles.isPlaying) { _rushTelegraphParticles.Stop(); }
            if(!_rushParticles.isPlaying) { _rushParticles.Play(); }
        }
        else if(_isMultishot && _isTelegraphing)
        {
            if (!_multishotTelegraphParticles.isPlaying)
            {
                _multishotTelegraphParticles.Play();
                if (_multishotTelegraphSound != null)
                {
                    AudioHelper.PlayClip2D(_multishotTelegraphSound, 1f);
                }
            }
        }
        else if (_isLaser && _isTelegraphing)
        {
            if (!_laserTelegraphParticles.isPlaying)
            {
                _laserTelegraphParticles.Play();
                if (_laserTelegraphSound != null)
                {
                    AudioHelper.PlayClip2D(_laserTelegraphSound, 1f);
                }
            }
        }

        if (!_isRushing)
        {
            if (_rushParticles.isPlaying) { _rushParticles.Stop(); }
        }
        
        if (_isMultishot && !_isTelegraphing)
        {
            if (_multishotTelegraphParticles.isPlaying) { _multishotTelegraphParticles.Stop(); }
        }

        if (_isLaser && !_isTelegraphing)
        {
            if (_laserTelegraphParticles.isPlaying) { _laserTelegraphParticles.Stop(); }
        }
    }

    private void Fire()
    {
        // allow firing if we've waited through the delay
        if(!_canFire && Time.time >= (_timeLastFired + _fireDelay) && !_isMultishot) { _canFire = true; }

        // potentially perform attack if idle and can fire
        if (!_isPatroling && !_isRushing && !_isTelegraphing && _canFire)
        {
            float fireAction = Random.value;
            if (fireAction <= 0.5f)
            {
                _canFire = false;
                _isTelegraphing = true;
                _isMultishot = true;
                StartCoroutine(MultiShot(_projectileSpawn.transform.rotation.eulerAngles.y, _multishotCount, _multishotSpread, _projectileSpawn.transform.position));
            }
            else if(fireAction <= 0.85f)
            {
                _canFire = false;
                _isTelegraphing = true;
                _isLaser = true;
                StartCoroutine(Laser());
            }
        }
    }

    private IEnumerator Laser()
    {
        float elapsedTime = 0;
        float duration = _telegraphDuration / 3;
        _timeToNextMovement += duration;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            Debug.DrawRay(_turret.transform.position, transform.TransformDirection(Vector3.back) * 5, Color.red);
            yield return new WaitForEndOfFrame();
        }

        _timeLastFired = Time.time;
        if (_laserFire != null)
        {
            AudioHelper.PlayClip2D(_laserFire, 1f);
        }

        if (!_laserParticles.isPlaying)
        {
            _laserParticles.Play();
        }



        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_turret.transform.position, transform.TransformDirection(Vector3.back), out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            Debug.Log("hit object");
            Debug.DrawRay(_turret.transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
            IDamageable damageableObj = hit.transform.gameObject.GetComponent<IDamageable>();
            if (damageableObj != null)
            {
                damageableObj.Damage(1);
            }
        }

        _isLaser = false;
    }

    private IEnumerator MultiShot(float rotation, int numProjectiles, float spread, Vector3 position)
    {
        // wait for multishot telegraph to complete
        float elapsedTime = 0;
        float duration = _telegraphDuration / 3;
        _timeToNextMovement += duration;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        float angleOffset = spread / numProjectiles; // find the angle between each projectile
        float startRotation = rotation - (spread / 2);
        
        for (float rot = startRotation; rot < startRotation + spread; rot += angleOffset)
        {
            GameObject projectile = Instantiate(_projectile, position, Quaternion.AngleAxis(rot, Vector3.up));
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), _ground.GetComponent<Collider>());
            projectile.transform.Translate(Vector3.forward * _multishotDistance);
        }

        _timeLastFired = Time.time;
        _muzzleFlash.Play();
        if (_projectileFire != null)
        {
            AudioHelper.PlayClip2D(_projectileFire, 1f);
        }

        _isMultishot = false;
    }
}
