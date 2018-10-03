select * from users t 
where t.mobile = @mobile and (@exceptUserId = 0 or t.userId <> @exceptUserId)