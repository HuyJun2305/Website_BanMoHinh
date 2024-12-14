using API.Data;
using API.IRepositories;
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
                Status = OrderStatus.ChoXacNhan,
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
            if (order.Status == OrderStatus.HoanThanh)
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

                // Kiểm tra tồn kho
                if (orderDetail.Quatity > orderDetail.Product.Stock)
                {
                    throw new InvalidOperationException($"Sản phẩm {orderDetail.Product.Name} không đủ hàng. Số lượng yêu cầu: {orderDetail.Quatity}, Tồn kho hiện tại: {orderDetail.Product.Stock}.");
                }

                // Cộng tổng tiền cho đơn hàng
                totalPrice += orderDetail.Quatity * orderDetail.Product.Price;
            }

            // Kiểm tra xem khách hàng có đủ tiền để thanh toán không
            if (amountGiven < totalPrice)
            {
                throw new InvalidOperationException("Số tiền khách hàng đưa không đủ để thanh toán.");
            }

            // Tính tiền thừa trả lại cho khách
            decimal change = amountGiven - totalPrice;

            // Cập nhật trạng thái đơn hàng
            order.Status = OrderStatus.HoanThanh;
            order.PaymentMethods = paymentMethod;
            order.DayPayment = DateTime.Now;
            order.CreateBy = staffId;
            order.AmountPaid = amountGiven;
            order.Change = change;

            // Cập nhật số lượng kho của từng sản phẩm trong đơn hàng
            foreach (var orderDetail in order.OrderDetails)
            {
                if (orderDetail.Product != null)
                {
                    // Giảm số lượng tồn kho của sản phẩm
                    orderDetail.Product.Stock -= orderDetail.Quatity;
                    _context.Entry(orderDetail.Product).State = EntityState.Modified; // Đánh dấu sản phẩm đã được thay đổi
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
                .ThenInclude(od => od.Product) 
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if (order.Status == OrderStatus.ChoXacNhan)
            {
                order.Status = OrderStatus.ChuanBiDonHang;
                

                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = orderDetail.Product;

                    if (product.Stock >= orderDetail.Quatity)
                    {
                        product.Stock -= orderDetail.Quatity;
                    }
                    else
                    {
                        throw new Exception($"Not enough stock for product {product.Name}");
                    }

                    var productSize = await _context.ProductSizes
                        .FirstOrDefaultAsync(ps => ps.ProductId == product.Id && ps.SizeId == orderDetail.SizeId);

                    if (productSize != null)
                    {
                        // Trừ số lượng trong bảng ProductSize
                        if (productSize.Stock >= orderDetail.Quatity)
                        {
                            productSize.Stock -= orderDetail.Quatity;
                        }
                        else
                        {
                            throw new Exception($"Not enough stock for product size {product.Name} - {productSize.Size.Value}");
                        }
                    }

                    // Đảm bảo rằng sản phẩm và ProductSize được theo dõi và cập nhật
                    _context.Products.Update(product);  // Cập nhật lại sản phẩm sau khi trừ số lượng
                    if (productSize != null)
                    {
                        _context.ProductSizes.Update(productSize);  // Cập nhật lại thông tin ProductSize nếu có
                    }
                }

                // Cập nhật lại trạng thái của đơn hàng
                _context.Orders.Update(order);  // Cập nhật đơn hàng với trạng thái mới

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order cannot be accepted because it's already processed.");
            }
        }
        public async Task CancelOrder(Guid orderId, string? note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order.Status == OrderStatus.ChoXacNhan || order.Status == OrderStatus.ChuanBiDonHang)
            {
                // Cập nhật trạng thái đơn hàng
                order.Status = OrderStatus.DaHuy;
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
        public async Task DeliveryOrder(Guid orderId, string?note)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if(order.Status == OrderStatus.ChuanBiDonHang)
            {
                order.Status = OrderStatus.DangGiaoHang;
                order.PaymentStatus = PaymentStatus.Advance;
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
            if (order.Status == OrderStatus.DangGiaoHang)
            {
                order.Status = OrderStatus.DangGiaoHang;
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
        public Task RefundOrder(Guid orderId, string? note)
        {
            throw new NotImplementedException();
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

            if (order.Status == OrderStatus.SaiThongTinDiaChi)
            {
                foreach (var detail in order.OrderDetails)
                {
                    // Cập nhật lại Product stock
                    var product = detail.Product;
                    if (product != null)
                    {
                        product.Stock += detail.Quatity; 
                    }

                    // Cập nhật lại ProductSize stock
                    var productSize = detail.ProductSize;
                    if (productSize != null)
                    {
                        productSize.Stock += detail.Quatity;
                    }
                }

                order.Status = OrderStatus.DaHuy;
                order.PaymentStatus = PaymentStatus.Failed;
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

        public async Task MissingInformation(Guid orderId , string? note)
        {
            var order =  await _context.Orders.FindAsync(orderId);
            if(order.Status == OrderStatus.SaiThongTinDiaChi)
            {
                order.Status = OrderStatus.DaHuy;
                order.PaymentStatus = PaymentStatus.Failed;
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
            oldOrder.Status = OrderStatus.MatHang; // Thêm trạng thái "Mất đơn hàng"
            oldOrder.Note = note ?? "Order marked as lost and recreated.";

            // Tạo một đơn hàng mới với thông tin từ đơn hàng cũ
            var newOrder = new Order
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                CustomerName = oldOrder.CustomerName,
                Price = oldOrder.Price,
                PaymentMethods = oldOrder.PaymentMethods,
                PaymentStatus = oldOrder.PaymentStatus,
                DayCreate = DateTime.Now, // Đặt lại ngày tạo mới
                Status = OrderStatus.ChoXacNhan, // Đặt trạng thái mới là "Chờ xác nhận"
                Note = "Order recreated due to loss.",

                // Sao chép chi tiết đơn hàng
                OrderDetails = oldOrder.OrderDetails.Select(od => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = od.ProductId,
                    SizeId = od.SizeId,
                    Quatity = od.Quatity,
                    TotalPrice = od.TotalPrice,
                }).ToList()
            };

            // Thêm đơn hàng mới vào cơ sở dữ liệu
            _context.Orders.Add(newOrder);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

    }
}
