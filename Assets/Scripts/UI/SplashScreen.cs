using UnityEngine;

namespace MVC.App.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loginScreen;

        //Function used by animation event
        public void OnSplashScreenAnimFinished()
        {
            PlayerPrefs.DeleteKey("TrainingSessionDone");
            Instantiate(loginScreen, transform.parent);
        }
    }
}