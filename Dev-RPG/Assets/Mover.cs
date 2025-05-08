using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    Vector3 target;
    NavMeshAgent agent;
    Ray targetRay;
    Camera targetCamera;
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Movement();
        }

        UpdateAnimation();

    }

    private void UpdateAnimation()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        animator.SetFloat("speed", localVelocity.z);
    }

    private void Movement()
    {
        if (Physics.Raycast(targetCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            agent.SetDestination(hit.point);

        }
    }
}
