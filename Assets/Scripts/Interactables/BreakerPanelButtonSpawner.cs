using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BreakerPanelButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject breakerPrefab;
    [SerializeField] private GameObject breakerStatusText;
    private SortedSet<string> breakerChars = new SortedSet<string> { "A", "B", "C", "D", "E", "F" };
    public int difficulty = 4;
    public List<Breaker> breakers = new List<Breaker>();

    void Start()
    {
        List<string> chars = new List<string>(breakerChars);

        for (int i = 0; i < difficulty; i++)
        {
            GameObject newBreaker = Instantiate(breakerPrefab, transform);
            var breakerScript = newBreaker.GetComponent<Breaker>();
            breakers.Add(breakerScript);
            TextMeshProUGUI buttonText = newBreaker.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = chars[i];
            GameObject statusTextObject = Instantiate(breakerStatusText, transform);
            TextMeshProUGUI statusText = statusTextObject.GetComponentInChildren<TextMeshProUGUI>();

            
            if (i == 0)
            {
                statusText.text = "ON";
            }
            if (i == 1)
            {
                statusText.text = "OFF";
            }
            if (i == 2)
            {
                statusText.text = "ON";
            }
            if (i == 3)
            {
                statusText.text = "ON";
            }
        }
    }

    public bool isSolved()
    {
        return breakers.All(b => b.isOn);
    }
}