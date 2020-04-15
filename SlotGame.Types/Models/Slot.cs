using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace SlotGame.Types.Models
{
    public class Slot
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool startSpin { get; set; } = false;
        [DataMember]
        public int? WalletId { get; set; } = null;
        [IgnoreDataMember]
        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }
        [DataMember]
        public virtual IList<Reel> Reels { get; set; }
        [NotMapped]
        public int[] Images { get; set; }

    }
}
