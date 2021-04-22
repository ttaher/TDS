using System;
using System.ComponentModel.DataAnnotations;

namespace TDS.Infrastructure.Models
{
    public class EntitytBase<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; internal set; }
    }
}
