using UnityEngine;
using System.Collections;

namespace WhackAMole
{
    public class WAMMole : MonoBehaviour
    {
        // A referemce to the Game Controller, which is taken by the first time this script runs, and is remembered across all other scripts of this type
        static GameObject gameController;

        // The animated part of the mole. By default this is taken from this object
        internal Animator moleAnimator;

        [Tooltip("The helmet object of this mole, assigned from inside the mole itself")]
        public GameObject helmet;

        [Tooltip("The broken helmet that appears when the helmet breaks. This is assigned from the project pane")]
        public Transform brokenHelmet;

        [Tooltip("The health of the mole when it's wearing a helmet")]
        public int helmetHealth = 2;

        // The health of the mole when it's not wearing a helmet
        internal int health = 1;

        [Tooltip("A multiplier for the bonus we get when hitting this target")]
        public int bonusMultiplier = 1;

        [Tooltip("The tag of the object that can hit this mole")]
        public string targetTag = "Player";

        // Is the mole dead?
        internal bool isDead = false;

        // How long to wait before showing the mole
        internal float showTime = 0;

        // How long to wait before hiding the mole, after it has been revealed
        internal float hideDelay = 0;

        [Tooltip("The animation name when showing a mole")]
        public string animationShow = "MoleShow";

        [Tooltip("The animation name when hiding a mole")]
        public string animationHide = "MoleHide";

        [Tooltip("A list of animations when the mole dies. The animation is chosen randomly from the list")]
        public string[] animationDie = { "MoleSmack", "MoleWhack", "MoleThud" };

        // Use this for initialization
        void Start()
        {
            // Hold the gamcontroller object in a variable for quicker access
            if (gameController == null) gameController = GameObject.FindGameObjectWithTag("GameController");

            // The animator of the mole. This holds all the animations and the transitions between them
            moleAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        void Update()
        {
            // Count down the time until the mole is hidden
            if (isDead == false && hideDelay > 0)
            {
                hideDelay -= Time.deltaTime;

                // If enough time passes, hide the mole
                if (hideDelay <= 0) HideMole();
            }
        }

        /// <summary>
        /// Raises the trigger enter2d event. Works only with 2D physics.
        /// </summary>
        /// <param name="other"> The other collider that this object touches</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            // Check if we hit the correct target
            if (isDead == false && other.tag == targetTag)
            {
                // Give hit bonus for this target
                gameController.SendMessage("HitBonus", this.transform);

                // Change the health of the target
                ChangeHealth(-1);
            }
        }

        /// <summary>
        /// Changes the health of the target, and checks if it should die
        /// </summary>
        /// <param name="changeValue"></param>
        public void ChangeHealth(int changeValue)
        {
            // Chnage health value
            health += changeValue;

            if (health > 0)
            {
                // Animated the hit effect
                moleAnimator.Play("MoleHit", -1, 0f);
            }
            else
            {
                // Health reached 0, so the target is dead
                Die();
            }
        }

        /// <summary>
        /// Kills the object and gives it a random animation from a list of death animations
        /// </summary>
        public void Die()
        {
            // The mole is now dead. It can't move.
            isDead = true;

            // If there is a helment object, deactivate it and create a helmet break effect
            if (helmet && helmet.activeSelf == true)
            {
                // Create the helmet break effect
                if (brokenHelmet) Instantiate(brokenHelmet, helmet.transform.position, helmet.transform.rotation);

                // Deactivate the helmet object
                helmet.SetActive(false);
            }
            
            // Choose one of the death animations randomly
            if (animationDie.Length > 0) moleAnimator.Play(animationDie[Mathf.FloorToInt(Random.Range(0, animationDie.Length))]);
        }

        /// <summary>
        /// Hides the target, animating it and then sets it to hidden
        /// </summary>
        void HideMole()
        {
            // Play the hiding animation
            GetComponent<Animator>().Play(animationHide);
        }

        /// <summary>
        /// Destroys the target object ( this is called from the Animator Die clip )
        /// </summary>
        public void RemoveTarget()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Shows the regular mole
        /// </summary>
        /// <returns>The target.</returns>
        public void ShowMole(float showDuration)
        {
            // The mole is not dead anymore, so it can appear from the hole
            isDead = false;

            // If the mole has a helmet, deactivate it
            if (helmet) helmet.SetActive(false);

            // Set the health of the mole to 1 hit
            health = 1;

            // Play the show animation
            GetComponent<Animator>().Play(animationShow);

            // Set how long to wait before hiding the mole again
            hideDelay = showDuration;
        }

        /// <summary>
        /// Shows the mole with a helmet
        /// </summary>
        /// <returns>The target.</returns>
        public void ShowHelmet(float showDuration)
        {
            // The mole is not dead anymore, so it can appear from the hole
            isDead = false;

            // If the mole has a helmet, deactivate it
            if (helmet) helmet.SetActive(true);

            // Set the health of the mole to the helmet health
            health = helmetHealth;

            // Play the show animation
            GetComponent<Animator>().Play(animationShow);

            // Set how long to wait before hiding the mole again
            hideDelay = showDuration;
        }

        /// <summary>
        /// Shows the quick mole
        /// </summary>
        /// <returns>The target.</returns>
        public void ShowQuick(float showDuration)
        {
            // The mole is not dead anymore, so it can appear from the hole
            isDead = false;

            // If the mole has a helmet, deactivate it
            if (helmet) helmet.SetActive(false);

            // Set the health of the mole to 1 hit
            health = 1;

            // Play the show animation
            GetComponent<Animator>().Play("MoleQuick");

            // Set how long to wait before hiding the mole again
            hideDelay = 0;
        }

    }
}
