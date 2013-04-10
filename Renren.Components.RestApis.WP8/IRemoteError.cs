using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renren.Components.RestApis
{
    public interface IRemoteError
    {
        bool Hits();
        string ToHumanString();
    }
}
