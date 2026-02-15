using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    public Joystick joystick;
    public Button shoot;
    public GameObject bulletPrefab;
    public Transform shootPos;
    public Image healthText;
    public GameObject damageEf;
    public MoneySCOB money;
    public AudioClip damageClip;

    private AudioSource audioSource;
    public float speed;
    private float MoveInputY;
    private float MoveInputX;
    public int health = 100;
    private bool facingRight = true;
    private Rigidbody2D rb;

    private float lastShootTime = -Mathf.Infinity;
    public float shootCooldown = 0.5f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        shoot.onClick.AddListener(Shooting);
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

        if (Input.GetKeyDown(KeyCode.Space)) Shooting();
    }

    void FixedUpdate() {
        if (facingRight == false && MoveInputX > 0) Flip();
        else if (facingRight == true && MoveInputX < 0) Flip();

        MoveInputY = joystick.Vertical;
        MoveInputX = joystick.Horizontal;

        rb.linearVelocity = rb.linearVelocity = new Vector2(MoveInputX * speed, MoveInputY * speed);
    }

    public void Shooting()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            Instantiate(bulletPrefab, shootPos.position, Quaternion.identity);
            lastShootTime = Time.time;
        }
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        Instantiate(damageEf, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(damageClip);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}