using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    public GameObject appleParticles;

    public int killCount = 0;

    private AudioSource mySound;
    public AudioClip myClip;

    public int applesCollected = 0;

    public Slider healthSlider;
    public TMP_Text appleText;

    private bool canMoove;

    public Transform spawnPosition;

    public Transform leftFoot, rightFoot;
    private bool isGrounded;
    private float rayDistance = 0.25f;
    public LayerMask whatIsGround;

    public float moveSpeed = 300f;
    public float jumpForce = 300f;

    public int startingHealth = 5;
    public int currentHealth;

    private SpriteRenderer myRenderer;
    private Rigidbody2D myRigidbody;
    private float horizontalValue;

    private Animator myAinmator;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAinmator = GetComponent<Animator>();

        mySound = GetComponent<AudioSource>();

        appleText.text = applesCollected.ToString();

        canMoove = true;

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalValue = Input.GetAxis("Horizontal"); //gets the horizontal value from the a d keyboard inputs

        if(horizontalValue < 0)
        {
            FlipSprite(true);
        }

        if (horizontalValue > 0) 
        {
            FlipSprite(false);
        }

        if (Input.GetButtonDown("Jump") && CheckIfGrounded() == true)
        {
            Jump();
        }

        myAinmator.SetFloat("MoveSpeed", Mathf.Abs(myRigidbody.velocity.x));
        myAinmator.SetFloat("VerticalSpeed", myRigidbody.velocity.y);
    }

    private void FixedUpdate()
    {
        if (killCount > 0)
        {
            mySound.pitch = Random.Range(0.5f, 1.5f);
            mySound.PlayOneShot(myClip, 1);
            killCount = 0;
        }

        if (!canMoove)
        {
            return;
        }

        myRigidbody.velocity = new Vector2(horizontalValue * moveSpeed * Time.deltaTime, myRigidbody.velocity.y); //modifies left/right movoment
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple"))
        {
            Destroy(other.gameObject);
            applesCollected++;
            appleText.text = applesCollected.ToString();
            Instantiate(appleParticles, other.transform.position, Quaternion.identity);
        }

        if (other.CompareTag("Cherries"))
        {
            Destroy(other.gameObject);
            RestoreHealth(1);
        }
    }

    private void FlipSprite(bool direction)
    {
        myRenderer.flipX = direction;
    }

    private void Jump()
    {
        myRigidbody.AddForce(new Vector2(0, jumpForce));
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, rayDistance, whatIsGround);

        if (leftHit.collider != null && leftHit.collider.CompareTag("Ground") || rightHit.collider != null && rightHit.collider.CompareTag("Ground"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Respawn();
        }

        UpdateHealthBar();
    }

    private void CanMoove()
    {
        canMoove = true;
    }

    public void TakeKnockback(float knockbackForce, float upwardForce)
    {
        canMoove = false;
        myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
        myRigidbody.AddForce(new Vector2(knockbackForce, upwardForce));
        Invoke("CanMoove", 0.3f);
    }

    public void RestoreHealth(int healthRestored)
    {
        if (currentHealth == startingHealth)
        {
            return;
        }
        else
        {
            currentHealth += healthRestored;
            UpdateHealthBar();
            if (currentHealth > startingHealth)
            {
                currentHealth = startingHealth;
            }
        }
    }

    private void Respawn()
    {
        mySound.PlayOneShot(myClip, 1);
        transform.position = spawnPosition.position;
        myRigidbody.velocity = Vector2.zero;
        currentHealth = startingHealth;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
    }
}