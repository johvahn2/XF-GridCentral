using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GridCentral.Models
{
    public class mSavelater
    {

        public string ProductId { get; set; }
    
        public string Owner { get; set; }

    }

    public class mSavelaterR
    {
        public ICommand RemoveCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string bName
        {
            get;
            set;
        }

        public string Price
        {
            get;
            set;
        }

        public string Thumbnail
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string StatusColor
        {
            get;
            set;
        }
        public string Manufacturer
        {
            get;
            set;
        }

        public string Quantity
        {
            get;
            set;
        }
    }
}
