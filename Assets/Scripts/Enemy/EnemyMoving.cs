using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoving : MonoBehaviour
{
    public PlayerController PCon;
    public float fov = 120f;
    public float viewDistance = 10f;

    [SerializeField] float StopDistance = 2.0f;
    [SerializeField] Transform Target1;
    [SerializeField] Transform Target2;
    [SerializeField] Transform Target3;

    private bool isAware = false;
    private NavMeshAgent agent;
    private Transform TheTarget;
    private float DistanceToTarget = 1;
    private int TargetNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        TheTarget = Target1;
    }

    // Update is called once per frame
    void Update()
    {
        TargetNumber = UnityEngine.Random.Range(1, 4);
        if (isAware)
        {
            agent.SetDestination(PCon.transform.position);
        }
        else
        {
            DistanceToTarget = Vector3.Distance(TheTarget.position, transform.position);
            SearchForPlayer();
        }

    }

    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward,transform.InverseTransformPoint(PCon.transform.position)) < fov / 2f)
        {
            if(Vector3.Distance(PCon.transform.position,transform.position) < viewDistance)
            {
                RaycastHit hit;
                if(Physics.Linecast(transform.position, PCon.transform.position,out hit, -1))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    
                }
            }
        }
        else if(DistanceToTarget > StopDistance)
        {
            agent.SetDestination(TheTarget.position);
        }
        else if (DistanceToTarget< StopDistance)
        {
            SetTarget();
        }
    }

    public void OnAware()
    {
        isAware = true;
        agent.speed = 4f;
    }

    private void SetTarget()
    {
        if(TargetNumber == 1)
        {
            TheTarget = Target1;
        }
        if(TargetNumber == 2)
        {
            TheTarget = Target2;
        }
        if(TargetNumber == 3)
        {
            TheTarget = Target3;
        }
    }
}
