using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly string collectionName = "Orders";
        public OrderRepository(IMongoDatabase mongoDatabase)
        {
            _orderCollection = mongoDatabase.GetCollection<Order>(collectionName);
        }
        public  async Task<Order?> AddOrder(Order order)
        {
            order.OrderID = Guid.NewGuid();
            order._id = order.OrderID;
            foreach (var orderItem in order.OrderItems) 
            {
                orderItem._id = Guid.NewGuid();
            }
           await _orderCollection.InsertOneAsync(order);
            return order;
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderId);
            Order existingOrder = _orderCollection.Find(filter).FirstOrDefault();
            if (existingOrder == null)
                return false;
           await _orderCollection.DeleteOneAsync(filter);
            return true;
        }

        public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
           return (await _orderCollection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return (await _orderCollection.FindAsync(Builders<Order>.Filter.Empty)).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            return (await _orderCollection.FindAsync(filter)).ToList();
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, order.OrderID);
            Order existingOrder = _orderCollection.Find(filter).FirstOrDefault();
            if (existingOrder == null)
                return null;
            order._id = existingOrder._id;
            var result = await _orderCollection.ReplaceOneAsync(filter, order);
            return order;
        }
    }
}
