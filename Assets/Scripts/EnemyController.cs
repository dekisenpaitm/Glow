using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    #region Attributes
    private int _speed;
    private bool _isTriggered;
    #endregion

    #region Referances
    private Movement _player;
    private HumanoidAnimationController _anim;
    public PlayerKnockback _knockback;
    private Enemy _enemy;
    #endregion

    #region Pathfinding
    private Transform[] _pois;
    public Transform _currentDestination;
    private bool _isIdling;
    #endregion

    void Start()
    {
        _enemy = GetComponent<Enemy>();
        _speed = _enemy.GetSpeed();
        _pois = FindObjectOfType<NavPointManager>().points;
        _anim = GetComponent<HumanoidAnimationController>();
        _player = FindObjectOfType<Movement>(includeInactive: true);
        _currentDestination = _pois[randomPoint()];
    }

    public void activateHitCollider()
    {
        _knockback.UseHand();
    }

    private int randomPoint()
    {
        return Random.Range(0, _pois.Length);
    }

    private void Triggered()
    {
        _isTriggered = true;
        AkSoundEngine.PostEvent("Play_zombie", gameObject);
    }

    private void SetNewDestination()
    {
        float distanceToDestination = Vector3.Distance(new Vector3(_currentDestination.position.x, 0, _currentDestination.position.z), transform.position);
        if(distanceToDestination < 1f && !_isIdling)
        {
            StartCoroutine(reachedLocation());
        }
    }

    private void GoToPoint(Transform target)
    {
        var step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, 0, target.position.z), step);
    }

    private void RotateEnemy(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 640 * Time.deltaTime);
    }

    void Update()
    {
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        SetNewDestination();

        float distance = Vector3.Distance(_player.gameObject.transform.position, transform.position);

        if (_enemy.Hit) return;

        if (distance <= 10f && distance >= 1f)
        {
            if (!_isTriggered)
            {
                Triggered();
            }
            _isIdling = false;
            GoToPoint(_player.transform);
            _anim.PlayRunning();
            RotateEnemy(_player.transform);
        }
        else if (distance <= 1f)
        {
            _isIdling = false;
            _anim.PlayAttack();
        }
        else if (distance > 10f && !_isIdling)
        {
            _isTriggered = false;
            _anim.PlayRunning();
            GoToPoint(_currentDestination);
            RotateEnemy(_currentDestination);
        }
    }

    private IEnumerator reachedLocation()
    {
        _isIdling = true;
        _anim.PlayIdle();
        yield return new WaitForSeconds(2f);
        _currentDestination = _pois[randomPoint()];
        _isIdling = false;
    }

    
}
