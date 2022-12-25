using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace memes
{
    [Serializable]
    public class Meme
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int Type { get; set; }

        public List<string> Tags { get; set; }

        public bool IsHasTag(string tag)
        {
            foreach (var item in Tags) 
            {
                if (item.Contains(tag)) return true;
            }
            return false;
        }
    }
}
