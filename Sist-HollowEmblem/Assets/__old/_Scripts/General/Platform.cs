using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Collider2D boxCollider;
    public bool playerCollides;
    public float distanceToPlayerTolerated=1.88f;
    private IEnumerator reestablishingCollider;
    float auxTimer;
    private void Start()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(playerCollides)
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                if (Input.GetKeyDown(KeyCode.Space)&& boxCollider.enabled == true && playerCollides)
                {
                    Legacy_PlayerInventory.Instance.TryGetComponent<Animator>(out var a);
                    a.SetBool("Falling",true);
                    boxCollider.enabled = false;
                }
            }
        }
        auxTimer = !boxCollider.enabled && auxTimer > 0 ? auxTimer - Time.deltaTime : 2;

        if (auxTimer==2)
        {
            boxCollider.enabled = true;
        }
    }

    IEnumerator ReestablishCollider()
    {
        yield return new WaitForSeconds(1f);
        playerCollides = false;
        boxCollider.enabled = true;
        reestablishingCollider = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&& !playerCollides)
        {
            //PlayerSnap(collision);
            //SI SE PONE MUY JODIDO QUE LOS CHABONES LE EMBOQUEN A LAS PLATAFORMAS, DESCOMENTAR ESTO
        }
    }

    public void PlayerSnap(Collision2D collision)
    {
        print("PlayerOverPlat");
        playerCollides = true;
        float distance = (Vector2.Distance(new Vector3(transform.position.x, transform.position.y), new Vector3(transform.position.x, collision.transform.position.y, 0)));
        Vector2 dir = new Vector3(transform.position.x, collision.transform.position.y, 0) - new Vector3(transform.position.x, transform.position.y);
        //Debug.Log(distance);

        if (0.5f > distance && reestablishingCollider == null && dir.y < 0)
        {
            boxCollider.enabled = false;
            reestablishingCollider = ReestablishCollider();
            StartCoroutine(reestablishingCollider);
        }
        else if (distance < 1.87f)
        {
            collision.gameObject.transform.position += new Vector3(0, (distanceToPlayerTolerated - distance));
            collision.rigidbody.velocity = new Vector3(collision.rigidbody.velocity.x, 0);
        }
    }
    public void PlayerSnap(Collider2D collision)
    {
        print("PlayerOverPlat");
        playerCollides = true;
        float distance = (Vector2.Distance(new Vector3(transform.position.x, transform.position.y), new Vector3(transform.position.x, collision.transform.position.y, 0)));
        Vector2 dir = new Vector3(transform.position.x, collision.transform.position.y, 0) - new Vector3(transform.position.x, transform.position.y);
        //Debug.Log(distance);

        if (0.5f > distance && reestablishingCollider == null && dir.y < 0)
        {
            boxCollider.enabled = false;
            reestablishingCollider = ReestablishCollider();
            StartCoroutine(reestablishingCollider);
        }
        else if (distance < 1.87f)
        {
            collision.gameObject.transform.position += new Vector3(0, (distanceToPlayerTolerated - distance));
            collision.TryGetComponent<Rigidbody2D>(out var playerRB);
            playerRB.velocity = new Vector3(playerRB.velocity.x, 0);
    

            // collision.rigidbody.velocity = new Vector3(collision.rigidbody.velocity.x, 0);

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            playerCollides = false;
            Vector2 dir =   new Vector3(transform.position.x, collision.transform.position.y, 0)- new Vector3(transform.position.x, transform.position.y);
            if (dir.y<0 )
            {
                StartCoroutine(ReestablishCollider());
            }
       
        }
    }
}
