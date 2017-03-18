using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoToDatabaseConverter
{
    class Product
    {
        public string sku { get; set; }
        public string name { get; set; }
        public string categories { get; set;}
        public bool enabled { get; set; }
        public bool silverDefault { get; set; }

        public Product(string sku,  string name, string categories, bool enabled, bool silverDefault)
        {
            this.sku = sku;
            this.name = name;
            this.categories = getCategoryLetters(categories);
            this.enabled = enabled;
            this.silverDefault = silverDefault;
        }

        private string getCategoryLetters(string categories)
        {
            string ret = "";
            string lowerCategories = categories.ToLower();
            HashSet<string> cats = new HashSet<string>();
            string[] pairList = {
                "abstract", "ab",
                "animals", "an",
                "botanical", "fl",
                "coastal", "se",
                "urban", "mo",
                "landscape", "la",
                "people", "pe",
                "places", "pl",
                "decorative", "vi",
                "zen", "ze",
                "icons", "pe",
                "college", "fu",
                "tv", "tv",
                "cars", "ca",
                "travels", "pl,la"
            };
            for (int i = 0; i <pairList.Length/2; i++)
            {
                if (lowerCategories.Contains(pairList[i*2]))
                {
                    cats.Add(pairList[i * 2 + 1]);
                }
            }
            foreach (string element in cats)
            {
                if (ret.Length > 1)
                {
                    ret = ret + ",";
                }
                ret = ret + element;
            }

            return ret;

        }

    }
}
