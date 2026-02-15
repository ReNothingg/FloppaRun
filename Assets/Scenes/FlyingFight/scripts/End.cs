using System;
using UnityEngine;
using UnityEngine.UI;

public class End : MonoBehaviour
{
    public int max = 5;
    public Text text;
    public GameObject endpanel;
    int now = 0;

    private void Update()
    {
        text.text = Convert.ToString(now + "/" + max);

        if (now >= max)
        {
            endpanel.SetActive(true);

            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyShip")
        {
            now++;
        }
    }
}
