﻿using Data.Models;

namespace View.IServices
{
	public interface IOrderServices
	{
		Task<List<Order>> GetAllOrder();
		Task<Order> GetOrderById(Guid id);
		Task<Order> CreateByStaff(Guid staffId, Guid? customerId = null, Guid? voucherid = null);
		Task Create(Order order);
		Task Update(Order order);
		Task Delete(Guid id);
	}
}