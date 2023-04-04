using UnityEngine;
using AISystem.Civil.CivilAISystem.V2;
using AISystem.Civil.Objects.V2;
using AISystem.Civil;
using System.Collections.Generic;
using AISystem.Civil.NeedSystem;
using AISystem.Civil.Ownership;
using AISystem.Common;
using AISystem.ItemSystem;
using AISystem.Common.Objects;

namespace AISystem
{
    public class AIDataBoard : MonoBehaviour
    {
        // Main state of the character and info

        [SerializeField] static GameManager gameManager;

        AIController aiController;
        [SerializeField]AIControllerQueue queue;

        // Controllers - These can be disabled 
        [SerializeField] WorkController workController;
        [SerializeField] NeedController needController;

        // Data
        [SerializeField] House home;
        [SerializeField] string job;
        [SerializeField] AI_STATE state;
        [SerializeField] float timer;

        // Civil Service
        [SerializeField] DutyList dutyManager;
        [SerializeField] public Duty dutyList;
        [SerializeField] public string currentDuty;
        [SerializeField] public Action currentAction;
        static Actions actionsSystem;

        // Current State
        [SerializeField] AI_CONTROLLER currentController;
        [SerializeField] AI_CONTROLLER previousController = AI_CONTROLLER.NONE;
        [SerializeField] Vector3? currentGoalLocation;
        [SerializeField] Item currentGoalObj = null;
        [SerializeField] Vector3? currentLookAtTarget;
        [SerializeField] ITEMS currentItem;
        [SerializeField] MountController mounted = null;
        [SerializeField] bool inControlOfMount = false;
        [SerializeField] public bool atWork;
        [SerializeField] public bool hasAction = false;
        [SerializeField] int? nodeOnRoute = null;
        [SerializeField] bool isPaused = false;

        // Ownership
        [SerializeField] OwnershipManager ownershipManager = new OwnershipManager();
        [SerializeField] GameObject itemCarried = null;
        [SerializeField] List<Room> rooms = new List<Room>();
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;

        // Start is called before the first frame update
        public void Setup()
        {
            aiController = GetComponent<AIController>();

            if (!aiController)
            {
                aiController = gameObject.AddComponent<AIController>();
            }

            if (actionsSystem == null)
            {
                actionsSystem = new Actions();
            }

            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
            }

            queue = new AIControllerQueue();
        }

        private void Update()
        {
            if (!isPaused)
            {
                timer += Time.deltaTime;
            }
        }

        public void UpdateState()
        {
            float currentTime = gameManager.GetTime();

            if (!aiController.CurrentlyMoving())
            {
                state = AI_STATE.IDLE;
            }

            if (workController != null)
            {
                workController.UpdateWeighting(this, currentTime);
            }

            if (needController != null)
            {
                needController.UpdateWeighting(this, currentTime);
            }

            if (queue.HasCandidates())
            {
                AI_CONTROLLER aiController = queue.GetHighest();

                currentController = aiController;

                if (previousController != AI_CONTROLLER.NONE && previousController != currentController)
                {
                    ResetState();
                }

                switch (aiController)
                {
                    case AI_CONTROLLER.JOB:
                        workController.Manage(this, currentTime);
                       break;
                    case AI_CONTROLLER.NEED:
                        needController.Manage(this, currentTime);
                        break;
                    default:
                        SetCurrentGoal(GetHome().getEntance());
                        break;
                }

                previousController = currentController;
            }
        }

        public void ResetState()
        {
            currentLookAtTarget = null;
            nodeOnRoute = null;
            if (currentGoalObj) { currentGoalObj.SetInUse(false); }
            currentGoalObj = null;
            if (mounted) { Dismount(); }
            if (itemCarried) { DropItem(); }
            SetDynamicAnimation(false);
            SetCurrentGoal(null);
            hasAction = false;
        }

        public ACTIONS_TYPES GetCurrentAction()
        {
            return currentAction.actionType();
        }

        #region Goal

        public bool HasGoal()
        {
            if (currentGoalLocation == null)
            {
                return false;
            }

            return true;
        }

        public Vector3? GetCurrentGoal()
        {
            return currentGoalLocation;
        }

        public void SetCurrentGoal(Vector3? command)
        {
            currentGoalLocation = command;

            if (mounted && inControlOfMount)
            {
                if (currentGoalLocation != null)
                {
                    mounted.setDestination(currentGoalLocation.Value);
                }
            }
        }

        public Item GetGoalItem()
        {
            return currentGoalObj;
        }

        public void SetGoalItem(Item candidate, bool changeInUse = true)
        {
            if (candidate == null)
            {
                if (currentGoalObj != null)
                {
                    if (changeInUse)
                    {
                        currentGoalObj.SetInUse(!currentGoalObj.IsInUse());
                    }
                }
            }
            else
            {
                if (changeInUse)
                {
                    candidate.SetInUse(true);
                }
            }
            currentGoalObj = candidate;
        }

        public int? GetNodeInRoute(bool reverse = false)
        {
            if (nodeOnRoute == null)
            {
                if (reverse)
                {
                    nodeOnRoute = workController.GetWorkplace().GetRoute().Count - 1;
                }
                else
                {
                    nodeOnRoute = 0;
                }
            }

            return nodeOnRoute;
        }

        public bool UpdateNode(bool reverse = false)
        {
            if (reverse)
            {
                nodeOnRoute--;
            }
            else
            {
                nodeOnRoute++;
            }

            if (nodeOnRoute < 0 || nodeOnRoute > workController.GetWorkplace().GetRoute().Count - 1)
            {
                nodeOnRoute = null;
                return false;
            }

            return true;
        }

        #endregion

        #region Look

        public bool HasLookAtTarget()
        {
            if (currentLookAtTarget != null)
            {
                return true;
            }

            return false;
        }

        public void SetLookAtTarget(Vector3? value)
        {
            // Sets to NPC level
            if (value.HasValue)
            {
                value = new Vector3(value.Value.x, this.transform.position.y, value.Value.z);
            }
            currentLookAtTarget = value;
        }

        public Vector3 GetLookAtTarget()
        {
            return currentLookAtTarget.Value;
        }

        #endregion

        public string GetJobName()
        {
            return job;
        }

        #region Mounting
        public void SetBoarded(MountController board, bool inControl)
        {
            mounted = board;
            inControlOfMount = inControl;
        }

        public bool IsBoarded()
        {
            if (mounted == null)
            {
                return false;
            }

            return true;
        }

        public void Dismount()
        {
            mounted.RemoveUser();
            mounted = null;
            inControlOfMount = false;

            transform.parent = FindObjectOfType<CivilManager>().GetComponent<Transform>();
        }

        public MountController GetMount()
        {
            return mounted;
        }

        #endregion

        #region Controllers
        public WorkController GetWorkController()
        {
            return workController;
        }

        public NeedController GetNeedController()
        {
            return needController;
        }

        public Controller GetActiveController()
        {
            switch(currentController)
            {
                case AI_CONTROLLER.JOB:
                    return workController;
                case AI_CONTROLLER.NEED:
                    return needController;
            }

            return null;
        }

        public bool IsPreviousControllerDifferent()
        {
            return previousController != AI_CONTROLLER.NONE && currentController != previousController;
        }

        #endregion

        public AI_STATE GetState()
        {
            return state;
        }

        public void SetState(AI_STATE state)
        {
            this.state = state;
        }

        public void SetHasAction(bool value)
        {
            hasAction = value;
        }

        #region Inventory

        public bool CarryingItem()
        {
            if (itemCarried)
            {
                return true;
            }

            return false;
        }

        public int? CheckInventoryForItem(ITEMS item)
        {
            int? qty = null;

            if (item == currentItem)
            {
                qty = 1;
            }

            return qty;
        }

        public void PickUpItem(GameObject item)
        {
            currentItem = item.GetComponent<Item>().itemName;
            itemCarried = item;
            itemCarried.GetComponent<Rigidbody>().isKinematic = true;
            itemCarried.transform.position = rightHand.position;
            itemCarried.transform.SetParent(rightHand);
        }

        public void DropItem()
        {
            currentItem = ITEMS.NULL;
            itemCarried.transform.SetParent(null);
            itemCarried.GetComponent<Rigidbody>().isKinematic = false;
            itemCarried = null;
        }

        public GameObject GetCarriedItem()
        {
            return itemCarried;
        }

        #endregion

        #region Timer

        public void ResetTimer()
        {
            timer = 0f;
        }

        public float GetTimer()
        {
            return timer;
        }

        #endregion

        #region Animation

        public void SetDynamicAnimation(string name, AnimationClip clip)
        {
            aiController.SetDynamicAnimation(name, clip);
        }

        public void SetDynamicAnimation(bool dynamic)
        {
            aiController.SetDynamicAnimation(dynamic);
        }

        #endregion

        #region Ownership

        public OwnershipManager GetOwnershipManager()
        {
            return ownershipManager;
        }

        public void AddRoom(Room room)
        {
            rooms.Add(room);
            ownershipManager.Upsert(OWNERSHIP.PERSONAL, room.items);
        }

        public House GetHome()
        {
            return home;
        }

        #endregion

        #region Queue

        public AIControllerQueue GetQueue()
        {
            if (queue == null)
            {
                queue = new AIControllerQueue();
            }

            return queue;
        }

        #endregion

        public void SetGoalLocation(Vector3 new_location)
        {
            currentGoalLocation = new_location;
        }

        public void SetPaused(bool pause, Vector3? newLookAt = null)
        {
            isPaused = pause;
            aiController.SetIsStopped(isPaused, newLookAt);
        }

        public bool IsPaused()
        {
            return isPaused;
        }

        public void SetupCharacter(House set_home, Workplace set_work, CIVIL_JOBS set_job, GameManager gameManager, bool hasNeeds = true)
        {
            home = set_home;
            job = set_job.ToString();

            // Make Job Componet 
            var jobInfo = CivilJobData.GetInstance().GetJobs()?.GetJobDetails(set_job);

            workController = this.gameObject.AddComponent<WorkController>();
            workController.Setup(this, set_work, jobInfo, actionsSystem);

            if (hasNeeds)
            {
                needController = this.gameObject.AddComponent<NeedController>();
                needController.Setup(this);
            }

            AIDataBoard.gameManager = gameManager;
        }

        public void SetupCharacter(House set_home, Workplace set_work, Job job_data, GameManager gameManager, bool hasNeeds = true)
        {
            home = set_home;
            job = job_data.name;

            workController = this.gameObject.AddComponent<WorkController>();
            workController.Setup(this, set_work, job_data, actionsSystem);

            if(hasNeeds)
            {
                needController = this.gameObject.AddComponent<NeedController>();
                needController.Setup(this);
            }

            AIDataBoard.gameManager = gameManager;
        }

        public void SetupCharacter(House set_home, GameManager gameManager)
        {
            home = set_home;
            AIDataBoard.gameManager = gameManager;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (home != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(home.getEntance(), 0.2f);
            }

            if (currentGoalLocation.HasValue)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(currentGoalLocation.Value, 0.2f);
            }

            if (HasLookAtTarget())
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(currentLookAtTarget.Value, 0.2f);
            }

            try
            {
                var agent = aiController.GetAgent();

                if (agent != null)
                {
                    if (agent.HasPath())
                    {
                        Vector3[] nodes = agent.GetPathNodes();

                        for (int i = 0; i < nodes.Length - 1; i++)
                        {
                            Debug.DrawLine(nodes[i], nodes[i + 1], Color.red);
                        }
                    }
                }
            }
            catch (System.Exception exception)
            {
                exception = null;
            }
        }
#endif
    }
}
