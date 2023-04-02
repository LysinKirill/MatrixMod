namespace Groups;

public class Field<T> : Ring<T>
{
    public T MultiplicativeId { get; init; }

    public Field(HashSet<T> set, Func<T, T, T> add, Func<T, T, T> mult, Func<T, T, bool> equals, Func<T, T> copy, bool validate = true) : base(set, add, mult, equals, copy, validate)
    {
        if (GetMultiplicativeIdentity() is null)
            throw new ArgumentException("The given set doesn't form a group under provided operation");
        MultiplicativeId = GetMultiplicativeIdentity()!;

        if (!CheckMultiplicativeInverses())
            throw new ArgumentException("The given set doesn't form a group under provided operation");
    }
    
    
    
    private bool CheckMultiplicativeIdentity(T a)
    {
        foreach(T x in _additiveGroup.Set)
            if (!(_multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(x, a), _multiplicativeSemigroup.Add(a, x)) &&
                  _multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(x, a), x)))
                return false;
        return true;
    }

    private T? GetMultiplicativeIdentity()
    {
        // Возможно стоит добавить проверку на единственность нейтрального элемента
        foreach (T x in _multiplicativeSemigroup.Set)
        {
            if (CheckMultiplicativeIdentity(x))
                return x;
        }

        Console.WriteLine("No identity element!");
        return default;
    }

    private bool CheckMultiplicativeInverses()
    {
        foreach (T x in _multiplicativeSemigroup.Set)
        {
            if(_multiplicativeSemigroup.GEquals(x, _additiveGroup.Id))
                continue;
            bool flag = false;
            foreach (T y in _multiplicativeSemigroup.Set)
                if (_multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(x, y), Id) && _multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(y, x), Id))
                {
                    flag = true;
                    break;
                }
            if (!flag)
            {
                Console.WriteLine($"Doesn't contain an inverse for {x}");
                return false;
            }
        }
        return true;
    }

    public T MultiplicativeInverse(T x)
    {
        if (_multiplicativeSemigroup.GEquals(x, _additiveGroup.Id))
            throw new ArgumentException("The field has no inverse for additive identity");

        foreach (T y in _multiplicativeSemigroup.Set)
        {
            if (_multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(x, y), MultiplicativeId) &&
                _multiplicativeSemigroup.GEquals(_multiplicativeSemigroup.Add(y, x), MultiplicativeId))
                return y;
        }

        throw new Exception("Разраб даун, в группе не нашлось обратного элемента");
    }

    public T AdditiveInverse(T x) => _additiveGroup.Inverse(x);
}