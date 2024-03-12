namespace CodingStrategy.Entities.Shop
{
    using System;
    public class SharedShop
    {
        private readonly InGameCommandPool _inGameCommandPool;
        private readonly CommandListsByGrade _commandListByGrade;
        public SharedShop(InGameCommandPool inGameCommadPool)
        {
            _inGameCommandPool = inGameCommadPool;
            _commandListByGrade = new CommandListsByGrade();
            Init();
        }
        private void Init()
        {
            throw new NotImplementedException();   
        }
    }
}