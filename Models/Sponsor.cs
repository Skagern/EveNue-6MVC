using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GruppNrSexMVC.Models
{
    public class Sponsor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
    }

    public class SponsorImageModel
    {
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; }
    }

}
