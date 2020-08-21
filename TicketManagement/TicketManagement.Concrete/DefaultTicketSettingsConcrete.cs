using System;
using System.Data.Entity;
using System.Linq;
using TicketManagement.Interface;
using TicketManagement.Models;

namespace TicketManagement.Concrete
{
    public class DefaultTicketSettingsConcrete : IDefaultTicketSettings
    {
        private readonly DatabaseContext _context;

        public DefaultTicketSettingsConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public int? AddDefaultTicketCount(DefaultTicketSettings defaultTicket)
        {
            if (defaultTicket.DefaultTicketId != null)
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    var defaultTicketsModel = (from defaultTickets in _context.DefaultTicketSettings
                                               where defaultTickets.DefaultTicketId == defaultTicket.DefaultTicketId
                                               select defaultTickets).FirstOrDefault();
                    try
                    {
                        int? result = -1;
                        if (defaultTicketsModel != null)
                        {
                            defaultTicketsModel.AutoTicketsCloseHour = defaultTicket.AutoTicketsCloseHour;
                            defaultTicketsModel.TicketsCount = defaultTicketsModel.TicketsCount;
                            defaultTicketsModel.UpdatedDate = DateTime.Now;
                            _context.Entry(defaultTicketsModel).State = EntityState.Modified;
                        }

                        result = _context.SaveChanges();
                        dbContextTransaction.Commit();

                        return result;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
            else
            {
                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        defaultTicket.CreateDate = DateTime.Now;
                        _context.DefaultTicketSettings.Add(defaultTicket);
                        _context.SaveChanges();
                        dbContextTransaction.Commit();
                        return 1;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public DefaultTicketSettings GetDefaultTicketCount()
        {
            var isaddedcheck = (from defaultTickets in _context.DefaultTicketSettings
                                select defaultTickets).FirstOrDefault();
            return isaddedcheck;
        }
    }
}