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
    public class ProductRepository : Repository<Product> , IProductRepository 
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public void Update(Product obj)
        {
            var objfromdb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if(objfromdb!=null)
            {
                objfromdb.InstrumentName = obj.InstrumentName;
                objfromdb.Description = obj.Description;
                objfromdb.price = obj.price;
                objfromdb.stock_quantity = obj.stock_quantity;
                objfromdb.CategoryId = obj.CategoryId;
                if(obj.ImageUrl!=null)
                {
                    objfromdb.ImageUrl = obj.ImageUrl;
                }
            }

        }
    }
}
