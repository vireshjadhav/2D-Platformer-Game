using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
         if(other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("KeyCollected");

            StartCoroutine(DestroyAfterDelay(1f));
        }

        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
