using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public GameManager theGameManager; 
    public EnvironmentController theEnvironmentController;
    public Vector2 jumpHeight;

    public bool godMode = false;

    private Animator animator;
    private Rigidbody2D myRigidbody;

    public bool canMove = true;

    public AudioClip jumpAudio;
    public AudioClip hitAudio;
    

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        theGameManager = GameManager.Instance;
        theEnvironmentController = EnvironmentController.Instance;
    }


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
        }
        if (GameManager.Instance.curGameState == GameManager.GameState.Playing)
        {
            if (canMove && (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))// Input.GetMouseButtonDown(0))) //Input.touchCount > 0))
            {
                // Check if finger is over a UI element
                if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
                Jump();
            }
        }
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
        canMove = true;
        myRigidbody.AddForce(jumpHeight, ForceMode2D.Impulse);

        GetComponent<AudioSource>().clip = jumpAudio;
        GetComponent<AudioSource>().Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponent<AudioSource>().clip = hitAudio;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

            if (godMode)
                return;

            theGameManager.GameOver();
            animator.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Pipes"))
        {
            GetComponent<AudioSource>().clip = hitAudio;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            if (godMode)
                return;

            theGameManager.GameOver();
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            ScoreManager.Instance.IncrementScore();
        }
    }
}
