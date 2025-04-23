using UnityEngine;
using UnityEngine.SceneManagement;

public class InGame_UI : MonoBehaviour
{
    private bool gamePaused;

    [Header("Menu gameobjects")]
    [SerializeField] private GameObject pauseUI;

    private void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CheckIfNotPaused();

    }

    private bool CheckIfNotPaused()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            SwitchUI(pauseUI);
            return true;
        }
        gamePaused = false;
        pauseUI.SetActive(false);
        return true;
    }

    public void SwitchUI(GameObject uiMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        uiMenu.SetActive(true);
    }

    public void ReloadCurrentLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
