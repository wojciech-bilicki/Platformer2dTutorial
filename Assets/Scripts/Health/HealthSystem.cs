using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private void Awake()
    {
        GetComponent<HealthUI>().Init(maxHealth);
    }
}
