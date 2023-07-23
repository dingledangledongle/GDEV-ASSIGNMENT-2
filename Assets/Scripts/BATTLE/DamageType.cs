public class DamageType
{
    private int numberOfHits;
    private float damagePerHit;
    private bool isStatus;
    private bool isAOE;


    public DamageType(int numOfHit, float dmgPerHit)
    {
        NumberOfHits = numOfHit;
        DamagePerHit = dmgPerHit;
        isStatus = false;
        isAOE = false;
    }
    public int NumberOfHits
    {
        get
        {
            return numberOfHits;
        }
        set
        {
            numberOfHits = value;
        }
    }

    public float DamagePerHit
    {
        get
        {
            return damagePerHit;
        }
        set
        {
            damagePerHit = value;
        }
    }

    public bool IsStatus
    {
        get
        {
            return isStatus;
        }
        set
        {
            isStatus = value;
        }
    }

    public bool IsAOE
    {
        get
        {
            return isAOE;
        }
        set
        {
            isAOE = value;
        }
    }
}