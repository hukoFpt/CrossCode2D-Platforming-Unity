using UnityEngine;
using System.Collections.Generic;

namespace CrossCode2D.Player
{
    [System.Serializable]
    public class PlayerStats
    {
        public int level = 1;
        public int experience = 0;
        public float maxHP;
        public float attack;
        public float defense;
        public float currentHP;

        private static readonly Dictionary<int, float> levelToMaxHP = new Dictionary<int, float>
        {
            { 1, 207f }, { 2, 209f }, { 3, 214f }, { 4, 224f }, { 5, 230f },
            { 6, 232f }, { 7, 238f }, { 8, 248f }, { 9, 256f }, { 10, 258f },
            { 11, 264f }, { 12, 275f }, { 13, 283f }, { 14, 286f }, { 15, 292f },
            { 16, 305f }, { 17, 314f }, { 18, 316f }, { 19, 323f }, { 20, 337f },
            { 21, 347f }, { 22, 349f }, { 23, 357f }, { 24, 373f }, { 25, 383f },
            { 26, 386f }, { 27, 394f }, { 28, 411f }, { 29, 423f }, { 30, 426f },
            { 31, 435f }, { 32, 453f }, { 33, 466f }, { 34, 469f }, { 35, 480f },
            { 36, 500f }, { 37, 513f }, { 38, 517f }, { 39, 528f }, { 40, 550f },
            { 41, 565f }, { 42, 569f }, { 43, 581f }, { 44, 605f }, { 45, 622f },
            { 46, 626f }, { 47, 639f }, { 48, 666f }, { 49, 683f }, { 50, 688f },
            { 51, 703f }, { 52, 732f }, { 53, 751f }, { 54, 756f }, { 55, 772f },
            { 56, 804f }, { 57, 825f }, { 58, 831f }, { 59, 848f }, { 60, 883f },
            { 61, 906f }, { 62, 912f }, { 63, 931f }, { 64, 969f }, { 65, 994f },
            { 66, 1001f }, { 67, 1022f }, { 68, 1063f }, { 69, 1091f }, { 70, 1099f },
            { 71, 1121f }, { 72, 1166f }, { 73, 1197f }, { 74, 1205f }, { 75, 1230f },
            { 76, 1279f }, { 77, 1312f }, { 78, 1321f }, { 79, 1349f }, { 80, 1402f },
            { 81, 1438f }, { 82, 1449f }, { 83, 1478f }, { 84, 1537f }, { 85, 1577f },
            { 86, 1588f }, { 87, 1620f }, { 88, 1684f }, { 89, 1728f }, { 90, 1740f },
            { 91, 1775f }, { 92, 1845f }, { 93, 1893f }, { 94, 1906f }, { 95, 1945f },
            { 96, 2022f }, { 97, 2073f }, { 98, 2088f }, { 99, 2130f }
        };

        private static readonly Dictionary<int, float> levelToAttack = new Dictionary<int, float>
        {
            { 1, 20f }, { 2, 21f }, { 3, 21f }, { 4, 22f }, { 5, 23f },
            { 6, 23f }, { 7, 23f }, { 8, 24f }, { 9, 25f }, { 10, 26f },
            { 11, 26f }, { 12, 27f }, { 13, 28f }, { 14, 29f }, { 15, 29f },
            { 16, 30f }, { 17, 31f }, { 18, 32f }, { 19, 32f }, { 20, 33f },
            { 21, 34f }, { 22, 35f }, { 23, 35f }, { 24, 36f }, { 25, 38f },
            { 26, 39f }, { 27, 39f }, { 28, 40f }, { 29, 42f }, { 30, 43f },
            { 31, 43f }, { 32, 44f }, { 33, 46f }, { 34, 47f }, { 35, 48f },
            { 36, 49f }, { 37, 51f }, { 38, 52f }, { 39, 52f }, { 40, 54f },
            { 41, 56f }, { 42, 57f }, { 43, 58f }, { 44, 59f }, { 45, 62f },
            { 46, 63f }, { 47, 63f }, { 48, 65f }, { 49, 68f }, { 50, 69f },
            { 51, 70f }, { 52, 72f }, { 53, 75f }, { 54, 76f }, { 55, 77f },
            { 56, 79f }, { 57, 82f }, { 58, 84f }, { 59, 84f }, { 60, 87f },
            { 61, 90f }, { 62, 92f }, { 63, 93f }, { 64, 95f }, { 65, 99f },
            { 66, 101f }, { 67, 102f }, { 68, 104f }, { 69, 109f }, { 70, 111f },
            { 71, 112f }, { 72, 115f }, { 73, 119f }, { 74, 122f }, { 75, 123f },
            { 76, 126f }, { 77, 131f }, { 78, 133f }, { 79, 134f }, { 80, 138f },
            { 81, 143f }, { 82, 146f }, { 83, 147f }, { 84, 151f }, { 85, 157f },
            { 86, 160f }, { 87, 162f }, { 88, 166f }, { 89, 172f }, { 90, 176f },
            { 91, 177f }, { 92, 182f }, { 93, 189f }, { 94, 193f }, { 95, 194f },
            { 96, 199f }, { 97, 207f }, { 98, 211f }, { 99, 213f }
        };

        private static readonly Dictionary<int, float> levelToDefense = new Dictionary<int, float>
        {
            { 1, 20f }, { 2, 21f }, { 3, 21f }, { 4, 22f }, { 5, 22f },
            { 6, 23f }, { 7, 24f }, { 8, 24f }, { 9, 25f }, { 10, 26f },
            { 11, 27f }, { 12, 27f }, { 13, 27f }, { 14, 28f }, { 15, 29f },
            { 16, 30f }, { 17, 30f }, { 18, 31f }, { 19, 33f }, { 20, 33f },
            { 21, 33f }, { 22, 35f }, { 23, 36f }, { 24, 37f }, { 25, 37f },
            { 26, 38f }, { 27, 40f }, { 28, 40f }, { 29, 41f }, { 30, 42f },
            { 31, 44f }, { 32, 45f }, { 33, 45f }, { 34, 47f }, { 35, 49f },
            { 36, 49f }, { 37, 50f }, { 38, 52f }, { 39, 54f }, { 40, 54f },
            { 41, 55f }, { 42, 57f }, { 43, 59f }, { 44, 60f }, { 45, 60f },
            { 46, 63f }, { 47, 65f }, { 48, 66f }, { 49, 66f }, { 50, 69f },
            { 51, 71f }, { 52, 72f }, { 53, 73f }, { 54, 76f }, { 55, 78f },
            { 56, 79f }, { 57, 80f }, { 58, 83f }, { 59, 86f }, { 60, 87f },
            { 61, 88f }, { 62, 91f }, { 63, 95f }, { 64, 96f }, { 65, 97f },
            { 66, 100f }, { 67, 104f }, { 68, 105f }, { 69, 106f }, { 70, 110f },
            { 71, 114f }, { 72, 115f }, { 73, 117f }, { 74, 121f }, { 75, 125f },
            { 76, 127f }, { 77, 128f }, { 78, 133f }, { 79, 137f }, { 80, 139f },
            { 81, 140f }, { 82, 145f }, { 83, 150f }, { 84, 152f }, { 85, 154f },
            { 86, 159f }, { 87, 165f }, { 88, 167f }, { 89, 169f }, { 90, 175f },
            { 91, 181f }, { 92, 183f }, { 93, 185f }, { 94, 191f }, { 95, 198f },
            { 96, 200f }, { 97, 203f }, { 98, 210f }, { 99, 217f }
        };

        public void InitializeStats()
        {
            maxHP = levelToMaxHP[level] + level *3;
            attack = levelToAttack[level] + level *2;
            defense = levelToDefense[level] + level * 2;
            currentHP = maxHP;
        }
    }
}