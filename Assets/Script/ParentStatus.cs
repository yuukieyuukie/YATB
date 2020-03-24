using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ParentStatus{

    int GetHp();

    int GetMaxHp();

    bool isLifeZero();

    void TakeDamage(int damage);
}