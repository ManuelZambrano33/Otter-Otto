using System.Collections;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;

    void Start()
    {
        waypoints = new Transform[waypointParent.childCount];
        for (int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    void Update()
    {
        if (!isWaiting)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.05f) // margen pequeño
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        if (loopWaypoints)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        else
        {
            currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        }

        isWaiting = false;
    }
}