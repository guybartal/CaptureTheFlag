using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour {

    public float attackDistance;
    Targeter targeter;

	// Use this for initialization
	void Start ()
    {
        targeter = GetComponent<Targeter>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (targeter.target && targeter.IsInRange(attackDistance))
        {
            Debug.Log("attacking" + targeter.target.name);
        }
	}
}
