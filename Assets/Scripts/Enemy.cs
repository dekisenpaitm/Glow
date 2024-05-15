using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHitable
{
    [SerializeField]
    private string enemyName;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int damage;

    private Movement _player;
    private HumanoidAnimationController _anim;
    private Rigidbody _rb;
    private bool _hit;
    private Transform[] pois;
    public Transform _currentDestination;
    private bool _isIdling;

    public Collider hitCollider;


    void Start()
    {
        pois = FindObjectOfType<NavPointManager>().points;
        currentHealth = maxHealth;
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<HumanoidAnimationController>();
        _player = FindObjectOfType<Movement>(includeInactive: true);
        _currentDestination = pois[randomPoint()];
    }

    public int GetDamage()
    {
        return damage;
    }

    public void activateHitCollider()
    {
        hitCollider.enabled = !hitCollider.enabled;
    }

    private int randomPoint()
    {
        return Random.Range(0, pois.Length);
    }

    private void SetNewDestination()
    {
        float distanceToDestination = Vector3.Distance(new Vector3(_currentDestination.position.x, 0, _currentDestination.position.z), transform.position);
        if(distanceToDestination < 1f && !_isIdling)
        {
            StartCoroutine(reachedLocation());
        }
    }

    private void GoToPoint()
    {
        var step = 1 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _currentDestination.position, step);
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

        if (_hit) return;

        if (distance <= 10f && distance >= 1f)
        {
            _isIdling = false;
            var step = 1 * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.gameObject.transform.position.x, 0, _player.gameObject.transform.position.z), step);
            _anim.PlayRunning();
            RotateEnemy(_player.gameObject.transform);
        }
        else if (distance <= 1f)
        {
            _isIdling = false;
            _anim.PlayAttack();
        }
        else if (distance > 10f && !_isIdling)
        {
            _anim.PlayRunning();
            GoToPoint();
            RotateEnemy(_currentDestination);
        }
    }

    private IEnumerator reachedLocation()
    {
        _isIdling = true;
        _anim.PlayIdle();
        yield return new WaitForSeconds(2f);
        _currentDestination = pois[randomPoint()];
        _isIdling = false;
    }

    public void Execute(Transform executionSource)
    {
        StartCoroutine(ApplyKnockbackOverTime(executionSource, 0.5f, 70));
    }

    private IEnumerator ApplyKnockbackOverTime(Transform executionSource, float duration, float force)
    {
        Vector3 direction = (transform.position - executionSource.transform.position).normalized;
        _rb.velocity = Vector3.zero; // Ensure existing movement doesn't interfere
        _rb.AddForce(direction * force, ForceMode.Impulse); // Strong initial impulse

        float elapsed = 0;
        Hit();
        while (elapsed < duration)
        {
            _rb.AddForce(direction * (force * 0.1f * Time.deltaTime / duration), ForceMode.Impulse);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Hit()
    {
        if (!_hit)
        {
            _anim.PlayHit();
            StartCoroutine(GotHit());
        }
    }

    private IEnumerator GotHit()
    {
        _hit = true;
        yield return new WaitForSeconds(1f);
        _hit = false;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
