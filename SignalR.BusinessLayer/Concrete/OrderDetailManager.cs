using SignalR.BusinessLayer.Abstract;
using SignalR.DataAccessLayer.Abstract;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Concrete
{
	public class OrderDetailManager : IOrderDetailService
	{
		private readonly IOrderDetailDal _orderDetailDal;

		public OrderDetailManager(IOrderDetailDal orderDetailDal)
		{
			_orderDetailDal = orderDetailDal;
		}

		public void TAdd(Order entity)
		{
			throw new NotImplementedException();
		}

		public void TDelete(Order entity)
		{
			throw new NotImplementedException();
		}

		public Order TGetByID(int id)
		{
			throw new NotImplementedException();
		}

		public List<Order> TGetListAll()
		{
			throw new NotImplementedException();
		}

		public void TUpdate(Order entity)
		{
			throw new NotImplementedException();
		}
	}
}
