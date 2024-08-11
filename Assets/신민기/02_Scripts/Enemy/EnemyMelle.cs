using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyMelle : Enemys
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().OnDamage(Stats.atkDamage);
        }
    }
}
