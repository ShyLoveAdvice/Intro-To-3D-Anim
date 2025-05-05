using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    private bool canWaitForIdle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }

        if (agent.velocity.magnitude < 0.1f && canWaitForIdle == true)
        {
            StartCoroutine("IdleAnimation");
        }
        if (agent.velocity.magnitude >= 0.1f)
        {
            StopCoroutine("IdleAnimation");
            canWaitForIdle = true;
        }
    }

    IEnumerator IdleAnimation()
    {
        canWaitForIdle = false;
        yield return new WaitForSeconds(Random.Range(0.4f, 3.2f));
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(Random.Range(6.2f, 12.2f));
        StartCoroutine("IdleAnimation");
    }
}
