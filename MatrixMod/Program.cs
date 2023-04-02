using System;
using Groups;

class Program {
    static void Main(string[] args)
    {
        const int N = 5;
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

        
        
         int[][] matrix = new int[][]{
             new[]{1, 4, 1, 1, 1},
             new[]{0, 3, 0, 1, 1},
             new[]{3, 2, 2, 3, 0},
             new[]{3, 2, 4, 3, 3}};
         
         
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
                Console.Write($"{y} ");
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
        }

        return matrix;
    }

}