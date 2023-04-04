using UnityEngine;
using UnityEngine.AI;
using AISystem.Common.Objects;
using AISystem.ItemSystem;
using AISystem.Common;

namespace AISystem
{
    public class AIController : MonoBehaviour
    {
        // In charge of animation and getting places
        [SerializeField]INavMeshAgent agent;
        Animator animator;
        AnimatorOverrideController animatorOverrideController;
        [SerializeField] AIDataBoard databoard;

        // Animation
        [SerializeField] bool moving = false;
        [SerializeField] bool dynamic = false;

        // Pause
        [SerializeField] bool wasDynamicOnPause = false;
        [SerializeField] Vector3? wasLookingAtOnPause = Vector3.zero;

        // Start is called before the first frame update
        void Awake()
        {
            agent = SettingsLoader.LoadNavMeshAgent(gameObject);
            animator = GetComponent<Animator>();
            databoard = GetComponent<AIDataBoard>();

            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            if (databoard)
            {
                databoard.Setup();
                databoard.UpdateState();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (databoard != null)
            {
                if (agent.RemainingDistance() < 0.5f && databoard.hasAction)
                {
                    moving = false;
                }

                animator.SetBool("Dynamic", dynamic);
                animator.SetBool("Moving", moving);

                if (databoard.HasLookAtTarget())
                {
                    this.transform.LookAt(databoard.GetLookAtTarget());
                }

                agent.SetStopped(databoard.IsPaused());

                if (!agent.IsStopped())
                {
                    if (databoard.IsBoarded())
                    {
                        agent = databoard.GetMount().GetComponent<INavMeshAgent>();
                    }

                    if (agent.HasPath())
                    {
                        moving = !databoard.IsBoarded();
                    }
                    else
                    {
                        //GoalReached(false);
                    }

                    if (databoard.HasGoal())
                    {
                        agent.SetDestination(databoard.GetCurrentGoal().Value);
                    }

                    if (databoard.hasAction && databoard.currentAction != null && databoard.currentAction.UpdateEachLoop)
                    {
                        Item item = databoard.GetGoalItem();

                        if (item != null)
                        {
                            databoard.SetCurrentGoal(item.transform.position);
                            agent.SetDestination(databoard.GetCurrentGoal().Value);
                        }
                    }

                    GoalReached(agent);
                }
            }
        }

        void GoalReached(INavMeshAgent currentAgent)
        {
            //Debug.Log("Getting to " + currentAgent.destination);

            databoard.UpdateState();
        }

        public bool CurrentlyMoving()
        {
            return moving;
        }

        public void SetIsStopped(bool stopAI, Vector3? newlookAt = null)
        {
            agent.SetStopped(stopAI);

            if (stopAI)
            {
                wasDynamicOnPause = dynamic;

                if (databoard.HasLookAtTarget())
                {
                    wasLookingAtOnPause = databoard.GetLookAtTarget();
                }

                databoard.SetLookAtTarget(newlookAt);
                dynamic = false;
                moving = false;
            }
            else
            {
                moving = false;
                
                if(wasLookingAtOnPause != null)
                {
                    databoard.SetLookAtTarget(wasLookingAtOnPause);
                    wasLookingAtOnPause = null;
                }

                if (wasDynamicOnPause)
                {
                    dynamic = true;
                    wasDynamicOnPause = false;
                }
            }
        }

        public INavMeshAgent GetAgent()
        {
            return agent;
        }

        #region Animations

        public void SetDynamicAnimation(string name, AnimationClip clip)
        {
            animatorOverrideController[name] = clip;
        }

        public void SetDynamicAnimation(bool newDynamic)
        {
            dynamic = newDynamic;
        }

        #endregion

    }
}