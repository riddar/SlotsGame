using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SlotGame.Types.Models
{
    public class Reel
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool Spin { get; set; }
        [DataMember]
        public int Speed { get; set; }
        [DataMember]
        public virtual IList<Symbol> Symbols { get; set; }
        [DataMember]
        public int? SlotId { get; set; } = null;
        [IgnoreDataMember]
        [ForeignKey("SlotId")]
        public Slot Slot { get; set; }

    }
}
