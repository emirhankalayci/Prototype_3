using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private float speed = 2.0f;

    public float jumpForce = 10;
    public float gravityModifier = 2;

    public bool isOnGround = true;
    public bool gameOver = false;

    private int jumpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }

        if (transform.position.x > 0)
        {
            playerAnim.SetFloat("Speed_f", 0.6f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && jumpCount<2 && transform.position.x >= 0)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            isOnGround = false;

            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);

            jumpCount += 1;
        }

        if (isOnGround)
        {
            jumpCount = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && transform.position.x > 0)
        {
            isOnGround = true;
            dirtParticle.Play();
            
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            explosionParticle.Play();
            gameOver = true;
            Debug.Log("GameOver");

            playerAudio.PlayOneShot(crashSound, 1.0f);

            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);

            dirtParticle.Stop();
        }

    }
}
