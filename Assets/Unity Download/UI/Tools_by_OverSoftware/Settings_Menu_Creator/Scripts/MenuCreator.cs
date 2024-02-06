using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCreator : MonoBehaviour
{
    [SerializeField]
    private TitleCreator myTitle;
    [SerializeField]
    private OptionsCreator myOptions;
    [SerializeField]
    private SubOptionsCreator mySubOptions;
    [SerializeField]
    private MenuPlayerPrefsCoordinator myPlayerPrefs;
    [SerializeField]
    private MenuInputController myInputController;
    [SerializeField]
    bool bResetPlayerPrefs = false;

    public void QuitGame(){
        myInputController.QuitGame();
    }

    public void ShowMenu(){
        myInputController.ShowMenu();
    }

    void Start()
    {
        myTitle.BuildTitle();

        if(bResetPlayerPrefs)
            myPlayerPrefs.ResetMenuPlayerPrefs();

        mySubOptions.LoadSubOptionsValuesFromPlayerPrefs();

        myOptions.CreateDictionaryWithOptions();

        myOptions.BuildOptions();

        mySubOptions.BuildSubOptions();
    }
}
