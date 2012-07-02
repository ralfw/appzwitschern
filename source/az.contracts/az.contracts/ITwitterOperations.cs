using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace az.contracts
{
    public interface ITwitterOperations
    {
        Versandauftrag Versandauftrag_um_access_token_erweitern(Versandauftrag versandauftrag);
        Versandauftrag Versenden(Versandauftrag versandauftrag);
    }
}
