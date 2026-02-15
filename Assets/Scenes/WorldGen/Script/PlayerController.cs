using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public Image healthText;
    public GameObject getDamageEffect;

    public AudioClip jumpClip;
    public AudioClip damageClip;

    public MoneySCOB money;

    public float speed;
    public float JumpForce;
    private float MoveInput;
    public bool canMove = true;
    private bool facingRight = true;
    private Rigidbody2D rb;
    public Joystick joystick;
    private bool isGrounded;
    public Transform feetPos;
    public Vector2 groundedRadius;
    public LayerMask layerGround;
    private float angle = 0;
    private AudioSource audioSource;


    public GameObject bronik;
    public bool bronikBool;
    public GameObject getDamageInBronikEffect;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        healthText.fillAmount = health / 100f;
        if (health <= 0)
        {
            money.money -= 10;
            PlayerPrefs.SetInt("Money", money.money);
            SceneManager.LoadScene("Menu");
        }


        if (Input.GetKeyDown(KeyCode.Space)) OnJumpButtonDown();

        if (Input.GetKey(KeyCode.D)) rb.linearVelocity = new Vector2(1 * speed, rb.linearVelocity.y);
        if (Input.GetKey(KeyCode.A)) rb.linearVelocity = new Vector2(-1 * speed, rb.linearVelocity.y);
    }

    private void FixedUpdate() 
    {
        if (canMove == true) MoveInput = joystick.Horizontal;
        if (canMove == true)
        {
            rb.linearVelocity = new Vector2(MoveInput * speed, rb.linearVelocity.y);
            isGrounded = Physics2D.OverlapBox(feetPos.position, groundedRadius, angle, layerGround);
            if (facingRight == false && MoveInput > 0) Flip();
            else if (facingRight == true && MoveInput < 0) Flip();
        }

        bronik.SetActive(bronikBool);
    }

    public void OnJumpButtonDown() 
    {
        if (isGrounded == true)
        {
            audioSource.PlayOneShot(jumpClip);
            rb.linearVelocity = Vector2.up * JumpForce;
        } 
    }

    void Flip() 
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(feetPos.position, groundedRadius);

    public void takeGamage(int damage) 
    {
        if(bronikBool == false) 
        {
            health -= damage;
            audioSource.PlayOneShot(damageClip);
            Instantiate(getDamageEffect, transform.position, Quaternion.identity);
        }
        else
        {
            if (damage >= 100) health = 0;
            Instantiate(getDamageInBronikEffect, transform.position, Quaternion.identity);
        }
    }
}