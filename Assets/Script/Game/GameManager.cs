using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform obsParent, spawnPointWall;
    [SerializeField] private GameObject[] wallPrefab;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private GameOverPopup gameOverPopup;
    public List<Transform> listWall;
    public bool isMove;
    public static GameManager instance;
    private float timeNext, timeRate;

    void Awake()
    {
        instance = this;
        isMove = false;
        timeNext = Time.time;
        timeRate = 1.7f;
        listWall = new List<Transform>();

    }

    private void InstiateWall()
    {
        var randomWall = wallPrefab[Random.Range(0, wallPrefab.Length)];
        var obsIns = Instantiate(randomWall, spawnPointWall.position, Quaternion.identity);
        obsIns.transform.SetParent(obsParent);
        listWall.Add(obsIns.transform);
        SortWall();
    }

    private void SortWall()
    {
        if (listWall.Count > 0)
        {
            for (int i = 0; i < listWall.Count - 1; i++)
            {
                listWall[i + 1].position = new Vector2(listWall[i].position.x + 4f, listWall[i + 1].position.y);
            }
        }
    }

    public void InvokeReWall()
    {
        if (Time.time > timeNext)
        {
            InstiateWall();
            timeNext = Time.time + timeRate;
        }
    }
    public void GameOver()
    {
        UIManager.EnableGameOverPopUp(true);
        gameOverPopup.LoadText();
        UIManager.EnableMainGame(false);
        UIManager.EnableEnviroment(false);
        ScoreManager.instance.SaveHighScore();
        CancelInvoke();
    }

    public void LoadScene() => SceneManager.LoadScene(0);


}
