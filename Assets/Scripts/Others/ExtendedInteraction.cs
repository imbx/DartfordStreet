using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExtendedInteraction : MonoBehaviour {
    public List<int> RequirementIds;
    public UnityEvent Action;
    private bool hasExecutedEvent = false;

    private void Update()
    {
        bool generalCondition = true;

        if(RequirementIds.Count > 0 && !hasExecutedEvent)
        {
            // Debug.Log("[ExtendedInteraction] Checking ExtendedInteractions");
            foreach(int i in RequirementIds)
            {
                // Debug.Log("[ExtendedInteraction] Requirement " + i + " is " + GameController.current.database.GetProgressionState(i));
                if(!GameController.current.database.GetProgressionState(i)) generalCondition = false;
                // Debug.Log("[ExtendedInteraction] General condition is " + generalCondition);
            }
            if(generalCondition)
            {
                hasExecutedEvent = true;
                Debug.Log("[ExtendedInteraction] Executing ExtendedInteractions " + generalCondition);
                Action.Invoke();
            }
        }
    }
}