using Bookstore.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookstore.Persistence.DTO
{
    public class LendingDTO
    {
        public int LendingId { get; set; }
        public BookVolume Volume { get; set; }
        public Visitor Visitor { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public static explicit operator Lending(LendingDTO dto) => new Lending
        {
            LendingId = dto.LendingId,
            Volume = dto.Volume,
            Visitor = dto.Visitor,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive
        };

        public static explicit operator LendingDTO(Lending i) => new LendingDTO
        {
            LendingId = i.LendingId,
            Volume = i.Volume,
            Visitor = i.Visitor,
            StartDate = i.StartDate,
            EndDate = i.EndDate,
            IsActive = i.IsActive
        };
    }
}
