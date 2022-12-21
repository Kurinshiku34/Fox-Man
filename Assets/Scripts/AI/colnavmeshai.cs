using UnityEngine;

public class colnavmeshai : MonoBehaviour
{
    [SerializeField] Transform bluebackk;
    [SerializeField] Transform run2;
    private Transform target;
    public GameManager managergame;
    private UnityEngine.AI.NavMeshAgent agent;
    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        run2 = GameObject.FindGameObjectWithTag("run2").transform;
        bluebackk = GameObject.FindGameObjectWithTag("blueback").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Update() {
        float distance = Vector2.Distance(transform.position, target.position);
        if (managergame.caneatghost == true) { agent.SetDestination(run2.position); }
        else  { 
            if (distance < 4f) { agent.SetDestination(target.position); }
            else { agent.SetDestination(bluebackk.position); } 
        }
    }
}