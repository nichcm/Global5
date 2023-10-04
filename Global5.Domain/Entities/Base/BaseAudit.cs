using AutoMapper.Configuration.Annotations;
using System;

namespace Global5.Domain.Entities
{
    public class BaseAudit
    {

        /// <summary>
        /// Informações sobre Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Informações sobre Created
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Informações sobre CreateBy
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Informações sobre Updated
        /// </summary>
        public DateTimeOffset? Updated { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Informações sobre UpdateBy
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// Informações sobre Deleted
        /// </summary>
        public DateTimeOffset? Deleted { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Informações sobre DeleteBy
        /// </summary>
        public string DeleteBy { get; set; }

        [Ignore]
        public string LastSqlCommand { get; set; }

    }
}