namespace Game.Scripts.Patterns
{
    public interface IVisitable<T>
    {
        public void Accept(IVisitor<T> visitor);
    }
}