
using AutoFileManager.Data.Entities;
using System.Collections.Generic;

namespace AutoFileManager.Dtos
{
    public class InformationContentDto
    {
        public string Name { get; set; }
        public List<ContentTypeEntity> Contents { get; set; }
    }
}