using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SFTPClient.Models
{
    [Serializable]
    public class SftpViewModel
    {
        [Required]
        [Display(Name = "SFTP Host name")]
        public string HostName { get; set; }

        [Required]
        [Display(Name = "Port")]
        public int Port { get; set; }
        
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }

        public SftpViewModel()
        {
            Files = new List<HttpPostedFileBase>();
        }

    }
}