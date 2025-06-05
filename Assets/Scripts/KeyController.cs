using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private Animator animator;

<<<<<<< .merge_file_KGy5SD
=======

>>>>>>> .merge_file_WTSD2K
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

<<<<<<< .merge_file_KGy5SD
    private void OnCollisionEnter2D(Collision2D collision)
    {
         if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerContoller = collision.gameObject.GetComponent<PlayerController>();
            playerContoller.PickUpKey();
            animator.SetTrigger("KeyCollected");

            StartCoroutine(DestroyAfterDelay(1f));

=======
    private void OnCollisionEnter2D(Collision2D other)
    {
         if(other.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("KeyCollected");

            StartCoroutine(DestroyAfterDelay(1f));
>>>>>>> .merge_file_WTSD2K
        }

        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
<<<<<<< .merge_file_KGy5SD

=======
>>>>>>> .merge_file_WTSD2K
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

}
