using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace SlotGame.Types.Models
{
    [DataContract]
    public class Payout
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public virtual IList<Symbol> Symbols { get; set; }
        [DataMember]
        public virtual IList<PayLine> PayLines { get; set; }
    }
}
