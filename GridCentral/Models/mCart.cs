using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GridCentral.Models
{
    public class mCart
    {

        public ICommand RemoveCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand QtyChgCommand { get; set; }

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


        public string Addon
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

        public string ProductId
        {
            get;
            set;
        }
    }

    public class mCartS
    {
        public string ProductId
        {
            get;
            set;
        }

        public string Owner
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
