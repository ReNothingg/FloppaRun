using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject EnemyBullet;
    public float shhotInterval;
    public float lifeTime;

    public GameObject moneySprite;
    public GameObject moneyEffects;

    public float health;
    public Image healthText;
    public MoneySCOB money;

    private int chanse = 0;

    private void Start()
    {
        chanse = Random.Range(0, 3);
        if (chanse == 1) moneySprite.SetActive(true);
        InvokeRepeating("Shoot", shhotInterval, shhotInterval);
    }

    private void Update()
    {
        healthText.fillAmount = health / 100f;
        if (health <= 0)
        {
            money.money += 5;
            if (chanse == 1) money.money += 10;
            Destroy(gameObject);
            Instantiate(moneyEffects, transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()  {
        transform.Translate(Vector3.left * speed);
        Destroy(gameObject, lifeTime);
    }

    void Shoot() => Instantiate(EnemyBullet, transform.position, Quaternion.identity);

    public void GetDamage(float GetDamage) => health -= GetDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= collision.gameObject.GetComponent<PlayerBulled>().damage;
            Destroy(collision.gameObject);
        }
    }
}
