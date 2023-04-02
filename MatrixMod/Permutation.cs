namespace Groups;

public class Permutation
{
    private int[] arr;
    private readonly int hashSum;
    public static Permutation Parse(string s)
    {
        string[] str = s.Split(" ");

        int[] arr = new int[str.Length];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = int.Parse(str[i]);
        Permutation p = new Permutation(arr);
        return p;
    }

    public static bool Equals(Permutation p1, Permutation p2)
    {
        if (p2.GetLength() != p1.GetLength())
            return false;
        for (int i = 0; i < p1.GetLength(); i++)
            if (p1.arr[i] != p2.arr[i])
                return false;
        return true;
    }

    public int CountInversions()
    {
        int counter = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (arr[i] > arr[j])
                    counter++;
            }
        }

        return counter;
    }

    public static Permutation Inversed(Permutation p)
    {
        int[] a = new int[p.GetLength()];
        for (int i = 0; i < p.GetLength(); i++)
        {
            a[p.arr[i] - 1] = i + 1;
        }

        return new Permutation(a);
    }

    public static Permutation Id(int n)
    {
        int[] a = new int[n];
        for (int i = 0; i < n; i++)
        {
            a[i] = i + 1;
        }

        return new Permutation(a);
    }

    public static Permutation operator *(Permutation p, Cycle c)
    {
        Permutation aux = Cycle.ToPermutation(c, c.getMax());
        return p * aux;
        //if (c.getMax() > p.GetLength())
        //    throw new Exception();


        //return p * Cycle.ToPermutation(c, p.GetLength());
    }

    public static Permutation operator *(Cycle c, Permutation p)
    {
        return Cycle.ToPermutation(c) * p;
    }

    public static Permutation Reversed(Permutation p)
    {
        int[] a = new int[p.GetLength()];
        for (int i = 0; i < p.GetLength(); i++)
        {
            a[i] = p.arr[i];
        }

        Array.Reverse(a);
        return new Permutation(a);
    }

    public int GetOrder()
    {
        int i = 1;
        Permutation k = this;
        while (!Equals(k, Id(this.GetLength())))
        {
            k = this * k;
            i++;
        }

        return i;
    }

    public static bool TryParse(string s, out Permutation p)
    {
        try
        {
            p = Parse(s);
            return true;
        }
        catch (FormatException)
        {
            p = new Permutation();
            return false;
        }
        catch
        {
            p = new Permutation();
            return false;
        }
    }

    private static Permutation Swap(Permutation p, int i1, int i2)
    {
        if ((i1 >= p.GetLength() || i1 < 0) || (i2 >= p.GetLength() || i2 < 0))
            throw new ArgumentOutOfRangeException("Elements are out of range");
        int[] temp = new int[p.GetLength()];
        (temp[i1], temp[i2]) = (p.arr[i2], p.arr[i1]);
        for (int i = 0; i < temp.Length; i++)
        {
            if (i == i1 || i == i2)
                continue;
            temp[i] = p.arr[i];
        }

        return new Permutation(temp);
    }






    public static Permutation operator *(Permutation f, Permutation g)
    {
        // Реализация умножения позволяет перемножать подстановки разной длины, при этом подстановка меньшей длины дополняется элемнетами вида a[i] = i;


        if (f.GetLength() < g.GetLength())
        {
            int[] temp = new int[g.GetLength()];
            for (int i = 0; i < temp.Length; i++)
            {
                if (i < f.GetLength())
                {
                    temp[i] = f.arr[i];
                }
                else
                {
                    temp[i] = i + 1;
                }
            }

            f = new Permutation(temp);
        }

        if (f.GetLength() > g.GetLength())
        {
            int[] temp = new int[f.GetLength()];
            for (int i = 0; i < temp.Length; i++)
            {
                if (i < g.GetLength())
                {
                    temp[i] = g.arr[i];
                }
                else
                {
                    temp[i] = i + 1;
                }
            }

            g = new Permutation(temp);
        }

        Permutation p;
        int[] arr = new int[f.GetLength()];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = f.arr[g.arr[i] - 1];
        }

        p = new Permutation(arr);
        return p;
    }

    public int GetParity()
    {

        int counter = CountInversions();
        return (counter % 2 == 0 ? 1 : -1);
    }

    public Cycle[] GetCycles()
    {
        List<Cycle> cycles = new List<Cycle>();
        bool flag = true;
        int[] nums = new int[arr.Length];
        bool[] flags = new bool[arr.Length];

        for (int i = 0; i < arr.Length; i++)
            nums[i] = arr[i];
        for (int i = 0; i < nums.Length; i++)
        {
            if (flags[i])
                continue;
            List<int> temp = new List<int>();
            temp.Add(i + 1);
            flags[i] = true;

            int ind = nums[i] - 1;
            while (ind != i)
            {
                flags[ind] = true;
                temp.Add(ind + 1);
                ind = nums[ind] - 1;
            }

            cycles.Add(new Cycle(temp.ToArray()));
        }



        return cycles.ToArray();
    }

    public string ToCycles()
    {
        string s = "";
        bool flag = true;
        int[] nums = new int[arr.Length];
        bool[] flags = new bool[arr.Length];

        for (int i = 0; i < arr.Length; i++)
            nums[i] = arr[i];
        for (int i = 0; i < nums.Length; i++)
        {
            if (flags[i])
                continue;

            s += "(" + (i + 1).ToString();
            flags[i] = true;
            int ind = nums[i] - 1;
            while (ind != i)
            {
                flags[ind] = true;
                s += $" {ind + 1}";
                ind = nums[ind] - 1;
            }

            s += ")";
        }



        return s;
    }

    public void Write()
    {
        foreach (var x in arr)
            Console.Write($"{x} ");
        Console.WriteLine();
    }

    public override string ToString()
    {
        return "Permutation(" + String.Join(" ", arr) + ")";
    }

    public int GetLength() => arr.Length;


    public Permutation(params int[] a)
    {
        arr = a;
        hashSum = arr.Sum();
    }

    public void WriteInfo(string name)
    {
        Console.Write($"\n{name}\n\t");
        Write();
        Console.Write("\tCycles = " + ToCycles() + "\n");
        Console.Write("\tInverses = ");
        Console.WriteLine(CountInversions());
        Console.Write("\tParity = ");
        Console.WriteLine($"{GetParity()}\t({(GetParity() == 1 ? "even" : "odd")})");
        Console.Write("\tOrder = ");
        Console.WriteLine(GetOrder());
    }

    public void WriteInfo()
    {
        Console.Write('\t');
        Write();
        Console.Write("\tCycles = " + ToCycles());
        Console.Write("\tInverses = ");
        Console.WriteLine(CountInversions());
        Console.Write("\tParity = ");
        Console.WriteLine($"{GetParity()}\t({(GetParity() == 1 ? "even" : "odd")})");
        Console.Write("\tOrder = ");
        Console.WriteLine(GetOrder());
        //Console.WriteLine("\tOrder parity: {0}", (GetOrder() % 2 == 1 ? "odd" : "even"));
    }


    public static Permutation[] S(int n)
    {
        List<Permutation> list = new List<Permutation>();
        Permute(n, ref list);
        return list.ToArray();
    }

    public static Permutation[] S_alt(int n, int[][]? arr = null, int curr = 2)
    {
        if (arr is null)
            arr = new[]{ new[]{1} };

        if (n == 1)
        {
            Permutation[] permutations = new Permutation[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                permutations[i] = new Permutation(arr[i]);
        }

        int[][] temp = new int[arr.Length * curr][];
        for(int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[i].Length + 1; j++)
            {
                temp[i * curr + j] = new int[curr];
                for (int k = 0; k < j; k++)
                    temp[i * n + j][k] = arr[i][k];
                temp[i * curr + j][j] = curr;
                for (int k = j; k < arr[i].Length; k++)
                    temp[i * curr + j][k + 1] = arr[i][k];
            }
        }

        return S_alt(n - 1, temp, ++curr);
        
    }

    public Permutation Copy()
    {
        int[] data = new int[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            data[i] = arr[i];
        }

        return new Permutation(data);
    }

    public static Permutation operator ^(Permutation p, int k)
    {
        k = Mod(k, p.GetOrder());
        if (k == 0)
            return Id(p.arr.Length);
        Permutation f = new Permutation(p.arr);
        for (int i = 1; i < k; i++)
            f = p * f;
        return f;
    }

    public static bool operator !=(Permutation f, Permutation g)
    {
        for(int i = 0; i < f.arr.Length; i++)
            if (f.arr[i] != g.arr[i])
                return true;
        return false;
    }

    public static bool operator ==(Permutation f, Permutation g) => !(f != g);

    // Работает - не трогай!!! Сам не понимаю эту древнюю магию 
    public static void Permute(int k, ref List<Permutation> p, string[] a = null, int i = 0)
    {
        if (a == null)
        {
            a = new string[k];
            for (int m = 0; m < k; m++)
            {
                a[m] = (m + 1).ToString();
            }
        }

        if (p == null)
        {
            p = new List<Permutation>();
        }

        int j;
        int n = k - 1;
        if (i == n)
        {
            p.Add(Permutation.Parse(String.Join(" ", a)));
        }
        else
        {
            string temp;
            for (j = i; j <= n; j++)
            {
                // swap(a[i], a[j]);
                temp = a[i];
                a[i] = a[j];
                a[j] = temp;

                Permute(k, ref p, a, i + 1);

                // swap(a[i], a[j]);
                temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != typeof(Permutation))
            return false;
        return this == ((Permutation)obj);
    }

    public override int GetHashCode()
    {
        // Гениальная реализация. Надо переделать
        return hashSum;
    }

    private static int Mod(int x, int m) {
        return (x%m + m)%m;
    }
}
    
