using UnityEngine;

public class BackroundInf : MonoBehaviour
{
    public float speed;
    public float endX;
    public float startX;
     
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if(transform.position.x <= endX) { 
            Vector2 pos = new Vector2(startX, transform.position.y);
            transform.position = pos;
        }
    }
}
