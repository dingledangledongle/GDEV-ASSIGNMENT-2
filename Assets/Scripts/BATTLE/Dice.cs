using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dice : MonoBehaviour
{
    //GOT THIS CODE FROM https://www.youtube.com/watch?v=9CTJRSCkG_k
    //ADJUSTED TO FIT MY GAME
    #region VARIABLES
    public Texture[] textureList;
    public AudioSource diceReadySFX;
    public AudioSource diceRollSFX;
    public AudioSource diceCollideDiceSFX;
    private Rigidbody rigidBody;
    private Vector3 gravity = new Vector3(0, 0, 80f);
    private bool hasLanded = false;
    private float currentRollTime = 0f;
    private float maxRollingTime = 6f;
    private Vector3 initialSpawn;
    private string material;
    #endregion

    private void Start()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        initialSpawn = transform.position;
        SetRandomState();
    }

    private void Update()
    {
        //Custom gravity pulling towards +ve Z direction as my game's depth is on the Z-axis
        //Using ForceMode.Force to apply constant force in that direction
        rigidBody.AddForce(gravity, ForceMode.Force);

        /* This checks if the dice has landed
         * If the dice exceeds the allowed time to roll, it would roll the dice again to prevent the player 
         * from getting stuck in a perpetual wait for the dice to reach a stable state
         */
        if (!hasLanded)
        {
            currentRollTime += Time.deltaTime;
            if(currentRollTime > maxRollingTime)
            {
                SetRandomState();
            }
        }

        /* This checks if the dice has stopped moving using the CheckDiceHasStopped() method.
         * Once it satisfies the condition, it would use FindFaceResult() which would return
         * the current face that is up.
         * It would also then update the texture of the currently up face to show the player that is has finished
         * rolling.
         * It would then check if the dice was already in the landed state, if it wasn't in that state,
         * it would play the SFX of it being ready and set isKinematic to true to prevent it from moving
         * It would then set hasLanded to true to indicate that it is in the landed state
         */
        if (CheckDiceHasStopped())
        {
            int result = FindFaceResult();
            ShowLitTexture(result);
            if (!hasLanded)
            {
                diceReadySFX.Play();
                rigidBody.isKinematic = true;
                hasLanded = true;
            }
        }
    }

    //Plays the corresponding sound depending on which object it has collided by comparing the tag
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            PlayDiceRollSFX();
        }

        if (collision.transform.CompareTag("Dice"))
        {
            PlayDiceCollideSFX();
        }
    }

    //this sets the timer for the amount of time that the dice is rolling to 0
    // it then produces an random force to roll the dice
    private void SetRandomState()
    {
        currentRollTime = 0f;
        AddRandomForce();
    }

    // This method resets the dice position to its initial location, then provies it with a random
    // force to roll it
    private void AddRandomForce()
    {
        transform.position = initialSpawn; // putting the dice at the initial spawn point
        int x = Random.Range(0, 360);
        int y = Random.Range(0, 360);
        int z = Random.Range(0, 360);
        Quaternion rotation = Quaternion.Euler(x, y, z); // puts the dice at a random rotation

        x = Random.Range(0, 20);
        y = Random.Range(0, 20);
        z = Random.Range(0, 20);
        Vector3 force = new(x, y, -z); // gets a random amount of force in the 3 axis

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);

        Vector3 torque = new(x, y, z);// gets a random amount of torque in the 3 axis

        transform.rotation = rotation;
        rigidBody.velocity = force;

        this.rigidBody.maxAngularVelocity = 1000; // uncaps the maximum rotational velocity so that the dice can spin fast
        rigidBody.AddTorque(torque, ForceMode.VelocityChange);
    }

    private bool CheckDiceHasStopped()
    {
        /* This checks the if the dice is close to being at a stop.
         * It doesn't check for a full stop as it takes a long time for the dice to actually
         * settle down to the point where there is actually no movement.
         * The velocity at this point would not be enough to actually affect the outcome of
         * the dice roll.
         */
        if(rigidBody.velocity.magnitude <0.05f && rigidBody.angularVelocity.magnitude <0.03f)
        {
            return true;
        }
        return false;
    }
    
    private int FindFaceResult()
    {
        /* The dice gameobject contains a detector for each face.
         * The detector is a child of the gameobject.
         * This goes through the list of detector on the dice and set the maxIndex to its own index
         * if the position of the detector is lesser than the current maxIndex's detector in the Z-axis.
         * The "up" direction in the game would be towards -ve Z
         * So, if the detector has the lowest Z value, it would be the detector that is currently facing "up"
         * It then returns the maxIndex which would be the face that is currently facing "up"
         */
        int maxIndex = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(maxIndex).position.z > transform.GetChild(i).position.z)
            {
                maxIndex = i;
            } 
        }
        return maxIndex;
    }

    private void ShowLitTexture(int result)
    {
        //switches the texture of the current face that is facing up to a "lit" up texture
        //also set the variable "material" to the current name of the material
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        switch (result)
        {
            case 0:
                meshRenderer.materials[4].mainTexture = textureList[result];
                material = "Metal";
                break;
            case 1:
                meshRenderer.materials[0].mainTexture = textureList[result];
                material = "Wood";

                break;
            case 2:
                meshRenderer.materials[1].mainTexture = textureList[result];
                material = "Cloth";

                break;
            case 3:
                meshRenderer.materials[5].mainTexture = textureList[result];
                material = "Stone";

                break;
            case 4:
                meshRenderer.materials[6].mainTexture = textureList[result];
                material = "Twine";

                break;
            case 5:
                meshRenderer.materials[7].mainTexture = textureList[result];
                material = "Leather";

                break;
            case 6:
                meshRenderer.materials[3].mainTexture = textureList[result];
                material = "Glass";

                break;
            case 7:
                meshRenderer.materials[2].mainTexture = textureList[result];
                material = "EleCrystal";

                break;
        }
    }

    private void PlayDiceRollSFX()
    {
        //checks if the sfx is already playing before playing it
        if (!diceRollSFX.isPlaying)
        {
            diceRollSFX.Play();
        }
    }
    public void PlayDiceCollideSFX()
    {
        //checks if the sfx is already playing before playing its
        if (!diceCollideDiceSFX.isPlaying)
        {
            diceCollideDiceSFX.Play();
        }
    }

    //GETTER
    public bool HasLanded
    {
        get
        {
            return hasLanded;
        }
    }

    public string Material
    {
        get
        {
            return material;
        }
    }
}
