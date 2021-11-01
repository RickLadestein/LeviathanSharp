using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sandbox
{
    public class DHTable : IEnumerable<DHEntry>
    {
        private List<DHEntry> table;

        public DHTable()
        {
            table = new List<DHEntry>();
        }

        public void AddEntry(DHEntry en)
        {
            en.id = table.Count;
            table.Add(en);
        }

        public List<RobotLink> ToRobotLinks()
        {
            List<RobotLink> output = new List<RobotLink>();

            for(int i = 0; i < (table.Count - 1); i++)
            {
                RobotLink rl = null;
                if (i == 0)
                {
                    rl = new RobotLink(table[i], new DHEntry());
                } else
                {
                    rl = new RobotLink(table[i], table[i-1]);
                }
                output.Add(rl);
            }
            return output;
        }

        public IEnumerator<DHEntry> GetEnumerator()
        {
            return table.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return table.GetEnumerator();
        }
    }
}
