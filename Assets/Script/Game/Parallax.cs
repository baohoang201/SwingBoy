using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    public static Parallax instance;
    public bool isMove;
    private float speed = 0.5f;
    private float offset;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        image = GetComponent<Image>();
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            offset += Time.deltaTime * speed;
            image.material.SetTextureOffset("_MainTex", new Vector2(0, 0));
        }
    }
}
