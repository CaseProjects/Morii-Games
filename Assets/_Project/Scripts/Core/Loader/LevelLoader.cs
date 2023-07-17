using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(
            PlayerPrefs.GetInt(Constants.PlayerPrefsKey.LEVEL, 1));
    }
}