namespace Game.Scripts.Patterns
{
    public interface IVisitable
    {
        public void Accept(IVisitor visitor);
    }
}