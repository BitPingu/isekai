using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;

    public void SetHUD(BattleData data)
    {
        nameText.text = data.name;
        levelText.text = "Lvl " + data.level;
        hpSlider.maxValue = data.maxHP;
        hpSlider.value = data.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
