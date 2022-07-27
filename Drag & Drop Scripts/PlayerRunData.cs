using UnityEngine;

[CreateAssetMenu(menuName = "Player Run Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerRunData : ScriptableObject
{
	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAccelTime; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
	[ReadOnly] public float runAccel; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDeccelTime; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
	[ReadOnly] public float runDeccel; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(10)]
	[Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0.01f, 1)] public float deccelInAir;


    private void OnValidate()
    {
		//Calculate are run acceleration & deceleration forces using formula (acceleration = velocity/time)
		runAccel = (runMaxSpeed) / runAccelTime;
		runDeccel = (runMaxSpeed) / runDeccelTime;

		#region NOT USED: Alternate Way to Calculate This
		// This commented out code is (in theory) the correct way to do this. Below is what you will get by rearranging
		// the movement formula, however the simpler, more intuitive, formula above seems to produce very similar results
		// and so in the video, I chose to just show it. But if you use this "more correct" method it will result in your character
		// reaching max speed (minus the necessary threshold) at the exact time you specify (when accelerating from 0 to maxSpeed).

		/*
		float threshold = 1;
		runAccel = (runMaxSpeed * threshold) / (runAccelTime - threshold);
		runDeccel = (runMaxSpeed * threshold) / (runDeccelTime - threshold);
		*/

		// A theshold is needed since, if we take a look at the acceleration graph, you'll see that we never actually reach are max speed.
		// We only get closer and closer to it and so a theshold (1.0 works great) is needed

		#endregion
	}
}
