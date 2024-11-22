using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface ICommandHandler<TCommand>
    {
        IEnumerable<TCommand> Handle(List<TCommand> commands);
    }
}
