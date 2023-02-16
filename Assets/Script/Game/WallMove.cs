using UnityEngine;

public class WallMove : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    void Start()
    {
        var randomNumber = Random.Range(0, 4);
        if (randomNumber == 1) coin.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isMove) transform.Translate(Vector3.left * 3 * Time.deltaTime);
        if (transform.position.x < PlayerController.instance.frameLeft.position.x)
        {
            GameManager.instance.listWall.Remove(transform);
            Destroy(gameObject);
        }
    }
}
