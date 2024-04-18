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

        [SerializeField]
        int maxQuantity;

        [SerializeField]
        int currentProduction;

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
                    if(currentProduction < maxQuantity)
                    {
                        currentProduction += genQuantity;
                        resourceManager.AddMoney(genQuantity); 
                    }
                    break;
                case ResourceType.Troops: 
                    resourceManager.AddTroops(); 
                    break;
            }
        }

        //To be called periodically by AI Script
        public bool LootResource()
        {
            if(resourceType == ResourceType.Money)
            {
                if(currentProduction <= 0)
                {
                    resourceManager.AddMoney(-genQuantity);
                    currentProduction -= genQuantity;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}