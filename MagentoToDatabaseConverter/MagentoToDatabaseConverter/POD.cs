using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoToDatabaseConverter
{
    class POD : Product
    {
        public string ratio { get; set; }

        public POD(string sku, string ratio, string name, string categories, bool enabled, bool silverDefault)
            :base(sku, name, categories, enabled, silverDefault)
        {
            this.ratio = ratio;
        }
    }
}
