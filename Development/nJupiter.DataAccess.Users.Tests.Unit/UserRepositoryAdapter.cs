﻿using System.Collections.Generic;

using nJupiter.Configuration;
using nJupiter.DataAccess.Users.Caching;

namespace nJupiter.DataAccess.Users.Tests.Unit {
	public class UserRepositoryAdapter : UserRepositoryBase {

		public UserRepositoryAdapter(string name, IConfig config, IPredefinedNames predefinedNames, IUserCache cache) : base(name, config, predefinedNames, cache){
		}

		private readonly IUserRepository repository;

		public UserRepositoryAdapter() {
		}

		public UserRepositoryAdapter(IUserRepository repository) {
			this.repository = repository;
		}

		public override IUser GetUserById(string userId) {
			return this.repository.GetUserById(userId); 
		}

		public override IUser GetUserByUserName(string userName, string domain) {
			return this.repository.GetUserByUserName(userName, domain);
		}

		public override IList<IUser> GetUsersBySearchCriteria(IEnumerable<SearchCriteria> searchCriteriaCollection) {
			return this.repository.GetUsersBySearchCriteria(searchCriteriaCollection);
		}

		public override IList<IUser> GetUsersByDomain(string domain) {
			return this.repository.GetUsersByDomain(domain);
		}

		public override string[] GetDomains() {
			return this.repository.GetDomains();
		}

		public override IUser CreateUserInstance(string userName, string domain) {
			return this.repository.CreateUserInstance(userName, domain);
		}

		public override void SaveUser(IUser user) {
			this.repository.SaveUser(user);
		}

		public override void SaveUsers(IList<IUser> users) {
			this.repository.SaveUsers(users);
		}

		public override void SetPassword(IUser user, string password) {
			this.repository.SetPassword(user, password);
		}

		public override bool CheckPassword(IUser user, string password) {
			return this.repository.CheckPassword(user, password);
		}

		public override void SaveProperties(IUser user, IPropertyCollection propertyCollection) {
			this.repository.SaveProperties(user, propertyCollection);
		}

		public override void DeleteUser(IUser user) {
			this.repository.DeleteUser(user);
		}

		public override IPropertyCollection GetProperties() {
			return this.repository.GetProperties();
		}

		public override IPropertyCollection GetProperties(IContext context) {
			return this.repository.GetProperties(context);
		}

		public override IEnumerable<IContext> GetContexts() {
			return this.repository.GetContexts();
		}

		public override IContext GetContext(string contextName) {
			return this.repository.GetContext(contextName);
		}

		public override IContext CreateContext(string contextName, ContextSchema schemaTable) {
			return this.repository.CreateContext(contextName, schemaTable);
		}

		public override void DeleteContext(IContext context) {
			this.repository.DeleteContext(context);
		}

		public override ContextSchema GetDefaultContextSchema() {
			return this.repository.GetDefaultContextSchema();
		}

		public override IPropertyCollection GetProperties(IUser user, IContext context) {
			return this.repository.GetProperties(user, context);
		}

	}
}