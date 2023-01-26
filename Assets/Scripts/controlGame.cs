using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlGame : MonoBehaviour
{
    public Slider slider;

    player player1;

    void Start()
    {
        player1 = FindObjectOfType<player>();
    }

    void Update()
    {
        slider.value = player1.health;
    }
}
