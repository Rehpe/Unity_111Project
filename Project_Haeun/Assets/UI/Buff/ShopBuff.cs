using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuff : Buff
{
    public GameObject Shop;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Execute()
    {
        Debug.Log("상점");
        GameManager.Instance.Pause();
        Shop = GameManager.Instance.shop;
        Shop.SetActive(true);
    }
}
