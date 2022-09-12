using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    public void StateStart();
    public void StateUpdate();
    public void OnTriggerEnter(Collider other);
    public void ChangeState(IEnemyState newState);
}
