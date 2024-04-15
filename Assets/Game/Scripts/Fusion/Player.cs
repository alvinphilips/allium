using Fusion;
using Game.Scripts.Game;
using Game.Scripts.GameResources;
using Game.Scripts.Patterns;

namespace Game.Scripts.Fusion
{
    public class Player : NetworkBehaviour, IVisitor<Resource>
    {
        public string Nickname { get; set; }
        
        public void Visit(Resource visitable)
        {
            // TODO: Implement
            if (visitable.Type == ResourceType.Money)
            {
                ResourceManager.Instance.AddMoney(visitable.Amount);
            }
        }
    }
}
