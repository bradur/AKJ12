using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotPartsConfig", menuName = "New RobotPartsConfig")]
public class RobotPartsConfig : ScriptableObject
{
    [SerializeField]
    List<RobotPartConfig> partConfigs;
    private System.Random random;

    public RobotPartsConfig() {
        random = new System.Random();
    }

    public RobotPartConfig getRandomPart() {
        float sum = 0f;
        List<WeightPair> set = new List<WeightPair>(partConfigs.Count);
        foreach (RobotPartConfig conf in partConfigs) {
            sum += conf.probability;
            WeightPair p = new WeightPair(sum, conf);
            set.Add(p);
        }
        double randomVal = random.NextDouble() * sum;
        return set.Where(x => x.weight >= randomVal).FirstOrDefault().config;
    }

    private class WeightPair {
        public WeightPair(float w, RobotPartConfig conf) {
            weight = w;
            config = conf;
        }
        public float weight;
        public RobotPartConfig config;
    }
}

[Serializable]
public class RobotPartConfig {
    public float value = 1f;
    public Sprite sprite;
    public float probability;
}