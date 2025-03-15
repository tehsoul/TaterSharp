using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaterSharp.CLI.Helpers;
public class OrdinalIgnoreCaseHashSet : HashSet<string>
{
    public OrdinalIgnoreCaseHashSet() : base(StringComparer.OrdinalIgnoreCase)
    {
        
    }
}
