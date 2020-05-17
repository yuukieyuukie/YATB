using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ParentStatus{

    int GetHp();

    int GetMaxHp();

    void TakeDamage(int damage);
}