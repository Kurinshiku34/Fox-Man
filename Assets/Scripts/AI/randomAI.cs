using UnityEngine;

public class randomAI : MonoBehaviour
{
    public Transform movementpoint;
    private UnityEngine.AI.NavMeshAgent agent;
    public float minX; public float maxX;
    public float minY; public float maxY; 
    public float startTime; public float waitTime;
    public float speed;

    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        movementpoint.position = new Vector2 ( Random.Range(minX, maxX), Random.Range(minY, maxY) );
    }
    void Update() {
        transform.position = Vector2.MoveTowards ( transform.position, movementpoint.position, speed * Time.deltaTime );
        if (Vector2.Distance(transform.position, movementpoint.position) < 0.2f)
        {
            if (waitTime <= 0) { 
                movementpoint.position = new Vector2 ( Random.Range(minX, maxX), Random.Range(minY, maxY) );
                waitTime = startTime;
            }
            else { waitTime -= Time.deltaTime; }
        }
    }
}