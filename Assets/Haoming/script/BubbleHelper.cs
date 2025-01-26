using System.Numerics;

public static class BubbleHelper
{
    public static BigInteger IdToBinaryId(int Id)
    {
        return BigInteger.One << (Id - 1);
    }
    
    public static BigInteger GetMergeId(BigInteger Id1, BigInteger Id2)
    {
        return Id1 + Id2;
    }
}
