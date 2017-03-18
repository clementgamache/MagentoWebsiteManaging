using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoToDatabaseConverter
{
    class Poster : Product
    {
        public string size { get; set; }
        public string location { get; set; }
        public string supplierCode { get; set; }
        public string priceCategory { get; set; }

        public Poster(string sku, string size, string location, string supplierCode, string name, string categories, string priceCat, bool enabled, bool silverDefault)
            :base(sku, name, categories, enabled, silverDefault)
        {
            this.size = size;
            this.location = location;
            this.supplierCode = supplierCode;
            priceCategory = priceCat;
        }
    }
}
