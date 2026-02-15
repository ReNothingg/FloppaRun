using UnityEngine;
using UnityEngine.UI;

public class ProcentWG : MonoBehaviour
{
    public Slider procent;
    [SerializeField] LevelGenerator levelGenerator;
    public Transform playerTransform;

    private void Start() {
        procent.maxValue = levelGenerator.levelLength;
    }

    private void Update()
    {
        procent.value = playerTransform.position.x;
    }
}
