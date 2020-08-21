using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketManagement.Interface;

namespace TicketManagement.Controllers
{
    public class SearchController : Controller
    {
        private readonly IKnowledgebase _knowledgebase;
        private readonly ICategory _category;
        public SearchController(IKnowledgebase knowledgebase, ICategory category)
        {
            _knowledgebase = knowledgebase;
            _category = category;
        }

        // GET: Search
        public ActionResult Article(string categoryid)
        {
            var result = _knowledgebase.SearchKnowledgebasebyCategoryId(categoryid);
            return View(result);
        }

        public ActionResult GetAllArticle(string searchtext)
        {
            try
            {
                return Json(_knowledgebase.SearchKnowledgebase(searchtext), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ArticleView(int id)
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


        public ActionResult AllArticles()
        {
            try
            {
                var allKnowledgebase = _knowledgebase.AllKnowledgebase();
                return View(allKnowledgebase);
            }
            catch (Exception)
            {

                throw;
            }
        }

     
    }
}