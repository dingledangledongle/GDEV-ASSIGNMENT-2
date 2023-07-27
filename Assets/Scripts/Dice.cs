using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dice : MonoBehaviour
{
    public Texture[] textureList;
    public AudioSource diceReadySFX;
    public AudioSource diceRollSFX;
    public AudioSource diceCollideDiceSFX;
    private Rigidbody rigidBody;
    private Vector3 gravity = new Vector3(0, 0, 20f);
    private bool hasLanded = false;
    private void Start()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody>();
        SetInitialState();
    }

    // Update is called once per frame
    private void Update()
    {
        rigidBody.AddForce(gravity, ForceMode.Force);
        if (CheckDiceHasStopped())
        {
            int result = FindFaceResult();
            ShowLitTexture(result);
            if (!hasLanded)
            {
                diceReadySFX.Play();
                hasLanded = true;
            }
        }
    }

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

    private void SetInitialState()
    {
        int x = Random.Range(0, 360);
        int y = Random.Range(0, 360);
        int z = Random.Range(0, 360);
        Quaternion rotation = Quaternion.Euler(x, y, z);

        x = Random.Range(0, 25);
        y = Random.Range(0, 25);
        z = Random.Range(0, 25);
        Vector3 force = new (x, y, -z);

        x = Random.Range(0, 50);
        y = Random.Range(0, 50);
        z = Random.Range(0, 50);

        Vector3 torque = new(x, y, z);

        transform.rotation = rotation;
        rigidBody.velocity = force;

        this.rigidBody.maxAngularVelocity = 1000;
        rigidBody.AddTorque(torque, ForceMode.VelocityChange);
    }

    private bool CheckDiceHasStopped()
    {
        if(rigidBody.velocity == Vector3.zero && rigidBody.angularVelocity == Vector3.zero)
        {
            return true;
        }
        return false;
    }
    
    private int FindFaceResult()
    {
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
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        switch (result)
        {
            case 0:
                meshRenderer.materials[4].mainTexture = textureList[result];
                break;
            case 1:
                meshRenderer.materials[0].mainTexture = textureList[result];
                break;
            case 2:
                meshRenderer.materials[1].mainTexture = textureList[result];
                break;
            case 3:
                meshRenderer.materials[5].mainTexture = textureList[result];
                break;
            case 4:
                meshRenderer.materials[6].mainTexture = textureList[result];
                break;
            case 5:
                meshRenderer.materials[7].mainTexture = textureList[result];
                break;
            case 6:
                meshRenderer.materials[3].mainTexture = textureList[result];
                break;
            case 7:
                meshRenderer.materials[2].mainTexture = textureList[result];
                break;
        }
    }

    private void PlayDiceRollSFX()
    {
        if (!diceRollSFX.isPlaying)
        {
            diceRollSFX.Play();
        }
    }
    public void PlayDiceCollideSFX()
    {
        if (!diceCollideDiceSFX.isPlaying)
        {
            diceCollideDiceSFX.Play();
        }
    }

    private void SimulatePhysics()
    {
    }
}
