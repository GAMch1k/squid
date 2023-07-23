using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : MonoBehaviour
{
    public Animator anim;
    public GameObject menu;
    public GameObject ShadowMenu;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    private void ExecuteTrigger(string trigger)
    {
        if (menu != null)
        {
            var animator = menu.GetComponent<Animator>();
            var animator1 = ShadowMenu.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger(trigger);
                animator1.SetTrigger(trigger);
            }
        }
    }
    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pressed"))
        {

            onMenuExtendNo();
        }
    }
    public void onMenuExtendYes()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(true);
        ExecuteTrigger("sett");

    }
    public void onMenuExtendNo()
    {
        
        button1.SetActive(true);
        button2.SetActive(true);
        button3.GetComponent<Animator>().SetTrigger("Back");
        ExecuteTrigger("def");
        button3.SetActive(false);

    }
    
}