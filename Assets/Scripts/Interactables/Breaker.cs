using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    public bool isOn;
    public List<Breaker> connectedBreakers = new List<Breaker>();

    [SerializeField] private TextMeshProUGUI statusText;

    public void Toggle()
    {
        isOn = !isOn;

        foreach (var b in connectedBreakers)
        {
            b.isOn = !b.isOn;
        }
    }
}