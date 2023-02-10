using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoder.Interfaces
{
    public interface IResultOf<T>
    {
        T GetResult();
    }
}
