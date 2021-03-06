using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.DataLayer.Contracts;
using Store.Core.DataLayer.DataContracts;
using Store.Core.EntityLayer.Dbo;
using Store.Core.EntityLayer.HumanResources;
using Store.Core.EntityLayer.Sales;

namespace Store.Core.DataLayer.Repositories
{
    public class SalesRepository : Repository, ISalesRepository
    {
        public SalesRepository(IUserInfo userInfo, StoreDbContext dbContext)
            : base(userInfo, dbContext)
        {
        }

        public IQueryable<Customer> GetCustomers()
            => DbContext.Set<Customer>();

        public async Task<Customer> GetCustomerAsync(Customer entity)
            => await DbContext.Set<Customer>().FirstOrDefaultAsync(item => item.CustomerID == entity.CustomerID);

        public async Task<int> AddCustomerAsync(Customer entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteCustomerAsync(Customer entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderInfo> GetOrders(short? currencyID = null, int? customerID = null, int? employeeID = null, short? orderStatusID = null, Guid? paymentMethodID = null, int? shipperID = null)
        {
            var query =
                from order in DbContext.Set<Order>()
                join currencyJoin in DbContext.Set<Currency>() on order.CurrencyID equals currencyJoin.CurrencyID into currencyTemp
                from currency in currencyTemp.DefaultIfEmpty()
                join customer in DbContext.Set<Customer>() on order.CustomerID equals customer.CustomerID
                join employeeJoin in DbContext.Set<Employee>() on order.EmployeeID equals employeeJoin.EmployeeID into employeeTemp
                from employee in employeeTemp.DefaultIfEmpty()
                join orderStatus in DbContext.Set<OrderStatus>() on order.OrderStatusID equals orderStatus.OrderStatusID
                join paymentMethodJoin in DbContext.Set<PaymentMethod>() on order.PaymentMethodID equals paymentMethodJoin.PaymentMethodID into paymentMethodTemp
                from paymentMethod in paymentMethodTemp.DefaultIfEmpty()
                join shipperJoin in DbContext.Set<Shipper>() on order.ShipperID equals shipperJoin.ShipperID into shipperTemp
                from shipper in shipperTemp.DefaultIfEmpty()
                select new OrderInfo
                {
                    OrderID = order.OrderID,
                    OrderStatusID = order.OrderStatusID,
                    CustomerID = order.CustomerID,
                    EmployeeID = order.EmployeeID,
                    ShipperID = order.ShipperID,
                    OrderDate = order.OrderDate,
                    Total = order.Total,
                    CurrencyID = order.CurrencyID,
                    PaymentMethodID = order.PaymentMethodID,
                    Comments = order.Comments,
                    CreationUser = order.CreationUser,
                    CreationDateTime = order.CreationDateTime,
                    LastUpdateUser = order.LastUpdateUser,
                    LastUpdateDateTime = order.LastUpdateDateTime,
                    Timestamp = order.Timestamp,
                    CurrencyCurrencyName = currency == null ? string.Empty : currency.CurrencyName,
                    CurrencyCurrencySymbol = currency == null ? string.Empty : currency.CurrencySymbol,
                    CustomerCompanyName = customer == null ? string.Empty : customer.CompanyName,
                    CustomerContactName = customer == null ? string.Empty : customer.ContactName,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeMiddleName = employee == null ? string.Empty : employee.MiddleName,
                    EmployeeLastName = employee.LastName,
                    EmployeeBirthDate = employee.BirthDate,
                    OrderStatusDescription = orderStatus.Description,
                    PaymentMethodPaymentMethodName = paymentMethod == null ? string.Empty : paymentMethod.PaymentMethodName,
                    PaymentMethodPaymentMethodDescription = paymentMethod == null ? string.Empty : paymentMethod.PaymentMethodDescription,
                    ShipperCompanyName = shipper == null ? string.Empty : shipper.CompanyName,
                    ShipperContactName = shipper == null ? string.Empty : shipper.ContactName,
                };

            if (currencyID.HasValue)
                query = query.Where(item => item.CurrencyID == currencyID);

            if (customerID.HasValue)
                query = query.Where(item => item.CustomerID == customerID);

            if (employeeID.HasValue)
                query = query.Where(item => item.EmployeeID == employeeID);

            if (orderStatusID.HasValue)
                query = query.Where(item => item.OrderStatusID == orderStatusID);

            if (paymentMethodID.HasValue)
                query = query.Where(item => item.PaymentMethodID == paymentMethodID);

            if (shipperID.HasValue)
                query = query.Where(item => item.ShipperID == shipperID);

            return query;
        }

        public async Task<Order> GetOrderAsync(Order entity)
            => await DbContext.Set<Order>().Include(p => p.OrderDetails).FirstOrDefaultAsync(item => item.OrderID == entity.OrderID);

        public Task<int> AddOrderAsync(Order entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<int> UpdateOrderAsync(Order changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteOrderAsync(Order entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public async Task<OrderDetail> GetOrderDetailAsync(OrderDetail entity)
            => await DbContext.Set<OrderDetail>().FirstOrDefaultAsync(item => item.OrderID == entity.OrderID && item.ProductID == entity.ProductID);

        public Task<int> AddOrderDetailAsync(OrderDetail entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<int> UpdateOrderDetailAsync(OrderDetail changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteOrderDetailAsync(OrderDetail entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<Shipper> GetShippers()
            => DbContext.Set<Shipper>();

        public async Task<Shipper> GetShipperAsync(Shipper entity)
            => await DbContext.Set<Shipper>().FirstOrDefaultAsync(item => item.ShipperID == entity.ShipperID);

        public async Task<int> AddShipperAsync(Shipper entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> UpdateShipperAsync(Shipper changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<int> DeleteShipperAsync(Shipper entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderStatus> GetOrderStatus()
            => DbContext.Set<OrderStatus>();

        public async Task<OrderStatus> GetOrderStatusAsync(OrderStatus entity)
            => await DbContext.Set<OrderStatus>().FirstOrDefaultAsync(item => item.OrderStatusID == entity.OrderStatusID);

        public async Task<int> AddOrderStatusAsync(OrderStatus entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<int> UpdateOrderStatusAsync(OrderStatus changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<int> RemoveOrderStatusAsync(OrderStatus entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<Currency> GetCurrencies()
            => DbContext.Set<Currency>();

        public IQueryable<PaymentMethod> GetPaymentMethods()
            => DbContext.Set<PaymentMethod>();
    }
}
