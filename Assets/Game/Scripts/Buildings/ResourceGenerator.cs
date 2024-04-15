using Game.Scripts.Game;
using UnityEngine;

namespace Game.Scripts.Buildings
{
    public class ResourceGenerator : Building
    {
        [SerializeField]
        ResourceType resourceType;

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
                case ResourceType.Money:
                    resourceManager.AddMoney(genQuantity); 
                    break;
                case ResourceType.Troops: 
                    resourceManager.AddTroops(); 
                    break;
            }
        }
    }
}