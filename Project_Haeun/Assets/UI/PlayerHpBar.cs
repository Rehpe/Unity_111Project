using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpBar : HpBar
{
    public TextMeshProUGUI hpText;

    public override void UpdateHpBar(float currentHp, float maxHp)
    {
        base.UpdateHpBar(currentHp, maxHp);
        UpdateHpText(currentHp);
    }
    
    public void UpdateHpText(float hp)
    {
        hpText.text = hp.ToString("F0");
    }
}
