select * from shops t 
where t.phoneNumber = @phoneNumber and (@exceptShopId = 0 or t.shopId <> @exceptShopId)