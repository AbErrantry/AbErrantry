using UnityEngine;

namespace Character2D
{
    public class BeaconManager : Trigger
    {
        
        public BehaviorAI aiBehavior;
        
        protected override void TriggerAction(bool isInTrigger)
        {
           // aiBehavior.SwitchBeacon();
        }
        
    }
}
