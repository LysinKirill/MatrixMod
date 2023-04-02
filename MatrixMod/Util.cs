namespace Groups;

public static class Util
{
    public static int Mod(int n, int m) => n < 0 ? m + (n % m) : n % m;

    public static int Pow(int a, int n)
    {
        if (n == 0)
            return a == 0 ? throw new ArgumentException("Cannot raise zero to the power of zero.") : 1;
        if (n < 0)
            throw new ArgumentException("This implementation of power only takes non-negative second argument");
        if (n % 2 == 0)
        {
            int t = Pow(a, n / 2);
            return t * t;
        }
        return a * Pow(a, n - 1);
    }
}