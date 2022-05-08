using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float speed;
    public bool moveOnTrigger;
    public bool useWaypoints;
    public bool reverseAtLastWayPoint;
    public bool stopAtLastWaypoint;
    public float waypointRestSecs;
    public GameObject[] waypoints;
    public Vector3 moveVector;

    int currWaypoint = 0;
    float waypointRestTimeCancel = 0f;
    float currTime = 0f;
    bool moving = false;
    bool moveInReverse = false;
    bool cutscenePlaying = false;

    public void OnEnable()
    {
        Messenger<bool>.AddListener(GameEvent.TRANSITION, OnTransition);
    }

    public void OnDisable()
    {
        Messenger<bool>.RemoveListener(GameEvent.TRANSITION, OnTransition);
    }

    void Start()
    {
        if (speed == 0.0f)
        {
            speed = 1f;
        }

        if (useWaypoints && waypoints == null)
        {
            Debug.Log("Waypoints should not be empty.");
        }

        if (!moveOnTrigger)
        {
            moving = true;
        }
    }

    void Update()
    {
        if (cutscenePlaying)
            return;
            
        float timeSpeed = speed * Time.deltaTime;
        currTime += Time.deltaTime;

        if (useWaypoints && currTime > waypointRestTimeCancel)
        {
            int lastWaypointIndex = waypoints.Length - 1;
            if (Vector3.Distance(transform.position, waypoints[currWaypoint].transform.position) == 0)
            {
                if (waypointRestSecs > 0)
                {
                    waypointRestTimeCancel = currTime + waypointRestSecs;
                }

                if (currWaypoint == lastWaypointIndex)
                {
                    if (stopAtLastWaypoint)
                    {
                        moving = false;
                    }

                    if (reverseAtLastWayPoint)
                    {
                        moveInReverse = true;
                    }

                }

                if (moveInReverse)
                {
                    currWaypoint = currWaypoint == 0 ? 1 : currWaypoint - 1;
                    moveInReverse = currWaypoint != 0;
                }
                else
                {
                    currWaypoint = currWaypoint == lastWaypointIndex ? 0 : currWaypoint + 1;
                }
            }

            if (moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currWaypoint].transform.position, timeSpeed);
            }
        }

        else
        {
            Vector3 transformPosition = new Vector3(moveVector.x * timeSpeed, moveVector.y * timeSpeed, moveVector.z * timeSpeed);
            transform.Translate(transformPosition);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //stops player from falling off of the moving platform by setting transform equal to the moving objects transform
        if (other.gameObject.tag == "Player" && other.transform.parent == null)
        {
            //Debug.Log("Player is on platform.");
            moving = true;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Player has left platform.");
            if (moveOnTrigger)
            {
                moving = false;
            }
            other.transform.parent = null;
        }
    }

    private void OnTransition(bool cutsceneActive)
    {
        cutscenePlaying = cutsceneActive;
    }
}
