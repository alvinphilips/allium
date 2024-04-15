namespace Game.Scripts.Patterns
{
    public interface IVisitor<T>
    {
        public void Visit(T visitable);
    }
}