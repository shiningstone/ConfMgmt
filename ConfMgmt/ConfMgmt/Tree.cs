using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JbConf
{
    public abstract class Leaf
    {
    }
    public class Tree
    {
        public List<Tree> Branches;
        public void Add(Tree branch)
        {
            Branches.Add(branch);
        }
    }
}
