using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    public class Item
    {
        public virtual string Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string Name { get; set; }

        public virtual ISet Bids { get; set; }

    }

    public class Bid
    {
        public virtual string Id { get; set; }


        public virtual int Amount { get; set; }

        public virtual Item Item { get; set; }
    }
}
