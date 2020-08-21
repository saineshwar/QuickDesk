using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Filters;
using TicketManagement.Helpers;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Controllers
{
    [AuthorizeAdmin]
    public class KnowledgebaseController : Controller
    {
        private readonly ICategory _category;
        private readonly IKnowledgebase _knowledgebase;
        SessionHandler _sessionHandler = new SessionHandler();

        public KnowledgebaseController(ICategory category, IKnowledgebase knowledgebase)
        {
            _category = category;
            _knowledgebase = knowledgebase;
        }
        // GET: Knowledgebase

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                var knowledgebaseViewModel = new KnowledgebaseViewModel()
                {
                    ListofCategory = _category.GetAllActiveSelectListItemCategory(),
                    ListofKnowledgebaseType = _knowledgebase.KnowledgebaseTypeList()
                };
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(KnowledgebaseViewModel knowledgebaseViewModel)
        {
            try
            {
                // ReSharper disable once JoinDeclarationAndInitializer
                // ReSharper disable once CollectionNeverQueried.Local
                var knowledgebaselist = new List<KnowledgebaseAttachments>();
                var knowledgebase = new Knowledgebase();
                var knowledgebaseDetails = new KnowledgebaseDetails();
                var knowledgebaseAttachmentsDetails = new KnowledgebaseAttachmentsDetails();
                var knowledgebaseAttachmentsDetailsList = new List<KnowledgebaseAttachmentsDetails>();

                if (ModelState.IsValid)
                {

                    knowledgebase.Subject = knowledgebaseViewModel.Subject;
                    knowledgebase.Status = knowledgebaseViewModel.Status;
                    knowledgebase.KnowledgebaseTypeId = knowledgebaseViewModel.KnowledgebaseTypeId;
                    knowledgebase.CreateDate = DateTime.Now;
                    knowledgebase.KnowledgebaseId = 0;
                    knowledgebase.UserId = Convert.ToInt32(_sessionHandler.UserId);
                    knowledgebase.CategoryId = knowledgebaseViewModel.CategoryId;
                    knowledgebaseDetails.Contents = knowledgebaseViewModel.Contents;
                    knowledgebaseDetails.Keywords = knowledgebaseViewModel.Keywords;
                    knowledgebaseDetails.KnowledgebaseDetailsId = 0;

                    for (int i = 1; i <= 3; i++)
                    {
                        var filename = "fileupload_" + Convert.ToString(i);
                        var file = Request.Files[filename];

                        if (file != null && file.ContentLength > 0)
                        {
                            var knowledgebaseAttachments1 = new KnowledgebaseAttachments();
                            string extension = Path.GetExtension(file.FileName);
                            var myUniqueFileName = String.Concat(Guid.NewGuid(), extension);
                            knowledgebaseAttachments1.AttachmentsName = myUniqueFileName;
                            knowledgebaseAttachments1.AttachmentsType = extension;

                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                byte[] fileSize = binaryReader.ReadBytes(file.ContentLength);
                                knowledgebaseAttachmentsDetails.AttachmentBytes = fileSize;
                            }

                            knowledgebaselist.Add(knowledgebaseAttachments1);
                            knowledgebaseAttachmentsDetailsList.Add(knowledgebaseAttachmentsDetails);
                        }
                    }

                    _knowledgebase.AddKnowledgebase(knowledgebase, knowledgebaselist, knowledgebaseDetails,
                        knowledgebaseAttachmentsDetailsList);
                    TempData["KnowledgebaseMessage"] = "Your Article Added Successfully";
                }

                knowledgebaseViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                knowledgebaseViewModel.ListofKnowledgebaseType = _knowledgebase.KnowledgebaseTypeList();
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AllKnowledgebase()
        {
            return View();
        }

        [HttpPost] //Gets the todo Lists.  
        public JsonResult KnowledgebaseList(string subject, int? categoryId, int jtStartIndex = 0, int jtPageSize = 0,
            string jtSorting = null)
        {
            try
            {
                var knowledgebaseCount = _knowledgebase.GetKnowledgebaseCount();
                var listofKnowledgebase =
                    _knowledgebase.GetKnowledgebaseList(subject, categoryId, jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = listofKnowledgebase, TotalRecordCount = knowledgebaseCount });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var knowledgebaseViewModel = _knowledgebase.GetKnowledgebasebyKnowledgebaseId(id);
                knowledgebaseViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                knowledgebaseViewModel.ListofKnowledgebaseType = _knowledgebase.KnowledgebaseTypeList();
                knowledgebaseViewModel.ListofAttachments = _knowledgebase.GetKnowledgebaseAttachments(id);
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditKnowledgebaseViewModel editKnowledgebaseViewModel)
        {
            try
            {
                // ReSharper disable once JoinDeclarationAndInitializer
                // ReSharper disable once CollectionNeverQueried.Local
                var knowledgebaselist = new List<KnowledgebaseAttachments>();
                var knowledgebase = new Knowledgebase();
                var knowledgebaseDetails = new KnowledgebaseDetails();
                var knowledgebaseAttachmentsDetails = new KnowledgebaseAttachmentsDetails();
                var knowledgebaseAttachmentsDetailsList = new List<KnowledgebaseAttachmentsDetails>();

                if (ModelState.IsValid)
                {

                    knowledgebase.Subject = editKnowledgebaseViewModel.Subject;
                    knowledgebase.Status = editKnowledgebaseViewModel.Status;
                    knowledgebase.KnowledgebaseTypeId = editKnowledgebaseViewModel.KnowledgebaseTypeId;
                    knowledgebase.CreateDate = DateTime.Now;
                    knowledgebase.UserId = Convert.ToInt32(_sessionHandler.UserId);
                    knowledgebase.CategoryId = editKnowledgebaseViewModel.CategoryId;
                    knowledgebase.KnowledgebaseId = Convert.ToInt64(editKnowledgebaseViewModel.KnowledgebaseId);
                    knowledgebaseDetails.Contents = editKnowledgebaseViewModel.Contents;
                    knowledgebaseDetails.Keywords = editKnowledgebaseViewModel.Keywords;

                    if (Request != null && (_knowledgebase.GetKnowledgebaseAttachmentsCount(editKnowledgebaseViewModel.KnowledgebaseId) > 0 && Request.Files["fileupload_1"].ContentLength != 0 && Request.Files["fileupload_2"].ContentLength != 0 && Request.Files["fileupload_3"].ContentLength != 0))
                    {
                        ModelState.AddModelError("", "Delete Pervious uploaded Attachments for Uploading New Attachments");
                        editKnowledgebaseViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                        editKnowledgebaseViewModel.ListofKnowledgebaseType = _knowledgebase.KnowledgebaseTypeList();
                        return View(editKnowledgebaseViewModel);
                    }
                    else
                    {
                        if ((_knowledgebase.GetKnowledgebaseAttachmentsCount(editKnowledgebaseViewModel.KnowledgebaseId) == 0))
                        {
                            for (int i = 1; i <= 3; i++)
                            {
                                var filename = "fileupload_" + Convert.ToString(i);
                                var file = Request?.Files[filename];

                                if (file != null && file.ContentLength > 0)
                                {
                                    var knowledgebaseAttachments1 = new KnowledgebaseAttachments();
                                    string extension = Path.GetExtension(file.FileName);
                                    var myUniqueFileName = String.Concat(Guid.NewGuid(), extension);
                                    knowledgebaseAttachments1.AttachmentsName = myUniqueFileName;
                                    knowledgebaseAttachments1.AttachmentsType = extension;

                                    using (var binaryReader = new BinaryReader(file.InputStream))
                                    {
                                        byte[] fileSize = binaryReader.ReadBytes(file.ContentLength);
                                        knowledgebaseAttachmentsDetails.AttachmentBytes = fileSize;
                                    }

                                    knowledgebaselist.Add(knowledgebaseAttachments1);
                                    knowledgebaseAttachmentsDetailsList.Add(knowledgebaseAttachmentsDetails);
                                }
                            }
                            _knowledgebase.UpdateKnowledgebase(knowledgebase, knowledgebaseDetails, knowledgebaselist,
                                knowledgebaseAttachmentsDetailsList);

                            TempData["KnowledgebaseMessage"] = "Your Article Updated Successfully";
                            return RedirectToAction("Edit", "Knowledgebase");

                        }

                        _knowledgebase.UpdateKnowledgebase(knowledgebase, knowledgebaseDetails, null, null);

                        TempData["KnowledgebaseMessage"] = "Your Article Updated Successfully";

                        return RedirectToAction("Edit", "Knowledgebase", new { id = editKnowledgebaseViewModel.KnowledgebaseId });
                    }
                }

                editKnowledgebaseViewModel.ListofCategory = _category.GetAllActiveSelectListItemCategory();
                editKnowledgebaseViewModel.ListofKnowledgebaseType = _knowledgebase.KnowledgebaseTypeList();
                editKnowledgebaseViewModel.ListofAttachments = _knowledgebase.GetKnowledgebaseAttachments(editKnowledgebaseViewModel.KnowledgebaseId);
                return View(editKnowledgebaseViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult DownloadAttachMent(long? id)
        {
            try
            {
                if (id != null)
                {
                    var document = _knowledgebase.GetAttachments(id);
                    if (document != null)
                    {
                        return File(document.AttachmentBytes, System.Net.Mime.MediaTypeNames.Application.Octet, document.AttachmentsName);
                    }

                    return RedirectToAction("Dashboard", "AdminDashboard");
                }
                else
                {
                    return RedirectToAction("Dashboard", "AdminDashboard");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DeleteAttachment(RequestDeleteAttachment model)
        {
            try
            {
                if (model.KnowledgebaseAttachmentsId != null)
                {
                    _knowledgebase.DeleteAttachments(model.KnowledgebaseAttachmentsId);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult BindCategory()
        {
            try
            {
                var listofcategory = _category.GetAllActiveSelectListItemCategory();
                return Json(listofcategory);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Article(int id)
        {
            try
            {
                var knowledgebaseViewModel = _knowledgebase.GetKnowledgebaseDetailsForArticle(id);
                knowledgebaseViewModel.ListofAttachments = _knowledgebase.GetKnowledgebaseAttachments(id);
                return View(knowledgebaseViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}