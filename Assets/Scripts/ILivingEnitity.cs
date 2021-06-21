using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for Living things, so they can loose/gain health and have health.
//With an Interface each object that has this will be able to have its' own implementation.

public interface ILivingEntity
{
    public void takeDamage(int damage, Vector3 point);
    public void gainHealth(int gainAmmount);
}
