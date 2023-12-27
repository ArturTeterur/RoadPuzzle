using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private int _numberBalls;
    public float speed = 3f;
    //  private Transform[] currentWaypoints;
    [SerializeField] private List<Transform> currentWaypoints;
    private int currentWaypointIndex = 0;
    private bool obstacleDetected = false;
    private Rigidbody _rigidbody;
    private bool _itsIndex;
    private FinishTrigger _finishTrigger;
    [SerializeField] bool _finished = false;
    [SerializeField] private BallMovement _targetBall;

    public bool ItsIndex => _itsIndex;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _finishTrigger = FindObjectOfType<FinishTrigger>();
    }

    private void FixedUpdate()
    {
        if (obstacleDetected)
        {
            return;
        }

        if (currentWaypoints != null && currentWaypointIndex < currentWaypoints.Count)
        {
            if (Vector3.Distance(transform.position, currentWaypoints[currentWaypointIndex].position) <= 0.4f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= currentWaypoints.Count)
                {
                    obstacleDetected = true;
                    return;
                }
                Debug.Log("MoveToNextPoint(Update)");
                MoveToNextPoint();
            }
        }
        else
        {
            _rigidbody.AddForce(0, 0, -1 * speed);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<Wall>(out Wall wallComponent))
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            
        }
        if (collider.gameObject.TryGetComponent<FinishTrigger>(out FinishTrigger finishComponent))
        {
            if (!_finished)
            {
                finishComponent.CountBalls();
                _finished = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BallMovement>(out BallMovement ball))
        {
            if (ball._numberBalls == _numberBalls - 1)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
                _targetBall = ball;
                Debug.Log("FreezeBall " + _numberBalls);
                

            }
        }
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground))
        {
            _finishTrigger.TakeAwayBall();
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BallMovement>(out BallMovement ball))
        {        
                _rigidbody.constraints = RigidbodyConstraints.None;
                MoveToNextPoint();
                Debug.Log("OnCollissionExit");
        }
    }

    public void MoveToNextPoint()
    {
        Debug.Log(currentWaypointIndex + " " + _numberBalls);
        if (currentWaypoints != null && currentWaypointIndex < currentWaypoints.Count)
        { 
         //   currentWaypointIndex++;        
            Vector3 moveDerection = (currentWaypoints[currentWaypointIndex].position - transform.position).normalized;       
           _rigidbody.velocity = moveDerection * speed;
             if (Vector3.Distance(transform.position, currentWaypoints[currentWaypointIndex].position) <= 0.6f)
             {
                currentWaypointIndex++;
             }
             else
             {
                _rigidbody.AddForce(0, 0, -1 * speed);

             }
        }
    }

    public void Destroy()
    {
        _finishTrigger.TakeAwayBall();
        Destroy(gameObject);
    }

    public void MoveToWaypoints(List<Transform> waypoints) 
    {
        currentWaypoints = waypoints;
        currentWaypointIndex = 0;
        MoveToNextPoint();
    }

    public void KeepMoving()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;      
    }
     
    public int GetNumberBalls(int numberball)
    {
        _numberBalls = numberball;
        return numberball;      
    }
}
