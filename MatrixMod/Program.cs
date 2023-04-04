using System;
using Groups;

class Program {
    static void Main(string[] args)
    {
        const int N = 19;
        Field<int> Zn = new Field<int>(
            Enumerable.Range(0, N).ToHashSet(),
            (a, b) => (a + b) % N,
            (a, b) => (a * b) % N,
            (a, b) => a == b,
            a => a);

        // while (true)
        // {
        //     int x = int.Parse(Console.ReadLine());
        //     Console.WriteLine(Zn.MultiplicativeInverse(x));
        //     Console.WriteLine();
        // }

        
        
         // int[][] matrix = new int[][]{
         //     new[]{1, 4, 1, 1, 1},
         //     new[]{0, 3, 0, 1, 1},
         //     new[]{3, 2, 2, 3, 0},
         //     new[]{3, 2, 4, 3, 3}};

         int[][] matrix = ParseMatrix(N);

         matrix = Reduce(matrix, Zn);
         Print(matrix);
         Console.WriteLine("___________________");
         matrix = Canonical(matrix, Zn);
         Print(matrix);
         Console.WriteLine("___________________");
    }

    public static void Print(int[][] matrix)
    {
        foreach (var x in matrix)
        {
            foreach(var y in x)
            {
                Console.Write($"{y}\t");
            }
        
            Console.WriteLine();
        }
    }

    public static int[][] Reduce(int[][] arr, Field<int> Zn)
    {
        int n = arr.Length;
        int m = arr[0].Length;
        
        int[][] matrix = new int[n][];
        Array.Copy(arr, matrix, n);

        
        for (int i = 0; i < m; i++)
        {
            var a = matrix.Take(i);
            var b = matrix.Skip(i).OrderByDescending(x => x[i]);
        
            matrix = a.Concat(b).ToArray();
             
            for (int j = n - 1; j >= i; j--)
            {
                if (matrix[j][i] == 0)
                    continue;
                int inverse = Zn.MultiplicativeInverse(matrix[j][i]);
                for (int k = i; k < m; k++)
                {
                    matrix[j][k] = Zn.Mult(matrix[j][k], inverse);
                }
            }
             
            for (int j = n - 1; j > i; j--)
            {
                if (matrix[j][i] == 0)
                    continue;
                for(int k = i; k < m; k++)
                    matrix[j][k] = Zn.Add(matrix[j][k], Zn.AdditiveInverse(matrix[j - 1][k]));
            }
            
            Print(matrix);
            Console.WriteLine("_____________________________\n");
        }

        return matrix;
    }

    public static int[][] Canonical(int[][] arr, Field<int> Zn)
    {
        
        int n = arr.Length;
        int m = arr[0].Length;
        
        int[][] matrix = new int[n][];
        Array.Copy(arr, matrix, n);
        matrix = Reduce(matrix, Zn);

        Console.WriteLine();
        for (int i = n - 1; i >= 0; i--)
        {
            for (int j = 0; j < m; j++)
            {
                if (matrix[i][j] == 1)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        int count = matrix[k][j];
                        for (int l = 0; l < m; l++)
                        {
                            matrix[k][l] = Zn.Add(matrix[k][l], Zn.Mult(Zn.AdditiveInverse(matrix[i][l]), count));
                        }
                    }
                    break;
                }
            }
            Print(matrix);
            Console.WriteLine("_____________________________\n");   
        }

        return matrix;
    }

    public static int[][] ParseMatrix(int m)
    {
        List<int[]> temp = new List<int[]>();
        string s;
        while (true)
        {
            s = Console.ReadLine();
            if (s == "")
                break;
            int[] t = Array.ConvertAll(s.Split(), x =>  Util.Mod(int.Parse(x), m));
            if (temp.Count != 0)
                if (temp[^1].Length != t.Length)
                {
                    Console.WriteLine("Inconsistent row sizes...");
                    continue;
                }
            temp.Add(t);
        }

        return temp.ToArray();
    }
}