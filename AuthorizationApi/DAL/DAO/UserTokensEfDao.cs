using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationApi.DAL.DAO
{
	public class UserTokensEfDao : IUserTokensDao
	{
		private TokenDb _db;

		public UserTokensEfDao(TokenDb db)
		{
			_db = db;
		}

		public int AddOrUpdate(UserTokens userTokens)
		{
			UserTokens trackedUserTokens = _db.UserTokens.SingleOrDefault(ut => ut.UserId == userTokens.UserId);

			if (trackedUserTokens == null)
				_db.Add(userTokens);
			else
				_db.Entry(trackedUserTokens).CurrentValues.SetValues(userTokens);

			return _db.SaveChanges();
		}

		public UserTokens GetByUserId(string userId)
		{
			return _db.UserTokens.SingleOrDefault(ut => ut.UserId == userId);
		}
	}
}
