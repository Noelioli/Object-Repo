public class Car
{
    public float gas;
    public float km;
    public float ratio;

    public virtual void gasKmHr()
    {
        ratio = gas/km;
    }
}
