using UnityEngine;

namespace Character2D
{
    public class BehaviorAI : MonoBehaviour
    {
        public CharacterMovement aiMovement; //reference to the character movement component
        public AICrouchTrigger topCrouch; //reference to the top trigger ai crouch trigger component
        public AICrouchTrigger botCrouch; //reference to the bottom trigger ai crouch trigger component
       
        public GameObject leftBeacon;
        public GameObject rightBeacon;
        public GameObject currBeacon;
        public GameObject closestBeacon;
        public float disBtwnBeacons;
        public float disToCurrBeacon;

        public float secondsToPatrol; //How many seconds should the AI patrol in one direction
        private float currDirection; 
        public float timeToDist;

        private static bool trackingPlayer;
        public bool beaconRight; //false will be left, true will be right
        //private bool shouldSwitch; TODO: use

        //used for initialization
        private void Start()
        {
            currDirection = 1;
            aiMovement.mvmtSpeed = 1;
            timeToDist = secondsToPatrol;
            beaconRight = true;
            //currBeacon = rightBeacon;
            //disBtwnBeacons = Vector2.Distance(rightBeacon.transform.position, leftBeacon.transform.position);
            //shouldSwitch = false;
        }

        //occurs after all Update methods
        //TODO: implement extents for ai pathing as opposed to time (and move out of update)
        private void LateUpdate()
        {
            if (Time.timeScale == 1 && aiMovement.jumpInput == false)
            {
               /*
               disToCurrBeacon = Vector2.Distance(this.transform.position,currBeacon.transform.position);
                Debug.Log(disToCurrBeacon);
                if ((trackingPlayer == false  && disToCurrBeacon <=0) || shouldSwitch == true)
                {
                    if (currBeacon == rightBeacon && disBtwnBeacons <= disToCurrBeacon)
                    {
                        SwitchBeacon();
                    }
                    else if(currBeacon == leftBeacon && disBtwnBeacons <= disToCurrBeacon)
                    {
                        SwitchBeacon();
                    }
                    shouldSwitch = false;
                }*/


                //OLD WALKING
                //As Long as the AI is not tracking the player then it can change directions.
                
                if (timeToDist <= 0 && trackingPlayer == false) 
                {
                    currDirection = aiMovement.mvmtSpeed * -1; //switch direction
                    aiMovement.mvmtSpeed = currDirection; 
                    timeToDist = secondsToPatrol; //Reset the time
                }
                else
                {
                    timeToDist -= Time.deltaTime;
                }


            }
        }

        //sets whether the character is tracking a target or not
        public void AITrack(bool shouldWalk)
        {
            if (shouldWalk)
            {
                aiMovement.runInput = true;
                trackingPlayer = true;
            }
            else
            {
                aiMovement.runInput = false;
                trackingPlayer = false;
            }
        }

        //ai tries to send input to jump or crouch depending on situation
        public void InputAttempt()
        {
            aiMovement.jumpInput = false;
            if(topCrouch.currentObjects.Count == 0 && botCrouch.currentObjects.Count != 0)
            {
                aiMovement.jumpInput = true; //send jump input
            }
        }
    }
}
