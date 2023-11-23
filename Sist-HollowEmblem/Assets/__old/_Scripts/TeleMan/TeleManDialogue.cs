using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleManDialogue : MonoBehaviour
{
    [Header("Classes")]
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;
    public PlayerAbilities playerAbilities;

    [Header("Bools")]
    public bool canInteract;
    public bool canTalk;
    public bool nearEnd;

    [Header("GameObjects")]
    public GameObject target;
    public GameObject newTarget;
    public GameObject textPosition;
    public GameObject textTutorial;

    public Transform secondPosition;
    public Transform thirdPosition;

    public Text telemanText;

    public Rigidbody2D rb;

    public float cooldown = 0.5f;

    public int textChanger = 0;

    private Animator animator;

    [SerializeField] AudioSource talkAudio;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCombat = FindObjectOfType<PlayerCombat>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }
    private void Update()
    {
        PositionText();
        CooldownUpdate();
        ChangeText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerCombat.canShoot && !playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            telemanText.text = "Hi There!";

            textTutorial.SetActive(true);
            talkAudio.Play();
        }

        if (collision.gameObject.CompareTag("Player") && playerCombat.canShoot && !playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            telemanText.text = "Hey, what do you have there?";
            textTutorial.SetActive(true);
            talkAudio.Play();
        }

        if (collision.gameObject.CompareTag("Player") && playerCombat.canShoot && playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            telemanText.text = "Where is the charging beast?";
            textTutorial.SetActive(true);
            talkAudio.Play();
        }

        if (collision.gameObject.CompareTag("Player") && playerCombat.canShoot && playerMovement.dashUnlocked && playerAbilities.slamUnlocked && !nearEnd)
        {
            telemanText.text = "Have you killed that slammer monster?";
            textTutorial.SetActive(true);
            talkAudio.Play();
        }

        if (collision.gameObject.CompareTag("Player") && playerCombat.canShoot && playerMovement.dashUnlocked && playerAbilities.slamUnlocked && nearEnd)
        {
            telemanText.text = "Hey!";
            textTutorial.SetActive(true);
            talkAudio.Play();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)  //Can´t detect inputs
    {
        canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            telemanText.text = "";
            textChanger = 0;
            canInteract = false;
            canTalk = false;
            textTutorial.SetActive(false);
            animator.SetBool("isTalking", false);
            target.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    void CooldownUpdate()
    {
        cooldown -= Time.deltaTime;

        if (cooldown < 0)
        {
            cooldown = 0;
        }
    }

    void PositionText()
    {
        telemanText.transform.position = new Vector2(textPosition.transform.position.x, textPosition.transform.position.y);
    }

    void ChangeText()
    {
        if (!playerCombat.canShoot && !playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract && !canTalk && cooldown <= 0 && textChanger == 0)
            {
                textTutorial.SetActive(false);
                canTalk = false;
                telemanText.text = "You´re not the first one who has entered this place";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 1;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            {
                canTalk = false;
                telemanText.text = "Be careful with that monsters, they´re so dangerous";
                cooldown = 0.5f;
                textChanger = 2;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 2)
            {
                canTalk = false;
                telemanText.text = "";
                cooldown = 0.5f;
                canTalk = true;
                return;
            }
        }

        if (playerCombat.canShoot && !playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract && !canTalk && cooldown <= 0 && textChanger == 0)
            {
                textTutorial.SetActive(false);
                canTalk = false;
                telemanText.text = "Oh, you found it";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 1;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            {
                canTalk = false;
                telemanText.text = "You can destroy your enemies now";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 2;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 2)
            {
                canTalk = false;
                telemanText.text = "Do it, please";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 3;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 3)
            {
                canTalk = false;
                telemanText.text = "They´re so dangerous";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 4;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 4)
            {
                canTalk = false;
                telemanText.text = "";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                canTalk = true;
                return;
            }
        }

        if (playerCombat.canShoot && playerMovement.dashUnlocked && !playerAbilities.slamUnlocked && !nearEnd)
        {
            transform.position = new Vector2(secondPosition.transform.position.x, secondPosition.transform.position.y);

            if (Input.GetKeyDown(KeyCode.E) && canInteract && !canTalk && cooldown <= 0 && textChanger == 0)
            {
                textTutorial.SetActive(false);
                canTalk = false;
                telemanText.text = "Have you killed it?";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 1;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            {
                canTalk = false;
                telemanText.text = "This is amazing";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 2;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 2)
            {
                canTalk = false;
                telemanText.text = "You might be the one";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 3;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 3)
            {
                canTalk = false;
                telemanText.text = "Please continue, there are some more things that you have to do";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 4;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 4)
            {
                canTalk = false;
                telemanText.text = "";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                canTalk = true;
                return;
            }
        }

        if (playerCombat.canShoot && playerMovement.dashUnlocked && playerAbilities.slamUnlocked && !nearEnd)
        {
            if (Input.GetKeyDown(KeyCode.E) && canInteract && !canTalk && cooldown <= 0 && textChanger == 0)
            {
                textTutorial.SetActive(false);
                canTalk = false;
                telemanText.text = "¿What? this is impossible";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 1;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            {
                canTalk = false;
                telemanText.text = "Now I realize";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 2;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 2)
            {
                canTalk = false;
                telemanText.text = "Go deeper, to the red zone";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 3;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 3)
            {
                canTalk = false;
                telemanText.text = "You will find the last of these monsters";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 4;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 4)
            {
                canTalk = false;
                telemanText.text = "Kill it";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 5;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 5)
            {
                canTalk = false;
                telemanText.text = "";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 6;
                canTalk = true;
                return;
            }
        }

        if (playerCombat.canShoot && playerMovement.dashUnlocked && playerAbilities.slamUnlocked && nearEnd)
        {
            transform.position = new Vector2(thirdPosition.transform.position.x, thirdPosition.transform.position.y);

            if (Input.GetKeyDown(KeyCode.E) && canInteract && !canTalk && cooldown <= 0 && textChanger == 0)
            {
                textTutorial.SetActive(false);
                canTalk = false;
                telemanText.text = "Wait";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 1;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            {
                canTalk = false;
                telemanText.text = "This will be difficult";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 2;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 2)
            {
                canTalk = false;
                telemanText.text = "You won´t be able to go back if you continue";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 3;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 3)
            {
                canTalk = false;
                telemanText.text = "The last beast is waiting for you";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 4;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 4)
            {
                canTalk = false;
                telemanText.text = "Be careful";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 5;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 5)
            {
                canTalk = false;
                telemanText.text = "Please don´t die";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 6;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 6)
            {
                canTalk = false;
                telemanText.text = "Please don´t die";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                textChanger = 7;
                canTalk = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 7)
            {
                canTalk = false;
                telemanText.text = "";
                animator.SetBool("isTalking", true);
                cooldown = 0.5f;
                canTalk = true;
                return;
            }

            //if (Input.GetKeyDown(KeyCode.E) && canInteract && canTalk && cooldown <= 0 && textChanger == 1)
            //{
            //    //textChanger++;

            //    //for(int i=textChanger; i<10;i++)
            //    //{

            //    //}
            //}

        }
    }
}

