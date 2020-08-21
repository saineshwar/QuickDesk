using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using TicketManagement.Concrete.CacheLibrary;
using TicketManagement.Interface;
using TicketManagement.Models;
using TicketManagement.ViewModels;

namespace TicketManagement.Concrete
{
    public class CategoryConcrete : ICategory
    {
        private readonly DatabaseContext _context;
        public CategoryConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddCategory(Category category)
        {
            try
            {
                int? result = -1;

                if (category != null)
                {
                    category.CreateDate = DateTime.Now;
                    _context.Category.Add(category);
                    _context.SaveChanges();
                    result = category.CategoryId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckCategoryNameExists(string categoryName)
        {
            try
            {
                var result = (from category in _context.Category
                              where category.CategoryName == categoryName
                              select category).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? DeleteCategory(int? categoryId)
        {
            try
            {
                var category = _context.Category.Find(categoryId);
                if (category != null)
                    category.Status = false;
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveSelectListItemCategory()
        {
            try
            {
                List<SelectListItem> categoryList;
                string key = "Category_Cache";

                if (!CacheHelper.CheckExists(key))
                {
                    categoryList = (from cat in _context.Category
                                    where cat.Status == true
                                    select new SelectListItem()
                                    {
                                        Text = cat.CategoryName,
                                        Value = cat.CategoryId.ToString()
                                    }).ToList();

                    categoryList.Insert(0, new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    });

                    CacheHelper.AddToCacheWithNoExpiration(key, categoryList);
                }
                else
                {
                    categoryList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return categoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectListItem> GetAllActiveCategoryforListbox()
        {
            try
            {
                List<SelectListItem> categoryList;
                string key = "Category_CacheListbox";

                if (!CacheHelper.CheckExists(key))
                {
                    categoryList = (from cat in _context.Category
                                    where cat.Status == true
                                    select new SelectListItem()
                                    {
                                        Text = cat.CategoryName,
                                        Value = cat.CategoryId.ToString()
                                    }).ToList();


                    CacheHelper.AddToCacheWithNoExpiration(key, categoryList);
                }
                else
                {
                    categoryList = (List<SelectListItem>)CacheHelper.GetStoreCachebyKey(key);
                }

                return categoryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Category GetCategoryById(int? categoryId)
        {
            try
            {
                var result = (from category in _context.Category
                              where category.CategoryId == categoryId
                              select category).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int? UpdateCategory(Category category)
        {
            try
            {
                int? result = -1;
                if (category != null)
                {
                    category.CreateDate = DateTime.Now;
                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = category.CategoryId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CategoryViewModel> GridGetCategory(string categoryName, int startIndex, int count, string sorting)
        {
            try
            {

                using (var db = new DatabaseContext())
                {
                    var query = from category in db.Category
                                select new CategoryViewModel()
                                {
                                    CategoryName = category.CategoryName,
                                    Status = category.Status,
                                    CategoryId = category.CategoryId,
                                    Code = category.Code
                                };

                    //Search
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        query = query.Where(p => p.CategoryName == categoryName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("CategoryId ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryId);
                    }
                    else if (sorting.Equals("CategoryId DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryId);
                    }
                    else if (sorting.Equals("CategoryName ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryName);
                    }
                    else if (sorting.Equals("CategoryName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.CategoryId); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList()  //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCategoryCount(string categoryName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        var result = (from category in db.Category
                                      where category.CategoryName == categoryName
                                      select category).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from category in db.Category
                                      select category).Count();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetCategoryCodeByCategoryId(int? categoryId)
        {
            try
            {
                var result = (from category in _context.Category
                              where category.CategoryId == categoryId
                              select category.Code).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public short GetCategoryIdsByUserId(long? userId)
        {
            try
            {
                using (var db = new DatabaseContext())
                {

                    var result = (from category in db.AgentCategoryAssigned
                                  where category.UserId == userId
                                  select category.CategoryId).FirstOrDefault();
                    return result;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? AddCategoryConfigration(CategoryConfigration category)
        {
            try
            {
                int? result = -1;

                if (category != null)
                {
                    category.CreateDate = DateTime.Now;
                    _context.CategoryConfigration.Add(category);
                    _context.SaveChanges();
                    result = category.CategoryConfigrationId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CategoryConfigration GetCategoryConfigrationDetails(int? categoryConfigrationId)
        {
            var data = (from cag in _context.CategoryConfigration
                        where cag.CategoryConfigrationId == categoryConfigrationId
                        select cag).SingleOrDefault();

            return data;
        }

        public List<ShowCategoryConfigration> GridGetCategoryConfigration(string userName, int startIndex, int count, string sorting)
        {
            try
            {

                using (var db = new DatabaseContext())
                {
                    var query = from categoryconfig in db.CategoryConfigration
                                join category in db.Category on categoryconfig.CategoryId equals category.CategoryId
                                join businessHour in db.BusinessHours on categoryconfig.BusinessHoursId equals businessHour.BusinessHoursId
                                join usermaster in db.Usermasters on categoryconfig.AgentAdminUserId equals usermaster.UserId
                                join hodUsermaster in db.Usermasters on categoryconfig.HodUserId equals hodUsermaster.UserId
                                select new ShowCategoryConfigration()
                                {
                                    CategoryName = category.CategoryName,
                                    Status = category.Status,
                                    Name = businessHour.Name,
                                    CategoryConfigrationId = categoryconfig.CategoryConfigrationId,
                                    UserName = usermaster.FirstName+" "+ usermaster.LastName,
                                    HodName = hodUsermaster.FirstName + " " + hodUsermaster.LastName,
                                };


                    if (!string.IsNullOrEmpty(userName))
                    {
                        query = query.Where(p => p.UserName == userName);
                    }

                    //Sorting Ascending and Descending
                    if (string.IsNullOrEmpty(sorting) || sorting.Equals("CategoryConfigrationId ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryConfigrationId);
                    }
                    else if (sorting.Equals("CategoryConfigrationId DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryConfigrationId);
                    }
                    else if (sorting.Equals("CategoryName ASC"))
                    {
                        query = query.OrderBy(p => p.CategoryName);
                    }
                    else if (sorting.Equals("CategoryName DESC"))
                    {
                        query = query.OrderByDescending(p => p.CategoryName);
                    }
                    else if (sorting.Equals("UserName ASC"))
                    {
                        query = query.OrderBy(p => p.UserName);
                    }
                    else if (sorting.Equals("UserName DESC"))
                    {
                        query = query.OrderByDescending(p => p.UserName);
                    }
                    else
                    {
                        query = query.OrderBy(p => p.CategoryConfigrationId); //Default!
                    }

                    return count > 0
                        ? query.Skip(startIndex).Take(count).ToList()  //Paging
                        : query.ToList(); //No paging
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCategoryConfigrationCount(string userName)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    if (!string.IsNullOrEmpty(userName))
                    {
                        var result = (from categoryconfig in db.CategoryConfigration
                                      join usermaster in db.Usermasters on categoryconfig.AgentAdminUserId equals usermaster.UserId
                                      where usermaster.UserName == userName && categoryconfig.Status == true
                                      select categoryconfig).Count();
                        return result;
                    }
                    else
                    {
                        var result = (from categoryconfig in db.CategoryConfigration
                                      select categoryconfig).Count();
                        return result;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int? UpdateCategoryConfigration(CategoryConfigration category)
        {
            try
            {
                int? result = -1;
                if (category != null)
                {
                    category.CreateDate = DateTime.Now;
                    _context.Entry(category).State = EntityState.Modified;
                    _context.SaveChanges();
                    result = category.CategoryId;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckDuplicateCategoryConfigration(long adminuserId, long hoduserId, int categoryId)
        {
            using (var db = new DatabaseContext())
            {
                var result = (from categoryconfig in db.CategoryConfigration
                              where categoryconfig.AgentAdminUserId == adminuserId && categoryconfig.AgentAdminUserId == hoduserId && categoryconfig.CategoryId == categoryId
                              select categoryconfig).Count();
                return result > 0;
            }
        }

        public int? DeleteCategoryConfigration(int? categoryConfigrationId)
        {
            try
            {
                var categoryConfigration = _context.CategoryConfigration.Find(categoryConfigrationId);
                if (categoryConfigration != null)
                    _context.CategoryConfigration.Remove(categoryConfigration);
                _context.SaveChanges();
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAdminCategory(long userId)
        {
            try
            {
                var result = (from categoryConfigration in _context.CategoryConfigration
                              where categoryConfigration.AgentAdminUserId == userId
                              select categoryConfigration.CategoryId).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetHodCategory(long userId)
        {
            try
            {
                var result = (from categoryConfigration in _context.CategoryConfigration
                    where categoryConfigration.HodUserId == userId
                    select categoryConfigration.CategoryId).FirstOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
