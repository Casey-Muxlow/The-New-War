using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangerToFocusedAndUnfocused : MonoBehaviour
{
    [SerializeField]
    Color colorButtonFocused;
    [SerializeField]
    Color colorButtonUnfocused;


    public void ChangeTextColorToFocused(Text textLabel){
        textLabel.color = new Color(colorButtonFocused.r, colorButtonFocused.g, colorButtonFocused.b, 1);
    }

    public void ChangeTextColorToUnfocused(Text textLabel){
        textLabel.color = new Color(colorButtonUnfocused.r, colorButtonUnfocused.g, colorButtonUnfocused.b, 1);
    }
/*
    public void ChangeBackButtonTextColorToUnfocused(Text textLabel){
        ChangeTextColorToUnfocused(textLabel);
    }
    public void ChangeBackButtonTextColorToFocused(Text textLabel){
        ChangeTextColorToFocused(textLabel);
    }

    public void ChangeQuitButtonTextColorToUnfocused(Text textLabel){
        ChangeTextColorToUnfocused(textLabel);
    }
    public void ChangeQuitButtonTextColorToFocused(Text textLabel){
        ChangeTextColorToFocused(textLabel);
    }

    public void ChangeOptionTextColorToUnfocused(Text textLabel){
        ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeOptionTextColorToFocused(Text textLabel){
        ChangeTextColorToFocused(textLabel);
    }

    public void ChangeSubOptionTextColorToUnfocused(Text textLabel){
        ChangeTextColorToUnfocused(textLabel);
    }

    public void ChangeSubOptionTextColorToFocused(Text textLabel){
        ChangeTextColorToFocused(textLabel);
    }
    */
}
