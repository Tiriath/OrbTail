using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAI : MonoBehaviour, IInputBroker
{
    /// <summary>
    /// Returns the throttle input status. -1 reverse, +1 maximum throttle.
    /// </summary>
    public float ThrottleInput { get; private set; }

    /// <summary>
    /// Returns the steer input status. -1 steer left, +1 steer right.
    /// </summary>
    public float SteerInput { get; private set; }

    /// <summary>
    /// Returns the fire input status.
    /// </summary>
    public bool PowerUpInput { get; private set; }

    /// <summary>
    /// Returns the special input status.
    /// </summary>
    public bool SpecialInput { get; private set; }
    
    //private EventLogger eventLogger;
    private PowerController powerController;
    
    private bool gameBuilt = false;
    private GameObject target = null;
    private Vector3 desideredDirection;
    private bool alreadyCollided = false;
    
    private OrbController orbController;
    private FloatingObject floatingObject;

    private float maxTimeToGoAway = 4f;
    private float maxTimeToFirePowerUp = 5f;
    private float maxVisibility = 60f;
    private float maxAcceleration = 0.9f;
    private int minOrbsToStartFight = 5;
    private float thresholdToGiveUp = 0.4f;
    private float sqrCheckpointDistanceThreshold = 15f;
    private float minTimeToGiveUp = 20f;
    private float maxTimeToGiveUp = 40f;

    private HashSet<GameObject> checkpoints;

    private void OnFieldOfViewEnter(object sender, Collider other) {
        GameObject colObject = other.gameObject;
        if (target == null || IsPatrolling()) {
            StopCoroutine("GiveUpHandler");

            if (colObject.tag == Tags.Ship && colObject.GetComponent<Ship>().TailLength >= minOrbsToStartFight) {
                target = colObject;
                StartCoroutine("GiveUpHandler");
            }
            else if (IsFreeOrb(colObject)) {
                target = colObject;
                orbController = colObject.GetComponent<OrbController>();
            }
        }
    }

    private IEnumerator GiveUpHandler() {
        yield return new WaitForSeconds(Random.Range(minTimeToGiveUp, maxTimeToGiveUp));
        ResetTarget();
    }
    
    public void Start ()
    {
        Ship.ShipCreatedEvent += OnShipCreated;
        Ship.ShipDestroyedEvent += OnShipDestroyed;

        floatingObject = GetComponent<FloatingObject>();
        powerController = GetComponent<PowerController>();

        powerController.PowerAcquiredEvent += OnEventPowerAttached;

        GetComponent<FightController>().FightEvent += OnEventFight;

        GetComponent<Ship>().OrbAttachedEvent += OnEventOrbAttached;

        GetComponentInChildren<AIFieldOfView>().EventOnFieldOfViewEnter += OnFieldOfViewEnter;

        checkpoints = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag(Tags.AICheckpoint));

        gameBuilt = true;
    }

    public void OnDestroy()
    {
        Ship.ShipCreatedEvent -= OnShipCreated;
        Ship.ShipDestroyedEvent -= OnShipDestroyed;

        powerController.PowerAcquiredEvent -= OnEventPowerAttached;

        GetComponent<FightController>().FightEvent -= OnEventFight;

        GetComponent<Ship>().OrbAttachedEvent -= OnEventOrbAttached;

        GetComponentInChildren<AIFieldOfView>().EventOnFieldOfViewEnter -= OnFieldOfViewEnter;

        var ship = gameObject.GetComponent<Ship>();

        if(ship)
        {
            ship.OrbAttachedEvent -= OnEventOrbAttached;
        }
    }

    // Update is called once per frame
    void Update () {
        if (gameBuilt) {
            CheckVisibility();
            
            if (target != null) {
                
                if (target.tag == Tags.Ship) {
                    
                    if (!alreadyCollided) {
                        ChasingPlayer();
                    }
                    else {
                        GoAway();
                    }
                    
                }
                else if (IsPatrolling()) {
                    Patrolling();
                }
                else {
                    ChasingOrb();
                }
            }
            else {
                ChangeCheckpoint();
            }
            
            AvoidOstacles();
        }
        
    }

    public void UpdateInput()
    {
        float steering = Vector3.Dot(-floatingObject.ArenaDown, Vector3.Cross(transform.forward, desideredDirection.normalized));

        SteerInput = Mathf.Clamp(steering * 5f, -1f, 1f);
        ThrottleInput = Mathf.Min(maxAcceleration, 1f - Mathf.Clamp01(Mathf.Abs(steering)));
    }

    void ChasingOrb() {
        if (!orbController.IsLinked) {
            Vector3 relVector = target.transform.position - gameObject.transform.position;
            desideredDirection = relVector;
        }
        else {
            ResetTarget();
        }
    }
    
    void GoAway() {
        Vector3 relVector = gameObject.transform.position - target.transform.position;
        desideredDirection = relVector;
    }

    void ChasingPlayer() {

        /* Now there's OnShipEliminated
        if (target.activeSelf == false) {
            ResetTarget();
            return;
        } */

        Vector3 relVector = target.transform.position - gameObject.transform.position;
        
        desideredDirection = relVector;
    }
    
    void AvoidOstacles()
    {
        // Yep, totally avoiding those obstacles!
    }
    
    void Patrolling() {
        Vector3 relVector = target.transform.position - gameObject.transform.position;
        desideredDirection = relVector;
        
        if (desideredDirection.sqrMagnitude < sqrCheckpointDistanceThreshold) {
            ChangeCheckpoint();
        }
    }
    
    void OnEventFight(GameObject attacker, GameObject defender, IList<GameObject> orbs) {
        
        if (attacker == this.gameObject && defender == target) {
            alreadyCollided = true;
            StartCoroutine("DecideWhetherContinueFight");
        }
        
    }
    
    void OnEventOrbAttached(Ship ship, GameObject orb)
    {
        if (orb == target)
        {
            ResetTarget();
        }
    }

    /// <summary>
    /// Called whenever a new ship is created.
    /// </summary>
    private void OnShipCreated(Ship ship)
    {
        ship.GetComponent<FightController>().FightEvent += OnEventFight;
    }

    /// <summary>
    /// Called whenever an existing ship is destroyed.
    /// </summary>
    private void OnShipDestroyed(Ship ship)
    {
        ship.GetComponent<FightController>().FightEvent -= OnEventFight;

        if (ship == target)
        {
            ResetTarget();
        }
    }

    private void CheckVisibility() {
        
        if (target != null && !IsPatrolling() && !IsVisible()) {
            ResetTarget();
        }
        
    }
    
    private IEnumerator DecideWhetherContinueFight() {
        float timeToWait = Random.value * maxTimeToGoAway;
        yield return new WaitForSeconds(timeToWait);
        alreadyCollided = false;
        
        if (Random.value <= thresholdToGiveUp) {
            ResetTarget();
        }
    }
    
    private void OnEventPowerAttached(PowerController sender, PowerUp power) {
        if (sender.GetComponent<Ship>() == gameObject)
        {
            StartCoroutine("FirePowerUp");
        }
    }
    
    private IEnumerator FirePowerUp() {
        float timeToWait = Random.value * maxTimeToFirePowerUp;
        yield return new WaitForSeconds(timeToWait);
        PowerUpInput = true;
    }
    
    private bool IsFreeOrb(GameObject orb) {
        return orb.tag == Tags.Orb && !orb.GetComponent<OrbController>().IsLinked;
    }
    
    private bool IsVisible() {
        if (target != null) {
            Vector3 relVector = target.transform.position - transform.position;
            float distance = relVector.magnitude;
            // If raycast collide something -> not visible
            return distance < maxVisibility && !Physics.Raycast(transform.position, relVector, distance, Layers.FieldAndObstacles);
        }
        
        return false;
    }
    
    private bool IsPatrolling() {
        return checkpoints.Contains(target);
    }
    
    private void ChangeCheckpoint() {
        var enumerator = checkpoints.GetEnumerator();
        
        int i = 0;
        int chosenNumber = Random.Range(0, checkpoints.Count);
        GameObject newTarget = null;
        
        while (i < chosenNumber) {
            newTarget = enumerator.Current;
            enumerator.MoveNext();
            i++;
        }
        
        target = newTarget;
    }
    
    private void ResetTarget() {
        target = null;
        orbController = null;
    }

}
