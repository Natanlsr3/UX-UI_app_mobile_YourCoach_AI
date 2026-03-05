using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MVC.App.UI
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loginScreen;

        //Function used by animation event
        public void OnSplashScreenAnimFinished()
        {
            Instantiate(loginScreen, transform.parent);
        }
    }
}