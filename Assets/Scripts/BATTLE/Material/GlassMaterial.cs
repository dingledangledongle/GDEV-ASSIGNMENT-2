public class GlassMaterial : PlayerMaterial
{
    public GlassMaterial()
    {
        DamageModifier = -2f;
        DefenseModifier = 1f;
        NumOfHits = 3;
        IsAOE = true;

    }
}
