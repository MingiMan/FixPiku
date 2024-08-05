using UnityEngine;

public enum MonsterType
{
    Pig,
    Cow,
    Chicken,
    Sheep,
    Wolf,
    Snake
}

    [CreateAssetMenu(fileName = "Monster Profile", menuName = "New Monster/Basic")]
public class BasicMonsters : MonoBehaviour
{
    [System.Serializable]
    public class Base
    {
        public int health;
        public int atkDamage;
        public float atkSpeed;
        public float walkSpeed;
        public float runSpeed;
        public float walkTime;
        public float waitTime;
        public float runTime;
        public MonsterType monsterType;
    }
}
