using System.Collections;
using System.Text;

namespace Groups;

public class MultiGroup<T1, T2, T3> : IEnumerable<(T1, T2, T3)>
{
    private Group<T1> G1;
    private Group<T2> G2;
    private Group<T3> G3;

    public MultiGroup(Group<T1> a, Group<T2> b, Group<T3> c)
    {
        G1 = a;
        G2 = b;
        G3 = c;
    }

    public int GetElementOrder((T1, T2, T3) el)
    {
        int order = 1;
        (T1, T2, T3) t = (G1.GetElementCopy(el.Item1), G2.GetElementCopy(el.Item2), G3.GetElementCopy(el.Item3));
        while (!(G1.GEquals(t.Item1, G1.Id) && G2.GEquals(t.Item2, G2.Id) && G3.GEquals(t.Item3, G3.Id)))
        {
            t.Item1 = G1.Add(t.Item1, el.Item1);
            t.Item2 = G2.Add(t.Item2, el.Item2);
            t.Item3 = G3.Add(t.Item3, el.Item3);
            order++;
        }

        return order;
    }

    public IEnumerator<(T1, T2, T3)> GetEnumerator()
    {
        return (from g1 in G1
            from g2 in G2
            from g3 in G3
            select (g1, g2, g3)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}