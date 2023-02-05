using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{

    
    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
}
