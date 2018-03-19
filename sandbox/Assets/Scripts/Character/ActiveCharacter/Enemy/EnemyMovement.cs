using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character2D
{
    public class EnemyMovement : CharacterMovement
    {
        public EnemyAttack enemyAttack;
        public AIJumpTrigger topJump;
        public AIJumpTrigger botJump;
        public bool isScanning;

        [Range(1.0f, 5.0f)]
        [Tooltip("How long(seconds) should the AI stop and scan an area for (1s min, 5s max). This number is how long the AI is looking in either direction.")]
        public float stopAndScanTime;
        // Use this for initialization
        private new void Start()
        {
            base.Start();
            mvmtSpeed = 1.0f;
        }

        // Update is called once per frame
        private new void Update()
        {
            if (enemyAttack.isWindingUp)
            {
                mvmtSpeed = 0.0f;
            }
            base.Update();
        }

        public void JumpAttempt()
        {
            jumpInput = false;

            if (topJump.currentObjects.Count == 0 && botJump.currentObjects.Count != 0)
            {
                jumpInput = true; //send jump input
            }

        }

        public void ChangeDirection()
        {
            if (isFacingRight)
            {
                mvmtSpeed = -1.0f;
            }
            else
            {
                mvmtSpeed = 1.0f;
            }
        }

        public IEnumerator StopAndScan()
        {
            Debug.Log("Scanning");
            float originalFace;
            originalFace = mvmtSpeed;

            mvmtSpeed = 0;

            yield return new WaitForSeconds(stopAndScanTime);

            if (isFacingRight)
            {
                mvmtSpeed = -1.0f;
                Debug.Log("-1.0");
            }
            else
            {
                mvmtSpeed = 1.0f;
                Debug.Log("1");
            }
            yield return new WaitForSeconds(0.5f);

            mvmtSpeed = 0;
            yield return new WaitForSeconds(stopAndScanTime);
            isScanning = false;
            mvmtSpeed = originalFace; //set the original facing

            yield return null;
        }
    }
}
