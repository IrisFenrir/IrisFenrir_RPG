using System;

namespace IrisFenrir.InputSystem
{
    public class DelegatedKey : IKey
    {
        public Func<bool> getKeyPressing;
        public Func<bool> getKeyDown;
        public Func<bool> getKeyUp;

        public override bool GetKeyPressing()
        {
            if (!Enable) return false;
            return getKeyPressing != null && getKeyPressing();
        }

        public override bool GetKeyDown()
        {
            if (!Enable) return false;
            return getKeyDown != null && getKeyDown();
        }

        public override bool GetKeyUp()
        {
            if (!Enable) return false;
            return getKeyUp != null && getKeyUp();
        }
    }
}
