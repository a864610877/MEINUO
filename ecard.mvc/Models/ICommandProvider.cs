using System.Collections;
using System.Collections.Generic;

namespace Ecard.Mvc.Models
{
    public interface ICommandProvider
    {
        IEnumerable<ActionMethodDescriptor> GetCommands( );
    }
}