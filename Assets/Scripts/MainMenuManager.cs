using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Panel List")]
    public GameObject MainMenu;
    public GameObject OptionPanel;
    public GameObject ShopPanel;
    public GameObject TopUpPanel;


    private void Start()
    {
        MainMenu.SetActive(true);
        OptionPanel.SetActive(false);
        ShopPanel.SetActive(false);
        TopUpPanel.SetActive(false);
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void OptionBtn()
    {
        OptionPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("Click");
    }

    public void ShopBtn() 
    { 
        ShopPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("Click");
    }

    public void TopUpBtn()
    {
        TopUpPanel.SetActive(true);
        AudioManager.Instance.PlaySFX("Click");
    }

    public void CloseBtn()
    {
        if (TopUpPanel.activeSelf)
        {
            TopUpPanel.SetActive(false);
        }
        else if (OptionPanel.activeSelf)
        {
            OptionPanel.SetActive(false);
        }else if (ShopPanel.activeSelf)
        {
            ShopPanel.SetActive(false);
        }

        AudioManager.Instance.PlaySFX("Click");
    }

    public void PlayBtn() 
    {
        SceneManager.LoadScene("Alternative");
    }
    public void ExitBtn()
    {
        Application.Quit();
    }


}
