using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemyController : MonoBehaviour
{
    public PathPoint[] points;
    public bool loopPath;
    private int index;
    private NavMeshAgent agent;
    private bool waiting;
    private bool start;
    // Does the ray intersect any objects excluding the player layer


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        index = 0;
        start = true;
        StartCoroutine(GotoNextPoint());
    }


    IEnumerator GotoNextPoint()
    {

        if (!start)
        {
            waiting = true;

            yield return new WaitForSecondsRealtime(points[index].timeToWaitAtDestination);

            index += 1;
            if (index >= points.Length && loopPath)
                index = 0;
            else if (!loopPath)
                yield break;

            waiting = false;
        }
        else
        {
            start = false;
        }

        agent.SetDestination(points[index].destination.position);

    }


    void Update()
    {
        if (points.Length == 0 && !waiting)
        {
            Debug.Log("Did you forget to add a path point?");
            waiting = true;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.02f && !waiting)
            StartCoroutine(GotoNextPoint());
    }
}

[System.Serializable]
public class PathPoint
{
    public float timeToWaitAtDestination;
    public Transform destination;
}