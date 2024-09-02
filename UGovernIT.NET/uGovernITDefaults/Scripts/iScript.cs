using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uGovernITDefault.Scripts
{
    public interface iScript
    {
        void Execute();
        string Help();
        bool isVisible();
    }
}
