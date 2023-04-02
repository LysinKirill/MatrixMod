namespace Groups;

public class Ring<T>
{
    protected Group<T> _additiveGroup;
    protected Semigroup<T> _multiplicativeSemigroup;
    
    public T Id => _additiveGroup.Id;
    
    public T? Mult(T el1, T el2)
    {
        if (!_additiveGroup.Set.Contains(el1) || !_additiveGroup.Set.Contains(el2))
            throw new ArgumentException("The group does not contain such elements");
        return _multiplicativeSemigroup.Add(el1, el2);
    }

    public T? Add(T el1, T el2) => _additiveGroup.Add(el1, el2);
    
    
    public Ring(HashSet<T> set, Func<T, T, T> add, Func<T, T, T> mult, Func<T, T, bool> equals, Func<T, T> copy, bool validate = true)
    {
        _additiveGroup = new Group<T>(set, add, equals, copy, validate);
        _multiplicativeSemigroup = new Semigroup<T>(set, mult, equals, copy, validate);
    }
}