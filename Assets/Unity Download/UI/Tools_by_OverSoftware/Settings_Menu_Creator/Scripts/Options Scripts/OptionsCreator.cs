using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsCreator : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField]
    private MenuInputController myMenuInputController;
    [SerializeField]
    private ColorChangerToFocusedAndUnfocused myColorChanger;
    [SerializeField]
    private DictionaryCreator myDictionary;
    [SerializeField]
    private OptionsAndSubOptionsComunicator myOptSuboptComunicator;

    [SerializeField]
    OptionsPositionConfigurator myOptionsPositionConfigurator;

    [Header("Objects")]
    [SerializeField]
    GameObject goOptionPrefab;

    [SerializeField]
    GameObject goCanvasMenu;

    [SerializeField]
    string sBackButtonText;
    [SerializeField]
    string sQuitButtonText;

    [Tooltip("These options will become the GameObject parents of the SubOptions.")]
    [SerializeField]
    string[] aMenuOptions;

    private List<GameObject> listOptions = new List<GameObject>();

    int iOptionFocused = 0;


    public void EnableClickInButtons(bool value)
    {
        for (int i = 0; i < listOptions.Count; i++)
        {
            listOptions[i].GetComponent<Button>().interactable = value;
        }
    }
    
    public void CreateDictionaryWithOptions(){
        myDictionary.CreateDictionary(aMenuOptions);
    }

    public void IncreaseOptionFocusedNumber(){
        iOptionFocused++;

        if(iOptionFocused > (listOptions.Count -1)){
            iOptionFocused = 0;
        }
    }

    public void DecreaseOptionFocusedNumber(){
        iOptionFocused--;

        if(iOptionFocused < 0){
            iOptionFocused = (listOptions.Count - 1);
        }
    }

    public void CheckOptionFocusedAndDoTheCorrectAction(){
        if(iOptionFocused == listOptions.Count - 2){
            Debug.Log("Back Button...");
            ResetAndHideMenuWithBackButton();
        }
        else if (iOptionFocused == listOptions.Count - 1){
            Debug.Log("Quitting game...");
            myMenuInputController.QuitGame();
        }
        else{
            myMenuInputController.SetOptionsFocused(false);
            EnableClickInButtons(false);
            myOptSuboptComunicator.ActivateAllSuboptionsOfAnOption(listOptions, GetTextFromOptionFocused());
        }
    }

    public List<GameObject> GetList(){
        return listOptions;
    }
    public int GetListCount(){
        return listOptions.Count;
    }
    public GameObject GetObjectFromListWithIndex(string sValueInDictionary){
        return (listOptions[myDictionary.GetIndexFromValue(sValueInDictionary)]);
    }
    public GameObject GetObjectFromListWithIndex(int iIndex){
        return (listOptions[iIndex]);
    }

    public int GetIndexOfOptionFocused(){
        return iOptionFocused;
    }

    public GameObject GetOptionPrefab(){
        return goOptionPrefab;
    }

    public int GetNumberOfChildrenOfOptionFocused(){
        return (listOptions[iOptionFocused].transform.GetChild(0).transform.childCount -1);
    }

    public string GetBackButtonText(){
        return sBackButtonText;
    }

    public void SetIndexOfOptionFocused(int iNewValue){
        iOptionFocused = iNewValue;
    }

    public void ResetAndHideMenuWithBackButton(){
        ResetOptions();
        myMenuInputController.ShowMenu();
    }

    public void ResetOptions()
    {
        int iCount = listOptions.Count;
        
        if(iCount > 0){
            for (int i = 0; i < iCount - 2; i++){ //Last 2 buttons are Back Button and Quit Button
                listOptions[i].transform.GetChild(0).gameObject.SetActive(false);

                ChangeOptionTextColorToUnfocused(listOptions[i].GetComponent<Text>());
            }
            ChangeBackButtonTextColorToUnfocused(listOptions[iCount - 2].GetComponent<Text>());
            ChangeQuitButtonTextColorToUnfocused(listOptions[iCount - 1].GetComponent<Text>());

            ChangeOptionTextColorToFocused(listOptions[0].GetComponent<Text>());
        }

        iOptionFocused = 0;
        myMenuInputController.SetOptionsFocused(true);
//        bIsOptionMenuFocused = true;
    }
    
    public string GetTextFromOptionFocused(){
        return listOptions[iOptionFocused].GetComponent<Text>().text;
    }


//Para The Power ups: lo he hecho así para favorecer la lectura pero  no sé si esto es duplicar código y sería mejor directamente llamar a la función 
    public void ChangeBackButtonTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeBackButtonTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeQuitButtonTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeOptionTextColorToFocused(Text textLabel){
        myColorChanger.ChangeTextColorToFocused(textLabel);
    }

    public void ChangeQuitButtonTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeOptionTextColorToUnfocused(Text textLabel){
        myColorChanger.ChangeTextColorToUnfocused(textLabel);
    }

///////////////////////////////////////

    public void BuildOptions(){
        for (int i = 0; i < aMenuOptions.Length; i++){
            GameObject goNewOption = Instantiate(goOptionPrefab, goCanvasMenu.transform);
            int currentOptionIndex = i;
            
            goNewOption.transform.localPosition = myOptionsPositionConfigurator.GetPositionForIndex(i);

            goNewOption.GetComponent<Text>().text = aMenuOptions[i];
            goNewOption.GetComponent<Button>().onClick.AddListener(delegate 
            {
                myMenuInputController.SetOptionsFocused(false);
                myOptSuboptComunicator.ActivateAllSuboptionsOfAnOption(listOptions, goNewOption.GetComponent<Text>().text);
                iOptionFocused = currentOptionIndex;
            });


            if(i == 0){
                ChangeOptionTextColorToFocused(goNewOption.GetComponent<Text>());
            }else
                ChangeOptionTextColorToUnfocused(goNewOption.GetComponent<Text>());

            listOptions.Add(goNewOption);

            myOptionsPositionConfigurator.InstantiatePanelForSuboptionsOfOptionI(goNewOption.transform, i);
        }

        //Button Back
        GameObject goBackButton = Instantiate(goOptionPrefab, goCanvasMenu.transform);
        goBackButton.transform.localPosition = myOptionsPositionConfigurator.GetPositionForIndex(listOptions.Count); //Back button is alwas the element before last in the list.

        goBackButton.GetComponent<Text>().text = sBackButtonText;
        goBackButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            ResetAndHideMenuWithBackButton();
        });

        ChangeBackButtonTextColorToUnfocused(goBackButton.GetComponent<Text>());
        
        listOptions.Add(goBackButton);


        //Button Quit
        GameObject goQuitButton = Instantiate(goOptionPrefab, goCanvasMenu.transform);
        goQuitButton.transform.localPosition = myOptionsPositionConfigurator.GetPositionForIndex(listOptions.Count);

        goQuitButton.GetComponent<Text>().text = sQuitButtonText;
        goQuitButton.GetComponent<Button>().onClick.AddListener(delegate{myMenuInputController.QuitGame();});
        
        ChangeQuitButtonTextColorToUnfocused(goQuitButton.GetComponent<Text>());
        listOptions.Add(goQuitButton);
    }
}
