using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerIW : MonoBehaviour
{
    private Vector2 targetPos;
    public float Yincrment;

    public AudioClip damageClip;

    public float speed;
    public float maxHeight;
    public float minHeight;
    public GameObject effect;

    public Image healthText;
    public int health;

    public Animator camAnim;

    private AudioSource audioSource;

    private void Start()
    {
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        targetPos = transform.position;
    }

    private void Update()
    {
        healthText.fillAmount = health / 100f;
        if (health <= 0)
        {
            SceneManager.LoadScene("Menu");
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
        transform.position = new Vector2(transform.position.x, clampedY);

        bool isAtTarget = Vector2.Distance(transform.position, targetPos) < 0.01f;

        if (Input.GetKeyDown(KeyCode.W) && transform.position.y < maxHeight && isAtTarget)
        {
            camAnim.SetTrigger("Shake");
            float newY = Mathf.Clamp(transform.position.y + Yincrment, minHeight, maxHeight);
            targetPos = new Vector2(transform.position.x, newY);
        }

        if (Input.GetKeyDown(KeyCode.S) && transform.position.y > minHeight && isAtTarget)
        {
            camAnim.SetTrigger("Shake");
            float newY = Mathf.Clamp(transform.position.y - Yincrment, minHeight, maxHeight);
            targetPos = new Vector2(transform.position.x, newY);
        }
    }

    public void Up()
    {
        if (transform.position.y < maxHeight && Vector2.Distance(transform.position, targetPos) < 0.01f)
        {
            camAnim.SetTrigger("Shake");
            float newY = Mathf.Clamp(transform.position.y + Yincrment, minHeight, maxHeight);
            targetPos = new Vector2(transform.position.x, newY);
        }
    }

    public void Down()
    {
        if (transform.position.y > minHeight && Vector2.Distance(transform.position, targetPos) < 0.01f)
        {
            camAnim.SetTrigger("Shake");
            float newY = Mathf.Clamp(transform.position.y - Yincrment, minHeight, maxHeight);
            targetPos = new Vector2(transform.position.x, newY);
        }
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        Instantiate(effect, transform.position, Quaternion.identity);
        camAnim.SetTrigger("Shake");
        audioSource.PlayOneShot(damageClip);
    }
}
