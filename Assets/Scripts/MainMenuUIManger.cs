using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class MainMenuUIManger : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;

    // Start is called before the first frame update
    void Start()
    {
        inputName.text = CurentPlayerName.Instance.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        EditorApplication.ExitPlaymode();
    }

    public void ToMainScene()
    {
        SceneManager.LoadScene("main");
    }

    public void NewNameSelected()
    {
        CurentPlayerName.Instance.playerName = inputName.text;
    }
}
