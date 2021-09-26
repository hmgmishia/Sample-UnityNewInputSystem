using System;

namespace Sample.KeyInput
{
    public interface IAttackInputMediator
    {
        event Action OnAttack;
    }
}