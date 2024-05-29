#nullable enable


namespace CodingStrategy.Entities.Runtime.Statement
{
    using Robot;
    using UnityEngine;

    public class AddMaxEnergyStatement : AbstractStatement
    {
        private readonly int _value;
        private int _previousEnergyPoint;
        private readonly bool _isReverse;
        public AddMaxEnergyStatement(IRobotDelegate robotDelegate, int value, int previousEnergyPoint=0, bool isReverse=false)
        :base(robotDelegate)
        {
            _previousEnergyPoint = previousEnergyPoint;
            _value = value;
            _isReverse = isReverse;
        }

        public override void Execute(RuntimeExecutorContext context)
        {
            Debug.Log("Execute AddMaxEnergyStatement");
            if(_isReverse)
            {
                _robotDelegate.MaxEnergyPoint += _value;
                _robotDelegate.EnergyPoint = _previousEnergyPoint;
                Debug.LogFormat("robot max energy point : {0}", _robotDelegate.MaxEnergyPoint);
                return;
            }
            _previousEnergyPoint = _robotDelegate.EnergyPoint;
            if(_previousEnergyPoint<=0)
            {
                return;
            }
            _robotDelegate.EnergyPoint = 0;
            _robotDelegate.MaxEnergyPoint += _value;
            Debug.LogFormat("robot max energy point : {0}", _robotDelegate.MaxEnergyPoint);
        }

        public override StatementPhase Phase => StatementPhase.Static;

        public override IStatement Reverse => new AddMaxEnergyStatement(_robotDelegate, -_value, _previousEnergyPoint, true);
    }
}
