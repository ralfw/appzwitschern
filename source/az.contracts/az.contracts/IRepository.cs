using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace az.contracts
{
    public interface IRepository
    {
        void Store(Versandauftrag versandauftrag, Action onEndOfStream);
        void Load(Action<Versandauftrag> onLoaded);
        void Delete(string versandauftragId, Action onEndOfStream);
    }
}
