using API.Data;
using API.IRepositories;
using Data.DTO;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task Create(Order order)
        {
            if (await GetOrderById(order.Id) != null) throw new DuplicateWaitObjectException($"Order : {order.Id} is existed!");
            await _context.Orders.AddAsync(order);
        }
        public async Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherId = null)
        {
            var staffAccount = await _context.Accounts.FirstOrDefaultAsync(p => p.Id == staffId);
            if (staffAccount == null)
            {
                throw new Exception($"Staff account with ID {staffId} does not exist.");
            }

            var isStaff = await _context.UserRoles.AnyAsync(ur =>
                ur.UserId == staffId && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Staff"));
            if (!isStaff)
            {
                throw new Exception($"Account with ID {staffId} is not authorized as a Staff.");
            }

            ApplicationUser? customerAccount = null;
            if (customerId.HasValue)
            {
                customerAccount = await _context.Accounts.FirstOrDefaultAsync(p => p.Id == customerId);
                if (customerAccount == null)
                {
                    throw new Exception($"Customer account with ID {customerId} does not exist.");
                }

                var isCustomer = await _context.UserRoles.AnyAsync(ur =>
                    ur.UserId == customerId && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Customer"));
                if (!isCustomer)
                {
                    throw new Exception($"Account with ID {customerId} is not authorized as a Customer.");
                }
            }

            Voucher? voucher = null;
            if (voucherId.HasValue)
            {
                voucher = await _context.Vouchers
                                       .Include(v => v.Account)
                                       .FirstOrDefaultAsync(v => v.Id == voucherId);

                if (voucher == null)
                {
                    throw new Exception($"Voucher with ID {voucherId} does not exist.");
                }

                if (customerId.HasValue && voucher.AccountId != customerId)
                {
                    throw new Exception("This voucher does not belong to the specified customer.");
                }
            }

            // Tạo đơn hàng mới
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                AccountId = customerId,
                CreateBy = staffId,
                DayCreate = DateTime.Now,
                Price = 0,
                PaymentMethods = PaymentMethod.Cash,
                Status = OrderStatus.CreateOrder,
                CustomerName = customerAccount?.Name ?? "Guest",
                VoucherId = voucherId
            };

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }
        public async Task Delete(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders.Include(p => p.Account).Include(p => p.OrderDetails).Include(p => p.OrderAddresses).ToListAsync();
        }
        public async Task<Order> GetOrderById(Guid id)
        {
            return await _context.Orders
                                 .Include(p => p.Account).Include(p => p.OrderAddresses)  // Bao gồm thông tin liên quan đến Account
                                 .FirstOrDefaultAsync(o => o.Id == id);  // Lọc theo ID đơn hàng
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        public async Task Update(Order order)
        {
            var existingOrder = await GetOrderById(order.Id);
            if (existingOrder == null) throw new KeyNotFoundException("Not found this Id!");

            // Chỉ cập nhật các trường cần thiết
            existingOrder.Price = order.Price;
            existingOrder.PaymentMethods = order.PaymentMethods;
            existingOrder.Status = order.Status;
            existingOrder.CustomerName = order.CustomerName;

            // Đánh dấu thực thể đã được thay đổi
            _context.Entry(existingOrder).State = EntityState.Modified;
        }
        public async Task CheckOutInStore(Guid orderId, Guid staffId, decimal amountGiven, PaymentMethod paymentMethod)
        {
            // Lấy thông tin đơn hàng cùng với chi tiết và sản phẩm
            var order = await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.ProductSizes) // Bao gồm thông tin về ProductSizes
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Không tìm thấy đơn hàng.");
            }

            // Kiểm tra nếu đơn hàng không có chi tiết
            if (!order.OrderDetails.Any())
            {
                throw new KeyNotFoundException("Đơn hàng không có sản phẩm nào.");
            }

            // Kiểm tra trạng thái đơn hàng (đảm bảo không thay đổi đơn hàng đã hoàn thành)
            if (order.Status == OrderStatus.Complete)
            {
                throw new InvalidOperationException("Đơn hàng đã hoàn thành, không thể thanh toán lại.");
            }

            decimal totalPrice = 0;

            // Kiểm tra từng sản phẩm trong đơn hàng
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.Product == null)
                {
                    throw new KeyNotFoundException($"Sản phẩm với ID {orderDetail.ProductId} không tồn tại.");
                }

                // Kiểm tra tồn kho trong ProductSize
                var productSize = orderDetail.Product.ProductSizes
                    .FirstOrDefault(ps => ps.SizeId == orderDetail.SizeId); // Lấy thông tin size tương ứng

                if (productSize == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy thông tin kích thước cho sản phẩm {orderDetail.Product.Name}.");
                }

                // Kiểm tra tồn kho trong ProductSize
                if (orderDetail.Quantity > productSize.Stock)
                {
                    throw new InvalidOperationException($"Sản phẩm {orderDetail.Product.Name} (Size: {productSize.Size.Value}) không đủ hàng. Số lượng yêu cầu: {orderDetail.Quantity}, Tồn kho hiện tại: {productSize.Stock}.");
                }

                // Cộng tổng tiền cho đơn hàng
                totalPrice += orderDetail.Quantity * orderDetail.Product.Price;

                // Giảm số lượng tồn kho trong ProductSize
                productSize.Stock -= orderDetail.Quantity;
                _context.Entry(productSize).State = EntityState.Modified; // Đánh dấu ProductSize đã thay đổi
            }

            // Kiểm tra xem khách hàng có đủ tiền để thanh toán không
            if (amountGiven < totalPrice)
            {
                throw new InvalidOperationException("Số tiền khách hàng đưa không đủ để thanh toán.");
            }

            // Tính tiền thừa trả lại cho khách
            decimal change = amountGiven - totalPrice;

            // Cập nhật trạng thái đơn hàng
            order.Status = OrderStatus.Complete;
            order.PaymentMethods = paymentMethod;
            order.PaymentStatus = PaymentStatus.Paid;
            order.Note = "Checkout in store";
            order.DayPayment = DateTime.Now;
            order.CreateBy = staffId;
            order.AmountPaid = amountGiven;
            order.Change = change;

            // Cập nhật số lượng kho tổng của sản phẩm trong bảng Product
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.Product != null)
                {
                    // Giảm số lượng tồn kho của sản phẩm chính trong bảng Product
                    orderDetail.Product.Stock -= orderDetail.Quantity;
                    _context.Entry(orderDetail.Product).State = EntityState.Modified; // Đánh dấu Product đã thay đổi
                }
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrderStatus()
        {
            return await _context.Orders.Include(p => p.Account).Where(o => o.Status == 0).ToListAsync();
        }
        public async Task<List<Order>> GetOrderByStatus(OrderStatus status)
        {
            return await _context.Orders.Include(o => o.Account)
                                 .Where(o => o.Status == status)
                                 .OrderByDescending(o => o.DayCreate)
                                 .ToListAsync();
        }
        public async Task<List<Order>> GetOrderByCustomerId(Guid customerId)
        {
            return await _context.Orders
        .Include(o => o.OrderDetails) // Bao gồm chi tiết đơn hàng
            .ThenInclude(od => od.Product) // Bao gồm sản phẩm từ chi tiết đơn hàng
        .Include(o => o.OrderAddresses) // Bao gồm địa chỉ đơn hàng
        .Where(o => o.AccountId == customerId) // Lọc theo CustomerId
        .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByCustomerIdAndStatus(Guid customerId, OrderStatus status)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                .Where(o => o.AccountId == customerId && o.Status == status)
                .ToListAsync();
        }
        public async Task AcceptOrder(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product).AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (order.Status == OrderStatus.WaitingForConfirmation)
            {
                order.Status = OrderStatus.PrepareOrder;

                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = orderDetail.Product;

                    // Kiểm tra số lượng tồn kho sản phẩm
                    if (product.Stock >= orderDetail.Quantity)
                    {
                        product.Stock -= orderDetail.Quantity;
                    }
                    else
                    {
                        throw new Exception($"Not enough stock for product {product.Name}");
                    }

                    var productSize = await _context.ProductSizes
                        .Include(ps => ps.Size).AsNoTracking() // Include thông tin Size
                        .FirstOrDefaultAsync(ps => ps.ProductId == product.Id && ps.SizeId == orderDetail.SizeId);

                    if (productSize != null)
                    {
                        // Kiểm tra tồn kho của ProductSize
                        if (productSize.Stock >= orderDetail.Quantity)
                        {
                            productSize.Stock -= orderDetail.Quantity;
                        }
                        else if (productSize.Stock == 0)
                        {
                            // Thông báo nếu không có hàng trong kho
                            throw new Exception($"Product size {product.Name} - {productSize.Size?.Value ?? "Unknown"} is out of stock. Please wait for restocking.");
                        }
                        else
                        {
                            throw new Exception($"Not enough stock for product size {product.Name} - {productSize.Size?.Value ?? "Unknown"}.");
                        }

                        _context.ProductSizes.Update(productSize); // Cập nhật ProductSize
                    }
                }

                _context.Products.UpdateRange(order.OrderDetails.Select(od => od.Product)); // Cập nhật danh sách sản phẩm
                _context.Orders.Update(order); // Cập nhật trạng thái đơn hàng

            }
            else
            {
                throw new Exception("Order cannot be accepted because it's already processed.");
            }
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

        }
        public async Task CancelOrder(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.WaitingForConfirmation || order.Status == OrderStatus.PrepareOrder)
            {
                // Cập nhật trạng thái đơn hàng
                order.Status = OrderStatus.Canceled;
                order.PaymentStatus = PaymentStatus.Failed;
                order.Note = note;
                _context.Orders.Update(order);

            }
            else
            {
                throw new Exception("Order cannot be canceled");
            }
            await _context.SaveChangesAsync();

        }
        public async Task DeliveryOrder(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.PrepareOrder)
            {
                order.Status = OrderStatus.OnDelivery;
                order.PaymentStatus = PaymentStatus.Advance;
                order.Note = note;
                _context.Orders.Update(order);
            }
            else
            {
                throw new Exception("Preparation is having problems");
            }
            await _context.SaveChangesAsync();

        }
        public async Task ConplateOrder(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.OnDelivery)
            {
                order.Status = OrderStatus.Delivered;
                order.PaymentStatus = PaymentStatus.Paid;
                _context.Orders.Update(order);
            }
            else
            {
                throw new Exception("Delivery is having trouble");
            }
            await _context.SaveChangesAsync();

        }
        // Refund vẫn chưa làm
        public async Task ReOrder(Guid orderId, string? note)
        {
            // Tìm đơn hàng cũ
            var oldOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product).AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (oldOrder == null)
            {
                throw new Exception("Order not found");
            }

            // Cập nhật trạng thái của đơn hàng cũ
            oldOrder.Status = OrderStatus.Canceled; // Thêm trạng thái "Mất đơn hàng"
            oldOrder.PaymentStatus = PaymentStatus.Paid;
            oldOrder.Note = note;

            // Tạo một đơn hàng mới với thông tin từ đơn hàng cũ
            var newOrder = new Order
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                CustomerName = oldOrder.CustomerName,
                Price = oldOrder.Price,
                PaymentMethods = oldOrder.PaymentMethods,
                PaymentStatus = PaymentStatus.Pending,
                DayCreate = DateTime.Now, // Đặt lại ngày tạo mới
                Status = OrderStatus.WaitingForConfirmation, // Đặt trạng thái mới là "Chờ xác nhận"
                Note = "Reorder.",

                // Sao chép chi tiết đơn hàng
                OrderDetails = oldOrder.OrderDetails.Select(od => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = od.ProductId,
                    SizeId = od.SizeId,
                    Quantity = od.Quantity,
                    TotalPrice = od.TotalPrice,
                }).ToList()
            };

            // Thêm đơn hàng mới vào cơ sở dữ liệu
            _context.Orders.Add(newOrder);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }
        public async Task ShippingError(Guid orderId, string? note)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.ProductSize)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (order.Status == OrderStatus.OnDelivery)
            {
                foreach (var detail in order.OrderDetails)
                {

                    order.Status = OrderStatus.IncorrectAddress;
                    order.PaymentStatus = PaymentStatus.Advance;
                    order.Note = note;

                    _context.Orders.Update(order);

                }
                await _context.SaveChangesAsync(); // Lưu toàn bộ thay đổi

            }
        }
        public async Task MissingInformation(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.OnDelivery)
            {
                order.Status = OrderStatus.WaitingForConfirmation;
                order.PaymentStatus = PaymentStatus.Pending;
                order.Note = note;
                _context.Orders.Update(order);
            }
            await _context.SaveChangesAsync();
        }
        public async Task LoseOrder(Guid orderId, string? note)
        {
            // Tìm đơn hàng cũ
            var oldOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (oldOrder == null)
            {
                throw new Exception("Order not found");
            }

            // Cập nhật trạng thái của đơn hàng cũ
            oldOrder.Status = OrderStatus.LostGoods; // Thêm trạng thái "Mất đơn hàng"
            oldOrder.PaymentStatus = PaymentStatus.Paid;
            oldOrder.Note = note ?? "Order marked as lost and recreated.";

            // Tạo một đơn hàng mới với thông tin từ đơn hàng cũ
            var newOrder = new Order
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                CustomerName = oldOrder.CustomerName,
                Price = oldOrder.Price,
                PaymentMethods = oldOrder.PaymentMethods,
                PaymentStatus = PaymentStatus.Pending,
                DayCreate = DateTime.Now, // Đặt lại ngày tạo mới
                Status = OrderStatus.WaitingForConfirmation, // Đặt trạng thái mới là "Chờ xác nhận"
                Note = "Order recreated due to loss.",

                // Sao chép chi tiết đơn hàng
                OrderDetails = oldOrder.OrderDetails.Select(od => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = od.ProductId,
                    SizeId = od.SizeId,
                    Quantity = od.Quantity,
                    TotalPrice = od.TotalPrice,
                }).ToList()
            };

            // Thêm đơn hàng mới vào cơ sở dữ liệu
            _context.Orders.Add(newOrder);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task AcceptRefund(Guid orderId, string? note)
        {
            // Tìm đơn hàng yêu cầu hoàn tiền
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.Refund)
            {
                order.Status = OrderStatus.AcceptRefund;
                order.PaymentStatus = PaymentStatus.Refunded;
                order.Note = note;
            }
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task CancelRefund(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);


            if (order.Status == OrderStatus.Refund)
            {
                order.Status = OrderStatus.Complete;
                order.PaymentStatus = PaymentStatus.Paid;
                order.Status = OrderStatus.Complete;
                order.Note = "The product does not meet return requirements";  // Optional: Ghi chú lý do hủy bỏ
            }


            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

        }
        public async Task RefundByCustomer(Guid orderId, string note)
        {
            var order = await _context.Orders
                .FindAsync(orderId);


            if (order.Status == OrderStatus.Complete)
            {
                order.Status = OrderStatus.Refund;
                order.PaymentStatus = PaymentStatus.Pending;
                order.Note = note;
                _context.Orders.Update(order);
            }
            else
            {
                throw new Exception("Information not correct");
            }
            await _context.SaveChangesAsync();
        }

        public async Task PaidOrder(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.Delivered)
            {
                order.Status = OrderStatus.Complete;
                order.PaymentStatus = PaymentStatus.Paid;
                _context.Orders.Update(order);
            }
            else
            {
                throw new Exception("??? Can't be");
            }
            await _context.SaveChangesAsync();
        }

        public async Task Accident(Guid orderId, string? note)
        {
            var order = await _context.Orders
                 .Include(o => o.OrderDetails)
                     .ThenInclude(od => od.Product)
                 .Include(o => o.OrderDetails)
                     .ThenInclude(od => od.ProductSize)
                 .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            if (order.Status == OrderStatus.OnDelivery)
            {
                foreach (var detail in order.OrderDetails)
                {
                    // Cập nhật lại Product stock
                    var product = detail.Product;
                    if (product != null)
                    {
                        product.Stock += detail.Quantity;
                    }

                    // Cập nhật lại ProductSize stock
                    var productSize = detail.ProductSize;
                    if (productSize != null)
                    {
                        productSize.Stock += detail.Quantity;
                    }
                }

                order.Status = OrderStatus.Accident;
                order.PaymentStatus = PaymentStatus.Advance;
                order.Note = note;

                _context.Orders.Update(order);
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Product != null)
                    {
                        _context.Products.Update(detail.Product);
                    }
                    if (detail.ProductSize != null)
                    {
                        _context.ProductSizes.Update(detail.ProductSize);
                    }
                }

                await _context.SaveChangesAsync(); // Lưu toàn bộ thay đổi
            }
        }

        public async Task ReShip(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);   
            if(order.Status == OrderStatus.Accident || order.Status == OrderStatus.IncorrectAddress)
            {
                order.Status = OrderStatus.OnDelivery;
                order.PaymentStatus= PaymentStatus.Advance;
                order.Note = note;
                _context.Orders.Update(order);
            }
            else
            {
                throw new Exception("Not found");
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderDetails(Guid orderId)
        {
            return await _context.Orders
           .Include(o => o.OrderAddresses)
           .FirstOrDefaultAsync(o => o.Id == orderId);

        }
    }
}
