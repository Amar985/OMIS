using OnlineMusic.DataAccess.Data;
using OnlineMusic.DataAccess.Repository.IRepository;
using OnlineMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMusic.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader> , IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

		public void updateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			var orderFromDb = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
		}

		public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
		{
			var orderfromDb = _db.OrderHeaders.FirstOrDefault(u=>u.Id == id);
            if(!string.IsNullOrEmpty(sessionId))
            {
                orderfromDb.SessionId= sessionId;
            }
			if (!string.IsNullOrEmpty(sessionId))
			{
				orderfromDb.PaymentIntentId=paymentIntentId;
                orderfromDb.PaymentDate= DateTime.Now;
			}
		}
	}
}
