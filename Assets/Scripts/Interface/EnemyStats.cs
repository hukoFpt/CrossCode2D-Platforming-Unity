using UnityEngine;

namespace CrossCode2D.Enemies
{
    [System.Serializable]
    public class EnemyStats
    {
        [SerializeField] public int level;
        [SerializeField] public float maxHP;
        [SerializeField] public float currentHP;
        [SerializeField] public float attack;
        [SerializeField] public float defense;
        [SerializeField] public float heatResistance;
        [SerializeField] public float coldResistance;
        [SerializeField] public float shockResistance;
        [SerializeField] public float waveResistance;

        public void InitializeStats(int level, float maxHP, float currentHP, float attack, float defense, float heatResistance, float coldResistance, float shockResistance, float waveResistance)
        {
            this.level = level;
            this.maxHP = maxHP;
            this.currentHP = currentHP;
            this.attack = attack;
            this.defense = defense;
            this.heatResistance = heatResistance;
            this.coldResistance = coldResistance;
            this.shockResistance = shockResistance;
            this.waveResistance = waveResistance;
        }
    }
}