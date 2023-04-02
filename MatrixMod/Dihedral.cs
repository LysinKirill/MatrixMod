namespace Groups;

public static class Dihedral
{
    //public int N { get; init; }

    private static Dictionary<int, List<DihedralEl>> _dElements = new Dictionary<int, List<DihedralEl>>();
    //public List<DihedralEl> Elements { get; private set; } = new List<DihedralEl>();

    public static List<DihedralEl> D(int n)
    {
        if (!_dElements.ContainsKey(n))
            _dElements[n] = GenerateElements(n);
        return _dElements[n];
    }

    private static List<DihedralEl> GenerateElements(int n)
    {
        if (n <= 0)
            throw new ArgumentException("N should be a positive integer");
        List<DihedralEl> list = new List<DihedralEl>();
        for(int i = 0; i < n; i++)
            list.Add(new DihedralEl(n, i, true));
        
        for(int i = 0; i < n; i++)
            list.Add(new DihedralEl(n, i, false));
        return list;
    }
    
    

    public class DihedralEl
    {
        public bool IsRotation { get; init; }
        public int N { get; init; }
        public int K { get; init; }

        private readonly int _hashSum;

        public static bool operator ==(DihedralEl el1, DihedralEl el2) =>
            (el1.IsRotation == el2.IsRotation) && (el1.N == el2.N) && (el1.K == el2.K);

        public static bool operator !=(DihedralEl el1, DihedralEl el2) => !(el1 == el2);

        public static DihedralEl operator *(DihedralEl el1, DihedralEl el2)
        {
            if (el1.N != el2.N)
                throw new ArgumentException("The elements do not belong to the same group.");

            int N = el1.N;
            List<DihedralEl> elements = D(N);
            
            if (el1.IsRotation && el2.IsRotation)
                return elements[Util.Mod(el1.K + el2.K, N)];
            if (el1.IsRotation && !el2.IsRotation)
                return elements[N + Util.Mod(el1.K + el2.K, N)];
            if (!el1.IsRotation && el2.IsRotation)
                return elements[N + Util.Mod(el1.K - el2.K, N)];

            return elements[Util.Mod(el1.K - el2.K, N)];
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(DihedralEl))
                return false;
            return this == ((DihedralEl)obj);
        }

        public DihedralEl Copy() => new DihedralEl(N, K, IsRotation);
        
        public override int GetHashCode()
        {
            return _hashSum;
        }
        public DihedralEl(int n, int k, bool isRotation)
        {
            N = n;
            K = k;
            IsRotation = isRotation;
            
            // По идее такая реализация не должна давать коллизий, но числа растут быстро с увеличением порядка группы, возможно стоит переделать
            _hashSum = Util.Pow(2, N) * Util.Pow(3, K) * (IsRotation ? 1 : -1);
        }

        public override string ToString()
        {
            if (IsRotation)
                return $"Rotation by {((2 * 180 * K / N)):f2}°";
            return $"Symmetry relative to the {K} axis";
        }
    }
}