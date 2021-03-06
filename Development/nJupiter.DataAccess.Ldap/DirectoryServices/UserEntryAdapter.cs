#region Copyright & License
// 
// 	Copyright (c) 2005-2012 nJupiter
// 
// 	Permission is hereby granted, free of charge, to any person obtaining a copy
// 	of this software and associated documentation files (the "Software"), to deal
// 	in the Software without restriction, including without limitation the rights
// 	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// 	copies of the Software, and to permit persons to whom the Software is
// 	furnished to do so, subject to the following conditions:
// 
// 	The above copyright notice and this permission notice shall be included in
// 	all copies or substantial portions of the Software.
// 
// 	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// 	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// 	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// 	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// 	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// 	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// 	THE SOFTWARE.
// 
#endregion

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

using nJupiter.DataAccess.Ldap.Configuration;
using nJupiter.DataAccess.Ldap.DirectoryServices.Abstraction;
using nJupiter.DataAccess.Ldap.DistinguishedNames;

namespace nJupiter.DataAccess.Ldap.DirectoryServices {
	public class UserEntryAdapter : EntryAdapterBase, IUserEntryAdapter {
		private readonly IDirectoryEntryAdapter directoryEntryAdapter;
		private readonly ILdapConfig configuration;
		private readonly INameParser nameParser;
		private readonly IFilterBuilder filterBuilder;

		public UserEntryAdapter(ILdapConfig configuration,
		                        IDirectoryEntryAdapter directoryEntryAdapter,
		                        ISearcherFactory searcherFactory,
		                        IFilterBuilder filterBuilder,
		                        INameParser nameParser) : base(searcherFactory) {
			this.configuration = configuration;
			this.directoryEntryAdapter = directoryEntryAdapter;
			this.nameParser = nameParser;
			this.filterBuilder = filterBuilder;
		}

		protected override IEntryConfig Config { get { return configuration.Users; } }

		public virtual IEntry GetUserEntry(string username) {
			return GetUserEntry(username, false);
		}

		public virtual IEntry GetUserEntryAndLoadProperties(string username) {
			return GetUserEntry(username, true);
		}

		public virtual IEntry GetUserEntryByEmail(string email) {
			using(var entry = directoryEntryAdapter.GetEntry(configuration.Users.Path)) {
				return GetUserEntryFromSearcher(entry, CreateUserEmailFilter(email), SearchScope.Subtree);
			}
		}

		public virtual string GetUserName(string entryName) {
			if(!nameParser.RdnInName(entryName, configuration.Users.RdnAttribute, configuration.Users.Base)) {
				using(var entry = GetUserDirectoryEntry(entryName)) {
					entryName = entry.GetProperties<string>(configuration.Users.RdnAttribute).First();
				}
			}
			return nameParser.GetName(configuration.Users.NameType, entryName);
		}

		public virtual IEnumerable<string> GetUsersFromEntry(IEntry entry, string propertyName) {
			var properties = entry.GetProperties<string>(propertyName);
			return properties.Select(GetUserName);
		}

		public virtual IEntry GetUserEntry(string username, string password) {
			using(var user = GetUserEntry(username)) {
				if(!user.IsBound()) {
					return null;
				}
				var userAsDn = nameParser.GetDn(user.Path);
				var uri = new Uri(configuration.Server.Url, userAsDn);
				var authenticatedUser = directoryEntryAdapter.GetEntry(uri, userAsDn, password);
				return GetUserEntryFromSearcher(authenticatedUser);
			}
		}

		public virtual IEntryCollection GetAllUserEntries(int pageIndex, int pageSize, out int totalRecords) {
			return GetUserEntries(configuration.Users.Filter, pageIndex, pageSize, out totalRecords);
		}

		public virtual IEntryCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
			return GetUserEntries(CreateUserNameFilter(usernameToMatch), pageIndex, pageSize, out totalRecords);
		}

		public virtual IEntryCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
			return GetUserEntries(CreateUserEmailFilter(emailToMatch), pageIndex, pageSize, out totalRecords);
		}

		private IEntry GetUserEntry(string username, bool loadProperties) {
			var user = GetUserDirectoryEntry(username);
			if(!loadProperties) {
				return user;
			}
			return GetUserEntryFromSearcher(user);
		}

		private IDirectoryEntry GetUserDirectoryEntry(string username) {
			return directoryEntryAdapter.GetEntry(username, configuration.Users, CreateSearcher);
		}

		private IEntryCollection GetUserEntries(string filter, int pageIndex, int pageSize, out int totalRecords) {
			using(var entry = directoryEntryAdapter.GetEntry(configuration.Users.Path)) {
				if(!entry.IsBound()) {
					totalRecords = 0;
					return new EntryCollection();
				}
				var searcher = CreateSearcher(entry);
				searcher.Filter = filter;
				if(configuration.Server.VirtualListViewSupport) {
					var offset = pageIndex * pageSize + 1;
					var afterCount = pageSize - 1;
					searcher.VirtualListView = CreateVirtualListView(afterCount, offset);
				}
				using(var users = searcher.FindAll()) {
					if(configuration.Server.VirtualListViewSupport) {
						totalRecords = searcher.VirtualListView.ApproximateTotal;
						return users;
					}
					totalRecords = users.Count();
					return users.GetPaged(pageIndex, pageSize);
				}
			}
		}

		protected virtual DirectoryVirtualListView CreateVirtualListView(int afterCount, int offset) {
			return new DirectoryVirtualListView(0, afterCount, offset);
		}

		private IEntry GetUserEntryFromSearcher(IEntry entry) {
			return GetUserEntryFromSearcher(entry, configuration.Users.Filter, SearchScope.Base);
		}

		private IEntry GetUserEntryFromSearcher(IEntry entry, string filter, SearchScope searchScope) {
			if(!entry.IsBound()) {
				return null;
			}
			var searcher = CreateSearcher(entry, searchScope);
			searcher.Filter = filter;
			return searcher.FindOne();
		}

		private string CreateUserEmailFilter(string emailToMatch) {
			return filterBuilder.AttachFilter(configuration.Users.EmailAttribute, emailToMatch, configuration.Users.Filter);
		}

		private string CreateUserNameFilter(string usernameToMatch) {
			var defaultFilter = configuration.Users.Filter;
			if(configuration.Users.Attributes.Any()) {
				return filterBuilder.AttachAttributeFilters(usernameToMatch,
				                                            defaultFilter,
				                                            configuration.Users.RdnAttribute,
				                                            configuration.Users.Attributes);
			}
			return filterBuilder.AttachFilter(configuration.Users.RdnAttribute, usernameToMatch, defaultFilter);
		}
	}
}