using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform frameRight, frameTop, frameBot;
    public Transform frameLeft;
    [SerializeField] private Sprite[] spriteNode;
    [SerializeField] private GameObject nodeStart;
    private float posX, posY;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private DistanceJoint2D distanceJoint2D;
    public static PlayerController instance;
    private GameObject[] allNode;
    private GameObject closetNode;
    private bool isClick, isOver;
    private Animator animator;

    void Awake()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint2D = GetComponent<DistanceJoint2D>();
        animator = GetComponent<Animator>();
        lineRenderer.enabled = true;
        isClick = false;
        isOver = false;
        PlayAnim(false);
    }
    void Start()
    {
        FindCloset();
    }
    void Update()
    {
        PlayerMove();
        ClampPos();
    }

    private void ClampPos()
    {
        if (transform.position.y < frameBot.position.y || transform.position.x > frameRight.position.x
         || transform.position.x < frameLeft.position.x) GameManager.instance.GameOver();

    }

    private void PlayerMove()
    {
        if (!isOver)
        {
            if (GameManager.instance.isMove)
            {
                distanceJoint2D.connectedAnchor = closetNode.transform.position;
                lineRenderer.SetPosition(0, closetNode.transform.position);
            }
            if (Input.GetMouseButton(0))
            {
                if (nodeStart != null) nodeStart.GetComponent<SpriteRenderer>().sprite = spriteNode[0];
                if (!isClick) FindCloset();
                isClick = true;
                distanceJoint2D.enabled = true;
                lineRenderer.enabled = true;
                PlayAnim(false);

            }
            else if (Input.GetMouseButtonUp(0))
            {
                distanceJoint2D.enabled = false;
                lineRenderer.enabled = false;
                isClick = false;
                PlayAnim(true);
                if (!isClick) Force();
                closetNode.GetComponent<SpriteRenderer>().sprite = spriteNode[0];
            }
            if (distanceJoint2D.enabled)
            {
                lineRenderer.SetPosition(1, transform.position);
            }
        }

    }

    private void FindCloset()
    {
        var distanceToCloset = Mathf.Infinity;
        closetNode = null;
        allNode = GameObject.FindGameObjectsWithTag(TAG.node);

        foreach (GameObject node in allNode)
        {
            var distanceToNode = (node.transform.position - transform.position).sqrMagnitude;
            if (distanceToNode < distanceToCloset)
            {
                distanceToCloset = distanceToNode;
                closetNode = node;
                lineRenderer.SetPosition(0, closetNode.transform.position);
                lineRenderer.SetPosition(1, transform.position);
                distanceJoint2D.connectedAnchor = closetNode.transform.position;
            }
        }
        closetNode.GetComponent<SpriteRenderer>().sprite = spriteNode[1];
        transform.SetParent(closetNode.transform);
    }

    private void Force() => rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

    private void PlayAnim(bool enable) => animator.SetBool(TAG.PlayerStatus, enable);

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TAG.Lose))
        {
            isOver = true;
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;
            GameManager.instance.GameOver();

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TAG.coin))
        {
            ScoreManager.instance.score++;
            ScoreManager.instance.UpdateScore();
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TAG.obstacle))
        {
            GameManager.instance.isMove = true;
            GameManager.instance.InvokeReWall();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TAG.obstacle))
        {
            GameManager.instance.isMove = false;
        }
    }

}
