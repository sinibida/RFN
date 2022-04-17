using System.Collections.Generic;
using System.Linq;

namespace Rfn.App.Commands
{
    public class RfnCommandList : List<RfnCommand>
    {
        public RfnCommandList()
        {
        }

        public RfnCommandList(int capacity) : base(capacity)
        {
        }

        public RfnCommandList(IEnumerable<RfnCommand> collection) : base(collection)
        {
        }

        public RfnCommand GetCommandFromAlias(string alias)
        {
            var lowerAlias = alias.ToLower();
            return this.FirstOrDefault(cmd => cmd.Alias.Any(x => x.ToLower() == lowerAlias));
        }
    }
}