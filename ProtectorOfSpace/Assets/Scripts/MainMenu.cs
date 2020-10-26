using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "SampleScene";
    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    // Update is called once per frame
    public void Quit()
    {
        Application.Quit();
    }
}
