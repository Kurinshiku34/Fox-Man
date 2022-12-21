using UnityEngine;

public class navmeshAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform run;
    public GameManager managergame;
    private UnityEngine.AI.NavMeshAgent agent;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        run = GameObject.FindGameObjectWithTag("run1").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update() {
        if (managergame.caneatghost == true) { agent.SetDestination(run.position); }
        else { agent.SetDestination(target.position); }
    }
}
