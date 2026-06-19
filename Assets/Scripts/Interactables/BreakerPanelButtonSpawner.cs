using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BreakerPanelButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject breakerPrefab;
    [SerializeField] private GameObject breakerStatusText;
    private SortedSet<string> breakerChars = new SortedSet<string> { "A", "B", "C", "D", "E", "F" };
    public int difficulty = 6;
    public List<Breaker> breakers = new List<Breaker>();
    private List<TextMeshProUGUI> statusTexts = new List<TextMeshProUGUI>();

    void Start()
    {
        List<string> chars = new List<string>(breakerChars);
        for (int i = 0; i < difficulty; i++)
        {
            GameObject newBreaker = Instantiate(breakerPrefab, transform);
            var breakerScript = newBreaker.GetComponent<Breaker>();
            var breakerText = newBreaker.GetComponentInChildren<TextMeshProUGUI>();
            breakerText.text = chars[i];
            breakers.Add(breakerScript);

            GameObject statusTextObject = Instantiate(breakerStatusText, transform);
            TextMeshProUGUI statusText = statusTextObject.GetComponentInChildren<TextMeshProUGUI>();
            statusTexts.Add(statusText);
            statusText.text = chars[i] + " -> OFF";
        }

        GenerateConnections();
        ScrambleBreakers();
    }

    void Update()
    {
        for (int i = 0; i < breakers.Count; i++)
        {
            statusTexts[i].text = breakers[i].isOn ? "ON" : "OFF";
        }
    }

    public bool IsSolved()
    {
        return breakers.All(b => b.isOn);
    }

    void GenerateConnections()
    {
        System.Random rand = new System.Random();
        foreach (var breaker in breakers)
        {
            breaker.connectedBreakers.Clear();
            int connectionCount = rand.Next(0, 2);
            for (int i = 0; i < connectionCount; i++)
            {
                Breaker target = GetValidTarget(breaker, rand);
                if (target != null)
                    breaker.connectedBreakers.Add(target);
            }
        }
    }

    Breaker GetValidTarget(Breaker source, System.Random rand)
    {
        List<Breaker> candidates = breakers
            .Where(b => b != source && !source.connectedBreakers.Contains(b))
            .ToList();

        return candidates.Count == 0 ? null : candidates[rand.Next(candidates.Count)];
    }

    void ScrambleBreakers()
    {
        // Start from solved state
        foreach (var breaker in breakers)
            breaker.isOn = true;

        // Apply random valid moves — guarantees solvability
        int shuffleMoves = Random.Range(8, 15);
        for (int i = 0; i < shuffleMoves; i++)
        {
            int randomIndex = Random.Range(0, breakers.Count);
            breakers[randomIndex].Toggle();
        }

        // If we accidentally landed back on solved, try again
        if (IsSolved())
            ScrambleBreakers();
    }
}