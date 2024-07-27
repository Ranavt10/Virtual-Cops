using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    
    public GameObject EnterEmailUI;
    public GameObject enterOtpUI;
    public GameObject passworResetUI;
    public GameObject popupPenal;
  
    //public GameObject scoreboardUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }


    private void Start()
    {
        // RegisterScreen();
        LoginScreen(); 
    }

    //Functions to change the login screen UI

    public void ClearScreen() //Turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
       
        //scoreboardUI.SetActive(false);
    }


    public void penalEnterOtp()
    {
        enterOtpUI.SetActive(true);
        EnterEmailUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }

    public void penalRestPasswordui()
    {
        passworResetUI.SetActive(true);
        enterOtpUI.SetActive(false);
        EnterEmailUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }
    public void enterEmailUI()
    {
        enterOtpUI.SetActive(false);
        EnterEmailUI.SetActive(true);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
    }

    public void LoginScreen() //Back button
    {
        
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        enterOtpUI.SetActive(false);
        EnterEmailUI.SetActive(false);
        passworResetUI.SetActive(false);
        popupPenal.SetActive(false);
    }
    public void RegisterScreen() // Regester button
    {
        
        registerUI.SetActive(true);
        loginUI.SetActive(false);
    }
    public void openPopUpPenal()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        enterOtpUI.SetActive(false);
        EnterEmailUI.SetActive(false);
        passworResetUI.SetActive(false);
        popupPenal.SetActive(true);
    }

  

   
   
}
