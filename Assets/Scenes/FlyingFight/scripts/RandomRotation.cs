using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int degres;

    public Sprite[] sprite;

    private void Start()
    {
        speed = Random.Range(1f, 5f);
        degres = Random.Range(-1, 1);

        GetComponent<SpriteRenderer>().sprite = sprite[Random.Range(0,1)];
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, (degres * speed));

        if (degres == 0) degres = Random.Range(-1, 1);
        if (speed == 0) speed = Random.Range(1f, 5f);
    }
}