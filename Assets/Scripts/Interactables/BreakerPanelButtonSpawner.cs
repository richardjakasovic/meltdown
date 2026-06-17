using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakerPanelButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject breakerButtonPrefab;
    public List<bool> breakerList = new List<bool>();
    private SortedSet<string> breakerChars = new SortedSet<string> { "A", "B", "C", "D", "E", "F" };

    void Start()
    {
        List<string> chars = new List<string>(breakerChars);

        for (int i = 0; i < 4; i++)
        {
            GameObject newButton = Instantiate(breakerButtonPrefab, transform);
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = chars[i];

        }
    }
}