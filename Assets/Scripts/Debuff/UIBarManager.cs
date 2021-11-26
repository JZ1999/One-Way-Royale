using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{

    public Image uiBarBackground;
    public Image uiBarFront;
    public Image uiIconDebuff;
    public Sprite _uiIconDebuffSprite;
    public int tierBar = 0;
    public int tierDebuff = -1;
    public Color[] color;
    public Debuff[] debuffs; 

    // Start is called before the first frame update
    void Start()
    {
        _uiIconDebuffSprite = uiIconDebuff.sprite;
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
            tierDebuff += 1;
            if(debuffs.Length > tierDebuff )
                uiIconDebuff.sprite = debuffs[tierDebuff].logo;
            if (color.Length > tierBar)
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
    public string UsedPowerUp()
    {
        string nameDebuff = "none";
        if (tierDebuff >= 0)
        {
            if (debuffs.Length > tierDebuff)
                nameDebuff = debuffs[tierDebuff].nameDebuff;
            else
                nameDebuff = "object";
            tierBar = 0;
            tierDebuff = -1;
            uiIconDebuff.sprite = _uiIconDebuffSprite;
            ChangeColorPairs();
        }
        return nameDebuff;
    }
}
