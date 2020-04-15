using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace SlotGame.Types.Models
{
    public class Wallet
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public double Balance { get; set; }
        [DataMember]
        public double BetAmount { get; set; }
        [DataMember]
        public double CreditsWon { get; set; }
        [DataMember]
        public double CreditsLost { get; set; }
        [DataMember]
        public virtual IList<Slot> Slots { get; set; }
    }
}
