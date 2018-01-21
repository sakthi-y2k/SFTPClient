using Renci.SshNet;
using SFTPClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFTPClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new SftpViewModel();
            if (Session["clientArgs"] != null)
            {
                model = (SftpViewModel)Session["clientArgs"];

                using (SftpClient client = new SftpClient(model.HostName, model.Port, model.UserName, model.Password))
                {
                    client.Connect();
                    client.Connect();
                    var items = client.ListDirectory(".");
                    if (items != null && items.Count() > 0)
                    {
                        ViewBag.Files = client.ListDirectory(".").Select(x => new FileViewModel
                        {
                            FileName = x.FullName,
                            Path = x.FullName,
                            Size = x.Attributes.Size.ToString(),
                            IsDirectory = x.IsDirectory
                        }).ToList();
                    }
                }
            }
            return View(model);
        }

        public ActionResult Connect(SftpViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (SftpClient client = new SftpClient(model.HostName, model.Port, model.UserName, model.Password))
                {
                    client.Connect();

                    Session["clientArgs"] = model;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", model);
            }
        }

        public ActionResult Upload(SftpViewModel data)
        {
            if (Session["clientArgs"] != null)
            {
                var model = (SftpViewModel)Session["clientArgs"];

                using (SftpClient client = new SftpClient(model.HostName, model.Port, model.UserName, model.Password))
                {
                    client.Connect();

                    if (data.Files != null && data.Files.Count > 0)
                    {
                        if (data.Files[0] != null)
                        {
                            client.UploadFile(data.Files[0].InputStream, Path.GetFileName(data.Files[0].FileName));
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Download(string path)
        {
            if (Session["clientArgs"] != null)
            {
                var args = (SftpViewModel)Session["clientArgs"];

                using (SftpClient client = new SftpClient(args.HostName, args.Port, args.UserName, args.Password))
                {
                    client.Connect();
                    var fileName = Path.GetFileName(path);
                    var bytes = client.ReadAllBytes(fileName);
                    var mimeType = MimeMapping.GetMimeMapping(fileName);
                    return File(bytes, mimeType, fileName);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string path)
        {
            if (Session["clientArgs"] != null)
            {
                var args = (SftpViewModel)Session["clientArgs"];

                using (SftpClient client = new SftpClient(args.HostName, args.Port, args.UserName, args.Password))
                {
                    client.Connect();
                    MemoryStream ms = null;
                    client.DeleteFile(path);
                }
            }

            return RedirectToAction("Index");
        }
    }
}