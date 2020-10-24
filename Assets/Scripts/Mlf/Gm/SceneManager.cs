using UnityEngine;

namespace Mlf.Gm
{
    public class SceneManager : MonoBehaviour
    {


        public static SceneManager instance;

        [SerializeField] private AudioSource audioSource;

        public void LoadScene(string name)
        {
            audioSource.Play();
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Debug.LogWarning("Singleton has a double instance");
                Destroy(this);
            }

        }

    }

}



