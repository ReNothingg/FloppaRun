using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScroreManagrIW : MonoBehaviour
{
    public Text scoreDisp;
    public int score;

    private void Update()
    {
        scoreDisp.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gear"))
        {
            score++;
        }
    }
}
