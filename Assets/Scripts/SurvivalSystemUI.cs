using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurvivalSystemUI : MonoBehaviour
{
    public GameObject Canvas;
    public Slider thirsh;
    public Slider hunger;
    public Slider stamina;

    SurvivalSystem survivalSystem;

    private void Awake()
    {
        survivalSystem= GameObject.FindGameObjectWithTag("Fox").GetComponent<SurvivalSystem>();
        stamina = Canvas.transform.GetChild(0).GetComponent<Slider>();
        thirsh = Canvas.transform.GetChild(1).GetComponent<Slider>();
        hunger = Canvas.transform.GetChild(2).GetComponent<Slider>();
    }

    private void Update()
    {
        thirsh.value = survivalSystem.thirstPercent;
        hunger.value = survivalSystem.hungerPercent;
        stamina.value = survivalSystem.staminaPercent;
    }
}
