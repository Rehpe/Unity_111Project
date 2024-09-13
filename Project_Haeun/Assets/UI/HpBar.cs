using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HpBar : MonoBehaviour
{
    public float maxHp;
    public Image hpBarSlider;

    public virtual void SetMaxHp(float hp)
    {
        maxHp = hp;
        hpBarSlider.fillAmount = 1f;  
    }

    public virtual void SetHp(float currentHp)
    {
        UpdateHpBar(currentHp, maxHp);
    }

    public virtual void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBarSlider.fillAmount = currentHp / maxHp;
    }
}
