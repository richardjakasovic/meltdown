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
    private List<TextMeshProUGUI> statusTexts = new List<TextMeshProUGUI>();

    void Start()
    {
        List<string> chars = new List<string>(breakerChars);

        for (int i = 0; i < difficulty; i++)
        {
            GameObject newBreaker = Instantiate(breakerPrefab, transform);
            var breakerScript = newBreaker.GetComponent<Breaker>();
            breakers.Add(breakerScript);

            GameObject statusTextObject = Instantiate(breakerStatusText, transform);
            TextMeshProUGUI statusText = statusTextObject.GetComponentInChildren<TextMeshProUGUI>();

            statusTexts.Add(statusText);

            // set label like A, B, C
            statusText.text = chars[i] + " -> OFF";
        }

        GenerateConnections();
        EnsureNoIsolatedBreakers();
    }

    void Update()
    {
        for (int i = 0; i < breakers.Count; i++)
        {
            statusTexts[i].text = breakers[i].isOn ? "ON" : "OFF";
        }
    }

    public bool isSolved()
    {
        return breakers.All(b => b.isOn);
    }

    void GenerateConnections()
    {
        System.Random rand = new System.Random();

        int n = breakers.Count;

        foreach (var breaker in breakers)
        {
            breaker.connectedBreakers.Clear();

            int connectionCount = rand.Next(1, 3); // 1–2 connections

            for (int i = 0; i < connectionCount; i++)
            {
                Breaker target = GetValidTarget(breaker, rand);

                if (target != null && !breaker.connectedBreakers.Contains(target))
                {
                    breaker.connectedBreakers.Add(target);
                }
            }
        }
    }

    Breaker GetValidTarget(Breaker source, System.Random rand)
    {
        List<Breaker> candidates = new List<Breaker>();

        foreach (var b in breakers)
        {
            // no self-links
            if (b == source)
                continue;

            // avoid duplicates
            if (source.connectedBreakers.Contains(b))
                continue;

            candidates.Add(b);
        }

        if (candidates.Count == 0)
            return null;

        return candidates[rand.Next(candidates.Count)];
    }

    void EnsureNoIsolatedBreakers()
    {
        foreach (var b in breakers)
        {
            if (b.connectedBreakers.Count == 0)
            {
                Breaker fallback = breakers[Random.Range(0, breakers.Count)];

                if (fallback != b)
                    b.connectedBreakers.Add(fallback);
            }
        }
    }
}