using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private string lateLevel;

    [SerializeField] private GameObject lockIcon;

    private void Start()
    {
        if (CheckLateLevel())
        {
            GetComponent<Button>().enabled = false;
            lockIcon.SetActive(true);
        }
    }

    private bool CheckLateLevel()
    {
        if (lateLevel == null)
        {
            return true;
        }

        LevelData levelData = GameDataStore.gameData.GetLevel(lateLevel);

        if (levelData != null)
        {
            return true;
        }

        return false;
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
