using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    [SerializeField] int woodDamage;
    [SerializeField] int rockDamage;
    [SerializeField] int monsterDamage;
    public ParticleSystem explosion;
}
