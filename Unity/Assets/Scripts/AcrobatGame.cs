using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;




public class AcrobatGame : MonoBehaviour
{
    private class TargetPair
    {
        public TargetPair(GameObject obj, float time)
        {
            this.obj = obj;
            this.time = time;
        }
        public GameObject obj;
        public float time;
    }
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField] private float growthRate;
    [SerializeField] private float maxTargetTime;
    [SerializeField] private float maxTime;
    [SerializeField] private float numTargets;
    [SerializeField] private GameObject target;
    [SerializeField] private Rect spawnArea;
    [SerializeField] private float zDepth;
    [SerializeField] private int targetExplodePenalty;
    [SerializeField] private int targetMissPenalty;
    [SerializeField] private int targetHitBonus;


    private List<TargetPair> targets;

    private float totalDuration;
    private int score;

    public void Awake()
    {
        targets = new List<TargetPair>();
        for (int i = 0; i < numTargets; i++)
        {
            
            string targetName = $"Target {i}";
            SpawnTarget(targetName);
        }
    }

    public void Update()
    {
        totalDuration += Time.deltaTime;
        foreach (TargetPair pair in targets.ToList())
        {
            pair.time += Time.deltaTime;
            if (pair.time >= maxTargetTime)
            {
                targets.Remove(pair);
                Destroy(pair.obj);
                SpawnTarget(pair.obj.name + "-1"); // This will create bad names really fast. Oh well
                score -= targetExplodePenalty;
            }
            else
            {
                float scale = Mathf.LerpUnclamped(target.transform.localScale.x, growthRate, pair.time);
                pair.obj.transform.localScale = new Vector3(scale,scale,scale);
            }
        }
        scoreText.SetText(score.ToString());
        timeText.SetText(TimeSpan.FromSeconds(maxTime - totalDuration).ToString("g"));
    }


    private void SpawnTarget(string targetName)
    {
        GameObject newTarget = Instantiate(target);
        newTarget.name = targetName;
        newTarget.transform.position = new Vector3(Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax),zDepth);
        targets.Add(new TargetPair(newTarget,0));

    }
}