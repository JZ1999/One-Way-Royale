﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVarManager : MonoBehaviour
{

    public Image uiBarBackground;
    public Image uiBarFront;
    public int tierBar = 0;
    public Color[] color;
    // Start is called before the first frame update
    void Start()
    {
        uiBarFront.fillAmount = 0f;
        uiBarBackground.fillAmount = 1;
        ChangeColorPairs();
    }
    private void ChangeColorPairs()
    {
        uiBarBackground.color = ChangeColorVar(uiBarBackground.color, color[tierBar]);
        tierBar += 1;
        uiBarFront.color = ChangeColorVar(uiBarFront.color, color[tierBar]);
        tierBar += 1;
    }
    public void FillBar(float amount)
    {
        uiBarFront.fillAmount += amount;
        if(uiBarFront.fillAmount >= 1)
        {
            if(color.Length > tierBar)
                FilledBar();
        }
    }
    private void FilledBar()
    {
        uiBarBackground.color = ChangeColorVar(uiBarBackground.color, uiBarFront.color);
        uiBarFront.color = ChangeColorVar(uiBarFront.color, color[tierBar]);
        tierBar += 1;
        uiBarFront.fillAmount = 0;
    }

    private Color ChangeColorVar(Color colorBase, Color colorToChange) {
        Color saveColor = colorBase;
        saveColor.r = colorToChange.r;
        saveColor.g = colorToChange.g;
        saveColor.b = colorToChange.b;
        return saveColor;
    }
    public bool UsedPowerUp(int powertier)
    {
        bool canUsed = false;
        if(powertier <= tierBar-2)
        {
            canUsed = true;
            tierBar -= powertier;
            tierBar -= 2;
            ChangeColorPairs();
        }
        return canUsed;
    }
}
