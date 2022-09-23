using FateGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour, IPooledObject
{
    [SerializeField] protected Animator animator = null;
    protected bool isReached = true;
    protected bool isRotating = false;
    protected NavMeshAgent agent;
    protected Transform _transform;
    protected UnityEvent onReached = new UnityEvent();
    public Vector3 Destination { get => agent.destination; }
    public Transform Transform { get => _transform; }

    protected virtual void Awake()
    {
        _transform = transform;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }
    protected virtual void Update()
    {
        //TODO bu iþlemin costu fazla. sqrmagnitude kullanýlabilir ve tek bir gameobjectte tüm agentlar için yapýlabilir
        if (animator)
        {
            animator.SetFloat("Velocity", agent.velocity.magnitude);
        }
        //TODO her bir customerda yapmak yerine bunu her bir customer için yapan tek bir obje olacak
        if (isReached == false && isRotating == false && agent.ReachedDestinationOrGaveUp())
        {
            isReached = true;
            onReached.Invoke();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        isReached = false;
        agent.SetDestination(destination);
    }

    public void SetDestination(Transform destinationPoint)
    {
        SetDestination(destinationPoint.position, destinationPoint.rotation);
    }

    public void SetDestination(Vector3 destination, Quaternion rotation)
    {
        SetDestination(destination);
        onReached.AddOneTimeListener(() =>
        {
            isRotating = true;
            _transform.DORotateQuaternion(rotation, 0.2f).OnComplete(() => { isRotating = false; });
        });
    }

    public void OnObjectSpawn()
    {
        agent.enabled = true;
        onReached.RemoveAllListeners();
    }
}
