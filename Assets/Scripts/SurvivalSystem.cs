using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    InputManager inputManager;

    [Header("Stamina")]
    public float maxStamina = 10;
    public float currentStamina;
    public float currentStaminaDelayCounter;
    public float staminaDepletionRate = 1;
    public float staminaRechargeDelay = 1;
    public float staminaRechargeRate = 0.1f;
    public float staminaPercent;

    [Header("Hunger")]
    public float maxHunger = 10;
    public float currentHunger;
    public float hungerDepletionRate = 1;
    public float hungerPercent;

    [Header("Thirst")]
    public float maxThirst = 10;
    public float currentThirst;
    public float thirstDepletionRate = 1;
    public float thirstPercent;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
        currentStamina = maxStamina;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    private void Update()
    {
        hungerPercent = currentHunger / maxHunger;
        staminaPercent = currentStamina / maxStamina;
        thirstPercent= currentThirst / maxThirst;

        currentHunger -= hungerDepletionRate * Time.deltaTime;
        currentThirst -= thirstDepletionRate * Time.deltaTime;

        if (currentHunger <= 0 || currentThirst <= 0)
        {
            currentHunger = 0;
            currentThirst = 0;
            //Die?
        }
        
        if(inputManager.runInput)
        {
            if(currentStamina>=0) {
                currentStamina -= staminaDepletionRate * Time.deltaTime;
            }
            currentStaminaDelayCounter = 0;
        }
        else if(currentStamina<maxStamina)
        {
            if(currentStaminaDelayCounter<staminaRechargeDelay)
            {
                currentStaminaDelayCounter += Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
            }
        }
    }

    public void RefillHungerAndThirst(float hungerAmount, float thirstAmount)
    {
        currentHunger += hungerAmount;
        currentThirst += thirstAmount;

        if(currentHunger>maxHunger)
        {
            currentHunger = maxHunger;
        }
        if(currentStamina>maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
}
