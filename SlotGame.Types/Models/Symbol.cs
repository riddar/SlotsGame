using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace SlotGame.Types.Models
{
    public class Symbol
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        [NotMapped]
        public int[] Multipliers { get; set; }

        [DataMember]
        public int? ReelId { get; set; } = null;
        [IgnoreDataMember]
        [ForeignKey("ReelId")]
        public Reel Reel { get; set; }

        [DataMember]
        public int? PayoutId { get; set; } = null;
        [IgnoreDataMember]
        [ForeignKey("PayoutId")]
        public Payout Payout { get; set; }
    }
}
