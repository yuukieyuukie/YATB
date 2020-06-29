using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ParentStatus{

    float GetHp();

    float GetMaxHp();

    void TakeDamage(int damage);
}