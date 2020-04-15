using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace SlotGame.Types.Models
{
    public class PayLine
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Offset { get; set; }
        [DataMember]
        public float Multiplier { get; set; }
        [DataMember]
        public int? PayoutId { get; set; } = null;
        [IgnoreDataMember]
        [ForeignKey("PayoutId")]
        public Payout Payout { get; set; }
    }
}
