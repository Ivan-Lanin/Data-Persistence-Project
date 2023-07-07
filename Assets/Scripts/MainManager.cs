using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public string playerName;
    public string playerNameInHighScore;
    public int highScore;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody ball;

    public Text ScoreText;
    public Text highScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/savefile.json")) {
            string jsonString = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
            SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonString);
            highScore = loadedData.m_PointsInSave;
            playerNameInHighScore = loadedData.playerNameInSave;
        } else
        {
            highScore = 0;
            playerNameInHighScore = "Nobody";
        }

        highScoreText.text = "High Score : " + playerNameInHighScore + " : " + highScore;

        playerName = CurentPlayerName.Instance.playerName;

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (ball == null)
        {
            Debug.Log(GameObject.Find("Ball"));
            ball = GameObject.Find("Ball")?.GetComponent<Rigidbody>();
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_GameOver = false;
                m_Started = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        UpdateHighScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
        ScoreText.text = "Score : " + playerName + " : " + m_Points;
    }

    [System.Serializable]
    class SaveData
    {
        public string playerNameInSave;
        public int m_PointsInSave;
    }

    public void UpdateHighScore()
    {
        if (m_Points < highScore) {
            return;
        }
        highScoreText.text = "High Score : " + playerName + " : " + m_Points;
        SaveData data = new SaveData();
        data.playerNameInSave = playerName;
        data.m_PointsInSave = m_Points;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
}
