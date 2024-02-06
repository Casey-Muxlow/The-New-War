using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInputController : MonoBehaviour
{
    [SerializeField]
    GameObject goCanvasMenu;

    [SerializeField]
    OptionsCreator myOptions;
    [SerializeField]
    SubOptionsCreator mySubOptions;
    [SerializeField]
    MenuSoundsProvider mySoundsProvider;


    private GameObject goParentOfCurrentSubOptions;

    private bool bIsOptionsFocused = false;

    public void SetOptionsFocused(bool bNewValue){
        bIsOptionsFocused = bNewValue;
    }

    void Start(){
        goCanvasMenu.SetActive(false);
    }

     void Update(){
         if (goCanvasMenu.activeSelf)
         {
             if(bIsOptionsFocused){ //Options
                 OptionsMenuFocused();
             }
             else{ //Suboptions
                 SubOptionsMenuFocused();
             }
         }
    }

    private void OptionsMenuFocused(){
        GameObject goOptionFocused = myOptions.GetObjectFromListWithIndex(myOptions.GetIndexOfOptionFocused());

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            UnfocusCurrentOption((goOptionFocused));
            myOptions.DecreaseOptionFocusedNumber();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            UnfocusCurrentOption((goOptionFocused));
            myOptions.IncreaseOptionFocusedNumber();
        }
        
        FocusNextOption();

        if(Input.GetKeyDown(KeyCode.Return)){
            mySoundsProvider.PlayAudioClick();
//            myOptions.EnableClickInButtons(false);
            myOptions.CheckOptionFocusedAndDoTheCorrectAction();
        }
    }

    private void UnfocusCurrentOption(GameObject option)
    {
        mySoundsProvider.PlayAudioNavigation();
        myOptions.ChangeOptionTextColorToUnfocused(option.GetComponent<Text>());
    }

    private void FocusNextOption(){        
        GameObject goNextOptionFocused = myOptions.GetObjectFromListWithIndex(myOptions.GetIndexOfOptionFocused());
        myOptions.ChangeOptionTextColorToFocused(goNextOptionFocused.GetComponent<Text>());
    }

    private void SubOptionsMenuFocused(){
        goParentOfCurrentSubOptions = myOptions.GetObjectFromListWithIndex(myOptions.GetIndexOfOptionFocused());

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            mySoundsProvider.PlayAudioNavigation();
            GoPreviousSubOption(goParentOfCurrentSubOptions);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            mySoundsProvider.PlayAudioNavigation();
            GoNextSubOption(goParentOfCurrentSubOptions);
        }

        mySubOptions.FocusCurrentSubOption(goParentOfCurrentSubOptions);
        
        CheckIfBackButtonIsPressedOrNot(goParentOfCurrentSubOptions);
    }

    private void GoPreviousSubOption(GameObject goParent){
        mySubOptions.UnfocusCurrentSubOption(goParent);
        mySubOptions.DecreaseSubOptionFocused(myOptions.GetNumberOfChildrenOfOptionFocused());
    }

    private void GoNextSubOption(GameObject goParent){
        mySubOptions.UnfocusCurrentSubOption(goParent);
        mySubOptions.IncreaseSubOptionFocused(myOptions.GetNumberOfChildrenOfOptionFocused());
    }

    private bool IsBackButtonSubOption(GameObject goParent){
        return (mySubOptions.GetCurrentSubOptionFocused() == goParent.transform.GetChild(0).transform.childCount -1);
    }

    private void CheckIfBackButtonIsPressedOrNot(GameObject parent){
        if(IsBackButtonSubOption(parent)) 
        {
            if(Input.GetKeyDown(KeyCode.Return)){
                BackButtonActionInSubOptions(parent);
            }
        }
        else{ //Not Back Button. Suboptions have side arrows functions
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                mySoundsProvider.PlayAudioNavigation();
                mySubOptions.CurrentSubOptionEventLeft(parent);
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)){
                mySoundsProvider.PlayAudioNavigation();
                mySubOptions.CurrentSubOptionEventRight(parent);
            }
        }
    }

    private void BackButtonActionInSubOptions(GameObject parent)
    {
        myOptions.EnableClickInButtons(true);
        mySoundsProvider.PlayAudioClick();

        mySubOptions.UnfocusCurrentSubOption(parent);

        mySubOptions.AddListenerToBackButtonInSubOptionMenu(parent.transform.GetChild(0).gameObject, true, myOptions.GetIndexOfOptionFocused());
                
        myOptions.SetIndexOfOptionFocused(0);
        bIsOptionsFocused = true;
        
    }
    
    public void QuitGame(){
        Debug.Log("Exiting game");
        Application.Quit();
    }

    public void ShowMenu(){
        SetOptionsFocused(!goCanvasMenu.activeSelf);
        goCanvasMenu.SetActive(!goCanvasMenu.activeSelf);

        bIsOptionsFocused = true;
        
        if (!goCanvasMenu.activeSelf)
        {
            myOptions.ResetOptions();
            myOptions.EnableClickInButtons(true);
        }
    }
 }
