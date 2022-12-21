using UnityEngine;

public class PointAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform[] positions;
    Transform nextPosition;
    int nextPosIndex;
    public float moveSpeed;

    void Start() {
        nextPosition = positions[0];
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update() {
        if (transform.position == nextPosition.position)
        {
            nextPosIndex++;
            if (nextPosIndex >= positions.Length) { nextPosIndex = 0; }
            nextPosition = positions[nextPosIndex];
        }
        transform.position = Vector3.MoveTowards(transform.position,nextPosition.position, moveSpeed * Time.deltaTime);
    }
}
