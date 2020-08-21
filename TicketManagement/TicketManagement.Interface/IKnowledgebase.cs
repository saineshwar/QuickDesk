using System.Collections.Generic;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Interface
{
    public interface IKnowledgebase
    {
        int AddKnowledgebase(Knowledgebase knowledgebase,
            List<KnowledgebaseAttachments> listknowledgebaseAttachments,
            KnowledgebaseDetails knowledgebaseDetails,
            List<KnowledgebaseAttachmentsDetails> knowledgebaseAttachmentsDetails);

        int UpdateKnowledgebase(
            Knowledgebase knowledgebase,
            KnowledgebaseDetails knowledgebaseDetails,
            List<KnowledgebaseAttachments> listknowledgebaseAttachments,
            List<KnowledgebaseAttachmentsDetails> knowledgebaseAttachmentsDetails
        );

        List<SelectListItem> KnowledgebaseTypeList();
        int GetKnowledgebaseCount();
        List<KnowledgebaseModel> GetKnowledgebaseList(string subject, int? categoryId, int startIndex, int count, string sorting);
        EditKnowledgebaseViewModel GetKnowledgebasebyKnowledgebaseId(long knowledgebaseId);
        List<KnowledgebaseAttachments> GetKnowledgebaseAttachments(long? knowledgebaseId);
        int GetKnowledgebaseAttachmentsCount(long? knowledgebaseId);
        KnowledgebaseAttachmentsDownload GetAttachments(long? knowledgebaseAttachmentsId);
        int DeleteAttachments(long? knowledgebaseAttachmentsId);
        KnowledgebaseArticleViewModel GetKnowledgebaseDetailsForArticle(long knowledgebaseId);
        List<KnowledgeSearch> SearchKnowledgebase(string search);
        List<AllKnowledgebase> AllKnowledgebase();
        List<KnowledgeSearch> SearchKnowledgebasebyCategoryId(string categoryId);
    }
}