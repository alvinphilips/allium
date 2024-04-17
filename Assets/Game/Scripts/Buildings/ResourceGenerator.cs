using Game.Scripts.Game;
using UnityEngine;
using Resources = Game.Scripts.Game.Resources;

namespace Game.Scripts.Buildings
{
    public class ResourceGenerator : Building
    {
        [SerializeField]
        Resources resourceType;

        [SerializeField]
        int genFrequency;

        [SerializeField]
        int genQuantity;

        ResourceManager resourceManager;

        protected override void Start()
        {
            base.Start();

            resourceManager = ResourceManager.Instance;

            //Manipulate Gen Quantity and Frequency based on level
        }

        private float elapsedTime = 0;

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > genFrequency)
            {
                GenerateResource();
                elapsedTime = 0;
            }
        }

        private void GenerateResource()
        {
            switch(resourceType)
            {
                case Resources.Money:
                    resourceManager.AddMoney(genQuantity); 
                    break;
                case Resources.Troops: 
                    resourceManager.AddTroops(); 
                    break;
            }
        }
    }
}