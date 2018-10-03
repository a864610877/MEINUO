select * from users t 
where t.phoneNumber = @phoneNumber and (@exceptUserId = 0 or t.userId <> @exceptUserId)