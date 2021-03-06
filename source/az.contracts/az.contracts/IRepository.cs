﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace az.contracts
{
    public interface IRepository : IDisposable
    {
        void Store(Versandauftrag versandauftrag, Action onEndOfStream);
        void List(Action<string> onListed);
        void Load(string persistentId, Action<Versandauftrag> onLoaded);
        void Delete(Versandauftrag versandauftrag, Action onEndOfStream);
    }
}
